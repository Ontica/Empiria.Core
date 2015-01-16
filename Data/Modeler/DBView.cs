using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Data.Modeler {

 public class DBView :  DbBaseTable {

    #region Fields

    DbDataSource dbDataSource;
    #endregion Fields

    #region Constructors and parsers

    private DBView(string databaseName): base(databaseName) {
      this.TableType = DbTableType.dbView;
      dbDataSource = DbDataSource.Parse(databaseName);
    }

    static public DBView Parse(string databaseName){
      var dbView = new DBView(databaseName);
      return dbView;
    }

    #endregion

    #region Public properties

    public string SqlCode {
      get { return GetSqlCode(); }
    }

    #endregion

    #region Protected methods

    protected override int GetCountColumns() {
      return dbDataSource.GetViewColumns(this.TableName);
    }

    #endregion

    #region Private methdos

    private string GetSqlCode() {
      return dbDataSource.GetObjectCode(this.TableName);
    }
    #endregion

    #region Public methods

    //public List<DBView> GetViews() {
    //  DataServices dataService = new DataServices();
    //  DataTable views = dataService.GetViews(this.DataBaseName);
    //  List<DBView> viewList = new List<DBView>();
    //  foreach (DataRow row in views.Rows) {
    //    DBView table = new DBView(this.DataBaseName);
    //    table.TableName = row["name"].ToString();
    //    table.CreateDate = Convert.ToDateTime(row["create_date"]);
    //    table.ModifyDate = Convert.ToDateTime(row["modify_date"]);
    //    viewList.Add(table);
    //  }
    //  return viewList;
    //}

    //public List<DBView> GetViews(string[] exceptions) {
    //  int index = 0;
    //  List<DBView> views = GetViews();
    //  foreach (string exception in exceptions) {
    //    index = views.FindIndex(table => table.TableName == exception);
    //    if (index != -1) {
    //      views.RemoveAt(index);
    //    } else {
    //      //no-op
    //    }
    //  }
    //  return views;
    //}

    #endregion Public methods

  }
}
