/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            Assembly : Empiria.Data.dll                  *
*  Type      : OracleMethods                                    Pattern  : Static Class                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : static internal class to read data stored in Oracle® databases.                               *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;
using System.Data.OracleClient;
using System.EnterpriseServices;
using System.Security.Permissions;

namespace Empiria.Data.Handlers {

  [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "8b7fe9c60c0f43bd")]
  static internal class OracleMethods {

    #region Internal methods

    static internal int CountRows(DataOperation operation) {
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable();

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
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
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);

      int affectedRows = 0;
      try {
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        connection.Open();
        if (ContextUtil.IsInTransaction) {
          connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
        }
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

    static internal int Execute(OracleConnection connection, DataOperation operation) {
      OracleCommand command = new OracleCommand(operation.SourceName, connection);

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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }

    static internal int Execute(OracleTransaction transaction, DataOperation operation) {
      OracleCommand command = new OracleCommand(operation.SourceName, transaction.Connection, transaction);

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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery, exception,
                                       operation.SourceName, parametersString);
      } finally {
        command.Parameters.Clear();
      }
      return affectedRows;
    }

    static internal OracleConnection GetConnection(string connectionString) {
      OracleConnection connection = new OracleConnection(connectionString);
      if (ContextUtil.IsInTransaction) {
        connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
      }
      return connection;
    }

    static internal IDataReader GetDataReader(DataOperation operation) {
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);
      OracleDataReader dataReader;

      try {
        command.CommandType = operation.CommandType;
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
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
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

    static internal DataTable GetDataTable(DataOperation operation, string dataTableName) {
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(dataTableName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
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

    static internal DataView GetDataView(DataOperation operation, string filter, string sort) {
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);
      DataTable dataTable = new DataTable(operation.SourceName);

      try {
        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
        command.CommandType = operation.CommandType;
        operation.FillParameters(command);
        OracleDataAdapter dataAdapter = new OracleDataAdapter(command);
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

    static internal object GetFieldValue(DataOperation operation, string fieldName) {
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);
      OracleDataReader dataReader;
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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetFieldValue,
                                       exception, operation.SourceName, fieldName);
      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
      return fieldValue;
    }

    static internal object GetScalar(DataOperation operation) {
      OracleConnection connection = new OracleConnection(operation.DataSource.Source);
      OracleCommand command = new OracleCommand(operation.SourceName, connection);

      try {
        command.CommandType = operation.CommandType;
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

  } // class OracleMethods

} // namespace Empiria.Data.Handlers
