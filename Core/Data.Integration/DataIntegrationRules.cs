/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Data Services                              Component : Data Integration Services               *
*  Assembly : Empiria.Core.dll                           Pattern   : Static class with data                  *
*  Type     : DataIntegrationRules                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides reading services for data integration rules.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Security;

namespace Empiria.Data.Integration {

  /// <summary>Provides reading services for data integration rules.</summary>
  static internal class DataIntegrationRules {

    #region Fields

    static private Dictionary<string, List<DataIntegrationRule>> dataIntegrationRules = null;

    #endregion Fields

    #region Methods

    static internal WebServer GetObjectIdServer(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules["I@" + sourceName.ToUpperInvariant()][0].TargetServer;
    }


    static internal DataIntegrationRule GetPostExecutionRule(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules["E@" + sourceName.ToUpperInvariant()][0];
    }


    static internal List<DataIntegrationRule> GetPublishRules(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules["P@" + sourceName.ToUpperInvariant()];
    }


    static internal WebServer GetReadRuleServer(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules["R@" + sourceName.ToUpperInvariant()][0].TargetServer;
    }


    static internal DataIntegrationRule GetWriteRule(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules["W@" + sourceName.ToUpperInvariant()][0];
    }


    static internal bool HasCachingRule(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules.ContainsKey("C@" + sourceName.ToUpperInvariant());
    }


    static internal bool HasExternalClusterCreateIdRule(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules.ContainsKey("I@" + sourceName.ToUpperInvariant());
    }


    static internal bool HasPostExecutionTask(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules.ContainsKey("E@" + sourceName.ToUpperInvariant());
    }


    static internal bool HasPublishRule(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules.ContainsKey("P@" + sourceName.ToUpperInvariant());
    }


    static internal bool HasReadRule(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules.ContainsKey("R@" + sourceName.ToUpperInvariant());
    }


    static internal bool HasWriteRule(string sourceName) {
      EnsureRulesAreLoaded();

      return dataIntegrationRules.ContainsKey("W@" + sourceName.ToUpperInvariant());
    }

    #endregion Methods

    #region Helpers

    static private void EnsureRulesAreLoaded() {
      if (dataIntegrationRules != null) {
        return;
      }

      LoadRulesCache();
    }


    static private void LoadRulesCache() {
      try {

        DataOperation operation = DataOperation.Parse("qryDBIntegrationRules", ExecutionServer.ServerId);

        DataTable table = DataReader.GetInternalDataTable(operation, "qryDBIntegrationRules");

        dataIntegrationRules = new Dictionary<string, List<DataIntegrationRule>>();

        for (int i = 0; i < table.Rows.Count; i++) {
          string dictionaryKey = (string) table.Rows[i]["DbRuleType"] + "@" +
                                 ((string) table.Rows[i]["SourceName"]).ToUpperInvariant();

          int integrationRuleId = (int) table.Rows[i]["DbRuleId"];

          string condition = (string) table.Rows[i]["DbRuleCondition"];

          WebServer targetServer = DataIntegratorWSProxy.GetIntegrationServer((int) table.Rows[i]["TargetServerId"]);

          DataIntegrationRulePriority priority = (DataIntegrationRulePriority) (short) table.Rows[i]["DbRulePriority"];

          DataIntegrationRule dataIntegrationRule = new DataIntegrationRule(integrationRuleId, targetServer, condition, priority);

          if (dataIntegrationRules.ContainsKey(dictionaryKey)) {
            dataIntegrationRules[dictionaryKey].Add(dataIntegrationRule);

          } else {
            List<DataIntegrationRule> list = new List<DataIntegrationRule>();
            list.Add(dataIntegrationRule);
            dataIntegrationRules.Add(dictionaryKey, list);

          }

        } // for

      } catch (ServiceException) {

        dataIntegrationRules = null;

        throw;

      } catch (Exception exception) {

        dataIntegrationRules = null;

        throw new EmpiriaDataException(EmpiriaDataException.Msg.CannotLoadDataIntegrationRules, exception);

      }
    }

    #endregion Helpers

  } //class DataIntegrationRules

} // namespace Empiria.Data.Integration
