/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : AuditTrail                                       Pattern  : Data Services Static Class        *
*  Version   : 2.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Audit trail log information.                                                                  *
*                                                                                                            *
********************************* Copyright (c) 2009-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Json;

namespace Empiria.Security {

  public enum AuditTrailType {
    Exception = 'E',
    Operation = 'O',
    Security = 'S'
  }

  public enum ResultFlag {
    Ok = 'T',
    Fail = 'F',
    Queued = 'Q',
  }

  /// <summary>Audit trail log information.</summary>
  public class AuditTrail {

    #region Constructors and parsers

    private AuditTrail(AuditTrailType auditTrailType, string eventTag, string operation,
                       int instanceId, ResultFlag result, JsonObject data) {
      Initialize();

      this.AuditTrailType = auditTrailType;
      if (ExecutionServer.IsAuthenticated) {
        this.SessionId = ExecutionServer.CurrentPrincipal.Session.Id;
      }
      this.Event = eventTag;
      this.Operation = operation;
      this.InstanceId = instanceId;
      this.Result = result;
      this.Data = data;

      this.Save();
    }

    static public AuditTrail Write(AuditTrailType auditTrailType, string eventTag, string operation,
                                   ResultFlag result, JsonObject data, int instanceId = -1) {
      return new AuditTrail(auditTrailType, eventTag, operation, instanceId, result, data);
    }

    static public AuditTrail Write(Exception exception, string eventTag, string operation,
                                   int instanceId = -1) {
      return new AuditTrail(AuditTrailType.Exception, eventTag, operation, instanceId,
                            ResultFlag.Fail, EmpiriaException.ToJson(exception));
    }

    static public AuditTrail WriteException(string eventTag, string operation,
                                            JsonObject data, int instanceId = -1) {
      return new AuditTrail(AuditTrailType.Exception, eventTag, operation, instanceId,
                            ResultFlag.Fail, data);
    }

    static public AuditTrail WriteOperation(string eventTag, string operation,
                                            JsonObject data, int instanceId = -1) {
      return new AuditTrail(AuditTrailType.Operation, eventTag, operation, instanceId,
                            ResultFlag.Ok, data);
    }

    private void Initialize() {
      this.SessionId = -1;
      this.Event = String.Empty;
      this.Operation = String.Empty;
      this.InstanceId = -1;
      this.Data = JsonObject.Empty;
      this.Timestamp = DateTime.Now;
    }

    #endregion Constructors and parsers

    #region Properties

    public int Id {
      get;
      private set;
    }

    public AuditTrailType AuditTrailType {
      get;
      private set;
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

    public int InstanceId {
      get;
      private set;
    }

    public ResultFlag Result {
      get;
      private set;
    }

    public JsonObject Data {
      get;
      private set;
    }

    public DateTime Timestamp {
      get;
      private set;
    }

    #endregion Properties

    #region Methods

    private void Save() {
      if (this.Id <= 0) {
        this.Id = Empiria.Data.DataWriter.CreateId("AuditTrails");
      }
      SecurityData.WriteAuditTrail(this);
    }

    #endregion Methods

  } // class AuditTrail

} // namespace Empiria.Security
