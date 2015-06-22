/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         Assembly : Empiria.Data.dll                  *
*  Type      : DataIntegrationRules                             Pattern  : Static Class With Instances Cache *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides read and write data operations allocated on external servers throw web services.     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      return dataIntegrationRules["I@" + sourceName.ToUpperInvariant()][0].TargetServer;
    }

    static internal DataIntegrationRule GetPostExecutionRule(string sourceName) {
      return dataIntegrationRules["E@" + sourceName.ToUpperInvariant()][0];
    }

    static internal List<DataIntegrationRule> GetPublishRules(string sourceName) {
      return dataIntegrationRules["P@" + sourceName.ToUpperInvariant()];
    }

    static internal IEmpiriaServer GetReadRuleServer(string sourceName) {
      return dataIntegrationRules["R@" + sourceName.ToUpperInvariant()][0].TargetServer;
    }

    static internal DataIntegrationRule GetWriteRule(string sourceName) {
      return dataIntegrationRules["W@" + sourceName.ToUpperInvariant()][0];
    }

    static internal bool HasCachingRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("C@" + sourceName.ToUpperInvariant());
    }

    static internal bool HasExternalCreateIdRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("I@" + sourceName.ToUpperInvariant());
    }

    static internal bool HasPostExecutionTask(string sourceName) {
      return dataIntegrationRules.ContainsKey("E@" + sourceName.ToUpperInvariant());
    }

    static internal bool HasPublishRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("P@" + sourceName.ToUpperInvariant());
    }

    static internal bool HasReadRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("R@" + sourceName.ToUpperInvariant());
    }

    static internal bool HasWriteRule(string sourceName) {
      return dataIntegrationRules.ContainsKey("W@" + sourceName.ToUpperInvariant());
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
                                 ((string) table.Rows[i]["SourceName"]).ToUpperInvariant();

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
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotLoadDataIntegrationRules, exception);
      } // try
    }

    #endregion Private methods

  } //class DataIntegrationRules

} // namespace Empiria.Data.Integration
