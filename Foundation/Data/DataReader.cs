﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataReader                                       Pattern  : Static Class                      *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static class with methods that performs data reading operations.                              *
*                                                                                                            *
********************************* Copyright (c) 1999-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data.Handlers;
using Empiria.Data.Integration;
using Empiria.Json;
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

      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.CountRows(operation);
        case DataTechnology.MySql:
          return MySqlMethods.CountRows(operation);
        case DataTechnology.OleDb:
          return OleDbMethods.CountRows(operation);
        case DataTechnology.Oracle:
          return OracleMethods.CountRows(operation);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.CountRows(operation);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         operation.DataSource.Technology);
      }
    }

    static public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      Assertion.AssertObject(operation, "operation");
      Assertion.AssertObject(fieldName, "fieldName");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        //return GetExternalBinaryFieldValue(operation);
        throw new NotImplementedException();
      }

      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetBinaryFieldValue(operation, fieldName);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         operation.DataSource.Technology);
      }
    }

    /// <summary>Retrives an IDataReader object giving a table name or stored procedure name.</summary>
    /// <returns>A generic IDataReader interface object.</returns>
    static public IDataReader GetDataReader(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        throw new NotImplementedException();
      }

      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetDataReader(operation);
        case DataTechnology.MySql:
          return MySqlMethods.GetDataReader(operation);
        case DataTechnology.OleDb:
          return OleDbMethods.GetDataReader(operation);
        case DataTechnology.Oracle:
          return OracleMethods.GetDataReader(operation);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.GetDataReader(operation);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         operation.DataSource.Technology);
      }
    }

    static public DataRow GetDataRow(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasReadRule(operation.SourceName)) {
        return GetExternalDataRow(operation);
      }

      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetDataRow(operation);
        case DataTechnology.MySql:
          return MySqlMethods.GetDataRow(operation);
        case DataTechnology.OleDb:
          return OleDbMethods.GetDataRow(operation);
        case DataTechnology.Oracle:
          return OracleMethods.GetDataRow(operation);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.GetDataRow(operation);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         operation.DataSource.Technology);
      }
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

      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetDataView(operation, filter, sort);
        case DataTechnology.MySql:
          return MySqlMethods.GetDataView(operation, filter, sort);
        case DataTechnology.OleDb:
          return OleDbMethods.GetDataView(operation, filter, sort);
        case DataTechnology.Oracle:
          return OracleMethods.GetDataView(operation, filter, sort);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.GetDataView(operation, filter, sort);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         operation.DataSource.Technology);
      }
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

      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetFieldValue(operation, fieldName);
        case DataTechnology.MySql:
          return MySqlMethods.GetFieldValue(operation, fieldName);
        case DataTechnology.OleDb:
          return OleDbMethods.GetFieldValue(operation, fieldName);
        case DataTechnology.Oracle:
          return OracleMethods.GetFieldValue(operation, fieldName);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.GetFieldValue(operation, fieldName);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                  operation.DataSource.Technology);
      }
    }

    static public List<T> GetFieldValues<T>(DataOperation operation,
                                            string fieldName = "", bool filterUniqueValues = true) {
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

    static public List<T> GetList<T>(DataOperation operation, Func<DataTable, List<T>> parser) {
      Assertion.AssertObject(operation, "operation");
      Assertion.AssertObject(parser, "parser");

      DataTable dataTable = DataReader.GetDataTable(operation);

      return parser.Invoke(dataTable);
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

      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetScalar(operation);
        case DataTechnology.MySql:
          return MySqlMethods.GetScalar(operation);
        case DataTechnology.OleDb:
          return OleDbMethods.GetScalar(operation);
        case DataTechnology.Oracle:
          return OracleMethods.GetScalar(operation);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.GetScalar(operation);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                  operation.DataSource.Technology);
      }
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
      switch (operation.DataSource.Technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetDataTable(operation, dataTableName);
        case DataTechnology.MySql:
          return MySqlMethods.GetDataTable(operation, dataTableName);
        case DataTechnology.OleDb:
          return OleDbMethods.GetDataTable(operation, dataTableName);
        case DataTechnology.Oracle:
          return OracleMethods.GetDataTable(operation, dataTableName);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.GetDataTable(operation, dataTableName);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         operation.DataSource.Technology);
      }
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

    static private int GetExternalCount(DataOperation dataOperation) {
      IEmpiriaServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

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
      IEmpiriaServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetDataTable(dataOperation.ToMessage(), String.Empty, String.Empty);
      }
    }

    static private DataView GetExternalDataView(DataOperation dataOperation, string filter, string sort) {
      IEmpiriaServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetDataTable(dataOperation.ToMessage(), filter, sort).DefaultView;
      }
    }

    static private object GetExternalFieldValue(DataOperation dataOperation, string fieldName) {
      IEmpiriaServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetFieldValue(dataOperation.ToMessage(), fieldName);
      }
    }

    static private object GetExternalScalar(DataOperation dataOperation) {
      IEmpiriaServer targetServer = DataIntegrationRules.GetReadRuleServer(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(targetServer)) {
        return proxy.GetScalar(dataOperation.ToMessage());
      }
    }

    #endregion Private methods

  } //class DataReader

} //namespace Empiria.Data