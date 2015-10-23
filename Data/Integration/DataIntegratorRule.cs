/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         Assembly : Empiria.Data.dll                  *
*  Type      : DataIntegrationRule                              Pattern  : Standard Class                    *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Class used for create messaging queues based on message type and content rules.               *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Security;

namespace Empiria.Data.Integration {

  #region Enumerations

  internal enum DataIntegrationRulePriority {
    Lowest = 0,
    BelowNormal = 1,
    Normal = 2,
    AboveNormal = 3,
    Highest = 4,
    Chained = 5,
  }

  #endregion Enumerations

  /// <summary>Represents a data integration rule.</summary>
  //[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey="8b7fe9c60c0f43bd")]
  internal class DataIntegrationRule : IIdentifiable {

    #region Fields

    private int id = 0;
    private IEmpiriaServer targetServer = null;
    private string condition = String.Empty;
    DataIntegrationRulePriority priority = DataIntegrationRulePriority.Normal;

    #endregion Fields

    #region Constructors and Parsers

    internal DataIntegrationRule(int id, IEmpiriaServer targetServer, string condition, DataIntegrationRulePriority priority) {
      this.id = id;
      this.targetServer = targetServer;
      this.condition = condition;
      this.priority = priority;
    }

    static internal string CreateDefaultTaskMessage() {
      return ExecutionServer.ServerId.ToString();
    }

    #endregion Constructors and Parsers

    #region Internal properties

    internal string Condition {
      get { return condition; }
    }

    public int Id {
      get { return id; }
    }

    internal DataIntegrationRulePriority Priority {
      get { return priority; }
    }

    internal IEmpiriaServer TargetServer {
      get { return targetServer; }
    }

    internal int TargetServerId {
      get { return ((targetServer != null) ? targetServer.Id : 0); }
    }

    #endregion Internal properties

    #region Internal methods

    internal bool IsCandidate(DataOperation operation) {
      if (condition.Length == 0) {
        return true;
      }

      string[] conditionsArray = condition.Split('|');
      for (int i = 0; i < conditionsArray.Length; i++) {
        string singleCondition = conditionsArray[i].Trim();
        if (singleCondition.Length == 0) {
          continue;
        }
        string leftOperand = singleCondition.Split(' ')[0];
        string op = singleCondition.Split(' ')[1];
        string rightOperand = singleCondition.Split(' ')[2];

        string value = Convert.ToString(operation.Parameters[int.Parse(leftOperand.Substring(1))]);

        if (value.IndexOf(rightOperand.Replace("'", String.Empty)) != -1) {
          return true;
        }
      }
      return false;
    }

    internal bool IsCandidate(DataOperationList operationList) {
      if (condition.Length == 0) {
        return true;
      }
      return true;
    }

    internal bool IsCandidate(SingleSignOnToken token, DataOperation operation) {
      if (token.ExistsOnPath(this.TargetServer)) {
        return false;
      }
      return IsCandidate(operation);
    }

    internal bool IsCandidate(SingleSignOnToken token, DataOperationList operationList) {
      if (token.ExistsOnPath(this.TargetServer)) {
        return false;
      }
      return IsCandidate(operationList);
    }

    #endregion Internal methods

  } // class DataIntegrationRule

} // namespace Empiria.Data.Integration
