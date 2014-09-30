/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria                                          Assembly : Empiria.Data.dll                  *
*  Type      : StorageContext                                   Pattern  : Context Class                     *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a context for object storage system transactional operations.                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

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

    private Guid guid = DataWriter.CreateGuid();
    private string name = String.Empty;
    private DateTime timeStamp = DateTime.Now;

    StorageChangeItemList changesList = new StorageChangeItemList();
    DataWriterContext dataWriterContext = null;
    private bool isRemote = false;
    private bool disposed = false;

    #endregion Fields

    #region Constructors and parsers

    private StorageContext() {
      // instances of this class are created using one of the Open() methods
    }

    static public StorageContext Open() {
      StorageContext context = new StorageContext();

      context.name = context.guid.ToString();
      context.dataWriterContext = DataWriter.CreateContext("StorageContext");

      return context;
    }

    static public StorageContext Open(string contextName) {
      StorageContext context = new StorageContext();

      context.name = contextName;
      context.dataWriterContext = DataWriter.CreateContext(contextName);

      return context;
    }

    ~StorageContext() {
      Dispose(false);
    }

    #endregion Constructors and parsers

    #region Public properties

    public Guid Guid {
      get { return guid; }
    }

    public bool IsRemote {
      get { return isRemote; }
    }

    public string Name {
      get { return name; }
    }

    public DateTime TimeStamp {
      get { return timeStamp; }
    }

    #endregion Public properties

    #region Public methods

    public ITransaction BeginTransaction() {
      return dataWriterContext.BeginTransaction();
    }

    public IAsyncResult BeginUpdate(AsyncCallback callback, object state) {
      FillDataWriterContext();
      return dataWriterContext.BeginUpdate(callback, state);
    }

    public void Close() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public void Dispose() {
      Close();
    }

    public void EndTransaction() {
      dataWriterContext.EndTransaction();
    }

    public int EndUpdate(IAsyncResult ayncResult) {
      return dataWriterContext.EndUpdate(ayncResult);
    }

    [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      throw new NotImplementedException();
    }

    public void Save(IStorable storableObject) {
      if (storableObject == null) {
        return;
      }
      changesList.Add(new StorageChangeItem(StorageContextOperation.Save, storableObject));
    }

    public int Update() {
      FillDataWriterContext();
      return dataWriterContext.Update();
    }

    #endregion Public methods

    #region Private methods

    private void Dispose(bool disposing) {
      if (!disposed) {
        disposed = true;
        if (disposing) {
          dataWriterContext.Close();
        }
      }
    }

    private void FillDataWriterContext() {
      foreach (StorageChangeItem changeItem in changesList) {  
      // OOJJOO
      //  dataWriterContext.Add(changeItem.StorableObject.ImplementsStorageUpdate(changeItem.Operation, timeStamp));
      }
    }

    #endregion Private methods

  } // class StorageContext

} // namespace Empiria
