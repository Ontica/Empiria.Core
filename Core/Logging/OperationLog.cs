/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : OperationLog                                     Pattern  : Service provider                  *
*                                                                                                            *
*  Summary   : Stores an operation log.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Data;
using Empiria.Security;

namespace Empiria {

  /// <summary>Stores an operation log.</summary>
  internal class OperationLog {

    static internal readonly bool OPERATION_LOG_IS_ACTIVE = ConfigurationData.Get("UseOperationsLog", false);

    #region Constructors and parsers

    internal OperationLog(LogOperationType logOperationType,
                          string operation, string description) {
      LogOperationType = logOperationType;
      Operation = operation;
      Description = description;
      SetSessionId();
      SetUserId();
    }

    internal OperationLog(LogOperationType logOperationType,
                          string operation, string description,
                          Exception exception) : this(logOperationType, operation, description) {
      Exception = exception.Message;
    }


    public OperationLog(LogOperationType logOperationType,
                        IEmpiriaSession session,
                        string operation,
                        string description,
                        Exception exception) {
      LogOperationType = logOperationType;
      SessionId = session.Id;
      UserId = session.UserId;
      Operation = operation;
      Description = description;
      Exception = exception.Message;
    }

    public OperationLog(LogOperationType logOperationType, Contact subject,
                        string operation, string description = "") : this(logOperationType, operation, description) {
      if (UserId == -1) {
        UserId = subject.Id;
      }
      SubjectId = subject.Id;
    }

    public OperationLog(LogOperationType logOperationType, IEmpiriaSession session,
                        string operation, string description) {
      LogOperationType = logOperationType;
      SessionId = session.Id;
      UserId = session.UserId;
      Operation = operation;
      Description = description;
    }

    #endregion Constructors and parsers

    #region Properties

    internal string UID {
      get {
        return Guid.NewGuid().ToString();
      }
    }


    internal int SessionId {
      get;
      private set;
    }

    internal int UserId {
      get;
      private set;
    }

    internal DateTime TimeStamp {
      get {
        return DateTime.Now;
      }
    }

    internal string UserHostAddress {
      get {
        if (ExecutionServer.IsAuthenticated) {
          return ExecutionServer.CurrentPrincipal.Session.UserHostAddress;
        } else {
          return !String.IsNullOrWhiteSpace(ExecutionServer.UserHostAddress) ?
                                        ExecutionServer.UserHostAddress : "0.0.0.0";
        }
      }
    }


    internal LogOperationType LogOperationType {
      get;
      private set;
    }

    internal string Operation {
      get;
      private set;
    }

    internal string Description {
      get;
      private set;
    }

    internal string Exception {
      get;
      private set;
    }

    internal int SubjectId {
      get;
      private set;
    } = -1;


    internal string SubjectObject {
      get;
      private set;
    } = string.Empty;


    #endregion Properties

    #region Methods

    internal void Save() {
      if (!OPERATION_LOG_IS_ACTIVE) {
        return;
      }

      if (UserId == -1 && SessionId == -1 && this.LogOperationType == LogOperationType.Successful) {
        return;
      }

      if (Description.Length == 0) {
        var systemOperation = SystemOperation.TryParse(this.Operation);
        if (systemOperation != null) {
          Description = systemOperation.Description;
        }
      }

      var op = DataOperation.Parse("apdOperationLog",
                    this.UID, this.LogOperationType.ToString(), this.SessionId,
                    this.UserId, this.TimeStamp, this.UserHostAddress, this.Operation,
                    this.Description, this.Exception, this.SubjectId, this.SubjectObject);

      DataWriter.Execute(op);
    }

    private void SetUserId() {
      if (ExecutionServer.IsAuthenticated) {
        UserId = ExecutionServer.CurrentContact.Id;
      } else {
        UserId = -1;
      }
    }

    private void SetSessionId() {
      if (ExecutionServer.IsAuthenticated) {
        SessionId = ExecutionServer.CurrentPrincipal.Session.Id;
      } else {
        SessionId = -1;
      }
    }

    #endregion Methods

  }  // class OperationLog

} // namespace Empiria
