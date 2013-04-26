/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            Assembly : Empiria.Data.dll                  *
*  Type      : SqlMethods                                       Pattern  : Static Class                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : static internal class to read data stored in SQL Server® databases.                           *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Security.Permissions;

namespace Empiria.Data.Handlers {

  [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "8b7fe9c60c0f43bd")]
  static internal class SqlMethods {

    #region Internal methods

    static internal int AppendRows(string tableName, DataTable table, string filter) {
      int result = 0;
      string queryString = "SELECT * FROM " + tableName;
      if (!String.IsNullOrEmpty(filter)) {
        queryString += " WHERE " + filter;
      }
      using (SqlConnection connection = new SqlConnection(DataSource.Parse(tableName).Source)) {
        connection.Open();
        SqlTransaction transaction = connection.BeginTransaction();
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        dataAdapter.SelectCommand = new SqlCommand(queryString, connection, transaction);
        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

        dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
        dataAdapter.InsertCommand.Transaction = transaction;
        DataTable source = DataReader.GetDataTable(DataOperation.Parse(queryString));
        dataAdapter.Fill(source);
        for (int i = 0; i < table.Rows.Count; i++) {
          table.Rows[i].SetAdded();
          source.ImportRow(table.Rows[i]);
        }
        result = dataAdapter.Update(source);
        transaction.Commit();
      }
      return result;
    }

    static internal int CountRows(DataOperation operation) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable();

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        return dataTable.Rows.Count;
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantGetDataTable, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }

    static internal int Execute(DataOperation operation) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        if (ContextUtil.IsInTransaction) {
          connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
        }
        affectedRows = command.ExecuteNonQuery();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantExecuteActionQuery, exception,
                                       operation.SourceName, operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return affectedRows;
    }

    static internal int Execute(SqlConnection connection, DataOperation operation) {
      SqlCommand command = new SqlCommand(operation.SourceName, connection);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        affectedRows = command.ExecuteNonQuery();
        command.Parameters.Clear();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantExecuteActionQuery, exception,
                                       operation.SourceName, operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }

    static internal int Execute(SqlTransaction transaction, DataOperation operation) {
      SqlCommand command = new SqlCommand(operation.SourceName, transaction.Connection, transaction);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        affectedRows = command.ExecuteNonQuery();
        command.Parameters.Clear();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantExecuteActionQuery, exception,
                                       operation.SourceName, operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }

    static internal SqlConnection GetConnection(string connectionString) {
      SqlConnection connection = new SqlConnection(connectionString);
      if (ContextUtil.IsInTransaction) {
        connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
      }
      return connection;
    }

    static internal byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      SqlDataReader reader = (SqlDataReader) SqlMethods.GetDataReader(operation);

      if (reader.Read()) {
        System.Data.SqlTypes.SqlBinary blob = reader.GetSqlBinary(reader.GetOrdinal(fieldName));
        return blob.Value;
      } else {
        return null;
      }
    }

    static internal IDataReader GetDataReader(DataOperation operation) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);
      SqlDataReader dataReader;

      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantGetDataReader, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        //Don't dipose the connection because this method returns a DataReader.
      }
      return dataReader;
    }

    static internal DataRow GetDataRow(DataOperation operation) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count != 0) {
          return dataTable.Rows[0];
        } else {
          return null;
        }
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantGetDataTable, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }

    static internal DataTable GetDataTable(DataOperation operation, string dataTableName) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(dataTableName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantGetDataTable, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return dataTable;
    }

    static internal DataView GetDataView(DataOperation operation, string filter, string sort) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);

        return new DataView(dataTable, filter, sort, DataViewRowState.CurrentRows);
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantGetDataTable, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }

    static internal object GetFieldValue(DataOperation operation, string fieldName) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);
      SqlDataReader dataReader;
      object fieldValue = null;

      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
        if (dataReader.Read()) {
          fieldValue = dataReader[fieldName];
        }
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantGetFieldValue,
                                       exception, operation.SourceName, fieldName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return fieldValue;
    }

    static internal object GetScalar(DataOperation operation) {
      SqlConnection connection = new SqlConnection(operation.DataSource.Source);
      SqlCommand command = new SqlCommand(operation.SourceName, connection);
      object scalar = System.DBNull.Value;

      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        scalar = command.ExecuteScalar();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantGetScalar, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return scalar;
    }

    #endregion Internal methods

  } // class SqlMethods

} // namespace Empiria.Data.Handlers 
