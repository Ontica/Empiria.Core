using System;
using System.Collections.Generic;

namespace Empiria.Data.Modeler {

  public class DbStoredProcedure : DbBaseQuery {

    #region Constructors and parsers

    private DbStoredProcedure(string databaseName): base(databaseName) {
    }

    static public DbStoredProcedure Parse(string databaseName){
      var dbSp = new DbStoredProcedure(databaseName);
      return dbSp;
    }

    #endregion Constructors and parsers

  }
}
