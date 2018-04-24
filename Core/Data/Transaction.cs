/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : Transaction                                      Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Allows the execution of data write operations in a transactional mode.                        *
*              Instances must created using the method BeginTransaction on a DataWriterContext object.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Data;
using System.EnterpriseServices;

namespace Empiria.Data {

  /// <summary>Allows the execution of data write operations in a transactional mode.
  /// Instances must created using the method BeginTransaction on a DataWriterContext object.</summary>
  public sealed class Transaction : ITransaction {

    #region Fields

    private Hashtable transactions = Hashtable.Synchronized(new Hashtable());
    private DataOperationList operations = null;
    private DataWriterContext context = null;
    private IsolationLevel isolationLevel = IsolationLevel.Unspecified;

    private bool wasCommited = false;
    private bool wasRolledBack = false;
    private bool disposed = false;
    private bool working = false;

    #endregion Fields

    #region Constructors and parsers

    internal Transaction(DataWriterContext context, IsolationLevel isolationLevel) {
      this.context = context;
      this.isolationLevel = isolationLevel;
      this.operations = new DataOperationList(context.Name);
    }

    ~Transaction() {
      Dispose(false);
    }

    #endregion Constructors and parsers

    #region Internal properties

    internal int Count {
      get { return operations.Count; }
    }

    internal bool WasCommited {
      get { return wasCommited; }
    }

    internal bool WasRolledBack {
      get { return wasRolledBack; }
    }

    #endregion Internal properties

    #region Public methods

    public int Commit() {

      try {
        if (context.AsynchronousUpdate) {

          context.TransactionCommitEvent.Set();

          return 0;

        } else {
          return PerformCommit();
        }

      } catch (Exception exception) {
        exception = exception.GetBaseException();

        throw new EmpiriaDataException(EmpiriaDataException.Msg.CommitFails,
                                       exception, exception.Message);

      }
    }

    public void Dispose() {
      Dispose(true);

      GC.SuppressFinalize(this);
    }

    public void Rollback() {
      try {

        if (wasCommited) {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.TransactionCommited);
        }

        lock (operations) {

          if (!context.IsInOSTransaction) {

            foreach (IDbTransaction transaction in transactions.Values) {

              if ((transaction != null) && (transaction.Connection != null) &&
                  (transaction.Connection.State != ConnectionState.Closed)) {
                transaction.Rollback();
              }

            } //foreach

          } else {
            ContextUtil.SetAbort();
          }

        }  //lock

      } catch (Exception innerException) {
        CleanResources();

        if (context.IsInOSTransaction) {
          ContextUtil.SetAbort();
        }

        throw new EmpiriaDataException(EmpiriaDataException.Msg.RollbackFails, innerException,
                                       innerException.Message);
      } finally {
        CleanResources();

        wasCommited = false;
        wasRolledBack = true;
      }
    }

    #endregion Public methods

    #region Internal methods

    internal int PerformCommit() {
      if (wasCommited == true) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.TransactionAlreadyCommited);
      }

      int counter = 0;

      if (!context.WasUpdated) {
        return 0;
      }

      try {
        working = true;

        operations = context.TransactionalOperations;

        counter = PrepareTransactions();

        CommitTransactions();

        operations.Clear();

        wasCommited = true;
        wasRolledBack = false;

        return counter;

      } catch (Exception innerException) {
        wasCommited = false;
        wasRolledBack = false;

        throw new EmpiriaDataException(EmpiriaDataException.Msg.CommitFails,
                                       innerException, innerException.Source);

      } finally {
        CleanResources();

        working = false;
      }
    }

    #endregion Internal methods

    #region Private methods

    private void CleanResources() {
      if (disposed) {
        return;
      }

      if (!context.IsInOSTransaction) {

        lock (transactions) {
          IDictionaryEnumerator enumerator = transactions.GetEnumerator();

          while (enumerator.MoveNext()) {

          IDbTransaction transaction = (IDbTransaction) enumerator.Value;

          if ((transaction != null) && (transaction.Connection != null)) {
              IDbConnection connection = transaction.Connection;

              connection.Dispose();

              transaction = null;
            } //if

          } //while
        }

      } else {

        foreach (IDbConnection connection in transactions.Values) {

          if (connection != null) {
            connection.Dispose();
          }

        } //foreach

      } //if

      transactions.Clear();
    }

    private void CommitTransactions() {
      if (context.IsInOSTransaction) {
        return;
      }

      IDictionaryEnumerator enumerator = transactions.GetEnumerator();

      while (enumerator.MoveNext()) {
        IDbTransaction transaction = (IDbTransaction) enumerator.Value;

        IDbConnection connection = transaction.Connection;

        transaction.Commit();

        transaction.Dispose();

        connection.Dispose();

        transaction = null;
      }
    }


    private void CreateConnections() {
      string lastSource = String.Empty;

      for (int i = 0, count = operations.Count; i < count; i++) {

        DataSource dataSource = operations[i].DataSource;

        if (lastSource != dataSource.Source) {
          lastSource = dataSource.Source;

          if (!transactions.ContainsKey(dataSource.Source)) {
            transactions.Add(dataSource.Source, dataSource.GetConnection());
          }

        } // if

      } // for

    }

    private void CreateTransactions() {

      for (int i = 0, count = operations.Count; i < count; i++) {

        DataSource dataSource = operations[i].DataSource;

        if (!transactions.ContainsKey(dataSource.Source)) {
          IDbConnection connection = dataSource.GetConnection();

          connection.Open();

          IDbTransaction transaction = connection.BeginTransaction(isolationLevel);

          transactions.Add(dataSource.Source, transaction);
        } // if

      } // for
    }

    private void Dispose(bool disposing) {
      if (disposed) {
        return;
      }

      disposed = true;

      try {

        if (context.AsynchronousUpdate) {
          while (working) {
            // waiting
          }
        }

        CleanResources();

        if (disposing) {
          operations.Clear();
        }

      } catch (Exception innerException) {
        EmpiriaLog.Error(innerException);

        throw;

      }
    }

    private int PrepareTransactions() {
      int counter = 0;
      string lastSource = String.Empty;

      if (!context.IsInOSTransaction) {
        CreateTransactions();

        IDbTransaction transaction = null;

        for (int i = 0, count = operations.Count; i < count; i++) {
          DataOperation operation = operations[i];

          if (operation.DeferExecution == true) {
            continue;
          }

          if (lastSource != operation.DataSource.Source) {
            lastSource = operation.DataSource.Source;

            transaction = (IDbTransaction) transactions[lastSource];
          }

          counter += DataWriter.Execute(transaction, operation);
        }

      } else {

        CreateConnections();

        IDbConnection connection = null;

        for (int i = 0, count = operations.Count; i < count; i++) {
          DataOperation operation = operations[i];

          if (operation.DeferExecution == true) {
            continue;
          }

          if (lastSource != operation.DataSource.Source) {
            lastSource = operation.DataSource.Source;

            connection = (IDbConnection) transactions[lastSource];
          }

          counter += DataWriter.Execute(connection, operation);
        }

      }

      return counter;
    }

    #endregion Private methods

  } //class Transaction

} //namespace Empiria.Data
