using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Data.Modeler {

  public abstract class DbBaseQuery {

    #region Fields

    DbDataSource dbDataSource;

    #endregion Fields

    #region Constructors and parsers

    protected DbBaseQuery(string dataBaseName) {
      this.DataBaseName = dataBaseName;
      this.QueryName = string.Empty;
      this.CreateDate = DateTime.MinValue;
      this.ModifyDate = DateTime.MinValue;
      dbDataSource = DbDataSource.Parse(this.DataBaseName);
    }

    #endregion Constructors and parsers

    #region Properties

    public string DataBaseName {
      get;
      protected set;
    }

    public string QueryName {
      get;
      internal set;
    }

    public string SqlCode {
      get { return GetSqlCode(); }
    }

    public int Count {
      get { return GetParametersCount(); }
    }

    public DateTime CreateDate {
      get;
      internal set;
    }

    public DateTime ModifyDate {
      get;
      internal set;
    }

    public List<DbQueryParameter> Paremeters {
      get { return dbDataSource.GetQueryParameters(this.QueryName); }
    }

    #endregion Properties

    #region Private methods

    private string GetSqlCode() {
      return dbDataSource.GetObjectCode(this.QueryName);
    }

    private int GetParametersCount() {
      List<DbQueryParameter> queryParametersList = dbDataSource.GetQueryParameters(this.QueryName);
      return queryParametersList.Count;
    }




    #endregion Private methods

  }
}
