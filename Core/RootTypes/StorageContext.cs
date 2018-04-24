/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Storage Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : StorageContext                                   Pattern  : Context Class                     *
*                                                                                                            *
*  Summary   : Represents a context for object storage system transactional operations.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;

using Empiria.Data;

namespace Empiria {

  [Serializable]
  public sealed class StorageContext : IUnitOfWork {

    #region Fields

    private static readonly string STORAGE_CONTEXT_KEY = "Empiria.DataWriter.StorageContext";

    private Guid guid = DataWriter.CreateGuid();
    private DateTime timestamp = DateTime.Now;
    private string name = String.Empty;

    DataOperationList dataOperations = null;

    #endregion Fields

    #region Constructors and parsers

    private StorageContext() {
      // Instances of this class are created using one of the static StorageContext.Open() methods.
      this.name = "Context " + guid.ToString().Substring(24);

      this.dataOperations = new DataOperationList(this.name);
    }

    private StorageContext(string contextName) {
      // Instances of this class are created using one of the static StorageContext.Open() methods.
      this.name = contextName;

      this.dataOperations = new DataOperationList(this.name);
    }

    internal static StorageContext ActiveStorageContext {
      get {
        return ExecutionServer.ContextItems.GetItem<StorageContext>(STORAGE_CONTEXT_KEY);
      }
    }

    internal static bool IsStorageContextDefined {
      get {
        return ExecutionServer.IsAuthenticated &&
               ExecutionServer.ContextItems.ContainsKey(STORAGE_CONTEXT_KEY);
      }
    }

    static public StorageContext Open() {
      if (!IsStorageContextDefined) {
        ExecutionServer.ContextItems.SetItem(STORAGE_CONTEXT_KEY, new StorageContext());
      }
      return ExecutionServer.ContextItems.GetItem<StorageContext>(STORAGE_CONTEXT_KEY);
    }

    //static public StorageContext Open(string contextName) {
    //  if (!IsStorageContextDefined) {
    //    ExecutionServer.ContextItems.SetItem(STORAGE_CONTEXT_KEY, new StorageContext(contextName));
    //  }
    //  return ExecutionServer.ContextItems.GetItem<StorageContext>(STORAGE_CONTEXT_KEY);
    //}

    ~StorageContext() {
      Dispose(false);
    }

    #endregion Constructors and parsers

    #region Public properties

    public Guid Guid {
      get {
        return guid;
      }
    }

    public string Name {
      get {
        return name;
      }
    }

    public DateTime Timestamp {
      get {
        return timestamp;
      }
    }

    #endregion Public properties

    #region Public methods

    internal void Add(DataOperation operation) {
      dataOperations.Add(operation);
    }


    internal void Add(DataOperationList operationList) {
      dataOperations.Add(operationList);
    }


    internal static void EnsureIsStorageContextDefined() {
      Assertion.Assert(StorageContext.IsStorageContextDefined,
                       new Exception("Programming Error: An opened StorageContext is required."));
    }


    public void Commit() {
      if (dataOperations.Count == 0) {
        return;
      }

      using (DataWriterContext writerContext = DataWriter.CreateContext(this.Name)) {
        ITransaction transaction = writerContext.BeginTransaction();

        var operationsToExecute = new DataOperationList(this.Name);

        operationsToExecute.Add(dataOperations.Where((x) => x.DeferExecution == false));

        writerContext.Add(operationsToExecute);

        writerContext.Commit();

        try {
          transaction.Commit();

          this.dataOperations.RemoveAll( (x) => operationsToExecute.Contains(x) );

        } catch {
          transaction.Rollback();

          throw;
        }
      }
    }


    public void Rollback() {
      dataOperations.Clear();
    }


    public void Watch(IIdentifiable instance) {

    }


    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion Public methods

    #region Private methods

    private bool _disposed = false;
    private void Dispose(bool disposing) {
      if (_disposed) {
        return;
      }
      if (disposing) {
        TryDumpPendingSystemDataOperations();
      }

      // Free unmanaged
      _disposed = true;
    }

    private void TryDumpPendingSystemDataOperations() {
      if (dataOperations.Count == 0) {
        return;
      }
      var systemOperations = dataOperations.Where((x) => x.IsSystemOperation && x.DeferExecution == false)
                                           .ToList();

      if (systemOperations == null || systemOperations.Count == 0) {
        return;
      }

      var dumpedList = new DataOperationList("DumpedSystemOperations");

      dumpedList.Add(systemOperations);

      DataWriter.ExecuteInternal(dumpedList);
    }

    #endregion Private methods

  } // class StorageContext

} // namespace Empiria
