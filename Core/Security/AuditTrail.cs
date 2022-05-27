/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : AuditTrail                                       Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Audit trail log information.                                                                  *
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
      Assertion.Require(request, "request");

      Initialize();
      this.Request = request;
      this.AuditTrailType = auditTrailType;
    }

    private void Initialize() {
      this.Id = -1;
      this.SessionId = -1;
      this.Event = String.Empty;
      this.Operation = String.Empty;
      this.OperationData = JsonObject.Empty;
      this.ResponseCode = 0;
      this.ResponseItems = 0;
      this.ResponseData = JsonObject.Empty;
      this.ResponseTime = 0m;
    }

    #endregion Constructors and parsers

    #region Properties

    public long Id {
      get;
      private set;
    }

    public IRequest Request {
      get;
      private set;
    }

    public AuditTrailType AuditTrailType {
      get;
      private set;
    }

    public DateTime Timestamp {
      get {
        return this.Request.StartTime;
      }
    }

    public int SessionId {
      get;
      private set;
    }

    public string Event {
      get;
      private set;
    }

    public string Operation {
      get;
      private set;
    }

    public JsonObject OperationData {
      get;
      private set;
    }

    public int ResponseCode {
      get;
      private set;
    }

    public int ResponseItems {
      get;
      private set;
    }

    public JsonObject ResponseData {
      get;
      private set;
    }

    public decimal ResponseTime {
      get;
      private set;
    }

    #endregion Properties

    #region Methods

    private decimal GetResponseTime() {
      return Convert.ToDecimal(DateTime.Now.Subtract(this.Timestamp).TotalSeconds);
    }

    protected void SetOperationInfo(string eventTag, string operationName, JsonObject operationData) {
      Assertion.Require(eventTag, "eventTag");
      Assertion.Require(operationName, "operationName");
      Assertion.Require(operationData, "operationData");

      this.Event = eventTag;
      this.Operation = operationName;
      this.OperationData = operationData;
    }

    private void SetResponse(int responseCode, Exception exception) {
      this.ResponseCode = responseCode;
      this.ResponseItems = 0;
      this.ResponseData = EmpiriaException.ToJson(exception);
      this.ResponseTime = this.GetResponseTime();
    }

    protected void SetResponse(int responseCode, int responseItems, JsonObject responseData) {
      Assertion.Require(responseData, "responseData");

      this.ResponseCode = responseCode;
      this.ResponseItems = responseItems;
      this.ResponseData = responseData;
      this.ResponseTime = this.GetResponseTime();
    }

    private void TrySetSessionData() {
      if (ExecutionServer.IsAuthenticated) {
        this.SessionId = ExecutionServer.CurrentPrincipal.Session.Id;
      } else if (this.Request.Principal != null) {
        this.SessionId = this.Request.Principal.Session.Id;
      }
    }

    protected void Write() {
      this.TrySetSessionData();
      try {
        this.Id = SecurityData.WriteAuditTrail(this);
      } catch (Exception inner) {
        var e = new SecurityException(SecurityException.Msg.CantWriteAuditTrail, inner,
                                      this.Operation);
        EmpiriaLog.Critical(e);
      }
    }

    #endregion Methods

  } // class AuditTrail

} // namespace Empiria.Security
