/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution : Empiria Foundation Framework                     System  : Data Access Library                 *
*  Assembly : Empiria.Foundation.dll                           Pattern : Information Holder (with cache)     *
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
  internal struct DataSource {

    #region Fields

    static private EmpiriaDictionary<string, DataSource> sourcesCache =
                                                                new EmpiriaDictionary<string, DataSource>(8);

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
      switch (this.Technology) {
        case DataTechnology.SqlServer:
          return new SqlMethods();
        case DataTechnology.MySql:
          return new MySqlMethods();
        case DataTechnology.OleDb:
          return new OleDbMethods();
        case DataTechnology.Oracle:
          return new OracleMethods();
        case DataTechnology.PostgreSql:
          return new PostgreSqlMethods();
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology,
                                         this.Technology);
      }
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

    internal string Name {
      get { return this.name; }
    }

    internal string Source {
      get { return this.source; }
    }

    internal DataTechnology Technology {
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
