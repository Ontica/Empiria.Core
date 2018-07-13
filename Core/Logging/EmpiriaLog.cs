﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaLog                                       Pattern  : Service provider                  *
*                                                                                                            *
*  Summary   : Public facade to invoke logging services.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Logging;
using Empiria.Reflection;

namespace Empiria {

  /// <summary>Public facade to invoke logging services.</summary>
  static public class EmpiriaLog {

    #region Static properties

    static private Guid CurrentTraceGuid {
      get;
      set;
    } = Guid.NewGuid();


    static private bool StopExecution {
      get;
      set;
    } = false;

    #endregion Static properties

    #region Static methods

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


    static public void Info(string message) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Info, message);
    }


    static public void Trace(string message) {
      CreateLogEntryInCurrentLogTrail(LogEntryType.Trace, message);
    }


    static public void EndTrace() {
      EmpiriaLog.CurrentTraceGuid = Guid.Empty;
    }


    #endregion Static methods

    #region Private methods

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

    static private Object _lockObject = new object();
    static private void CreateLogEntryInCurrentLogTrail(LogEntryType type, string data) {
      lock (_lockObject) {
        try {
          if (EmpiriaLog.StopExecution) {
            return;
          }
          EmpiriaLog.StopExecution = true;

          ILogTrail logTrail = GetDefaultLogTrail();

          var logEntry = CreateLogEntry(type, data);

          logTrail.Write(logEntry);

        } catch (Exception innerException) {
          throw new LoggingException(LoggingException.Msg.LoggingIssue, innerException);
        } finally {
          EmpiriaLog.StopExecution = false;
        }
      }
    }

    static private ILogTrail _defaultLogTrail = null;
    static internal ILogTrail GetDefaultLogTrail() {
      if (_defaultLogTrail == null) {
        _defaultLogTrail = new LogTrail();
      }
      return _defaultLogTrail;
    }

    #endregion Private methods

  } // class EmpiriaLog

} // namespace Empiria
