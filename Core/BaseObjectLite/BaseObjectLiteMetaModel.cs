/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : MetaModel                                        Pattern  : Standard class                    *
*                                                                                                            *
*  Summary   : Contains meta model data for all BaseObject types.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

using Empiria.Data;

namespace Empiria {

  internal class BaseObjectLiteMetaModel<T> where T : BaseObjectLite {

    #region Delegates

    private delegate T DefaultConstructorDelegate();

    #endregion Delegates

    #region Fields

    private readonly DefaultConstructorDelegate _defaultConstructorDelegate;
    private readonly DataModelAttribute _dataModel;

    #endregion Fields

    #region Constructors and parsers

    private BaseObjectLiteMetaModel() {
      Type type = typeof(T);

      _defaultConstructorDelegate = GetDefaultConstructorDelegate(type);

      var attribute = Attribute.GetCustomAttribute(type, typeof(DataModelAttribute));

      Assertion.Require(attribute, type.FullName + " has not defined the attribute DataModelAttribute.");

      _dataModel = (DataModelAttribute) attribute;
    }


    static public BaseObjectLiteMetaModel<T> Parse() {
      return new BaseObjectLiteMetaModel<T>();
    }

    #endregion Constructors and parsers

    #region Properties

    public string DataSource {
      get {
        return _dataModel.SourceName;
      }
    }


    public string DataSourceIdField {
      get {
        return _dataModel.IdFieldName;
      }
    }


    public string DataSourceKeyField {
      get {
        return _dataModel.KeyFieldName;
      }
    }


    public Type UnderlyingType {
      get {
        return typeof(T);
      }
    }


    public bool UseInstancesCache {
      get {
        return !_dataModel.NoCache;
      }
    }

    #endregion Properties

    #region Methods

    internal FixedList<T> GetFixedList(string sql) {
      var dataTable = DataReader.GetDataTable(DataOperation.Parse(sql));

      List<T> list = new List<T>(dataTable.Rows.Count);

      for (int i = 0; i < dataTable.Rows.Count; i++) {
        DataRow dataRow = dataTable.Rows[i];
        T instance = this.InvokeInstanceConstructor((int) dataRow[this.DataSourceIdField]);

        instance.OnLoadObjectData(dataRow);

        list.Add(instance);
      }
      return list.ToFixedList();
    }


    internal T GetInstance(int id) {
      DataRow dataRow = this.TryGetInstanceDataRow(id);

      if (dataRow == null) {
        throw new ResourceNotFoundException(UnderlyingType.Name + ".NotFound",
                                            "An object of type '{0}' with id = {1} was not found.",
                                            UnderlyingType.Name, id);
      }

      T instance = this.InvokeInstanceConstructor(id);

      instance.OnLoadObjectData(dataRow);

      return instance;
    }


    internal T GetInstance(string instanceUID) {
      DataRow dataRow = this.GetInstanceDataRow(instanceUID);

      T instance = this.InvokeInstanceConstructor((int) dataRow[this.DataSourceIdField]);

      instance.OnLoadObjectData(dataRow);

      return instance;
    }


    internal T GetInstance(DataOperation dataOperation) {
      DataRow dataRow = this.GetInstanceDataRow(dataOperation);

      T instance = this.InvokeInstanceConstructor((int) dataRow[this.DataSourceIdField]);

      instance.OnLoadObjectData(dataRow);

      return instance;
    }


    internal T GetInstance(DataRow dataRow) {
      int id = (int) dataRow[DataSourceIdField];

      T instance = this.InvokeInstanceConstructor(id);

      instance.OnLoadObjectData(dataRow);

      return instance;
    }


    internal T GetInstanceWithQuery(string query) {
      DataRow dataRow = this.TryGetInstanceDataRow(query);

      if (dataRow != null) {
        return this.GetInstance(dataRow);
      } else {
        throw new ResourceNotFoundException(UnderlyingType.Name + ".NotFound",
                                            "An object of type '{0}' was not found using the query {1}.",
                                            UnderlyingType.Name, query);
      }
    }


    internal int GetNextInstanceId() {
      return DataWriter.CreateId(this.DataSource);
    }


    internal T TryGetInstance(int id) {
      DataRow dataRow = this.TryGetInstanceDataRow(id);

      if (dataRow == null) {
        return null;
      }

      T instance = this.InvokeInstanceConstructor(id);

      instance.OnLoadObjectData(dataRow);

      return instance;
    }


    internal T TryGetInstance(string key) {
      string filter = "{0} = '{1}'";

      var dataRow = this.TryGetInstanceDataRow(String.Format(filter, this.DataSourceKeyField, key));

      if (dataRow != null) {
        return this.GetInstance(dataRow);
      } else {
        return null;
      }
    }


    internal T TryGetInstanceWithFilter(string filter) {
      DataRow dataRow = this.TryGetInstanceDataRow(filter);

      if (dataRow != null) {
        return this.GetInstance(dataRow);
      } else {
        return null;
      }
    }

    #endregion Methods

    #region Helpers

    private DefaultConstructorDelegate GetDefaultConstructorDelegate(Type type) {
      ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                        BindingFlags.NonPublic, null, CallingConventions.HasThis,
                                                        new Type[0], null);

      var dynMethod = new DynamicMethod(type.Name + "Ctor", type, null, constructor.Module, true);

      // Generate the intermediate language.
      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Newobj, constructor);
      codeGenerator.Emit(OpCodes.Ret);

      return (DefaultConstructorDelegate) dynMethod.CreateDelegate(typeof(DefaultConstructorDelegate));
    }


    private DataRow TryGetInstanceDataRow(int id) {
      string sql = "SELECT * FROM {0} WHERE {1} = {2}";

      sql = String.Format(sql, this.DataSource, this.DataSourceIdField, id);

      return DataReader.GetDataRow(DataOperation.Parse(sql));
    }


    private DataRow GetInstanceDataRow(string instanceUID) {
      string sql = "SELECT * FROM {0} WHERE {1} = '{2}'";

      sql = String.Format(sql, this.DataSource, this.DataSourceKeyField, instanceUID);

      var dataRow = DataReader.GetDataRow(DataOperation.Parse(sql));

      if (dataRow == null) {
        throw new ResourceNotFoundException(UnderlyingType.Name + ".NotFound",
                                            "An object of type '{0}' with Unique ID = '{1}' was not found.",
                                            UnderlyingType.Name, instanceUID);
      }
      return dataRow;
    }


    private DataRow GetInstanceDataRow(DataOperation dataOperation) {
      DataRow dataRow = DataReader.GetDataRow(dataOperation);

      if (dataRow == null) {
        throw new ResourceNotFoundException(UnderlyingType.Name + ".NotFound",
                                            "An object of type '{0}' was not found using data operation '{1}'.",
                                            UnderlyingType.Name, dataOperation.Name);
      }
      return dataRow;
    }


    private DataRow TryGetInstanceDataRow(string filter) {
      string sql = "SELECT * FROM {0} WHERE {1}";

      sql = String.Format(sql, this.DataSource, filter);

      return DataReader.GetDataRow(DataOperation.Parse(sql));
    }


    private T InvokeInstanceConstructor(int id) {
      T instance = (T) _defaultConstructorDelegate();

      instance.Id = id;

      return instance;
    }

    #endregion Helpers

  }  // class BaseObjectLiteMetaModel

} // namespace Empiria
