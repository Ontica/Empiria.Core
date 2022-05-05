/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : OleDb Database Handler                     Component : Data Access Library                     *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Handler                            *
*  Type     : OleDbMethods                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data handler used to connect Empiria-based solutions to OleDb-based databases.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;
using System.Data.OleDb;

namespace Empiria.Data.Handlers {

  /// <summary> Data handler used to connect Empiria-based solutions with OleDb-based databases.</summary>
  internal class OleDbMethods : IDataHandler {

    #region Internal methods

    public int AppendRows(IDbConnection connection, string tableName,
                          DataTable table, string filter) {
      throw new NotImplementedException();
    }


    public int CountRows(DataOperation operation) {
      var dataTable = GetDataTable(operation, String.Empty);

      return dataTable.Rows.Count;
    }


    public int Execute(DataOperation operation) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection(connection);

        return command.ExecuteNonQuery();

      } catch (ServiceException) {
        throw;

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

        TryOpenConnection(connection);

        object result = command.ExecuteScalar();

        if (result != null) {
          return (T) result;
        } else {
          throw new EmpiriaDataException(EmpiriaDataException.Msg.ActionQueryDoesntReturnAValue,
                                         operation.SourceName);
        }

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception, operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public int Execute(IDbConnection connection, DataOperation operation) {
      var command = new OleDbCommand(operation.SourceName, (OleDbConnection) connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection((OleDbConnection) connection);

        return command.ExecuteNonQuery();

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception, operation.SourceName, operation.ParametersToString());

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

        TryOpenConnection((OleDbConnection) transaction.Connection);

        return command.ExecuteNonQuery();

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception,  operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
      }
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

        TryOpenConnection(connection);

        return command.ExecuteReader(CommandBehavior.CloseConnection);

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetDataReader,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        // Do not dispose the connection because this method returns a DataReader.
      }
    }


    public DataRow GetDataRow(DataOperation operation) {
      DataTable dataTable = GetDataTable(operation, operation.SourceName);

      if (dataTable.Rows.Count != 0) {
        return dataTable.Rows[0];
      } else {
        return null;
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

        TryOpenConnection(connection);

        dataAdapter.Fill(dataTable);
        dataAdapter.Dispose();

        return dataTable;

      } catch (ServiceException) {
        throw;

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
      DataTable dataTable = GetDataTable(operation, operation.SourceName);

      return new DataView(dataTable, filter, sort, DataViewRowState.CurrentRows);
    }


    public object GetFieldValue(DataOperation operation, string fieldName) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection(connection);

        OleDbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        if (dataReader.Read()) {
          return dataReader[fieldName];
        } else {
          return null;
        }

      } catch (ServiceException) {
        throw;

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


    public IDataParameter[] GetParameters(string source, string name, object[] values) {
      return OleDbParameterCache.GetParameters(source, name, values);
    }


    public object GetScalar(DataOperation operation) {
      var connection = new OleDbConnection(operation.DataSource.Source);
      var command = new OleDbCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection(connection);

        return command.ExecuteScalar();

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetScalar,
                                       exception, operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    static internal void TryOpenConnection(OleDbConnection connection) {
      try {
        connection.Open();

      } catch (Exception innerException) {
        throw new ServiceException("DATABASE_SERVER_CONNECTION_FAILED",
          "No se pudo hacer una conexión del sistema con la base de datos. " +
          "Puede ser que el servidor de base de datos no esté disponible, " +
          "o que la conexión entre el servidor de base de datos " +
          "y el servidor de aplicaciones se haya perdido.",
          innerException);
      }
    }

    #endregion Internal methods

  } // class OleDbMethods

} // namespace Empiria.Data.Handlers
