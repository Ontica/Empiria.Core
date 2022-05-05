/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Sql Data Handler                           Component : Data Access Library                     *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Handler                            *
*  Type     : SqlMethods                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data handler used to connect Empiria-based solutions to Microsoft Sql Server databases.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using System.EnterpriseServices;

using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Empiria.Data.Handlers {

  /// <summary>Data handler used to connect Empiria-based solutions to Microsoft Sql Server databases.</summary>
  internal class SqlMethods : IDataHandler {

    #region Internal methods

    public int AppendRows(IDbConnection connection, string tableName, DataTable table, string filter) {

      string queryString = "SELECT * FROM " + tableName;

      if (!String.IsNullOrEmpty(filter)) {
        queryString += " WHERE " + filter;
      }

      var dataAdapter = new SqlDataAdapter();

      try {

        TryOpenConnection((SqlConnection) connection);

        SqlTransaction transaction = ((SqlConnection) connection).BeginTransaction();

        dataAdapter.SelectCommand = new SqlCommand(queryString,
                                                  (SqlConnection) connection,
                                                  transaction);

        var commandBuilder = new SqlCommandBuilder(dataAdapter);

        dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
        dataAdapter.InsertCommand.Transaction = transaction;

        DataTable source = DataReader.GetDataTable(DataOperation.Parse(queryString));

        dataAdapter.Fill(source);

        for (int i = 0; i < table.Rows.Count; i++) {
          table.Rows[i].SetAdded();
          source.ImportRow(table.Rows[i]);
        }

        int result = dataAdapter.Update(source);

        transaction.Commit();

        return result;

      } finally {
        dataAdapter.Dispose();
        connection.Dispose();
      }
    }


    public int Execute(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection(connection);

        if (ContextUtil.IsInTransaction) {
          connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
        }

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
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection(connection);

        if (ContextUtil.IsInTransaction) {
          connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
        }

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
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public int Execute(IDbConnection connection, DataOperation operation) {
      var command = new SqlCommand(operation.SourceName, (SqlConnection) connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection((SqlConnection) connection);

        return command.ExecuteNonQuery();

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
      }
    }


    public int Execute(IDbTransaction transaction, DataOperation operation) {
      var command = new SqlCommand(operation.SourceName,
                                  (SqlConnection) transaction.Connection,
                                  (SqlTransaction) transaction);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection((SqlConnection) transaction.Connection);

        return command.ExecuteNonQuery();

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotExecuteActionQuery,
                                       exception,
                                       operation.SourceName, operation.ParametersToString());

      } finally {
        command.Parameters.Clear();
      }
    }


    public byte[] GetBinaryFieldValue(DataOperation operation, string fieldName) {
      byte[] value = new byte[0];

      var reader = (SqlDataReader) this.GetDataReader(operation);

      if (reader.Read()) {
        SqlBinary blob = reader.GetSqlBinary(reader.GetOrdinal(fieldName));

        value = blob.Value;
      }

      reader.Close();

      return value;
    }


    public IDbConnection GetConnection(string connectionString) {
      var connection = new SqlConnection(connectionString);

      if (ContextUtil.IsInTransaction) {
        connection.EnlistDistributedTransaction((System.EnterpriseServices.ITransaction) ContextUtil.Transaction);
      }

      return connection;
    }


    public IDataReader GetDataReader(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

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


    public DataTable GetDataTable(DataOperation operation, string dataTableName) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        var dataAdapter = new SqlDataAdapter(command);

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


    public object GetFieldValue(DataOperation operation, string fieldName) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

      try {
        operation.PrepareCommand(command);

        TryOpenConnection(connection);

        SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        if (dataReader.Read()) {
          return dataReader[fieldName];
        } else {
          return null;
        }

      } catch (ServiceException) {
        throw;

      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotGetFieldValue,
                                       exception, operation.SourceName, operation.ParametersToString(),
                                       fieldName);

      } finally {
        command.Parameters.Clear();
        connection.Dispose();
      }
    }


    public IDataParameter[] GetParameters(string source, string name, object[] values) {
      return SqlParameterCache.GetParameters(source, name, values);
    }

    public object GetScalar(DataOperation operation) {
      var connection = new SqlConnection(operation.DataSource.Source);
      var command = new SqlCommand(operation.SourceName, connection);

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


    static internal void TryOpenConnection(SqlConnection connection) {
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

  } // class SqlMethods

} // namespace Empiria.Data.Handlers
