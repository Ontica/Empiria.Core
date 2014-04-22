/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : Transaction                                      Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Allows the execution of data write operations in a transactional mode. Instances are created  *
*              using the method BeginTransaction on a DataWriterContext object.                              *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Data;
using System.EnterpriseServices;

namespace Empiria.Data {

  /// <summary>Allows the execution of data write operations in a transactional mode.</summary>
  public sealed class Transaction : ITransaction {

    #region Fields

    [ThreadStatic]
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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CommitFails, exception, exception.Message);
      }
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public void Rollback() {
      try {
        if (wasCommited == true) {
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
        }
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

    //internal void Add(DataOperation operation) {
    //  lock(operations) {
    //    operations.Add(operation);
    //  }
    //}

    //internal int Clear() {
    //  int count = operations.Count;

    //  operations.Clear();
    //  return count;
    //}

    //internal void Execute(DataOperationList collection) {
    //  operations = collection;
    //}   

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
        return counter;
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CommitFails,
                                       innerException, innerException.Source);
      } finally {
        CleanResources();
        working = false;
      }
    }

    //internal void RemoveLast(int count) {
    //  if (operations.Count >= count) {
    //    operations.RemoveLast(count);
    //  } else {
    //    throw new EmpiriaDataException(EmpiriaDataException.Msg.DataContextTooManyItemsForRemove);
    //  }
    //}

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
      if (!context.IsInOSTransaction) {
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
      wasCommited = true;
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
        } //if
      } //for
    }

    private void CreateTransactions() {
      for (int i = 0, count = operations.Count; i < count; i++) {
        DataSource dataSource = operations[i].DataSource;
        if (!transactions.ContainsKey(dataSource.Source)) {
          IDbConnection connection = dataSource.GetConnection();
          connection.Open();
          IDbTransaction transaction = connection.BeginTransaction(isolationLevel);
          transactions.Add(dataSource.Source, transaction);
        }
      }
    }

    private void Dispose(bool disposing) {
      if (!disposed) {
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
          Empiria.Messaging.Message message = new Empiria.Messaging.Message(innerException);
          Empiria.Messaging.Publisher.Publish(message);
          throw;
        }// try
      } // if
    }

    private int PrepareTransactions() {
      int counter = 0;
      string lastSource = String.Empty;

      if (!context.IsInOSTransaction) {
        CreateTransactions();
        IDbTransaction transaction = null;
        for (int i = 0, count = operations.Count; i < count; i++) {
          DataOperation operation = operations[i];
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