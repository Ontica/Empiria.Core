﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            Assembly : Empiria.Data.dll                  *
*  Type      : MySqlMethods                                     Pattern  : Static Class                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : static internal class to read data stored in MySQL Server® databases.                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;
using System.Security.Permissions;
using MySql.Data.MySqlClient;

namespace Empiria.Data.Handlers {

  [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "8b7fe9c60c0f43bd")]
  static internal class MySqlMethods {

    #region Internal methods

    static internal int CountRows(DataOperation operation) {
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable();

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
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
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        connection.Open();
        // NOTE: DISTRIBUTED TRANSACTIONS NOT SUPPORTED YET FOR MYSQL
        //if (ContextUtil.IsInTransaction)  {
        //  connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
        //}
        affectedRows = command.ExecuteNonQuery();
        command.Parameters.Clear();
      } catch (Exception exception) {
        string parametersString = String.Empty;
        for (int i = 0; i < operation.Parameters.Length; i++) {
          parametersString += (parametersString.Length != 0 ? ", " : String.Empty) + Convert.ToString(operation.Parameters[i]);
        }
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return affectedRows;
    }

    static internal int Execute(MySqlConnection connection, DataOperation operation) {
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        affectedRows = command.ExecuteNonQuery();
        command.Parameters.Clear();
      } catch (Exception exception) {
        string parametersString = String.Empty;
        for (int i = 0; i < operation.Parameters.Length; i++) {
          parametersString += (parametersString.Length != 0 ? ", " : String.Empty) + Convert.ToString(operation.Parameters[i]);
        }
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }

    static internal int Execute(MySqlTransaction transaction, DataOperation operation) {
      MySqlCommand command = new MySqlCommand(operation.SourceName, transaction.Connection, transaction);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        affectedRows = command.ExecuteNonQuery();
        command.Parameters.Clear();
      } catch (Exception exception) {
        string parametersString = String.Empty;
        for (int i = 0; i < operation.Parameters.Length; i++) {
          parametersString += (parametersString.Length != 0 ? ", " : String.Empty) + Convert.ToString(operation.Parameters[i]);
        }
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }

    static internal MySqlConnection GetConnection(string connectionString) {
      MySqlConnection connection = new MySqlConnection(connectionString);
      // NOTE: DISTRIBUTED TRANSACTIONS NOT SUPPORTED YET FOR MYSQL
      //if (ContextUtil.IsInTransaction)  {
      //  connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
      //}
      return connection;
    }

    static internal IDataReader GetDataReader(DataOperation operation) {
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);
      MySqlDataReader dataReader;

      try {
        command.CommandType = operation.CommandType;
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
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
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
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(dataTableName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
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
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
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
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);
      MySqlDataReader dataReader;
      object fieldValue = null;

      try {
        command.CommandType = operation.CommandType;
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
      MySqlConnection connection = new MySqlConnection(operation.DataSource.Source);
      MySqlCommand command = new MySqlCommand(operation.SourceName, connection);
      object scalar = null;

      try {
        command.CommandType = operation.CommandType;
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

  } // class MySqlMethods

} // namespace Empiria.Data.Handlers
