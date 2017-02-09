using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Data.Modeler {

  public abstract class DbBaseTable {

    public enum DbTableType {
      Undefined,
      dbTable,
      dbView
    }

    #region Fields

    private DbDataSource dbDataSource;

    #endregion Fields

    #region Constructors and parsers

    protected DbBaseTable(string dataBaseName) {
      this.DatabaseName = dataBaseName;
      this.TableName = string.Empty;
      this.CreateDate = DateTime.MinValue;
      this.ModifyDate = DateTime.MinValue;
      this.TableType = DbTableType.Undefined;
      dbDataSource = DbDataSource.Parse(dataBaseName);
    }

    #endregion Constructors and parsers

    #region Public properties

    public string DatabaseName {
      get;
      private set;
    }

    public string TableName {
      get;
      internal set;
    }

    public int CountColumns {
      get { return GetCountColumns(); }
    }

    public DateTime CreateDate {
      get;
      internal set;
    }

    public DateTime ModifyDate {
      get;
      internal set;
    }

    public DbTableType TableType {
      get;
      set;
    }

    public List<DbField> Fields {
      get { return GetFields(); }
    }

    public List<DbDependencies> Dependencies {
      get { return GetTableDependencies(); }
    }

    #endregion Public properties

    #region Protected methods

    protected abstract int GetCountColumns();

    #endregion

    #region Private methods


    private List<DbField> GetFields() {
      return dbDataSource.GetFields(this.TableName, this.TableType);
    }

    private List<DbDependencies> GetTableDependencies() {
      return dbDataSource.GetTableDependencies(this.TableName);
    }

    #endregion
  }
}
