using System;
using System.Collections.Generic;

namespace Empiria.Data.Modeler {

  public class DbFunction :DbBaseQuery {

    #region Constructors and parsers

    private DbFunction(string databaseName): base(databaseName) {
      this.Type = string.Empty;
    }

    static public DbFunction Parse(string databaseName){
      var dbFuction = new DbFunction(databaseName);
      return dbFuction;
    }

    #endregion Constructors and parsers

    #region Properties

    public string Type {
      get;
      internal set;
    }

    #endregion Properties


  }
}
