/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         Assembly : Empiria.Data.dll                  *
*  Type      : DataIntegrator                                   Pattern  : Static Class                      *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides read and write data operations allocated on external servers throw web services.     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

using Empiria.Security;

namespace Empiria.Data.Integration {

  /// <summary>Provides read and write data operations allocated on external servers throw web services.</summary>
  static public class DataPublisher {

    #region Public methods

    static public void Publish(DataOperation operation) {
      Assertion.AssertObject(operation, "operation");

      if (!DataIntegrationRules.HasPublishRule(operation.SourceName)) {
        return;
      }

      List<DataIntegrationRule> publishRules = DataIntegrationRules.GetPublishRules(operation.SourceName);
      for (int i = 0; i < publishRules.Count; i++) {
        DataIntegrationRule publishRule = publishRules[i];

        if (!publishRule.IsCandidate(operation)) {
          continue;
        }
        SingleSignOnToken token = SingleSignOnToken.Create(publishRule.TargetServer);
        if (publishRule.Priority == DataIntegrationRulePriority.Chained) {
          using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(publishRule.TargetServer)) {
            proxy.Execute(token.ToMessage(), operation.ToMessage());
          }
        } else {
          SendToQueue(token, publishRule, i, operation);
        }
      }
    }

    static public void Publish(DataOperationList operationList) {
      Assertion.AssertObject(operationList, "operationList");

      if (!DataIntegrationRules.HasPublishRule(operationList.Name)) {
        return;
      }

      List<DataIntegrationRule> publishRules = DataIntegrationRules.GetPublishRules(operationList.Name);
      for (int i = 0; i < publishRules.Count; i++) {
        DataIntegrationRule publishRule = publishRules[i];

        if (!publishRule.IsCandidate(operationList)) {
          continue;
        }
        SingleSignOnToken token = SingleSignOnToken.Create(publishRule.TargetServer);
        if (publishRule.Priority == DataIntegrationRulePriority.Chained) {
          using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(publishRule.TargetServer)) {
            proxy.ExecuteList(token.ToMessage(), operationList.ToMessagesArray());
          }
        } else {
          SendToQueue(token, publishRule, operationList);
        }
      }
    }

    #endregion Public methods

    #region Internal methods

    static internal DataOperationList GetPublishOperations(DataWriterContext context, DataOperation operation) {
      List<DataIntegrationRule> publishRules = DataIntegrationRules.GetPublishRules(operation.SourceName);
      DataOperationList dataOperations = new DataOperationList(context.Name);

      for (int i = 0; i < publishRules.Count; i++) {
        DataIntegrationRule publishRule = publishRules[i];

        if (publishRule.IsCandidate(operation)) {
          SingleSignOnToken token = SingleSignOnToken.Create(publishRule.TargetServer);
          DataOperation publishOperation = GetPublishDataOperation(token, publishRule.Id, context.Guid,
                                                                   context.Name, operation, i);
          dataOperations.Add(publishOperation);
        }
      }

      return dataOperations;
    }

    static internal DataOperationList GetPublishOperations(SingleSignOnToken token,
                                                           DataWriterContext context, DataOperation operation) {
      List<DataIntegrationRule> publishRules = DataIntegrationRules.GetPublishRules(operation.SourceName);
      DataOperationList dataOperations = new DataOperationList(context.Name);

      for (int i = 0; i < publishRules.Count; i++) {
        DataIntegrationRule publishRule = publishRules[i];

        if (publishRule.IsCandidate(token, operation)) {
          SingleSignOnToken newToken = token.SignOnServer(publishRule.TargetServer);
          DataOperation publishOperation = GetPublishDataOperation(newToken, publishRule.Id, context.Guid,
                                                                   context.Name, operation, i);
          dataOperations.Add(publishOperation);
        }
      }

      return dataOperations;
    }

    static public void Publish(SingleSignOnToken token, DataOperation operation) {
      if (!DataIntegrationRules.HasPublishRule(operation.SourceName)) {
        return;
      }

      List<DataIntegrationRule> publishRules = DataIntegrationRules.GetPublishRules(operation.SourceName);

      for (int i = 0; i < publishRules.Count; i++) {
        DataIntegrationRule publishRule = publishRules[i];
        if (!publishRule.IsCandidate(token, operation)) {
          continue;
        }
        if (publishRule.Priority == DataIntegrationRulePriority.Chained) {
          using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(publishRule.TargetServer)) {
            SingleSignOnToken newToken = token.SignOnServer(publishRule.TargetServer);
            proxy.Execute(newToken.ToMessage(), operation.ToMessage());
          }
        } else {
          SingleSignOnToken newToken = token.SignOnServer(publishRule.TargetServer);
          SendToQueue(newToken, publishRule, 0, operation);
        }
      }
    }

    static internal void Publish(DataIntegrationRule rule, DataOperation operation, Exception innerException) {
      SingleSignOnToken token = SingleSignOnToken.Create(rule.TargetServer);

      SendToQueue(token, rule, 0, operation);
    }

    static public void Publish(SingleSignOnToken token, DataOperationList operationList) {
      if (!DataIntegrationRules.HasPublishRule(operationList.Name)) {
        return;
      }

      List<DataIntegrationRule> publishRules = DataIntegrationRules.GetPublishRules(operationList.Name);

      for (int i = 0; i < publishRules.Count; i++) {
        DataIntegrationRule publishRule = publishRules[i];
        if (!publishRule.IsCandidate(token, operationList)) {
          continue;
        }
        if (publishRule.Priority == DataIntegrationRulePriority.Chained) {
          using (DataIntegratorWSProxy proxy = new DataIntegratorWSProxy(publishRule.TargetServer)) {
            SingleSignOnToken newToken = token.SignOnServer(publishRule.TargetServer);
            proxy.ExecuteList(newToken.ToMessage(), operationList.ToMessagesArray());
          }
        } else {
          SingleSignOnToken newToken = token.SignOnServer(publishRule.TargetServer);
          SendToQueue(newToken, publishRule, operationList);
        }
      }
    }

    #endregion Internal methods

    #region Private methods

    static private DataOperation GetPublishDataOperation(SingleSignOnToken token, int dataIntegrationRuleId,
                                                         Guid unitOfWorkGuid, string contextName,
                                                         DataOperation operation, int operationIndex) {
      int dataTaskId = DataWriter.CreateInternalId("DbIntegrationTasks");

      return DataOperation.Parse("writeDBIntegrationTask", dataTaskId, dataIntegrationRuleId, token.ToMessage(),
                                 unitOfWorkGuid, contextName, operation.SourceName,
                                 operation.ParametersToString(), operationIndex, DateTime.Now,
                                 ExecutionServer.DateMaxValue, 'N', -1, 'P');
    }

    static private void SendToQueue(SingleSignOnToken token, DataIntegrationRule publishRule,
                                    int dataTaskIndex, DataOperation operation) {
      try {
        DataOperation publishOperation = GetPublishDataOperation(token, publishRule.Id, Guid.Empty, "Null",
                                                             operation, dataTaskIndex);
        DataWriter.ExecuteInternal(publishOperation);
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotCreateDataTask, exception,
                                       ExecutionServer.ServerId, publishRule.TargetServerId,
                                       operation.SourceName, operation.ParametersToString());
      }
    }

    static private void SendToQueue(SingleSignOnToken token, DataIntegrationRule publishRule,
                                    DataOperationList operationList) {
      try {
        Guid guid = Guid.NewGuid();
        for (int i = 0; i < operationList.Count; i++) {
          DataOperation publishOperation = GetPublishDataOperation(token, publishRule.Id, guid, operationList.Name,
                                                               operationList[i], i);
          DataWriter.ExecuteInternal(publishOperation);
        }
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotCreateDataTask, exception,
                                       ExecutionServer.ServerId, publishRule.TargetServerId,
                                       operationList.Name, operationList.Count);
      }
    }

    #endregion Private methods

  } //class DataIntegrator

} // namespace Empiria.Data.Integration
