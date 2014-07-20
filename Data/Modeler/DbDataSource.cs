using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Empiria.Data.Modeler {

  public class DbDataSource {

    #region Fields
    private string connectionString = "Server=CHRIS-PC\\SQLEXPRESS;Database=Zacatecas;Trusted_Connection=True;";
    #endregion Fields

    #region Constructors and parsers

    private DbDataSource(string databaseName) {
      this.DatabaseName = databaseName;

    }

    static public DbDataSource Parse(string dataBaseName) {
      var dbDataSoruce = new DbDataSource(dataBaseName);
      return dbDataSoruce;
    }

    #endregion Constructors and parsers

    #region Public propierties

    public string DatabaseName {
      get;
      private set;
    }


    #endregion Public propierties

    #region General Methods

    private DataTable GetObjectTable(string sql) {
      SqlConnection conn = new SqlConnection(connectionString);
      SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
      DataTable tables = new DataTable();
      adapter.Fill(tables);
      return tables;
    }

    private int GetIntValue(string sql) {
      DataTable table = GetObjectTable(sql);
      if (table.Rows.Count == 0) {
        return 0;
      } else {
        return Convert.ToInt32(table.Rows[0][0]);
      }
    }

    private string GetStringValue(string sql) {
      DataTable table = GetObjectTable(sql);
      if (table.Rows.Count == 0) {
        return string.Empty;
      } else {
        return table.Rows[0][0].ToString();
      }
    }

    #endregion General Methods

    #region Tables and Views

    public DataTable GetDataBaseTables() {
      var op = DataOperation.Parse("@qryDBTables", this.DatabaseName);

      return DataReader.GetDataTable(op);

      //string sql = "USE " +  + " " +
      //             "SELECT * FROM sys.tables  " +
      //             "ORDER BY sys.tables.name ";
      //return GetObjectTable(sql);
    }

    public int GetTableRows(string tableName) {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT CONVERT(bigint, rows) TotalRows " +
                   "FROM sysindexes  " +
                   "WHERE id = OBJECT_ID(" + " '" + tableName + "' " + ") " +
                   "AND indid < 2 ";
      return GetIntValue(sql);
    }

    public int GetTableColumns(string tableName) {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT COUNT(c.column_id) AS TotalColumn " +
                   "FROM sys.tables t INNER JOIN sys.columns c " +
                   "ON t.object_id = c.object_id AND t.name = " + "'" + tableName + "'";
      return GetIntValue(sql);
    }

    private DataTable GetDatabaseViews() {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT * FROM sys.views  " +
                   "ORDER BY sys.views.name ";
      return GetObjectTable(sql);
    }

    public int GetViewColumns(string viewName) {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT COUNT(c.column_id) AS TotalColumn " +
                   "FROM sys.views t INNER JOIN sys.columns c " +
                   "ON t.object_id = c.object_id AND t.name = " + "'" + viewName + "'";
      return GetIntValue(sql);
    }

    #endregion Tables and Views

    #region Get object code

    public string GetObjectCode(string objectName) {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT DEFINITION AS Code " +
                   "FROM sys.sql_modules " +
                   "WHERE object_id = OBJECT_ID( " + "'" + objectName + "'" + " )";
      return GetStringValue(sql);
    }

    #endregion Get object code

    #region Fields

    private DataTable GetTableFields(string tableName) {
      string sql = "USE " + this.DatabaseName + "  " +
                   "SELECT  c.column_id AS FieldId, c.name AS FieldName,  c.user_type_id AS DataType, " +
                   "c.max_length AS MaxLength, c.precision AS Precision, c.scale AS Scale, " +
                   "c.is_identity AS IsIdentity, c.is_computed AS IsComputed, c.is_nullable AS IsNullable, " +
                   "c.is_replicated AS IsReplicated,  object_definition(default_object_id) AS DefaultValue, " +
                   "c.collation_name AS Collation " +
                   "FROM sys.columns c, sys.tables v " +
                   "WHERE (c.object_id = v.object_id) " +
                   "AND (v.name = " + "'" + tableName + "')";
      return GetObjectTable(sql);
    }

    private DataTable GetViewFields(string viewName) {
      string sql = "USE " + this.DatabaseName + "  " +
                   "SELECT  c.column_id AS FieldId, c.name AS FieldName,  c.user_type_id AS DataType, " +
                   "c.max_length AS MaxLength, c.precision AS Precision, c.scale AS Scale, " +
                   "c.is_identity AS IsIdentity, c.is_computed AS IsComputed, c.is_nullable AS IsNullable, " +
                   "c.is_replicated AS IsReplicated,  object_definition(default_object_id) AS DefaultValue, " +
                   "c.collation_name AS Collation " +
                   "FROM sys.columns c, sys.views v " +
                   "WHERE (c.object_id = v.object_id) " +
                   "AND (v.name = " + "'" + viewName + "')";
      return GetObjectTable(sql);
    }

    public DataTable GetPrimaryKeys(string tableName) {
      string sql = "exec [sys].[sp_primary_keys_rowset] @table_name= '" + tableName + "'";
      return GetObjectTable(sql);
    }


    #endregion Fields

    #region index

    private DataTable GetDBTableIndex(string tableName) {
      string sql = "USE " + this.DatabaseName + "  " +
                   "SELECT so.name AS TableName , si.name AS IndexName , si.type_desc AS IndexType " +
                   "FROM sys.indexes si " +
                   "JOIN sys.objects so ON si.[object_id] = so.[object_id] " +
                   "WHERE so.type = 'U' " +
                   "AND (si.name IS NOT NULL) " +
                   "AND so.name = " + "'" + tableName + "' " +
                   "ORDER BY IndexType ";
      return GetObjectTable(sql);
    }

    private DataTable GetDataBaseIndexes() {
      string sql = "USE " + this.DatabaseName + "  " +
                   "SELECT so.name AS TableName , si.name AS IndexName , si.type_desc AS IndexType " +
                   "FROM sys.indexes si " +
                   "JOIN sys.objects so ON si.[object_id] = so.[object_id] " +
                   "WHERE so.type = 'U' " +
                   "AND (si.name IS NOT NULL) " +
                   "ORDER BY IndexType ";
      return GetObjectTable(sql);
    }

    #endregion

    #region Dependencies

    private DataTable GetTableDependences(string TableName) {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT  referencing_entity_name AS DependenceName, " +
                   "referencing_class_desc AS EntityDescription " +
                   "FROM sys.dm_sql_referencing_entities ('dbo." + TableName + "', 'OBJECT'); ";
      return GetObjectTable(sql);
    }


    #endregion

    #region Functions and StoredProcedures

    private DataTable GetDatabaseStoredProcedures() {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT a.Name AS Name, a.type_desc AS Type, a.create_date AS CreateDate, a.modify_date AS ModifyDate " +
                   "FROM sys.objects a " +
                   "INNER JOIN sys.schemas b " +
                   "ON a.schema_id = b.schema_id " +
                   "WHERE (TYPE = 'P') " +
                   "ORDER BY Type, Name ";
      return GetObjectTable(sql);
    }

    public DataTable GetDatabaseFunctions() {
      string sql = "USE " + this.DatabaseName + " " +
                   "SELECT a.Name AS Name, a.type_desc AS Type, a.create_date AS CreateDate, a.modify_date AS ModifyDate " +
                   "FROM sys.objects a " +
                   "INNER JOIN sys.schemas b " +
                   "ON a.schema_id = b.schema_id " +
                   "WHERE TYPE in ('FN', 'IF', 'TF') " +
                   "ORDER BY Type, Name ";
      return GetObjectTable(sql);
    }
    #endregion

    #region Parameter

    private DataTable dtParamTable() {
      DataTable dtPTable = new DataTable();
      DataColumn dcIndex = new DataColumn("Index", typeof(int));
      DataColumn dcName = new DataColumn("Name", typeof(string));
      DataColumn dcType = new DataColumn("Type", typeof(string));
      DataColumn dcPrecision = new DataColumn("Precision", typeof(string));
      DataColumn dcDecimal = new DataColumn("Scale", typeof(string));
      DataColumn dcSize = new DataColumn("Size", typeof(string));
      DataColumn dcDirection = new DataColumn("Direction", typeof(string));
      dtPTable.Columns.Add(dcIndex);
      dtPTable.Columns.Add(dcName);
      dtPTable.Columns.Add(dcType);
      dtPTable.Columns.Add(dcPrecision);
      dtPTable.Columns.Add(dcDecimal);
      dtPTable.Columns.Add(dcSize);
      dtPTable.Columns.Add(dcDirection);
      return dtPTable;
    }

    private DataTable GetParameters(string queryName) {
      SqlConnection scConnect = new SqlConnection(connectionString);
      SqlCommand scParams = new SqlCommand();
      scParams.Connection = scConnect;
      scParams.CommandText = queryName;
      scParams.CommandType = CommandType.StoredProcedure;
      scParams.Connection.Open();
      SqlCommandBuilder.DeriveParameters(scParams);
      scParams.Connection.Close();
      int intCountParameters = scParams.Parameters.Count;
      DataTable dtParamsTable = dtParamTable();
      for (int i = 0; i < intCountParameters; i++) {
        if (scParams.Parameters[i].Direction == ParameterDirection.Input) {
          DataRow Row = dtParamsTable.NewRow();
          Row["Index"] = i;
          Row["Name"] = scParams.Parameters[i].ParameterName.ToString();
          Row["Type"] = (int) scParams.Parameters[i].SqlDbType;
          Row["Precision"] = scParams.Parameters[i].Precision.ToString();
          Row["Scale"] = scParams.Parameters[i].Scale.ToString();
          Row["Size"] = scParams.Parameters[i].Size.ToString();
          Row["Direction"] = (int) scParams.Parameters[i].Direction;
          dtParamsTable.Rows.Add(Row);
        }
      }
      return dtParamsTable;
    }

    #endregion

    #region GetTables and Views

    public List<DBTable> GetTables() {

      DataTable tables = GetDataBaseTables();
      List<DBTable> tableList = new List<DBTable>();
      foreach (DataRow row in tables.Rows) {
        var table = DBTable.Parse(this.DatabaseName);
        table.TableName = row["name"].ToString();
        table.CreateDate = Convert.ToDateTime(row["create_date"]);
        table.ModifyDate = Convert.ToDateTime(row["modify_date"]);
        tableList.Add(table);
      }
      return tableList;
    }

    public List<DBTable> GetTables(string[] exceptions) {
      int index = 0;
      List<DBTable> tables = GetTables();
      foreach (string exception in exceptions) {
        index = tables.FindIndex(table => table.TableName == exception);
        if (index != -1) {
          tables.RemoveAt(index);
        } else {
          //no-op
        }
      }
      return tables;
    }

    #region Views

    public List<DBView> GetViews() {

      DataTable views = GetDatabaseViews();
      List<DBView> viewList = new List<DBView>();
      foreach (DataRow row in views.Rows) {
        var table = DBView.Parse(this.DatabaseName);
        table.TableName = row["name"].ToString();
        table.CreateDate = Convert.ToDateTime(row["create_date"]);
        table.ModifyDate = Convert.ToDateTime(row["modify_date"]);
        viewList.Add(table);
      }
      return viewList;
    }

    public List<DBView> GetViews(string[] exceptions) {
      int index = 0;
      List<DBView> views = GetViews();
      foreach (string exception in exceptions) {
        index = views.FindIndex(table => table.TableName == exception);
        if (index != -1) {
          views.RemoveAt(index);
        } else {
          //no-op
        }
      }
      return views;
    }

    #endregion Views


    #endregion GetTables and Views

    #region GetFields

    private DataTable GetFieldsTable(string tableName, DbBaseTable.DbTableType tableType) {
      DataTable FieldsTable = new DataTable();
      switch (tableType) {
        case DbBaseTable.DbTableType.dbTable: {
            FieldsTable = GetTableFields(tableName);
          } break;
        case DbBaseTable.DbTableType.dbView: {
            FieldsTable = GetViewFields(tableName);
          } break;
      }
      return FieldsTable;
    }

    public List<DbField> GetFields(string tableName, DbBaseTable.DbTableType tableType) {
      List<DbField> FieldList = new List<DbField>();
      DataTable fieldTable = GetFieldsTable(tableName, tableType);
      foreach (DataRow row in fieldTable.Rows) {
        var field = DbField.Parse(this.DatabaseName, tableName, tableType);
        field.Name = row["FieldName"].ToString();
        field.Index = Convert.ToInt32(row["FieldId"]);
        field.Collation = row["Collation"].ToString();
        field.DefaultValue = row["DefaultValue"].ToString();
        field.DataType = Convert.ToInt32(row["DataType"]);
        field.MaxLength = Convert.ToInt32(row["MaxLength"]);
        field.Precision = Convert.ToInt32(row["Precision"]);
        field.Scale = Convert.ToInt32(row["Scale"]);
        field.IsIdentity = (bool) row["IsIdentity"];
        field.IsComputed = (bool) row["IsComputed"];
        field.IsNullable = (bool) row["IsNullable"];
        field.IsReplicated = (bool) row["IsReplicated"];
        FieldList.Add(field);
      }
      return FieldList;
    }



    #endregion GetFields

    #region GetIndexes

    public List<DbIndex> GetTableIndex(string tableName) {
      List<DbIndex> indexList = new List<DbIndex>();
      DataTable indexTable = GetDBTableIndex(tableName);
      foreach (DataRow row in indexTable.Rows) {
        var index = DbIndex.Parse(this.DatabaseName, tableName);
        index.Name = row["IndexName"].ToString();
        index.Type = row["IndexType"].ToString();
        indexList.Add(index);
      }
      return indexList;
    }

    #endregion GetIndexes

    #region GetDependencies

    public List<DbDependencies> GetTableDependencies(string tableName) {
      var references = new List<DbDependencies>();
      DataTable referencesTable = GetTableDependences(tableName);
      foreach (DataRow row in referencesTable.Rows) {
        var dbDependence = DbDependencies.Parse(this.DatabaseName, tableName);
        dbDependence.DependentObject = row["DependenceName"].ToString();
        dbDependence.Description = row["EntityDescription"].ToString();
        references.Add(dbDependence);
      }
      return references;
    }

    #endregion GetDependencies

    #region GetStoredProcedures

    public List<DbStoredProcedure> GetStoredProcedures() {
      List<DbStoredProcedure> storedProceuresList = new List<DbStoredProcedure>();
      DataTable storedProceduresTable = GetDatabaseStoredProcedures();
      foreach (DataRow row in storedProceduresTable.Rows) {
        var storedProcedure = DbStoredProcedure.Parse(this.DatabaseName);
        storedProcedure.QueryName = row["Name"].ToString();
        storedProcedure.CreateDate = (DateTime) row["CreateDate"];
        storedProcedure.ModifyDate = (DateTime) row["ModifyDate"];
        storedProceuresList.Add(storedProcedure);
      }
      return storedProceuresList;
    }

    public List<DbStoredProcedure> GetStoredProcedures(string[] exceptions) {
      int index = 0;
      List<DbStoredProcedure> storedProceduresList = GetStoredProcedures();
      foreach (string exception in exceptions) {
        index = storedProceduresList.FindIndex(storedProcedure => storedProcedure.QueryName == exception);
        if (index != -1) {
          storedProceduresList.RemoveAt(index);
        } else {
          //no-op
        }
      }
      return storedProceduresList;
    }

    #endregion GetStoredProcedures

    #region GetFuncttions

    public List<DbFunction> GetFunctions() {
      DataTable functionsTable = GetDatabaseFunctions();
      List<DbFunction> functionsList = new List<DbFunction>();
      foreach (DataRow row in functionsTable.Rows) {
        var function = DbFunction.Parse(this.DatabaseName);
        function.QueryName = row["Name"].ToString();
        function.Type = row["Type"].ToString();
        function.CreateDate = Convert.ToDateTime(row["CreateDate"]);
        function.ModifyDate = Convert.ToDateTime(row["ModifyDate"]);
        functionsList.Add(function);
      }
      return functionsList;
    }

    public List<DbFunction> GetFunctions(string[] exceptions) {
      int index = 0;
      List<DbFunction> FunctionList = GetFunctions();
      foreach (string exception in exceptions) {
        index = FunctionList.FindIndex(storedProcedure => storedProcedure.QueryName == exception);
        if (index != -1) {
          FunctionList.RemoveAt(index);
        } else {
          //no-op
        }
      }
      return FunctionList;
    }

    #endregion GetFunctions

    #region GetQueryParameters

    public List<DbQueryParameter> GetQueryParameters(string queryName) {
      List<DbQueryParameter> queryParameters = new List<DbQueryParameter>();
      DataTable queryParametersTable = GetParameters(queryName);
      foreach (DataRow row in queryParametersTable.Rows) {
        var queryParameter = DbQueryParameter.Parse(queryName);
        queryParameter.Index = (int) row["Index"];
        queryParameter.Name = row["Name"].ToString();
        queryParameter.SqlDbType = Convert.ToInt32(row["Type"]);
        queryParameter.Direction = Convert.ToInt32(row["Direction"]);
        queryParameter.Size = Convert.ToInt32(row["Size"]);
        queryParameter.Scale = Convert.ToInt32(row["Scale"]);
        queryParameter.Precision = Convert.ToInt32(row["Precision"]);
        queryParameter.DefaultValue = string.Empty;
        queryParameters.Add(queryParameter);
      }
      return queryParameters;
    }

    #endregion GetQueryParameters

  }
}
