/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Core                                     System  : Data Access Library                 *
*  Assembly : Empiria.Core.dll                                 Pattern : Interface                           *
*  Type     : IDataHandler                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Defines data handling methods to connect Empiria solutions to different database providers.    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

namespace Empiria.Data.Handlers {

  /// <summary>Static class with methods that performs data reading operations.</summary>
  public interface IDataHandler {

    int AppendRows(IDbConnection connection, string tableName, DataTable table, string filter);


    int Execute(DataOperation operation);


    int Execute(IDbConnection connection, DataOperation operation);


    int Execute(IDbTransaction transaction, DataOperation operation);


    T Execute<T>(DataOperation operation);


    byte[] GetBinaryFieldValue(DataOperation operation, string fieldName);


    IDbConnection GetConnection(string connectionString);


    IDataReader GetDataReader(DataOperation operation);


    DataTable GetDataTable(DataOperation operation, string dataTableName);


    object GetFieldValue(DataOperation operation, string fieldName);


    IDataParameter[] GetParameters(string source, string name, object[] values);


    object GetScalar(DataOperation operation);

  }  // interface IDataHandler

}  // namespace Empiria.Data.Handlers
