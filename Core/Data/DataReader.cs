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
using Empiria.Json;
using Empiria.ORM;
using Empiria.Reflection;

using Empiria.Data.Handlers;

namespace Empiria.Data {

  /// <summary>Static class with methods that performs data reading operations.</summary>
  static public class DataReader {

    #region Methods

    static public int Count(DataOperation operation) {
      Assertion.Require(operation, "operation");

      var dataTable = GetDataTable(operation);

      return dataTable.Rows.Count;
    }


    static public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      Assertion.Require(operation, "operation");
      Assertion.Require(fieldName, "fieldName");

      IDataHandler handler = GetDataHander(operation);

      return handler.GetBinaryFieldValue(operation, fieldName);
    }


    /// <summary>Retrives an IDataReader object giving a table name or stored procedure name.</summary>
    /// <returns>A generic IDataReader interface object.</returns>
    static public IDataReader GetDataReader(DataOperation operation) {
      Assertion.Require(operation, "operation");

      IDataHandler handler = GetDataHander(operation);

      return handler.GetDataReader(operation);
    }


    static public DataRow GetDataRow(DataOperation operation) {
      Assertion.Require(operation, "operation");

      DataTable dataTable = GetDataTable(operation, operation.SourceName);

      if (dataTable.Rows.Count != 0) {
        return dataTable.Rows[0];
      } else {
        return null;
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


    static public DataTable GetDataTable(DataOperation operation, string dataTableName) {
      Assertion.Require(operation, "operation");
      Assertion.Require(dataTableName, "dataTableName");

      IDataHandler handler = GetDataHander(operation);

      return handler.GetDataTable(operation, dataTableName);
    }


    static public DataView GetDataView(DataOperation operation) {
      return DataReader.GetDataView(operation, String.Empty, String.Empty);
    }


    static public DataView GetDataView(DataOperation operation, string filter) {
      return DataReader.GetDataView(operation, filter, String.Empty);
    }


    static public DataView GetDataView(DataOperation operation, string filter, string sort) {
      Assertion.Require(operation, "operation");

      DataTable dataTable = GetDataTable(operation, operation.SourceName);

      return new DataView(dataTable, filter, sort, DataViewRowState.CurrentRows);
    }


    static public dynamic GetDynamicObject(DataRow row, string fieldName) {
      return JsonConverter.ToObject((string) row[fieldName]);
    }


    static public object GetFieldValue(DataOperation operation, string fieldName) {
      Assertion.Require(operation, "operation");
      Assertion.Require(fieldName, "fieldName");

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
      Assertion.Require(operation, "operation");
      Assertion.Require(hashFunction, "hashFunction");

      DataTable dataTable = DataReader.GetDataTable(operation);

      return BaseObject.ParseHashTable<T>(dataTable, hashFunction);
    }


    static public List<T> GetList<T>(DataOperation operation) where T : BaseObject {
      Assertion.Require(operation, "operation");

      DataTable dataTable = DataReader.GetDataTable(operation);

      return BaseObject.ParseList<T>(dataTable);
    }


    static public List<T> GetList<T>(DataOperation operation, Func<DataTable, List<T>> parser) {
      Assertion.Require(operation, "operation");
      Assertion.Require(parser, "parser");

      DataTable dataTable = DataReader.GetDataTable(operation);

      return parser.Invoke(dataTable);
    }


    static public T GetObject<T>(DataOperation operation) where T : BaseObject {
      Assertion.Require(operation, "operation");

      DataRow dataRow = DataReader.GetDataRow(operation);

      if (dataRow == null) {
        Assertion.Require(dataRow, $"dataRow can't be the null instance. Operation: {operation.AsText()}");
      }
      return BaseObject.ParseDataRow<T>(dataRow);
    }


    static public T GetObject<T>(DataOperation operation, T defaultValue) where T : BaseObject {
      Assertion.Require(operation, "operation");

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

      if (dataRow == null) {
        Assertion.Require(dataRow, $"dataRow can't be the null instance. Operation: {operation.AsText()}");
      }

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
      Assertion.Require(operation, "operation");

      IDataHandler handler = GetDataHander(operation);

      return handler.GetScalar(operation);
    }


    static public bool IsEmpty(DataOperation operation) {
      return (Count(operation) == 0);
    }


    static public T ReadValue<T>(DataRow row, string columnName, T defaultValue) {
      if (row[columnName] == null || row[columnName] == DBNull.Value) {
        return defaultValue;
      } else {
        return (T) row[columnName];
      }
    }

    #endregion Methods

    #region Helpers

    static private IDataHandler GetDataHander(DataOperation operation) {
      return operation.DataSource.GetDataHandler();
    }

    #endregion Helpers

  } //class DataReader

} //namespace Empiria.Data
