/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataCache                                        Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Central repository to persist data using the caching mechanism of ASP .NET.                   *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Data;
using System.Web.Caching;

using Empiria.Data.Integration;
using Empiria.Messaging;

namespace Empiria.Data {

  #region Delegates

  public delegate object ReloadDataCallback(DataOperation dataOperation, string dataTableName);

  #endregion Delegates

  /// <summary>Central repository to persist data using the caching mechanism of ASP .NET.</summary>
  static public class DataCache {

    #region Fields

    static private readonly int placeBackExpirationMinutes = ConfigurationData.GetInteger("DataCache.PlaceBackExpirationMinutes");
    private const string keyPrefix = "empiria.data.data_cache.";

    static private CacheItemRemovedCallback onCacheItemRemoved = new CacheItemRemovedCallback(RemovedCallback);
    static private AsyncCallback onRestoreRemovedObject = new AsyncCallback(RestoreRemovedObjectCallback);

    #endregion Fields

    #region Public methods

    static public Cache SyncRoot {
      get { return System.Web.HttpRuntime.Cache; }
    }

    static private Cache SystemCache {
      get { return System.Web.HttpRuntime.Cache; }
    }

    static public void Clear() {
      lock (SystemCache) {
        IDictionaryEnumerator enumerator = SystemCache.GetEnumerator();

        while (enumerator.MoveNext()) {
          string key = enumerator.Key.ToString();
          if (key.StartsWith(keyPrefix)) {
            SystemCache.Remove(key);
          }
        }
      }
    }

    static public bool Contains(string key) {
      return (SystemCache.Get(keyPrefix + key) != null);
    }

