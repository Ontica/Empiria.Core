﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  Assembly : Empiria.Foundation.dll            *
*  Type      : LogTrail                                         Pattern  : Structurer                        *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Empiria log service.                                                                          *
*                                                                                                            *
********************************* Copyright (c) 2009-2016. Ontica LLC, La Vía Óntica SC, and contributors.  **/
using System;

using Empiria.Data;
using Empiria.Security;

namespace Empiria.Logging {

  /// <summary>A log trail contains a set of log entries belonging to the same log trail.</summary>
  public class LogTrail : ILogTrail {

    #region Constructors and parsers

    public LogTrail() {
      this.ClientApplication = ClientApplication.Inner;
    }

    public LogTrail(ClientApplication clientApplication) {
      Assertion.AssertObject(clientApplication, "clientApplication");

      this.ClientApplication = clientApplication;
    }

    #endregion Constructors and parsers

    #region Properties

    public ClientApplication ClientApplication {
      get;
    }

    #endregion Properties

    #region Public methods

    public void Write(ILogEntry logEntry) {
      Assertion.AssertObject(logEntry, "logEntry");

      WriteLogEntry(logEntry);
    }


    public void Write(ILogEntry[] logEntries) {
      Assertion.AssertObject(logEntries, "logEntries");

      if (logEntries.Length > 1) {
        WriteLogEntries(logEntries);
      } else {
        WriteLogEntry(logEntries[0]);
      }
    }

    #endregion Public methods

    #region Private methods

    private DataOperation GetDataOperation(ILogEntry o) {
      return DataOperation.Parse("apdLogEntry", this.ClientApplication.Id,
                                 o.UserSessionId, o.Timestamp,
                                 (char) o.EntryType, o.TraceGuid, o.Data);
    }


    private void WriteLogEntry(ILogEntry o) {
      var dataOperation = GetDataOperation(o);

      DataWriter.Execute(dataOperation);
    }


    private void WriteLogEntries(ILogEntry[] logEntries) {
      var dataOperationList = new DataOperationList("LogEntries");

      foreach (var logEntry in logEntries) {
        DataOperation op = GetDataOperation(logEntry);

        dataOperationList.Add(op);
      }
      DataWriter.Execute(dataOperationList);
    }

    #endregion Private methods

  } // class LogTrail

} // namespace Empiria.Logging
