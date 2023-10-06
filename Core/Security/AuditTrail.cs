/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Logging services                      *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : AuditTrail                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Audit trail log information.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Security {

  public enum AuditTrailType {

    Exception = 'E',

    FileSystem = 'F',

    Operation = 'O',

    Process = 'P',

    Security = 'S',

    WebApiCall = 'W',

    Unknown = 'U',

  }

  /// <summary>Audit trail log information.</summary>
  public class AuditTrail {

    #region Constructors and parsers

    protected AuditTrail(IRequest request, AuditTrailType auditTrailType) {
      Assertion.Require(request, nameof(request));

      this.Request = request;
      this.AuditTrailType = auditTrailType;
    }


    #endregion Constructors and parsers

    #region Properties

    public long Id {
      get; private set;
    } = -1;


    public IRequest Request {
      get; private set;
    }


    public AuditTrailType AuditTrailType {
      get; private set;
    }


    public DateTime Timestamp {
      get {
        return this.Request.StartTime;
      }
    }


    public int SessionId {
      get; private set;
    }


    public string UserHostAddress {
      get; private set;
    } = string.Empty;


    public string Event {
      get; private set;
    } = string.Empty;


    public string Operation {
      get; private set;
    } = string.Empty;


    public string Content {
      get; private set;
    } = string.Empty;


    public JsonObject OperationData {
      get; private set;
    } = JsonObject.Empty;


    public int ResponseCode {
      get; private set;
    }


    public int ResponseItems {
      get; private set;
    }


    public JsonObject ResponseData {
      get; private set;
    } = JsonObject.Empty;


    public decimal ResponseTime {
      get; private set;
    }

    #endregion Properties

    #region Methods

    private decimal GetResponseTime() {
      return Convert.ToDecimal(DateTime.Now.Subtract(this.Timestamp).TotalSeconds);
    }


    protected void SetOperationInfo(string eventTag, string operationName,
                                    JsonObject operationData, string content) {
      Assertion.Require(eventTag, nameof(eventTag));
      Assertion.Require(operationName, nameof(operationName));
      Assertion.Require(operationData, nameof(operationData));

      this.Event = eventTag;
      this.Operation = operationName;
      this.OperationData = operationData;
      this.Content = content;
    }



    protected void SetResponse(int responseCode, int responseItems, JsonObject responseData) {
      Assertion.Require(responseData, nameof(responseData));

      this.ResponseCode = responseCode;
      this.ResponseItems = responseItems;
      this.ResponseData = responseData;
      this.ResponseTime = this.GetResponseTime();
    }


    private void TrySetSessionData() {

      if (ExecutionServer.IsAuthenticated) {

        this.SessionId = ExecutionServer.CurrentPrincipal.Session.Id;
        this.UserHostAddress = ExecutionServer.CurrentPrincipal.Session.UserHostAddress;

      } else if (this.Request.Principal != null) {

        this.SessionId = this.Request.Principal.Session.Id;
        this.UserHostAddress = this.Request.Principal.Session.UserHostAddress;

      } else {

        this.SessionId = -1;
        this.UserHostAddress = ExecutionServer.UserHostAddress;

      }
    }


    protected void Write() {

      this.TrySetSessionData();

      try {

        this.Id = AuditTrailData.WriteAuditTrail(this);

      } catch (Exception inner) {

        var e = new SecurityException(SecurityException.Msg.CantWriteAuditTrail, inner,
                                      this.Operation);
        EmpiriaLog.Critical(e);
      }
    }

    #endregion Methods

  } // class AuditTrail

} // namespace Empiria.Security
