/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : StorageContext                                   Pattern  : Context Class                     *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a context for object storage system transactional operations.                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

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

    public int Add(DataOperation operation) {
      dataOperations.Add(operation);

      return 1;
    }

    public int Add(DataOperationList operationList) {
      dataOperations.Add(operationList);

      return operationList.Count;
    }

    public int Update() {
      if (dataOperations.Count == 0) {
        return 0;
      }
      int operationsCount = 0;
      using (DataWriterContext writerContext = DataWriter.CreateContext(this.Name)) {
        ITransaction transaction = writerContext.BeginTransaction();
        writerContext.Add(dataOperations);
        writerContext.Update();
        operationsCount = transaction.Commit();
      }
      dataOperations.Clear();

      return operationsCount;
    }

    public void Watch(IStorable instance) {

    }

    public void Close() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public void Dispose() {
      Close();
    }

    #endregion Public methods

    #region Private methods

    private bool _disposed = false;
    private void Dispose(bool disposing) {
      if (!_disposed) {
        _disposed = true;
        if (disposing) {
          // no-op
        }
      }
    }

    #endregion Private methods

  } // class StorageContext

} // namespace Empiria
