/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : MetaModel                                        Pattern  : Standard class                    *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Contains meta model data for all BaseObject types.                                            *
*                                                                                                            *
********************************* Copyright (c) 2014-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

using Empiria.Data;

namespace Empiria {

  internal class MetaModel<T> where T : BaseObjectLite {

    #region Delegates

    private delegate T DefaultConstructorDelegate();

    #endregion Delegates

    #region Fields

    private DefaultConstructorDelegate defaultConstructorDelegate;
    private DataModelAttribute dataModel;

    #endregion Fields

    #region Constructors and parsers

    private MetaModel() {
      Type type = typeof(T);
      defaultConstructorDelegate = GetDefaultConstructorDelegate(type);
      var attribute = Attribute.GetCustomAttribute(type, typeof(DataModelAttribute));

      Assertion.AssertObject(attribute, type.FullName + " has not defined the attribute DataModelAttribute.");

      dataModel = (DataModelAttribute) attribute;
    }

    static public MetaModel<T> Parse() {
      return new MetaModel<T>();
    }

    #endregion Constructors and parsers

    #region Properties

    public string DataSource {
      get {
        return dataModel.SourceName;
      }
    }

    public string DataSourceIdField {
      get {
        return dataModel.IdFieldName;
      }
    }

    public string DataSourceKeyField {
      get {
        return dataModel.KeyFieldName;
      }
    }

    public Type UnderlyingType {
      get {
        return typeof(T);
      }
    }

    public bool UseInstancesCache {
      get {
        return !dataModel.NoCache;
      }
    }

    #endregion Properties

    #region Public methods

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

    #endregion Public methods

    #region Private methods

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

    private DataRow TryGetInstanceDataRow(string filter) {
      string sql = "SELECT * FROM {0} WHERE {1}";

      sql = String.Format(sql, this.DataSource, filter);

      return DataReader.GetDataRow(DataOperation.Parse(sql));
    }

    private T InvokeInstanceConstructor(int id) {
      T instance = (T) defaultConstructorDelegate();

      instance.Id = id;

      return instance;
    }

    #endregion Private methods

  }  // class MetaModel

} // namespace Empiria
