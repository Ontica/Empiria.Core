/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     License  : Please read LICENSE.txt file      *
*  Type      : DataWriterContext                                Pattern  : Unit of Work                      *
*                                                                                                            *
*  Summary   : Represents a context for run transactional and non transactional data operations in synchro-  *
*              nous or asynchronous mode. Instances are returned by the DataWriter.CreateContext() method.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.Threading;

using Empiria.Data.Integration;
using Empiria.Security;

namespace Empiria.Data {

  /// <summary>Represents a context for run transactional and non transactional data operations in synchronous
  /// or asynchronous mode. Instances are returned by the DataWriter.CreateContext() method. </summary>
  public sealed class DataWriterContext : IUnitOfWork {

    #region Delegates

    private delegate void UpdateMethodDelegate();

    #endregion Delegates

    #region Fields

    private ManualResetEvent transactionCommitEvent = new ManualResetEvent(false);
    private UpdateMethodDelegate updateDelegate = null;
    private IAsyncResult asyncResult = null;

    private DataOperationList internalOp = null;
    private DataOperationList transactionalOp = null;
    private Transaction currentTransaction = null;
    private bool wasUpdated = false;
    private bool disposed = false;

    #endregion Fields

    #region Constructors and parsers

    internal DataWriterContext(string contextName) {
      //Instances of this class needs be constructed with the method GetContext of the class DataWriter
      this.Guid = Guid.NewGuid();
      this.Name = contextName;
      this.Timestamp = DateTime.Now;

      this.internalOp = new DataOperationList(contextName);
      this.transactionalOp = new DataOperationList(contextName);
    }


    ~DataWriterContext() {
      Dispose(false);
    }

    #endregion Constructors and parsers

    #region Public properties

    public int Count {
      get {
        int count = 0;
        if (IsInTransaction) {
          count = transactionalOp.Count;
        }
        count += internalOp.Count;
        return count;
      }
    }


    public string Name {
      get;
    }


    public Guid Guid {
      get;
    }


    public DateTime Timestamp {
      get;
    }


    public bool IsInTransaction {
      get { return (currentTransaction != null); }
    }

    #endregion Public properties

    #region Internal properties

    internal bool AsynchronousUpdate {
      get {
        return (asyncResult != null);
      }
    }


    internal bool IsInOSTransaction {
      get {
        return ContextUtil.IsInTransaction;
      }
    }


    internal ITransaction OSTransaction {
      get { return (ITransaction) ContextUtil.Transaction; }
    }


    internal DataOperationList TransactionalOperations {
      get {
        return transactionalOp;
      }
    }


    internal ManualResetEvent TransactionCommitEvent {
      get {
        return transactionCommitEvent;
      }
    }


    internal bool WasUpdated {
      get {
        return wasUpdated;
      }
    }

    #endregion Internal properties

    #region Public methods

    public void Add(DataOperation operation) {
      if (operation == null) {
        return;
      }
      if (IsInTransaction) {
        transactionalOp.Add(operation);

        if (DataIntegrationRules.HasPublishRule(operation.SourceName)) {
          transactionalOp.Add(DataPublisher.GetPublishOperations(this, operation));
        }

      } else {

        internalOp.Add(operation);

        if (DataIntegrationRules.HasPublishRule(operation.SourceName)) {
          internalOp.Add(DataPublisher.GetPublishOperations(this, operation));
        }

      }
    }


    public void Add(DataOperationList operationList) {
      if (operationList == null) {
        return;
      }

      if (IsInTransaction) {

        foreach (DataOperation operation in operationList) {
          transactionalOp.Add(operation);

          if (DataIntegrationRules.HasPublishRule(operation.SourceName)) {
            internalOp.Add(DataPublisher.GetPublishOperations(this, operation));
          }

        }

      } else {

        foreach (DataOperation operation in operationList) {

          internalOp.Add(operation);

          if (DataIntegrationRules.HasPublishRule(operation.SourceName)) {
            internalOp.Add(DataPublisher.GetPublishOperations(this, operation));
          }
        }

      } // if
    }


    internal void Add(SingleSignOnToken token, DataOperationList operationList) {
      if (operationList == null) {
        return;
      }

      if (IsInTransaction) {

        foreach (DataOperation operation in operationList) {
          transactionalOp.Add(operation);

          if (DataIntegrationRules.HasPublishRule(operation.SourceName)) {
            internalOp.Add(DataPublisher.GetPublishOperations(token, this, operation));
          }

        }

      } else {

        foreach (DataOperation operation in operationList) {
          internalOp.Add(operation);

          if (DataIntegrationRules.HasPublishRule(operation.SourceName)) {
            internalOp.Add(DataPublisher.GetPublishOperations(token, this, operation));
          }
        }

      }  // if
    }


