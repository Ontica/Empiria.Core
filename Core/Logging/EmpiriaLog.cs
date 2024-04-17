/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaLog                                       Pattern  : Service provider                  *
*                                                                                                            *
*  Summary   : Public facade to invoke logging services.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Contacts;
using Empiria.Logging;
using Empiria.Security;

namespace Empiria {


  public enum LogOperationType {

    Error = 'E',

    Successful = 'S',

    UserManagement = 'U',

    PermissionsManagement = 'P'

  }

  /// <summary>Public facade to invoke logging services.</summary>
  static public class EmpiriaLog {

    #region Properties

    static private Guid CurrentTraceGuid {
      get;
      set;
    } = Guid.NewGuid();


    #endregion Properties

    #region Methods

    static public void Critical(string message) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Critical, message);
    }


    static public void Critical(Exception exception) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Critical, exception.ToString());
    }

    static public void Debug(string message) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Debug, message);
    }


    static public void Error(string message) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Error, message);
    }


    static public void Error(Exception exception) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Error, exception.ToString());
    }


    static public void FailedOperationLog(string operationName, string description) {
      CreateOperationLogEntry(LogOperationType.Error, operationName, description);
    }


    static public void FailedOperationLog(string operationName, Exception exception) {
      CreateOperationLogEntry(LogOperationType.Error, operationName, string.Empty, exception);
    }


    static public void FailedOperationLog(Contact subject, string operationName, string description) {
      var operationLog = new OperationLog(LogOperationType.Error,
                                          subject, operationName, description);

      operationLog.Save();
    }


    static public void FailedOperationLog(IEmpiriaSession session, string operationName, string description) {
      CreateOperationLogEntry(LogOperationType.Error, session, operationName, description);
    }

    static public void Info(string message) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Info, message);
    }


    static public void Operation(string operation, string description) {
      CreateOperationLogEntry(LogOperationType.Successful, operation, description);
    }


    static public void Operation(IEmpiriaSession session, string operation,
                                 string description) {
      CreateOperationLogEntry(LogOperationType.Successful, session, operation, description);
    }


    static public void PermissionsLog(Contact subject, string operationName,
                                      string subjectObject) {
      var operationLog = new OperationLog(LogOperationType.PermissionsManagement,
                                          subject, operationName, subjectObject);

      operationLog.Save();
    }


    static public void UserManagementLog(Contact subject, string operationName, string description = "") {
      var operationLog = new OperationLog(LogOperationType.UserManagement,
                                          subject, operationName, description);

      operationLog.Save();
    }


    static public void Trace(string message) {
      EmpiriaLog.CurrentTraceGuid = Guid.NewGuid();

      CreateLogEntryInCurrentLogTrail(LogEntryType.Trace, message);
    }


    static public void EndTrace() {
      EmpiriaLog.CurrentTraceGuid = Guid.Empty;
    }


    #endregion Methods

    #region Helpers

    static private ILogEntry CreateLogEntry(LogEntryType type, string data) {
      var logEntry = new LogEntryModel();

      logEntry.EntryType = type;
      if (ExecutionServer.IsAuthenticated) {
        logEntry.SessionToken = ExecutionServer.CurrentPrincipal.Session.Token;
      }
      logEntry.Data = data;
      if (logEntry.EntryType == LogEntryType.Trace) {
        logEntry.TraceGuid = EmpiriaLog.CurrentTraceGuid;
      }
      return logEntry;
    }


    static private void CreateLogEntryInCurrentLogTrail(LogEntryType type, string data) {
      ILogTrail logTrail = GetDefaultLogTrail();

      ILogEntry logEntry = CreateLogEntry(type, data);

      try {

        logTrail.Write(logEntry);

      } catch {

        TryWriteLogEntryToFile(logEntry);

        // WARINING: Never try to catch this error without retrowing it, because it causes an inifinite loop in EmpiriaException.Publish()

        throw;
      }
    }


    static private void CreateOperationLogEntry(LogOperationType logOperationType,
                                        string operation, string description) {
      var operationLog = new OperationLog(logOperationType, operation, description);

      operationLog.Save();
    }


    static private void CreateOperationLogEntry(LogOperationType logOperationType,
                                                string operation, string description,
                                                Exception exception) {
      var operationLog = new OperationLog(logOperationType, operation, description, exception);

      operationLog.Save();
    }


    static private void CreateOperationLogEntry(LogOperationType logOperationType, IEmpiriaSession session,
                                                string operation, string description) {
      var operationLog = new OperationLog(logOperationType, session, operation, description);

      operationLog.Save();
    }


    static private void TryWriteLogEntryToFile(ILogEntry logEntry) {
      // Log the CreateLogEntry exception to another kind of log outside the database.
      // Also log the original logEntry to the same chosen log technology.
      // Catch any exception
    }


    static private Object _lockObject = new object();
    static private ILogTrail _defaultLogTrail = null;

    static private ILogTrail GetDefaultLogTrail() {
      if (_defaultLogTrail == null) {
        lock (_lockObject) {
          if (_defaultLogTrail == null) {
            _defaultLogTrail = new LogTrail();
          }
        }
      }

      return _defaultLogTrail;
    }


    #endregion Helpers

  } // class EmpiriaLog

} // namespace Empiria
