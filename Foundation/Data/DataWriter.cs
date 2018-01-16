/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution : Empiria Foundation Framework                     System  : Data Access Library                 *
*  Assembly : Empiria.Foundation.dll                           Pattern : Static Class                        *
*  Type     : DataWriter                                       License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Static class with methods that performs data writing operations.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

using Empiria.Data.Handlers;
using Empiria.Data.Integration;
using Empiria.Reflection;
using Empiria.Security;
using Empiria.WebApi.Client;

namespace Empiria.Data {

  /// <summary>Static class with methods that performs data writing operations.</summary>
  static public class DataWriter {

    #region Fields

    static private readonly bool IsObjectIdGeneratorServer =
                                        ConfigurationData.Get<Boolean>("IsObjectIdGeneratorServer", false);

    #endregion Fields

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
      if (DataIntegrationRules.HasExternalClusterCreateIdRule(sourceName)) {
        return CreateExternalClusterIdAsync(sourceName).Result;
      }
      return CreateThisClusterIdAsync(sourceName).Result;
    }

    static public int Execute(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (StorageContext.IsStorageContextDefined) {
        return StorageContext.ActiveStorageContext.Add(operation);
      }

      if (DataIntegrationRules.HasWriteRule(operation.SourceName)) {
        return ExecuteExternal(operation);
      }

      int result = DataWriter.ExecuteInternal(operation);
      DataWriter.WriteDataLog(operation);

      DataWriter.DoPostExecutionTask(operation);


      DataPublisher.Publish(operation);

      return result;
    }


    static public T Execute<T>(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      //if (StorageContext.IsStorageContextDefined) {
      //  return StorageContext.ActiveStorageContext.Add(operation);
      //}

      //if (DataIntegrationRules.HasWriteRule(operation.SourceName)) {
      //  return ExecuteExternal(operation);
      //}

      T result = DataWriter.ExecuteInternal<T>(operation);
      DataWriter.WriteDataLog(operation);

      DataWriter.DoPostExecutionTask(operation);

      DataPublisher.Publish(operation);

      return result;
    }

    static public int Execute(DataOperationList operationList) {
      Assertion.AssertObject(operationList, "operationList");

      if (StorageContext.IsStorageContextDefined) {
        return StorageContext.ActiveStorageContext.Add(operationList);
      }

      using (DataWriterContext context = DataWriter.CreateContext(operationList.Name)) {
        ITransaction transaction = context.BeginTransaction();

        context.Add(operationList);
        context.Update();
        return transaction.Commit();
      }
    }

    static public int Execute(SingleSignOnToken token, DataOperation operation) {
      Assertion.AssertObject(token, "token");
      Assertion.AssertObject(operation, "operation");

      if (DataIntegrationRules.HasWriteRule(operation.SourceName)) {
        return ExecuteExternal(operation);
      }

      int result = ExecuteInternal(operation);

      DoPostExecutionTask(operation);

      DataPublisher.Publish(token, operation);

      return result;
    }

    static public int Execute(SingleSignOnToken token, DataOperationList operationList) {
      Assertion.AssertObject(token, "token");
      Assertion.AssertObject(operationList, "operationList");

      using (DataWriterContext context = DataWriter.CreateContext(operationList.Name)) {
        ITransaction transaction = context.BeginTransaction();

        context.Add(token, operationList);
        context.Update();

        return transaction.Commit();
      }
    }

    #endregion Public methods

    #region Internal methods

    static internal int Execute(IDbConnection connection, DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute(connection, operation);
    }

    static internal int Execute(IDbTransaction transaction, DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute((System.Data.SqlClient.SqlTransaction) transaction, operation);
    }

    static internal int ExecuteInternal(DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute(operation);
    }

    private static T ExecuteInternal<T>(DataOperation operation) {
      IDataHandler handler = GetDataHander(operation);

      return handler.Execute<T>(operation);
    }

    #endregion Internal methods

    #region Private methods

    static private async Task<int> CreateThisClusterIdAsync(string sourceName) {
      if (DataWriter.IsObjectIdGeneratorServer) {
        return ObjectIdFactory.Instance.GetNextId(sourceName, 0);
      } else {
        IWebApiClient apiClient = WebApiClientFactory.CreateWebApiClient();

        return await apiClient.GetAsync<int>("Empiria.IdGenerator.NextTableRowId", sourceName);
      }
    }

    static private async Task<int> CreateExternalClusterIdAsync(string sourceName) {
      WebServer targetServer = DataIntegrationRules.GetObjectIdServer(sourceName);

      IWebApiClient apiClient = WebApiClientFactory.CreateWebApiClient(targetServer.WebSiteURL);

      return await apiClient.GetAsync<int>("Empiria.IdGenerator.NextTableRowId", sourceName);
    }

    static private IDataHandler GetDataHander(DataOperation operation) {
      return operation.DataSource.GetDataHandler();
    }

    static private void DoPostExecutionTask(DataOperation operation) {
      if (!DataIntegrationRules.HasPostExecutionTask(operation.SourceName)) {
        return;
      }

      DataIntegrationRule postExecutionRule = null;
      try {
        postExecutionRule = DataIntegrationRules.GetPostExecutionRule(operation.SourceName);
        string assemblyName = postExecutionRule.Condition.Split('|')[0];
        string typeName = postExecutionRule.Condition.Split('|')[1];
        string methodName = postExecutionRule.Condition.Split('|')[2];

        Type type = ObjectFactory.GetType(assemblyName, typeName);

        MethodInfo method = type.GetMethod(methodName, BindingFlags.ExactBinding | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                                           null, CallingConventions.Any, new Type[] { typeof(DataOperation) }, null);

        method.Invoke(null, new object[] { operation });

      } catch (Exception innerException) {
        DataPublisher.Publish(postExecutionRule, operation, innerException);
        Exception exception = new EmpiriaDataException(EmpiriaDataException.Msg.CannotDoPostExecutionTask, innerException,
                                                       operation.SourceName, operation.Parameters.ToString());
        if (postExecutionRule.Priority == DataIntegrationRulePriority.Chained) {
          throw exception;
        }
      }
    }

    static private int ExecuteExternal(DataOperation dataOperation) {
      DataIntegrationRule rule = DataIntegrationRules.GetWriteRule(dataOperation.SourceName);

      using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(rule.TargetServer)) {
        SingleSignOnToken token = SingleSignOnToken.Create(rule.TargetServer);
        return proxy.Execute(token.ToMessage(), dataOperation.ToMessage());
      }
    }

    static private void WriteDataLog(DataOperation operation) {
      var dataLog = new DataLog(operation);

      dataLog.Write();
    }

    #endregion Private methods

  } //class DataWriter

} //namespace Empiria.Data
