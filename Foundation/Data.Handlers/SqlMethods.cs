﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            Assembly : Empiria.Data.dll                  *
*  Type      : SqlMethods                                       Pattern  : Static Class                      *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static internal class used to read data stored in SQL Server databases.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;

namespace Empiria.Data.Handlers {

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
        dataAdapter.Dispose();
      }
      return result;
    }

    static internal int CountRows(DataOperation operation) {
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

    static internal int Execute(DataOperation operation) {
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

    static internal int Execute(SqlConnection connection, DataOperation operation) {
      var command = new SqlCommand(operation.SourceName, connection);

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

    static internal int Execute(SqlTransaction transaction, DataOperation operation) {
      var command = new SqlCommand(operation.SourceName, transaction.Connection, transaction);

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

    static internal SqlConnection GetConnection(string connectionString) {
      var connection = new SqlConnection(connectionString);
      if (ContextUtil.IsInTransaction) {
        connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
      }
      return connection;
    }

    static internal byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      var reader = (SqlDataReader) SqlMethods.GetDataReader(operation);

      if (reader.Read()) {
        System.Data.SqlTypes.SqlBinary blob = reader.GetSqlBinary(reader.GetOrdinal(fieldName));
        return blob.Value;
      } else {
        return null;
      }
    }

    static internal IDataReader GetDataReader(DataOperation operation) {
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

    static internal DataRow GetDataRow(DataOperation operation) {
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

    static internal DataTable GetDataTable(DataOperation operation, string dataTableName) {
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

    static internal DataView GetDataView(DataOperation operation, string filter, string sort) {
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

    static internal object GetFieldValue(DataOperation operation, string fieldName) {
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

    static internal object GetScalar(DataOperation operation) {
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