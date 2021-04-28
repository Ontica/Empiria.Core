/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Core                                     System  : Data Access Library                 *
*  Assembly : Empiria.Core.dll                                 Pattern : Information Holder (with cache)     *
*  Type     : DataSource                                       License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents a data source formed by the source or connection string and the data technology.    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Collections;
using Empiria.Data.Handlers;

namespace Empiria.Data {

  /// <summary>Represents a data source formed by the source or connection string and
  /// the data technology.</summary>
  public struct DataSource {

    #region Fields

    static private EmpiriaDictionary<string, DataSource> sourcesCache =
                                                                new EmpiriaDictionary<string, DataSource>(8);

    static private EmpiriaDictionary<string, IDataHandler> handlersCache =
                                                                new EmpiriaDictionary<string, IDataHandler>(4);

    private readonly string name;
    private readonly string source;
    private readonly DataTechnology technology;

    #endregion Fields

    #region Constructors and parsers

    private DataSource(string dataSourceName) {
      this.name = dataSourceName;
      this.source = GetSource(dataSourceName);
      this.technology = GetDataTechnology(dataSourceName);
    }

    static internal DataSource Parse(string sourceName) {
      string dataSourceName = GetDataSourceName(sourceName);

      if (!sourcesCache.ContainsKey(dataSourceName)) {
        var dataSource = new DataSource(dataSourceName);
        sourcesCache.Insert(dataSourceName, dataSource);
      }
      return sourcesCache[dataSourceName];
    }

    internal IDataHandler GetDataHandler() {
      if (handlersCache.ContainsKey(this.Source)) {
        return handlersCache[this.Source];
      }

      IDataHandler handler;
      Type type;

      switch (this.Technology) {
        case DataTechnology.SqlServer:
          handler = new SqlMethods();
          break;

        case DataTechnology.MySql:
          type = Reflection.ObjectFactory.GetType("Empiria.Data.MySql", "Empiria.Data.Handlers.MySqlMethods");

          handler = (IDataHandler) Reflection.ObjectFactory.CreateObject(type);
          break;

        case DataTechnology.OleDb:
          handler = new OleDbMethods();
          break;

        case DataTechnology.Oracle:
          type = Reflection.ObjectFactory.GetType("Empiria.Data.Oracle", "Empiria.Data.Handlers.OracleMethods");

          handler = (IDataHandler) Reflection.ObjectFactory.CreateObject(type);
          break;

        case DataTechnology.PostgreSql:
          type = Reflection.ObjectFactory.GetType("Empiria.Data.PostgreSql", "Empiria.Data.Handlers.PostgreSqlMethods");

          handler = (IDataHandler) Reflection.ObjectFactory.CreateObject(type);
          break;

        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         this.Technology);
      }
      handlersCache.Insert(this.Source, handler);

      return handler;
    }

    static internal DataSource Default {
      get {
        return DataSource.Parse("Default");
      }
    }

    #endregion Constructors and parsers

    #region Internal methods

    static private string GetDataSourceName(string sourceName) {
      return "Default";
    }

    internal IDbConnection GetConnection() {
      IDataHandler handler = this.GetDataHandler();

      return handler.GetConnection(source);
    }

    #endregion Internal methods

    #region Internal properties

    public string Name {
      get { return this.name; }
    }

    public string Source {
      get { return this.source; }
    }

    public DataTechnology Technology {
      get { return this.technology; }
    }

    #endregion Internal properties

    #region Private methods

    static private string GetSource(string dataSourceName) {
      return ConfigurationData.GetString("§DataSource." + dataSourceName);
    }

    static private DataTechnology GetDataTechnology(string dataSourceName) {
      return (DataTechnology) ConfigurationData.GetInteger("DataTechnology." + dataSourceName);
    }

    #endregion Private methods

  } // struct DataSource

} //namespace Empiria.Data
