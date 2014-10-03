/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataWriterContext                                Pattern  : Unit of Work                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a context for run transactional and non transactional data operations in synchro-  *
*              nous or asynchronous mode. Instances are returned by the DataWriter.CreateContext() method.   *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    private delegate int UpdateMethodDelegate();

    #endregion Delegates

    #region Fields

    private ManualResetEvent transactionCommitEvent = new ManualResetEvent(false);
    private UpdateMethodDelegate updateDelegate = null;
    private IAsyncResult asyncResult = null;

    private Guid guid = Guid.NewGuid();
    private string name = String.Empty;
    private DateTime timestamp = DateTime.Now;

    private DataOperationList internalOp = null;
    private DataOperationList transactionalOp = null;
    private Transaction currentTransaction = null;
    private bool wasUpdated = false;
    private bool disposed = false;

    #endregion Fields

    #region Constructors and parsers

    internal DataWriterContext(string contextName) {
      //Instances of this class needs be constructed with the method GetContext of the class DataWriter
      this.name = contextName;
      this.internalOp = new DataOperationList(contextName);
      this.transactionalOp = new DataOperationList(contextName);
    }

    static internal DataWriterContext Parse(Guid guid, string name) {
      DataWriterContext context = new DataWriterContext(name);
      context.guid = guid;
      return context;
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
      get { return name; }
    }

    public Guid Guid {
      get { return guid; }
    }

    public bool IsInTransaction {
      get { return (currentTransaction != null); }
    }

    public DateTime Timestamp {
      get { return timestamp; }
    }

    #endregion Public properties

    #region Internal properties

    internal bool AsynchronousUpdate {
      get { return updateDelegate != null; }
    }

    internal bool IsInOSTransaction {
      get { return ContextUtil.IsInTransaction; }
    }

    internal ITransaction OSTransaction {
      get { return (ITransaction) ContextUtil.Transaction; }
    }

    internal DataOperationList TransactionalOperations {
      get { return transactionalOp; }
    }

    internal ManualResetEvent TransactionCommitEvent {
      get { return transactionCommitEvent; }
    }

    internal bool WasUpdated {
      get { return wasUpdated; }
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
      }
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
      }
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
      updateDelegate = new UpdateMethodDelegate(this.Update);
      wasUpdated = true;
      asyncResult = updateDelegate.BeginInvoke(callback, state);
      return asyncResult;
    }

    public int Clear() {
      int count = 0;

      if (IsInTransaction) {
        count = transactionalOp.Count;
        transactionalOp.Clear();
      } else {
        count = internalOp.Count;
        internalOp.Clear();
      }
      return count;
    }

    public int ClearAll() {
      int count = 0;

      if (IsInTransaction) {
        count = transactionalOp.Count;
        transactionalOp.Clear();
      }
      count += internalOp.Count;
      internalOp.Clear();
      return count;
    }

    public void Close() {
      Dispose(true);
      GC.SuppressFinalize(this);
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

    public int EndUpdate(IAsyncResult asyncResult) {
      wasUpdated = false;
      return updateDelegate.EndInvoke(asyncResult);
    }

    public void RemoveLast(int count) {
      int toRemove = count;

      if (this.Count < toRemove) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataContextTooManyItemsForRemove);
      }
      if (IsInTransaction) {
        if (transactionalOp.Count > toRemove) {
          transactionalOp.RemoveLast(toRemove);
          return;
        } else if (transactionalOp.Count == toRemove) {
          transactionalOp.Clear();
          return;
        } else if (transactionalOp.Count < toRemove) {
          toRemove = toRemove - transactionalOp.Count;
          transactionalOp.Clear();
        }
      }
      if (internalOp.Count > toRemove) {
        internalOp.RemoveLast(toRemove);
        return;
      } else if (internalOp.Count == toRemove) {
        internalOp.Clear();
        return;
      } else if (internalOp.Count < toRemove) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataContextTooManyItemsForRemove);
      }
    }

    public int Update() {
      try {
        wasUpdated = true;
        if (IsInTransaction && AsynchronousUpdate) {
          int count = 0;
          if (TransactionCommitEvent.WaitOne(TimeSpan.FromSeconds(2), false)) { //Waits two seconds for transaction.Commit()
            count = currentTransaction.PerformCommit();
            count += UpdateInternalOperations();
          } else {
            throw new EmpiriaDataException(EmpiriaDataException.Msg.AsynchronousCommitNotCalled);
          }
          return count;
        } else {
          return UpdateInternalOperations();
        }
      } catch {
        wasUpdated = false;
        throw;
      }
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
        if (lastSource != dataSource.Source) {
          lastSource = dataSource.Source;
          if (!connections.ContainsKey(lastSource)) {
            IDbConnection connection = dataSource.GetConnection();
            connection.Open();
            connections.Add(lastSource, connection);
          }
        } //if
      } //for
    }

    private void Dispose(bool disposing) {
      if (!disposed) {
        disposed = true;
        try {
          if (IsInTransaction && AsynchronousUpdate) {
            while (!asyncResult.IsCompleted) {
              // wait until transaction ends
            }
          }
          if (disposing) {
            if (IsInTransaction) {
              transactionalOp.Clear();
              currentTransaction.Dispose();
              TransactionCommitEvent.Close();
            }
            internalOp.Clear();
          }
        } finally {

        } //try
      } // if
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
          if (lastSource != operation.DataSource.Source) {
            lastSource = operation.DataSource.Source;
            connection = connections[lastSource];
          }
          counter += DataWriter.Execute(connection, operation);
        }
        internalOp.Clear();
      } catch {
        internalOp.RemoveRange(0, counter);
        throw;
      } finally {
        CloseConnections(connections);
      }
      return counter;
    }

    #endregion Private methods

  } //class DataWriterContext

} //namespace Empiria.Data