    public ITransaction BeginTransaction() {
      return BeginTransaction(IsolationLevel.ReadCommitted);
    }


    public Transaction BeginTransaction(IsolationLevel isolationLevel) {
      if (IsInTransaction) {
        currentTransaction.Dispose();

        wasUpdated = false;
      }

      Transaction transaction = new Transaction(this, isolationLevel);

      currentTransaction = transaction;

      return transaction;
    }


    public IAsyncResult BeginUpdate(AsyncCallback callback, object state) {
      updateDelegate = new UpdateMethodDelegate(this.Commit);

      wasUpdated = true;

      asyncResult = updateDelegate.BeginInvoke(callback, state);

      return asyncResult;
    }


    public void Close() {
      Dispose(true);

      GC.SuppressFinalize(this);
    }


    public void Commit() {
      try {
        wasUpdated = true;

        if (IsInTransaction && AsynchronousUpdate) {

          if (TransactionCommitEvent.WaitOne(TimeSpan.FromSeconds(2), false)) { // Waits two seconds for transaction.Commit()

            currentTransaction.PerformCommit();

            UpdateInternalOperations();

          } else {
            throw new EmpiriaDataException(EmpiriaDataException.Msg.AsynchronousCommitNotCalled);

          }

        } else {

          UpdateInternalOperations();

        }

      } catch {

        wasUpdated = false;

        throw;

      }
    }

    public void Dispose() {
      Close();
    }


    public void EndTransaction() {
      if (IsInTransaction) {
        transactionalOp.Clear();

        currentTransaction.Dispose();

      } else {

        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataContextOutOfTransaction);

      }
    }


    public void EndUpdate(IAsyncResult asyncResult) {
      wasUpdated = false;

      updateDelegate.EndInvoke(asyncResult);
    }


    public void Rollback() {
      if (IsInTransaction) {
        transactionalOp.Clear();
      }
      internalOp.Clear();
    }

    #endregion Public methods

    #region Private methods

    private void CloseConnections(Dictionary<string, IDbConnection> connections) {

      foreach (IDbConnection connection in connections.Values) {

        if (connection != null) {
          connection.Dispose();
        }

      } //foreach

      connections.Clear();
    }


    private void CreateConnections(Dictionary<string, IDbConnection> connections) {
      string lastSource = String.Empty;

      for (int i = 0, count = internalOp.Count; i < count; i++) {

        DataSource dataSource = internalOp[i].DataSource;

        if (lastSource == dataSource.Source) {
          continue;
        }

        lastSource = dataSource.Source;

        if (!connections.ContainsKey(lastSource)) {
          IDbConnection connection = dataSource.GetConnection();

          connection.Open();

          connections.Add(lastSource, connection);
        }

      }  // for

    }


    private void Dispose(bool disposing) {
      if (disposed) {
        return;
      }

      disposed = true;

      try {

        if (IsInTransaction && AsynchronousUpdate) {
          while (!asyncResult.IsCompleted) {
            // wait until transaction ends
          }
        }

        if (!disposing) {

          if (IsInTransaction) {
            transactionalOp.Clear();

            currentTransaction.Dispose();

            TransactionCommitEvent.Close();

          }

          internalOp.Clear();
        }

      } finally {

        // no-op

      }
    }


    private int UpdateInternalOperations() {
      Dictionary<string, IDbConnection> connections = new Dictionary<string, IDbConnection>();
      string lastSource = String.Empty;
      int counter = 0;

      try {

        CreateConnections(connections);

        IDbConnection connection = null;

        for (int i = 0, count = internalOp.Count; i < count; i++) {

          DataOperation operation = internalOp[i];

          if (operation.DeferExecution == true) {
            continue;
          }

          if (lastSource != operation.DataSource.Source) {

            lastSource = operation.DataSource.Source;

            connection = connections[lastSource];
          }

          counter += DataWriter.Execute(connection, operation);
          internalOp.Remove(operation);
        }

        internalOp.Clear();

      } catch {

        throw;

      } finally {

        CloseConnections(connections);
      }

      return counter;
    }

    #endregion Private methods

  } //class DataWriterContext

} //namespace Empiria.Data
