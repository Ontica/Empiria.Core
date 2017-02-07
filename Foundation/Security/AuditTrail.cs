/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : AuditTrail                                       Pattern  : Standard Class                    *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Audit trail log information.                                                                  *
*                                                                                                            *
********************************* Copyright (c) 2009-2016. Ontica LLC, La Vía Óntica SC, and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Json;
using Empiria.Messaging;

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
      Assertion.AssertObject(request, "request");

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
      Assertion.AssertObject(eventTag, "eventTag");
      Assertion.AssertObject(operationName, "operationName");
      Assertion.AssertObject(operationData, "operationData");

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
      Assertion.AssertObject(responseData, "responseData");

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
