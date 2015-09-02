﻿/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : MetaModel                                        Pattern  : Standard class                    *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Contains meta model data for all BaseObject types.                                            *
*                                                                                                            *
********************************* Copyright (c) 2014-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

using Empiria.Data;

namespace Empiria {

  internal class MetaModel<T> where T : BaseObject {

    #region Delegates

    private delegate T DefaultConstructorDelegate(int id);

    #endregion Delegates

    #region Fields

    private DefaultConstructorDelegate defaultConstructorDelegate;
    private DataModelAttribute dataModel;

    #endregion Fields

    #region Constructors and parsers

    private MetaModel() {
      Type type = typeof(T);
      defaultConstructorDelegate = GetDefaultConstructorDelegate(type);
      dataModel = (DataModelAttribute) Attribute.GetCustomAttribute(type, typeof(DataModelAttribute));
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

    internal T GetInstance(int id) {
      var dataRow = this.GetInstanceDataRow(id);

      T instance = this.InvokeInstanceConstructor(id);

      instance.OnLoadObjectData(dataRow);

      return instance;
    }

    internal T GetInstance(string instanceUID) {
      var dataRow = this.GetInstanceDataRow(instanceUID);

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
      var dataRow = this.TryGetInstanceDataRow(query);

      if (dataRow != null) {
        return this.GetInstance(dataRow);
      } else {
        throw new ResourceNotFoundException(UnderlyingType.Name + ".NotFound",
                                            "An object of type '{0}' was not found using the query {1}.",
                                            UnderlyingType.Name, query);
      }
    }

    internal T TryGetInstance(string key) {
      string query = "{0} = '{1}'";

      var dataRow = this.TryGetInstanceDataRow(String.Format(query, this.DataSourceKeyField, key));

      if (dataRow != null) {
        return this.GetInstance(dataRow);
      } else {
        return null;
      }
    }

    internal T TryGetInstanceWithQuery(string query) {
      var dataRow = this.TryGetInstanceDataRow(query);

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
                                                        new Type[] { typeof(Int32) }, null);

      var dynMethod = new DynamicMethod(type.Name + "Ctor", type, new Type[] { typeof(Int32) },
                                        constructor.Module, true);

      // Generate the intermediate language.
      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Ldarg_0);
      codeGenerator.Emit(OpCodes.Newobj, constructor);
      codeGenerator.Emit(OpCodes.Ret);

      return (DefaultConstructorDelegate) dynMethod.CreateDelegate(typeof(DefaultConstructorDelegate));
    }

    private DataRow GetInstanceDataRow(int id) {
      string sql = "SELECT * FROM {0} WHERE {1} = {2}";

      sql = String.Format(sql, this.DataSource, this.DataSourceIdField, id);

      var dataRow = DataReader.GetDataRow(DataOperation.Parse(sql));

      if (dataRow == null) {
        throw new ResourceNotFoundException(UnderlyingType.Name + ".NotFound",
                                            "An object of type '{0}' with id = {1} was not found.",
                                            UnderlyingType.Name, id);
      }
      return dataRow;
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

    private DataRow TryGetInstanceDataRow(string query) {
      string sql = "SELECT * FROM {0} WHERE {1}";

      sql = String.Format(sql, this.DataSource, query);

      return DataReader.GetDataRow(DataOperation.Parse(sql));
    }

    private T InvokeInstanceConstructor(int id) {
      return (T) defaultConstructorDelegate(id);
    }

    #endregion Private methods

  }  // class MetaModel

} // namespace Empiria