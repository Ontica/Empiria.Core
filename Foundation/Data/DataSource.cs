/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataSource                                       Pattern  : Static Class With Objects Cache   *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a data source formed by the source or connection string and the data technology.   *
*                                                                                                            *
********************************* Copyright (c) 1999-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Collections;
using Empiria.Data.Handlers;

namespace Empiria.Data {

  #region Enumerations

  public enum DataTechnology {
    SqlServer = 1,
    MySql = 2,
    Oracle = 3,
    PostgreSql = 4,
    OleDb = 5,
    Odbc = 6,
    MSQueue = 7,
    XMLFile = 8,
  }

  #endregion Enumerations

  internal struct DataSource {

    #region Fields

    static private EmpiriaDictionary<string, DataSource> sourcesCache = new EmpiriaDictionary<string, DataSource>(8);

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
      switch (technology) {
        case DataTechnology.SqlServer:
          return SqlMethods.GetConnection(source);
        case DataTechnology.MySql:
          return MySqlMethods.GetConnection(source);
        case DataTechnology.OleDb:
          return OleDbMethods.GetConnection(source);
        case DataTechnology.Oracle:
          return OracleMethods.GetConnection(source);
        case DataTechnology.PostgreSql:
          return PostgreSqlMethods.GetConnection(source);
        default:
          throw new EmpiriaDataException(EmpiriaDataException.Msg.InvalidDatabaseTechnology, technology);
      }
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
