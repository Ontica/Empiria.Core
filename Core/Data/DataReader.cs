/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Data                               Component : Data Access Layer                       *
*  Assembly : Empiria.Core.dll                           Pattern   : Service provider                        *
*  Type     : DataReader                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static class with methods that performs data reading operations.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Collections;

using Empiria.Data.Handlers;
using Empiria.Data.Integration;

using Empiria.Json;
using Empiria.ORM;
using Empiria.Reflection;
using Empiria.Security;

namespace Empiria.Data {

  /// <summary>Static class with methods that performs data reading operations.</summary>
  static public class DataReader {

    #region Delegates

    static private ReloadDataCallback OnReloadDataCallback = new ReloadDataCallback(OnReloadData);

    #endregion Delegates

    #region Public methods

    static public int Count(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        return GetExternalCount(operation);
      }

      IDataHandler handler = GetDataHander(operation);

      return handler.CountRows(operation);
    }

    static public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      Assertion.AssertObject(operation, "operation");
      Assertion.AssertObject(fieldName, "fieldName");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        //return GetExternalBinaryFieldValue(operation);
        throw new NotImplementedException();
      }

      IDataHandler handler = GetDataHander(operation);

      return handler.GetBinaryFieldValue(operation, fieldName);
    }

    /// <summary>Retrives an IDataReader object giving a table name or stored procedure name.</summary>
    /// <returns>A generic IDataReader interface object.</returns>
    static public IDataReader GetDataReader(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        throw new NotImplementedException();
      }

      IDataHandler handler = GetDataHander(operation);

      return handler.GetDataReader(operation);
    }

    static public DataRow GetDataRow(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        return GetExternalDataRow(operation);
      }

      IDataHandler handler = GetDataHander(operation);

      return handler.GetDataRow(operation);
    }

    static public DataRowView GetDataRowView(DataOperation operation) {
      DataView dataView = GetDataView(operation);

      if (dataView.Count != 0) {
        return dataView[0];
      } else {
        return null;
      }
    }

    static public DataTable GetDataTable(DataOperation operation) {
      return DataReader.GetDataTable(operation, operation.Name);
    }

    static public DataTable GetDataTable(DataOperation operation, DataQuery query) {
      var dataTable = DataReader.GetDataTable(operation, operation.Name);

      return query.ApplyTo(dataTable);
    }

    static public DataTable GetDataTable(DataOperation operation, string dataTableName) {
      Assertion.AssertObject(operation, "operation");
      Assertion.AssertObject(dataTableName, "dataTableName");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        return GetExternalDataTable(operation, dataTableName);
      }

      if (DataIntegrationRules.HasCachingRule(operation.SourceName)) {
        return GetCachedDataTable(operation, dataTableName);
      }

      return GetInternalDataTable(operation, dataTableName);
    }

    static public DataView GetDataView(DataOperation operation) {
      return DataReader.GetDataView(operation, String.Empty, String.Empty);
    }

    static public DataView GetDataView(DataOperation operation, string filter) {
      return DataReader.GetDataView(operation, filter, String.Empty);
    }

    static public DataView GetDataView(DataOperation operation, string filter, string sort) {
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        return GetExternalDataView(operation, filter, sort);
      }

      IDataHandler handler = GetDataHander(operation);

      return handler.GetDataView(operation, filter, sort);
    }


    static public dynamic GetDynamicObject(DataRow row, string fieldName) {
      return JsonConverter.ToObject((string) row[fieldName]);
    }


    static public object GetFieldValue(DataOperation operation, string fieldName) {
      Assertion.AssertObject(operation, "operation");
      Assertion.AssertObject(fieldName, "fieldName");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        return GetExternalFieldValue(operation, fieldName);
      }

      IDataHandler handler = GetDataHander(operation);

      return handler.GetFieldValue(operation, fieldName);
    }

    static public List<T> GetFieldValues<T>(DataOperation operation,
                                            string fieldName = "",
                                            bool filterUniqueValues = true) {
      var view = DataReader.GetDataView(operation);

      if (String.IsNullOrWhiteSpace(fieldName)) {
        fieldName = view.Table.Columns[0].ColumnName;
      }
      List<T> list = new List<T>(view.Count);

      for (int i = 0; i < view.Count; i++) {
        T value = (T) view[i][fieldName];
        if (filterUniqueValues && list.Contains(value)) {
          // no-op
        } else {
          list.Add(value);
        }
      }
      return list;
    }


    static public FixedList<T> GetFixedList<T>(DataOperation operation) where T : BaseObject {
      return GetList<T>(operation).ToFixedList();
    }


    static public EmpiriaHashTable<T> GetHashTable<T>(DataOperation operation,
                                                      Func<T, string> hashFunction) where T : BaseObject {
      Assertion.AssertObject(operation, "operation");
      Assertion.AssertObject(hashFunction, "hashFunction");

      DataTable dataTable = DataReader.GetDataTable(operation);

      return BaseObject.ParseHashTable<T>(dataTable, hashFunction);
    }


    static public List<T> GetList<T>(DataOperation operation) where T : BaseObject {
      Assertion.AssertObject(operation, "operation");

      DataTable dataTable = DataReader.GetDataTable(operation);

      return BaseObject.ParseList<T>(dataTable);
    }


    static public List<T> GetList<T>(DataOperation operation, Func<DataTable, List<T>> parser) {
      Assertion.AssertObject(operation, "operation");
      Assertion.AssertObject(parser, "parser");

      DataTable dataTable = DataReader.GetDataTable(operation);

      return parser.Invoke(dataTable);
    }


    static public T GetObject<T>(DataOperation operation) where T : BaseObject {
      Assertion.AssertObject(operation, "operation");

      DataRow dataRow = DataReader.GetDataRow(operation);

      return BaseObject.ParseDataRow<T>(dataRow);
    }


    static public T GetObject<T>(DataOperation operation, T defaultValue) where T : BaseObject {
      Assertion.AssertObject(operation, "operation");

      DataRow dataRow = DataReader.GetDataRow(operation);

      if (dataRow != null) {
        return BaseObject.ParseDataRow<T>(dataRow);
      } else {
        return defaultValue;
      }
    }


    static public T GetPlainObject<T>(DataOperation operation) {
      var rules = DataMappingRules.Parse(typeof(T));

      DataRow dataRow = DataReader.GetDataRow(operation);

      T instance = ObjectFactory.CreateObject<T>();

      rules.DataBind(instance, dataRow);

      return instance;
    }


    static public T GetPlainObject<T>(DataOperation operation, T defaultValue) {
      var rules = DataMappingRules.Parse(typeof(T));

      DataRow dataRow = DataReader.GetDataRow(operation);

      if (dataRow == null) {
        return defaultValue;
      }

      T instance = ObjectFactory.CreateObject<T>();

      rules.DataBind(instance, dataRow);

      return instance;
    }


    static public EmpiriaHashTable<T> GetPlainObjectHashTable<T>(DataOperation operation,
                                                                 Func<T, string> hashFunction) {
      var rules = DataMappingRules.Parse(typeof(T));

      DataTable dataTable = DataReader.GetDataTable(operation);

      var hashTable = new EmpiriaHashTable<T>(dataTable.Rows.Count);

      foreach (DataRow dataRow in dataTable.Rows) {
        T instance = ObjectFactory.CreateObject<T>();

        rules.DataBind(instance, dataRow);

        hashTable.Insert(hashFunction.Invoke(instance), instance);
      }

      return hashTable;
    }


    static public List<T> GetPlainObjectList<T>(DataOperation operation) {
      return GetPlainObjectList<T>(operation, () => ObjectFactory.CreateObject<T>());
    }


    static public List<T> GetPlainObjectList<T>(DataOperation operation, Func<T> factory) {
      var rules = DataMappingRules.Parse(typeof(T));

      DataTable dataTable = DataReader.GetDataTable(operation);

      var list = new List<T>(dataTable.Rows.Count);

      foreach (DataRow dataRow in dataTable.Rows) {
        T instance = factory.Invoke();

        rules.DataBind(instance, dataRow);

        list.Add(instance);
      }

      return list;
    }


    static public FixedList<T> GetPlainObjectFixedList<T>(DataOperation operation) {
      return GetPlainObjectList<T>(operation).ToFixedList();
    }


    static public FixedList<T> GetPlainObjectFixedList<T>(DataOperation operation, Func<T> factory) {
      return GetPlainObjectList<T>(operation, factory).ToFixedList();
    }


    static public T GetScalar<T>(DataOperation operation, T defaultValue = default(T)) {
      object scalar = GetScalar(operation);

      if (scalar != null && scalar != DBNull.Value) {
        return (T) scalar;
      } else {
        return defaultValue;
      }
    }


    static private object GetScalar(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        return GetExternalScalar(operation);
      }

      IDataHandler handler = GetDataHander(operation);

      return handler.GetScalar(operation);
    }


    static public bool IsEmpty(DataOperation operation) {
      return (Count(operation) == 0);
    }

    static public void Optimize() {
      DataOperation dataOperation = DataOperation.Parse("doOptimization");
      dataOperation.ExecutionTimeout = 200;
      DataWriter.ExecuteInternal(dataOperation);

      string message = "Se ejecutó satisfactoriamente el procedimiento de optimización " +
                       "de índices para todas las bases de datos del sistema.";

      EmpiriaLog.Info(message);
    }

    static public T ReadValue<T>(DataRow row, string columnName, T defaultValue) {
      if (row[columnName] == null || row[columnName] == DBNull.Value) {
        return defaultValue;
      } else {
        return (T) row[columnName];
      }
    }


    #endregion Public methods

    #region Internal methods

    static internal DataTable GetInternalDataTable(DataOperation operation, string dataTableName) {
      IDataHandler handler = GetDataHander(operation);

      return handler.GetDataTable(operation, dataTableName);
    }

    #endregion Internal methods

    #region Private methods

    static private DataTable GetCachedDataTable(DataOperation dataOperation, string dataTableName) {
      DataTable table = DataCache.GetTable(dataTableName);
      if (table == null) {
        lock (DataCache.SyncRoot) {
          table = DataCache.GetTable(dataTableName);
          if (table == null) {
            table = LoadDataTableIntoCache(dataOperation, dataTableName);
          }
        }
      }
      return table;
    }

    static private DataTable LoadDataTableIntoCache(DataOperation dataOperation, string dataTableName) {
      DataTable table = DataReader.GetInternalDataTable(dataOperation, dataTableName);

      DataCache.InsertTable(dataOperation, dataTableName, table,
                            null, new TimeSpan(0, 4, 0, 0), System.Web.Caching.CacheItemPriority.NotRemovable, null);
      return table;
    }

    static private object OnReloadData(DataOperation dataOperation, string dataTableName) {
      return LoadDataTableIntoCache(dataOperation, dataTableName);
    }

    static private IDataHandler GetDataHander(DataOperation operation) {
      return operation.DataSource.GetDataHandler();
    }

    static private int GetExternalCount(DataOperation dataOperation) {
      WebServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.CountData(dataOperation.ToMessage());
      }
    }

    static private DataRow GetExternalDataRow(DataOperation dataOperation) {
      DataTable dataTable = GetExternalDataTable(dataOperation, dataOperation.SourceName);
      if (dataTable.Rows.Count != 0) {
        return dataTable.Rows[0];
      } else {
        return null;
      }
    }

    static private DataTable GetExternalDataTable(DataOperation dataOperation, string dataTableName) {
      WebServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetDataTable(dataOperation.ToMessage(), String.Empty, String.Empty);
      }
    }

    static private DataView GetExternalDataView(DataOperation dataOperation, string filter, string sort) {
      WebServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetDataTable(dataOperation.ToMessage(), filter, sort).DefaultView;
      }
    }

    static private object GetExternalFieldValue(DataOperation dataOperation, string fieldName) {
      WebServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetFieldValue(dataOperation.ToMessage(), fieldName);
      }
    }

    static private object GetExternalScalar(DataOperation dataOperation) {
      WebServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetScalar(dataOperation.ToMessage());
      }
    }

    #endregion Private methods

  } //class DataReader

} //namespace Empiria.Data
