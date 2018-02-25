/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Core                                     System  : Data Access Library                 *
*  Assembly : Empiria.Core.dll                                 Pattern : Provider                            *
*  Type     : OleDbMethods                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Empiria data handler to connect solutions to OLE DB-based databases.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;
using System.Data.OleDb;
using System.EnterpriseServices;

namespace Empiria.Data.Handlers {

  /// <summary>Empiria data handler to connect solutions to OLE DB-based databases.</summary>
  internal class OleDbMethods : IDataHandler {

    #region Internal methods

    public int AppendRows(IDbConnection connection, string tableName,
                          DataTable table, string filter) {
      throw new NotImplementedException();
    }


    public int CountRows(DataOperation operation) {
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable();

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
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
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        affectedRows = command.ExecuteNonQuery();
        command.Parameters.Clear();
      } catch (Exception exception) {
        string parametersString = String.Empty;
        for (int i = 0; i < operation.Parameters.Length; i++) {
          parametersString += (parametersString.Length != 0 ? ", " : String.Empty) + Convert.ToString(operation.Parameters[i]);
        }
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return affectedRows;
    }


    public T Execute<T>(DataOperation operation) {
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);

      T result = default(T);
      try {
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        connection.Open();
        result = (T) command.ExecuteScalar();
        command.Parameters.Clear();
      } catch (Exception exception) {
        string parametersString = String.Empty;
        for (int i = 0; i < operation.Parameters.Length; i++) {
          parametersString += (parametersString.Length != 0 ? ", " : String.Empty) + Convert.ToString(operation.Parameters[i]);
        }
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return result;
    }


    public int Execute(IDbConnection connection, DataOperation operation) {
      OleDbCommand command = new OleDbCommand(operation.SourceName, (OleDbConnection) connection);

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
        string parametersString = String.Empty;
        for (int i = 0; i < operation.Parameters.Length; i++) {
          parametersString += (parametersString.Length != 0 ? ", " : String.Empty) + Convert.ToString(operation.Parameters[i]);
        }
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }


    public int Execute(IDbTransaction transaction, DataOperation operation) {
      OleDbCommand command = new OleDbCommand(operation.SourceName,
                                              (OleDbConnection) transaction.Connection,
                                              (OleDbTransaction) transaction);

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
        string parametersString = String.Empty;
        for (int i = 0; i < operation.Parameters.Length; i++) {
          parametersString += (parametersString.Length != 0 ? ", " : String.Empty) + Convert.ToString(operation.Parameters[i]);
        }
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }


    public IDataParameter[] GetParameters(string connectionString,
                                          string sourceName,
                                          object[] parameterValues) {
      return OleDbParameterCache.GetParameters(connectionString, sourceName, parameterValues);
    }


    public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      throw new NotImplementedException();
    }


    public IDbConnection GetConnection(string connectionString) {
      OleDbConnection connection = new OleDbConnection(connectionString);
      if (ContextUtil.IsInTransaction) {
        connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
      }
      return connection;
    }

    public IDataReader GetDataReader(DataOperation operation) {
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);
      OleDbDataReader dataReader;

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
        //Don't dispose the connection because this method returns a DataReader.
      }
      return dataReader;
    }


    public DataRow GetDataRow(DataOperation operation) {
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
        if (dataTable.Rows.Count != 0) {
          return dataTable.Rows[0];
        } else {
          return null;
        }
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public DataTable GetDataTable(DataOperation operation, string dataTableName) {
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(dataTableName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable, exception, operation.SourceName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return dataTable;
    }


    public DataView GetDataView(DataOperation operation, string filter, string sort) {
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        if (operation.ExecutionTimeout != 0) {
          command.CommandTimeout = operation.ExecutionTimeout;
        }
        operation.FillParameters(command);
        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
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
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);
      OleDbDataReader dataReader;
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
      OleDbConnection connection = new OleDbConnection(operation.DataSource.Source);
      OleDbCommand command = new OleDbCommand(operation.SourceName, connection);

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

  } // class OleDbMethods

} // namespace Empiria.Data.Handlers
