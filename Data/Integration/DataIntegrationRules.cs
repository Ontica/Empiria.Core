/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         Assembly : Empiria.Data.dll                  *
*  Type      : DataIntegrationRules                             Pattern  : Static Class With Instances Cache *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Provides read and write data operations allocated on external servers throw web services.     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Security;

namespace Empiria.Data.Integration {

  /// <summary>Provides read and write data operations allocated on external servers throw web services.</summary>
  //[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey="8b7fe9c60c0f43bd")]
  static internal class DataIntegrationRules {

    #region Fields

    static private Dictionary<string, List<DataIntegrationRule>> dataIntegrationRules = null;

    #endregion Fields

    static DataIntegrationRules() {
      LoadRulesCache();
    }

    #region Internal methods

    static internal IEmpiriaServer GetObjectIdServer(string sourceName) {
      return dataIntegrationRules["I@" + sourceName.ToLowerInvariant()][0].TargetServer;
    }

    static internal DataIntegrationRule GetPostExecutionRule(string sourceName) {
      return dataIntegrationRules["E@" + sourceName.ToLowerInvariant()][0];
    }

    static internal List<DataIntegrationRule> GetPublishRules(string sourceName) {
      return dataIntegrationRules["P@" + sourceName.ToLowerInvariant()];
    }

    static internal IEmpiriaServer GetReadRuleServer(string sourceName) {
      return dataIntegrationRules["R@" + sourceName.ToLowerInvariant()][0].TargetServer;
    }

    static internal DataIntegrationRule GetWriteRule(string sourceName) {
      return dataIntegrationRules["W@" + sourceName.ToLowerInvariant()][0];
    }

    static internal bool HasCachingRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("C@" + sourceName.ToLowerInvariant());
    }

    static internal bool HasExternalCreateIdRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("I@" + sourceName.ToLowerInvariant());
    }

    static internal bool HasPostExecutionTask(string sourceName) {
      return dataIntegrationRules.ContainsKey("E@" + sourceName.ToLowerInvariant());
    }

    static internal bool HasPublishRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("P@" + sourceName.ToLowerInvariant());
    }

    static internal bool HasReadRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("R@" + sourceName.ToLowerInvariant());
    }

    static internal bool HasWriteRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("W@" + sourceName.ToLowerInvariant());
    }

    #endregion Internal methods

    #region Private methods

    static private void LoadRulesCache() {
      try {
        dataIntegrationRules = new Dictionary<string, List<DataIntegrationRule>>();

        DataOperation operation = DataOperation.Parse("qryDBIntegrationRules", ExecutionServer.ServerId);
        DataTable table = DataReader.GetInternalDataTable(operation, "qryDBIntegrationRules");

        for (int i = 0; i < table.Rows.Count; i++) {
          string dictionaryKey = (string) table.Rows[i]["DbRuleType"] + "@" +
                                 ((string) table.Rows[i]["SourceName"]).ToLowerInvariant();

          int integrationRuleId = (int) table.Rows[i]["DbRuleId"];
          string condition = (string) table.Rows[i]["DbRuleCondition"];
          IEmpiriaServer targetServer = DataIntegratorWSProxy.GetIntegrationServer((int) table.Rows[i]["TargetServerId"]);
          DataIntegrationRulePriority priority = (DataIntegrationRulePriority) (short) table.Rows[i]["DbRulePriority"];
          DataIntegrationRule dataIntegrationRule = new DataIntegrationRule(integrationRuleId, targetServer, condition, priority);

          if (dataIntegrationRules.ContainsKey(dictionaryKey)) {
            dataIntegrationRules[dictionaryKey].Add(dataIntegrationRule);
          } else {
            List<DataIntegrationRule> list = new List<DataIntegrationRule>();
            list.Add(dataIntegrationRule);
            dataIntegrationRules.Add(dictionaryKey, list);
          } // if
        } // for
      } catch (Exception exception) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantLoadDataIntegrationRules, exception);
      } // try
    }

    #endregion Private methods

  } //class DataIntegrationRules

} // namespace Empiria.Data.Integration