/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Core                                     System  : Data Access Library                 *
*  Assembly : Empiria.Core.dll                                 Pattern : Static Class                        *
*  Type     : DataWriter                                       License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Static class with methods that performs data writing operations.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Data.Handlers;

namespace Empiria.Data {

  /// <summary>Static class with methods that performs data writing operations.</summary>
  static public class DataWriter {

    #region Public methods

    static public int AppendRows(string tableName, DataTable table, string filter) {
      DataSource dataSource = DataSource.Parse(tableName);

      IDbConnection connection = dataSource.GetConnection();
      IDataHandler handler = dataSource.GetDataHandler();

      return handler.AppendRows(connection, tableName, table, filter);
    }


    static public Guid CreateGuid() {
      return Guid.NewGuid();
    }


    static public DataWriterContext CreateContext(string contextName) {
      return new DataWriterContext(contextName);
    }


    static public int CreateId(string sourceName) {
      return ObjectIdFactory.Instance.GetNextId(sourceName, 0);
    }


    static public void Execute(DataOperation operation) {
      Assertion.Require(operation, "operation");

      if (StorageContext.IsStorageContextDefined) {

        StorageContext.ActiveStorageContext.Add(operation);

      } else {

        ExecuteInternal(operation);
      }

      WriteDataLog(operation);
    }


    static public void Execute(DataOperationList operationList) {
      Assertion.Require(operationList, "operationList");

      foreach (var operation in operationList) {
        Execute(operation);
      }
    }


    static public T Execute<T>(DataOperation operation) {
      T result = DataWriter.ExecuteInternal<T>(operation);

      WriteDataLog(operation);

      return result;
    }


    #endregion Public methods

    #region Internal methods

    static internal int Execute(IDbConnection connection, DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute(connection, operation);
    }


    static internal int Execute(IDbTransaction transaction, DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute(transaction, operation);
    }


    static internal int ExecuteInternal(DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute(operation);
    }


    static private T ExecuteInternal<T>(DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute<T>(operation);
    }


    static internal void ExecuteInternal(DataOperationList operationList) {
      if (operationList.Count == 0) {
        return;
      }

      if (operationList.Count == 1) {
        ExecuteInternal(operationList[0]);

        return;
      }

      using (DataWriterContext context = DataWriter.CreateContext(operationList.Name)) {
        ITransaction transaction = context.BeginTransaction();

        context.Add(operationList);

        transaction.Commit();
      }
    }


    #endregion Internal methods

    #region Private methods

    static private IDataHandler GetDataHander(DataOperation operation) {
      return operation.DataSource.GetDataHandler();
    }


    static private void WriteDataLog(DataOperation operation) {
      var dataLog = new DataLog(operation);

      dataLog.Write();
    }

    #endregion Private methods

  } //class DataWriter

} //namespace Empiria.Data
