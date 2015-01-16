using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Data.Modeler {

  public class DbField {

    #region Fields

    private DbDataSource dbDataSource;

    #endregion Fields

    #region Constructors and parsers

    private DbField(string dataBaseName, string tableName, DbBaseTable.DbTableType tableType)  {
      this.DataBaseName = dataBaseName;
      this.TableName = tableName;
      this.TableType = tableType;
      this.Name = string.Empty;
      this.Index = 0;
      this.Collation = string.Empty;
      this.DefaultValue = string.Empty;
      this.DataType = 0;
      this.MaxLength = 0;
      this.Precision = 0;
      this.Scale = 0;
      this.IsIdentity = false;
      this.IsComputed = false;
      this.IsNullable = false;
      this.IsReplicated = false;
      dbDataSource = DbDataSource.Parse(this.DataBaseName);
    }

    static public DbField Parse(string databaseName, string tableName, DbBaseTable.DbTableType tableType){
      var dbField = new DbField(databaseName,tableName,tableType);
      return dbField;
    }

    #endregion Constructors and parsers

    #region Properties

    public string DataBaseName {
      get;
      private set;
    }
    public DbBaseTable.DbTableType TableType {
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

    public int Index {
      get;
      internal set;
    }

    public string Collation {
      get;
      internal set;
    }

    public string DefaultValue {
      get;
      internal set;
    }

    public int DataType {
      get;
      internal set;
    }

    public int MaxLength {
      get;
      internal set;
    }

    public int Precision {
      get;
      internal set;
    }

    public int Scale {
      get;
      internal set;
    }

    public bool IsIdentity {
      get;
      internal set;
    }

    public bool IsComputed {
      get;
      internal set;
    }

    public bool IsNullable {
      get;
      internal set;
    }

    public bool IsReplicated {
      get;
      internal set;
    }

    public bool IsPrimaryKey {
      get { return GetIsPrimaryKey(); }
    }

    #endregion Properties

    #region Private methods

    private bool GetIsPrimaryKey() {
      DataTable keysTable = dbDataSource.GetPrimaryKeys(this.TableName);
      foreach (DataRow row in keysTable.Rows) {
        if (this.Name == row["COLUMN_NAME"].ToString()) {
          return true;
        }
      }
      return false;
    }

    #endregion Private methods

    #region Public methods


    #endregion Public methods

  }
}
