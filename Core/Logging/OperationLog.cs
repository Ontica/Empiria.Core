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

using Empiria.Data;

namespace Empiria {

  /// <summary>Stores an operation log.</summary>
  internal class OperationLog {

    #region Constructors and parsers

    internal OperationLog(LogOperationType logOperationType,
                          string operation, string description) {
      this.LogOperationType = logOperationType;
      this.Operation = operation;
      this.Description = description;
    }


    internal OperationLog(LogOperationType logOperationType,
                          string operation, string description,
                          Exception exception) : this(logOperationType, operation, description) {
      this.Exception = exception.Message;
    }

    #endregion Constructors and parsers

    #region Properties

    internal string UID {
      get {
        return Guid.NewGuid().ToString();
      }
    }

    internal int SessionId {
      get {
        if (!ExecutionServer.IsAuthenticated) {
          return -1;
        }
        return ExecutionServer.CurrentPrincipal.Session.Id;
      }
    }

    internal int UserId {
      get {
        if (!ExecutionServer.IsAuthenticated) {
          return -1;
        }
        return ExecutionServer.CurrentContact.Id;
      }
    }

    internal DateTime TimeStamp {
      get {
        return DateTime.Now;
      }
    }

    internal string UserHostAddress {
      get {
        return ExecutionServer.UserHostAddress;
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

    #endregion Properties

    #region Methods

    internal void Save() {
      if (!ConfigurationData.Get("UseOperationsLog", false)) {
        return;
      }
      var op = DataOperation.Parse("apdOperationLog",
                    this.UID, this.LogOperationType.ToString(), this.SessionId,
                    this.UserId, this.TimeStamp, this.UserHostAddress, this.Operation,
                    this.Description, this.Exception);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class OperationLog

} // namespace Empiria