    static public object GetObject(string key) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + key) as CachedObject;
      if (cachedObject != null) {
        return cachedObject.CachedValue;
      } else {
        return null;
      }
    }

    static public DataTable GetTable(string dataTableName) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + dataTableName) as CachedObject;
      if (cachedObject != null) {
        return (DataTable) cachedObject.CachedValue;
      } else {
        return null;
      }
    }

    static public DataView GetView(string dataTableName) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + dataTableName) as CachedObject;
      if (cachedObject != null) {
        return new DataView((DataTable) cachedObject.CachedValue, String.Empty, String.Empty,
                            DataViewRowState.CurrentRows);
      } else {
        return null;
      }
    }

    static public DataView GetView(string dataTableName, DataViewRowState rowState) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + dataTableName) as CachedObject;
      if (cachedObject != null) {
        return new DataView((DataTable) cachedObject.CachedValue, String.Empty, String.Empty, rowState);
      } else {
        return null;
      }
    }

    static public DataView GetView(string dataTableName, string filter) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + dataTableName) as CachedObject;
      if (cachedObject != null) {
        return new DataView((DataTable) cachedObject.CachedValue, filter, String.Empty,
                             DataViewRowState.CurrentRows);
      } else {
        return null;
      }
    }

    static public DataView GetView(string dataTableName, string filter, DataViewRowState rowState) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + dataTableName) as CachedObject;
      if (cachedObject != null) {
        return new DataView((DataTable) cachedObject.CachedValue, filter, String.Empty, rowState);
      } else {
        return null;
      }
    }

    static public DataView GetView(string dataTableName, string filter, string sort) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + dataTableName) as CachedObject;
      if (cachedObject != null) {
        return new DataView((DataTable) cachedObject.CachedValue, filter, sort, DataViewRowState.CurrentRows);
      } else {
        return null;
      }
    }

    static public DataView GetView(string dataTableName, string filter, string sort, DataViewRowState rowState) {
      CachedObject cachedObject = SystemCache.Get(keyPrefix + dataTableName) as CachedObject;
      if (cachedObject != null) {
        return new DataView((DataTable) cachedObject.CachedValue, filter, sort, rowState);
      } else {
        return null;
      }
    }

    static public object Remove(string key) {
      object removedObject = null;

      lock (SystemCache) {
        removedObject = SystemCache.Remove(keyPrefix + key);

        if (ExecutionServer.IsWebServicesServer()) {
          DataIntegratorWSProxy.SynchronizeServerCaches(key);
        }
      }
      return removedObject;
    }

    static public void SetDataTableRow(string key, DataRow refreshedRow, string filterExpression) {
      DataTable cachedDataTable = GetTable(key);
      if (cachedDataTable == null) {
        return;
      }

      lock (SystemCache) {
        DataRow[] dataCachedRows = cachedDataTable.Select(filterExpression);

        if (dataCachedRows.Length != 0) {
          dataCachedRows[0].BeginEdit();
          for (int i = 0; i < dataCachedRows[0].Table.Columns.Count; i++) {
            dataCachedRows[0][i] = refreshedRow[i];
          }
          dataCachedRows[0].EndEdit();
        } else {
          cachedDataTable.ImportRow(refreshedRow);
        }
      }
    }

    #endregion Public methods

    #region Internal methods

    static internal CachedObject InsertTable(DataOperation dataOperation, string tableName,
                                             DataTable table, string[] dependencies,
                                             DateTime absoluteExpiration, CacheItemPriority priority,
                                             ReloadDataCallback reloadDataCallback) {
      CachedObject cachedObject = CachedObject.Parse(dataOperation, tableName,
                                                     table, dependencies, absoluteExpiration,
                                                     Cache.NoSlidingExpiration, priority, reloadDataCallback);

      InsertCachedObject(cachedObject);

      return (CachedObject) SystemCache.Get(keyPrefix + cachedObject.TableName);
    }

    static internal CachedObject InsertTable(DataOperation dataOperation, string tableName,
                                             DataTable table, string[] dependencies,
                                             TimeSpan slidingExpiration, CacheItemPriority priority,
                                             ReloadDataCallback reloadDataCallback) {
      CachedObject cachedObject = CachedObject.Parse(dataOperation, tableName,
                                                     table, dependencies, Cache.NoAbsoluteExpiration,
                                                     slidingExpiration, priority, reloadDataCallback);

      InsertCachedObject(cachedObject);

      return (CachedObject) SystemCache.Get(keyPrefix + cachedObject.TableName);
    }

    #endregion Internal methods

    #region Private methods

    static private void InsertCachedObject(CachedObject cachedObject) {
      if (cachedObject.UseSlidingExpiration) {
        SystemCache.Insert(keyPrefix + cachedObject.TableName, cachedObject, cachedObject.Dependencies,
                           Cache.NoAbsoluteExpiration, cachedObject.SlidingExpiration,
                           cachedObject.Priority, onCacheItemRemoved);
      } else {
        SystemCache.Insert(keyPrefix + cachedObject.TableName, cachedObject, cachedObject.Dependencies,
                          cachedObject.AbsoluteExpiration, Cache.NoSlidingExpiration, cachedObject.Priority,
                          onCacheItemRemoved);
      }
    }

    static private void PlaceBackRemovedObject(string key, object removedObject) {
      SystemCache.Insert(keyPrefix + key, removedObject, null,
                         DateTime.Now.AddSeconds(placeBackExpirationMinutes),
                         Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
    }

    static private void RemovedCallback(string key, object _object, CacheItemRemovedReason reason) {
      CachedObject removedObject = (CachedObject) _object;

      string message = "Limpieza o eliminación de objetos de la caché del sistema.";
      message += System.Environment.NewLine;
      message += System.Environment.NewLine;
      message += "El objeto con clave " + key.Replace(keyPrefix, String.Empty);
      message += " fue removido de la caché del sistema, ya que se cumplió la regla " + reason.ToString();

      Publisher.Publish(new Message(MessageType.EventMessage, message));

      //  switch (reason) {
      //    case CacheItemRemovedReason.Removed:
      //      return;
      //    case CacheItemRemovedReason.DependencyChanged:
      //      PlaceBackRemovedObject(key, removedObject);
      //      removedObject.ReloadCallback.BeginInvoke(removedObject.DataOperation, removedObject.TableName,
      //                                              onRestoreRemovedObject, removedObject);
      //      return;
      //    case CacheItemRemovedReason.Expired:
      //      PlaceBackRemovedObject(key, removedObject);
      //      removedObject.ReloadCallback.BeginInvoke(removedObject.DataOperation, removedObject.TableName,
      //                                              onRestoreRemovedObject, removedObject);
      //      return;
      //    case CacheItemRemovedReason.Underused:
      //      break;
      //    default:
      //      throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidCacheItemRemovedReason, reason.ToString());
      //  }
    }

    static public void RestoreRemovedObjectCallback(IAsyncResult ar) {
      if (ar == null) {
        return;
      }
      try {
        CachedObject cachedObject = (CachedObject) ar.AsyncState;
        ReloadDataCallback reloadDataCallback = cachedObject.ReloadCallback;
        object restoredObject = reloadDataCallback.EndInvoke(ar);
        cachedObject.CachedValue = restoredObject;
        InsertCachedObject(cachedObject);
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CacheRestoreRemovedObjectCallbackFails,
                                       innerException);
      }
    }

    #endregion Private methods

  } //class DataCache

} //namespace Empiria.Data
