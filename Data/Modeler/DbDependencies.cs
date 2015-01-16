using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Data.Modeler {

  public class DbDependencies {

    #region Constructors and parsers

    private DbDependencies(string databaseName, string tableName) {
      this.TableName = tableName;
      this.DatabaseName = databaseName;
      this.DependentObject = string.Empty;
      this.Description = string.Empty;
    }

    static public DbDependencies Parse(string databaseName, string tableName) {
      var dbDependcies = new DbDependencies(databaseName, tableName);
      return dbDependcies;
    }

    #endregion Constructors and parsers

    #region Properties

    public string DatabaseName {
      get;
      private set;
    }

    public string TableName {
      get;
      private set;
    }

    public string DependentObject {
      get;
      internal set;
    }

    public string Description {
      get;
      internal set;
    }

    #endregion Properties

  }
}
