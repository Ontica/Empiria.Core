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

  #region Enumerations

  public enum StorageContextOperation {
    Save = 'S',
    Delete = 'X'
  }

  #endregion Enumerations

  [Serializable]
  public sealed class StorageContext : IUnitOfWork {

    #region Fields

    private static readonly string storageContextKey = "Empiria.DataWriter.StorageContext";

    private Guid guid = DataWriter.CreateGuid();
    private DateTime timestamp = DateTime.Now;
    private string name = String.Empty;

    DataOperationList dataOperations = null;
    DataWriterContext dataWriterContext = null;

    #endregion Fields

    #region Constructors and parsers

    private StorageContext() {
      // Instances of this class are created using one of the static StorageContext.Open() methods.
      this.name = "Context " + guid.ToString().Substring(24);
      this.dataWriterContext = DataWriter.CreateContext(this.name);
      this.dataOperations = new DataOperationList(this.name);
    }

    private StorageContext(string contextName) {
      // Instances of this class are created using one of the static StorageContext.Open() methods.
      this.name = contextName;
      this.dataWriterContext = DataWriter.CreateContext(this.name);
      this.dataOperations = new DataOperationList(this.name);
    }

    internal static StorageContext ActiveStorageContext {
      get {
        return ExecutionServer.ContextItems.GetItem<StorageContext>(storageContextKey);
      }
    }

    internal static bool IsStorageContextDefined {
      get {
        return ExecutionServer.IsAuthenticated &&
               ExecutionServer.ContextItems.ContainsKey(storageContextKey);
      }
    }

    static public StorageContext Open() {
      if (!ExecutionServer.ContextItems.ContainsKey(storageContextKey)) {
        ExecutionServer.ContextItems.SetItem(storageContextKey, new StorageContext());
      }
      return ExecutionServer.ContextItems.GetItem<StorageContext>(storageContextKey);
    }

    static public StorageContext Open(string contextName) {
      if (!IsStorageContextDefined) {
        ExecutionServer.ContextItems.SetItem(storageContextKey, new StorageContext(contextName));
      }
      return ExecutionServer.ContextItems.GetItem<StorageContext>(storageContextKey);
    }

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


    public void Update() {
      if (dataOperations.Count == 0) {
        return;
      }

      using (DataWriterContext writerContext = DataWriter.CreateContext(this.Name)) {
        ITransaction transaction = writerContext.BeginTransaction();

        writerContext.Add(dataOperations);

        writerContext.Update();

        transaction.Commit();
      }

      dataOperations.Clear();
    }


    public void Watch(IStorable instance) {

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
      var systemOperations = dataOperations.Where((x) => x.IsSystemOperation)
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
