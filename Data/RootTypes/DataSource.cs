/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataSource                                       Pattern  : Static Class With Objects Cache   *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a data source formed by the source or connection string and the data technology.   *
*                                                                                                            *
********************************* Copyright (c) 1999-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

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

    static private Dictionary<string, DataSource> sourcesCache = new Dictionary<string, DataSource>();

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

      DataSource dataSource;
      if (!sourcesCache.TryGetValue(dataSourceName, out dataSource)) {
        dataSource = new DataSource(dataSourceName);
        sourcesCache[dataSourceName] = dataSource;
      }
      return dataSource;
    }

    static internal DataSource Default {
      get {
        return DataSource.Parse("Default");
      }
    }

    //static internal DataSource Parse(string callerTypeName) {
    //  DataSource dataSource;

    //  if (!sourcesCache.TryGetValue(callerTypeName, out dataSource)) {
    //    dataSource = new DataSource(callerTypeName);
    //    sourcesCache[callerTypeName] = dataSource;
    //  }
    //  return dataSource;
    //}

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
