using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Data.Modeler {

  public class DbIndex {

    #region Constructors and parsers

    private DbIndex(string databaseName, string tableName) {
      this.DatabaseName = databaseName;
      this.TableName = tableName;
      this.Name = string.Empty;
      this.Type = string.Empty;
    }

    static public DbIndex Parse(string databaseName, string tableName) {
      var dbIndex = new DbIndex(databaseName, tableName);
      return dbIndex;
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

    public string Name {
      get;
      internal set;
    }

    public string Type {
      get;
      internal set;
    }

    #endregion Properties

  }

}
