/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  License  : Please read LICENSE.txt file      *
*  Type      : LogTrail                                         Pattern  : Structurer                        *
*                                                                                                            *
*  Summary   : Empiria log service.                                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;
using Empiria.Security;

namespace Empiria.Logging {

  /// <summary>A log trail contains a set of log entries belonging to the same log trail.</summary>
  public class LogTrail : ILogTrail {

    #region Constructors and parsers

    public LogTrail() {

    }

    public LogTrail(ClientApplication clientApplication) {
      Assertion.Require(clientApplication, "clientApplication");

      this.ClientApplicationId = clientApplication.Id;
    }

    #endregion Constructors and parsers

    #region Properties

    public int ClientApplicationId {
      get;
    } = -1;

    #endregion Properties

    #region Public methods

    public void Write(ILogEntry logEntry) {
      Assertion.Require(logEntry, "logEntry");
      logEntry.AssertIsValid();

      WriteLogEntry(logEntry);
    }


    public void Write(ILogEntry[] logEntries) {
      Assertion.Require(logEntries, "logEntries");
      Assertion.Require(logEntries.Length > 0,
                       "logEntries can't be an empty array.");

      foreach (var logEntry in logEntries) {
        logEntry.AssertIsValid();
      }

      if (logEntries.Length > 1) {
        WriteLogEntries(logEntries);
      } else {
        WriteLogEntry(logEntries[0]);
      }
    }

    #endregion Public methods

    #region Private methods

    private DataOperation GetDataOperation(ILogEntry o) {
      return DataOperation.Parse("apdLogEntry", this.ClientApplicationId,
                                 o.SessionToken, o.Timestamp,
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
