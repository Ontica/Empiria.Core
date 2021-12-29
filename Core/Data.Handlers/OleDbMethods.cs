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
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        var dataAdapter = new OleDbDataAdapter(command);

        var dataTable = new DataTable();

        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();

        return dataTable.Rows.Count;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public int Execute(DataOperation operation) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        connection.Open();

        return command.ExecuteNonQuery();

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public T Execute<T>(DataOperation operation) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        connection.Open();

        object result = command.ExecuteScalar();

        if (result != null) {
          return (T) result;
        } else {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.ActionQueryDoesntReturnAValue,
                                         operation.SourceName);
        }

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public int Execute(IDbConnection connection, DataOperation operation) {
      var command = new OleDbCommand(operation.SourceName, (OleDbConnection) connection);

      try {
        operation.PrepareCommand(command);

        return command.ExecuteNonQuery();

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
      }
    }


    public int Execute(IDbTransaction transaction, DataOperation operation) {
      var command = new OleDbCommand(operation.SourceName,
                                     (OleDbConnection) transaction.Connection,
                                     (OleDbTransaction) transaction);
      try {
        operation.PrepareCommand(command);

        return command.ExecuteNonQuery();

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
      }
    }


    public IDataParameter[] GetParameters(string source, string name, object[] values) {
      return OleDbParameterCache.GetParameters(source, name, values);
    }


    public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      throw new NotImplementedException();
    }


    public IDbConnection GetConnection(string connectionString) {
      return new OleDbConnection(connectionString);
    }

    public IDataReader GetDataReader(DataOperation operation) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        connection.Open();

        return command.ExecuteReader(CommandBehavior.CloseConnection);

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataReader,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        //Don't dispose the connection because this method returns a DataReader.
      }
    }


    public DataRow GetDataRow(DataOperation operation) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        var dataAdapter = new OleDbDataAdapter(command);

        var dataTable = new DataTable(operation.SourceName);

        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();

        if (dataTable.Rows.Count != 0) {
          return dataTable.Rows[0];
        } else {
          return null;
        }

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public DataTable GetDataTable(DataOperation operation, string dataTableName) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        var dataAdapter = new OleDbDataAdapter(command);

        var dataTable = new DataTable(dataTableName);

        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();

        return dataTable;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataTable,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public DataView GetDataView(DataOperation operation, string filter, string sort) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);

        DataTable dataTable = new DataTable(operation.SourceName);

        dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();

        return new DataView(dataTable, filter, sort,
                            DataViewRowState.CurrentRows);

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataView,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public object GetFieldValue(DataOperation operation, string fieldName) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        connection.Open();

        OleDbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        if (dataReader.Read()) {
          return dataReader[fieldName];
        } else {
          return null;
        }

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetFieldValue,
                                       exception,
                                       operation.SourceName, operation.ParametersToString(),
                                       fieldName);

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public object GetScalar(DataOperation operation) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        connection.Open();

        return command.ExecuteScalar();

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetScalar,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }

    #endregion Internal methods

  } // class OleDbMethods

} // namespace Empiria.Data.Handlers
