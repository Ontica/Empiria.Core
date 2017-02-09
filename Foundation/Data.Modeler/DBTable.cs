using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Data.Modeler {

   public class DBTable : DbBaseTable {

    #region Fields

    private DbDataSource dbDataSource ;

    #endregion Fields

    #region Constructors and parsers

    private DBTable(string dataBaseName): base(dataBaseName) {
      this.TableType = DbTableType.dbTable;
      dbDataSource = DbDataSource.Parse(this.DatabaseName);
    }

    static public DBTable Parse(string dataBaseName){
      var dbTable = new DBTable(dataBaseName);
      return dbTable;
    }

    #endregion Constructors and parsers

    #region Public properties

    public int CountRows {
      get { return GetTotalRows(); }
    }

    public List<DbIndex> Index {
      get { return GetTableIndex(); }
    }

    #endregion Public properties

    #region Protected methods

    protected override int GetCountColumns() {
      return dbDataSource.GetTableColumns(this.TableName);
    }

    #endregion

    #region Private methods

    private int GetTotalRows() {
      return dbDataSource.GetTableRows(this.TableName);
    }

    private List<DbIndex> GetTableIndex(){
      return dbDataSource.GetTableIndex(this.TableName);
    }

    #endregion

    #region Public methods


    #endregion Public methods

  }
}
