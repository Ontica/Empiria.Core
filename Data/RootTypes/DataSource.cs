/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : DataSource                                       Pattern  : Static Class With Objects Cache   *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a data source formed by the source or connection string and the data technology.   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;

using Empiria.Data.Handlers;

namespace Empiria.Data {

  #region Enumerations

  public enum DataTechnology {
    SqlServer = 1,
    MySql = 2,
    Oracle = 3,
    PostgreSQL = 4,
    OleDb = 5,
    Odbc = 6,
    MSQueue = 7,
    XMLFile = 8,
  }

  #endregion Enumerations

  [StrongNameIdentityPermission(SecurityAction.LinkDemand,
                                PublicKey = "00240000048000009400000006020000002400005253413100040000010001007f32" +
                                            "5fc5c22fde128ef71827d4c9682bbc9b5c722a5b5becf866167211f931bc2afbf2bb" +
                                            "f85e478d2a436ecb50a2d21a3cdd64153180245a42fa1bea2e51acbe6ee3b26a39b9" +
                                            "404a2e010231c6dccdfc6d0c26c4957b4f10538e367010ac1735639c145f819db968" +
                                            "fc7c84c8f6875bc95e3f1a9e66bd65a66dd0115efaae0bb4")]
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
        case DataTechnology.PostgreSQL:
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
      return ConfigurationData.GetString("DataSource." + dataSourceName);
    }

    static private DataTechnology GetDataTechnology(string dataSourceName) {
      return (DataTechnology) ConfigurationData.GetInteger("DataTechnology." + dataSourceName);
    }

    #endregion Private methods

  } // struct DataSource

} //namespace Empiria.Data