/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution : Empiria Foundation Framework                     System  : Data Access Library                 *
*  Assembly : Empiria.Foundation.dll                           Pattern : Provider                            *
*  Type     : SqlMethods                                       License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Empiria data handler to connect solutions to Microsoft SQL Server databases.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;

namespace Empiria.Data.Handlers {

  /// <summary>Empiria data handler to connect to Microsoft SQL Server databases.</summary>
  internal class SqlMethods : IDataHandler {

    #region Internal methods

    public int AppendRows(string tableName, DataTable table, string filter) {
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
        dataAdapter.Dispose();
      }
      return result;
    }


    public int CountRows(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);
      var dataTable = new DataTable();

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
        return dataTable.Rows.Count;
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public int Execute(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return affectedRows;
    }


    public T Execute<T>(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

      T result = default(T);
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
        result = (T) command.ExecuteScalar();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return result;
    }


    public int Execute(IDbConnection connection, DataOperation operation) {
      var command = new SqlCommand(operation.SourceName, (SqlConnection) connection);

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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }


    public int Execute(IDbTransaction transaction, DataOperation operation) {
      var command = new SqlCommand(operation.SourceName,
                                  (SqlConnection) transaction.Connection,
                                  (SqlTransaction) transaction);

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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }


    public IDataParameter[] GetParameters(string connectionString,
                                          string sourceName,
                                          object[] parameterValues) {
      return SqlParameterCache.GetParameters(connectionString, sourceName, parameterValues);
    }


    public IDbConnection GetConnection(string connectionString) {
      var connection = new SqlConnection(connectionString);
      if (ContextUtil.IsInTransaction) {
        connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
      }
      return connection;
    }


    public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      var reader = (SqlDataReader) this.GetDataReader(operation);

      if (reader.Read()) {
        System.Data.SqlTypes.SqlBinary blob = reader.GetSqlBinary(reader.GetOrdinal(fieldName));
        return blob.Value;
      } else {
        return null;
      }
    }


    public IDataReader GetDataReader(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);
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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataReader, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        //Don't dipose the connection because this method returns a DataReader.
      }
      return dataReader;
    }


    public DataRow GetDataRow(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);
      var dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
        if (dataTable.Rows.Count != 0) {
          return dataTable.Rows[0];
        } else {
          return null;
        }
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable, exception, operation.Name,
                                       operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public DataTable GetDataTable(DataOperation operation, string dataTableName) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);
      var dataTable = new DataTable(dataTableName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable, exception, operation.Name,
                                       operation.ParametersToString());
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return dataTable;
    }


    public DataView GetDataView(DataOperation operation, string filter, string sort) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);
      var dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
        return new DataView(dataTable, filter, sort, DataViewRowState.CurrentRows);
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataView, exception, operation.SourceName, filter, sort);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public object GetFieldValue(DataOperation operation, string fieldName) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);
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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetFieldValue,
                                       exception, operation.SourceName, fieldName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return fieldValue;
    }


    public object GetScalar(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        return command.ExecuteScalar();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetScalar, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }

    #endregion Internal methods

  } // class SqlMethods

} // namespace Empiria.Data.Handlers
