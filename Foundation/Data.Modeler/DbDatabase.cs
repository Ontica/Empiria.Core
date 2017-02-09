using System;
using System.Collections.Generic;
using System.Linq;


namespace Empiria.Data.Modeler {

  public class DbDatabase {

  private DbDataSource  dbDataSource;

    #region Constructors and parsers

    private DbDatabase(string databaseName){
      this.DatabaseName = databaseName;
      dbDataSource = DbDataSource.Parse(this.DatabaseName);
    }
    static public DbDatabase Parse(string dataBaseName){
      var dbDatabase = new DbDatabase(dataBaseName);
      return dbDatabase;
    }
    #endregion Constructors and parsers

    #region Public properties

    public string DatabaseName{
      get;
      private set;
    }

    public List<DBTable> Tables{
      get {return dbDataSource.GetTables();}
    }

    public List<DBView> Views{
      get{return dbDataSource.GetViews();}
    }

    public List<DbStoredProcedure> StoredProcedures{
      get{return dbDataSource.GetStoredProcedures(); }
    }
    public List<DbFunction> Functions{
      get{ return dbDataSource.GetFunctions(); }
    }

    #endregion Public properties

    public List<DBTable> GetTables(string[] exceptions){
      return dbDataSource.GetTables(exceptions);
    }

    public List<DBView> GetViews(string[] exceptions){
      return dbDataSource.GetViews(exceptions);
    }

    public List<DbStoredProcedure> GetStoredProcedures(string[] exceptions) {
      return dbDataSource.GetStoredProcedures(exceptions);
    }

  }
}
