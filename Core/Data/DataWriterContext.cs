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

using Empiria.Security;

namespace Empiria.Data {

  /// <summary>Represents a context for run transactional and non transactional data operations in synchronous
  /// or asynchronous mode. Instances are returned by the DataWriter.CreateContext() method. </summary>
  public sealed class DataWriterContext : IUnitOfWork {

    #region Fields

    private readonly DataOperationList internalOp = null;
    private readonly DataOperationList transactionalOp = null;

    private Transaction currentTransaction = null;

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
        int count = internalOp.Count;

        if (IsInTransaction) {
          count += transactionalOp.Count;
        }

        return count;
      }
    }


    public bool IsInTransaction {
      get {
        return currentTransaction != null;
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


    internal DataOperationList TransactionalOperations {
      get {
        return transactionalOp;
      }
    }

    #endregion Public properties

    #region Public methods

    public void Add(DataOperation operation) {
      if (operation == null) {
        return;
      }

      if (IsInTransaction) {
        transactionalOp.Add(operation);
      } else {
        internalOp.Add(operation);
      }
    }


    public void Add(DataOperationList operationList) {
      if (operationList == null) {
        return;
      }

      foreach (DataOperation operation in operationList) {
        if (IsInTransaction) {
          transactionalOp.Add(operation);
        } else {
          internalOp.Add(operation);
        }
      }
    }


    public ITransaction BeginTransaction() {
      return BeginTransaction(IsolationLevel.ReadCommitted);
    }


    public Transaction BeginTransaction(IsolationLevel isolationLevel) {
      if (IsInTransaction) {
        throw new Exception("Another transaction was started." +
                            "DataWriterContext can't handle nested transactions.");
      }

      Transaction transaction = new Transaction(this, isolationLevel);

      currentTransaction = transaction;

      return transaction;
    }


    public void Close() {
      Dispose(true);
    }


    public void Update() {
      var connections = new Dictionary<string, IDbConnection>();

      try {

        CreateConnections(connections);

        IDbConnection connection = null;

        string lastSource = String.Empty;

        foreach (var operation in internalOp) {

          if (lastSource != operation.DataSource.Source) {

            lastSource = operation.DataSource.Source;

            connection = connections[lastSource];
          }

          DataWriter.Execute(connection, operation);

          internalOp.Remove(operation);
        }

        internalOp.Clear();

      } finally {
        CloseConnections(connections);

      }
    }

    public void Dispose() {
      Close();
    }


    public void EndTransaction() {
      if (!IsInTransaction) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataContextOutOfTransaction);
      }

      transactionalOp.Clear();

      currentTransaction.Dispose();
    }


    public void Clear() {
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


    private bool _disposed = false;

    private void Dispose(bool disposing) {
      if (_disposed) {
        return;
      }

      _disposed = true;

      try {
        if (!disposing) {

          if (IsInTransaction) {
            transactionalOp.Clear();

            currentTransaction.Dispose();
          }

          internalOp.Clear();
        }

      } finally {

        // no-op

      }
    }

    #endregion Private methods

  } //class DataWriterContext

} //namespace Empiria.Data
