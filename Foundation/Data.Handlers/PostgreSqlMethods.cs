/* Empiria Extensions Framework ******************************************************************************
*                                                                                                            *
*  Solution : Empiria Extensions Framework                     System  : Data Access Library                 *
*  Assembly : Empiria.Data.PostgreSql.dll                      Pattern : Provider                            *
*  Type     : PostgreSqlMethods                                License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Empiria data handler to connect solutions to PostgreSQL databases.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;
using System.EnterpriseServices;

using Npgsql;

namespace Empiria.Data.Handlers {

  /// <summary>Empiria data handler to connect solutions to PostgreSQL databases.</summary>
  internal class PostgreSqlMethods : IDataHandler {

    #region Internal methods

    public int AppendRows(IDbConnection connection, string tableName, DataTable table, string filter) {
      int result = 0;
      string queryString = "SELECT * FROM " + tableName;
      if (!String.IsNullOrEmpty(filter)) {
        queryString += " WHERE " + filter;
      }
      using (connection) {
        connection.Open();
        NpgsqlTransaction transaction = ((NpgsqlConnection) connection).BeginTransaction();
        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter();
        dataAdapter.SelectCommand = new NpgsqlCommand(queryString,
                                                      (NpgsqlConnection) connection,
                                                      transaction);

        NpgsqlCommandBuilder commandBuilder = new NpgsqlCommandBuilder(dataAdapter);

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
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);

      DataTable dataTable = new DataTable();

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
        return dataTable.Rows.Count;
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable,
                                       exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public int Execute(DataOperation operation) {
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        if (ContextUtil.IsInTransaction) {
          connection.EnlistTransaction((System.Transactions.Transaction) ContextUtil.Transaction);
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
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);

      T result = default(T);
      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        if (ContextUtil.IsInTransaction) {
          connection.EnlistTransaction((System.Transactions.Transaction) ContextUtil.Transaction);
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
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, (NpgsqlConnection) connection);

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
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName,
                                                (NpgsqlConnection) transaction.Connection,
                                                (NpgsqlTransaction) transaction);

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
      return PostgreSqlParameterCache.GetParameters(connectionString, sourceName, parameterValues);
    }


    public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      throw new NotImplementedException();
    }


    public IDbConnection GetConnection(string connectionString) {
      NpgsqlConnection connection = new NpgsqlConnection(connectionString);
      if (ContextUtil.IsInTransaction) {
        connection.EnlistTransaction((System.Transactions.Transaction) ContextUtil.Transaction);
      }
      return connection;
    }


    public IDataReader GetDataReader(DataOperation operation) {
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);
      NpgsqlDataReader dataReader;

      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataReader,
                                       exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        //Do not dipose the NpgsqlConnection object because this method returns a DataReader.
      }
      return dataReader;
    }


    public DataRow GetDataRow(DataOperation operation) {
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
        if (dataTable.Rows.Count != 0) {
          return dataTable.Rows[0];
        } else {
          return null;
        }
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable,
                                       exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public DataTable GetDataTable(DataOperation operation, string dataTableName) {
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);

      try {
        DataTable dataTable = new DataTable(dataTableName);
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();

        return dataTable;
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable,
                                       exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }

    }


    public DataView GetDataView(DataOperation operation, string filter, string sort) {
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);

      try {
        DataTable dataTable = new DataTable(operation.SourceName);
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
        return new DataView(dataTable, filter, sort, DataViewRowState.CurrentRows);
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataView, exception,
                                       operation.SourceName, filter, sort);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public object GetFieldValue(DataOperation operation, string fieldName) {
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);
      NpgsqlDataReader dataReader;
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
      NpgsqlConnection connection = new NpgsqlConnection(operation.DataSource.Source);
      NpgsqlCommand command = new NpgsqlCommand(operation.SourceName, connection);

      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        return command.ExecuteScalar();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetScalar,
                                       exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }

    #endregion Internal methods

  } // class PostgreSqlMethods

} // namespace Empiria.Data.Handlers
