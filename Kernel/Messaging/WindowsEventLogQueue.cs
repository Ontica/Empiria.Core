/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : WindowsEventLogQueue                             Pattern  : Messaging Queue Class             *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Class used for publish information in the Windows Event Log.                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Diagnostics;

namespace Empiria.Messaging {

  /// <summary>Queue used for publish information using the Windows Event Log.</summary>
  //[EventLogPermissionAttribute(SecurityAction.Demand)]
  internal sealed class WindowsEventLogQueue : Queue {

    #region Fields

    private readonly string DefaultEventLogSource = ExecutionServer.IsSpecialLicense ?
                                                    ExecutionServer.LicenseName : "Empiria";

    #endregion Fields

    #region Constructors and parsers

    internal WindowsEventLogQueue(string queueName) : base(queueName) {

    }

    #endregion Constructors and parsers

    #region Protected methods

    protected sealed override void ImplementsStart() {
      try {
        using (EventLog logEntry = new EventLog(base.Name)) {
          logEntry.Source = DefaultEventLogSource;
          string msg = "Windows event log message queue initialized at " + DateTime.Now.ToLongTimeString() +
                       " for {0} server under the process PID = ";
          msg = String.Format(msg, ExecutionServer.LicenseName);
          if (System.Diagnostics.Process.GetCurrentProcess() != null) {
            msg += System.Diagnostics.Process.GetCurrentProcess().Id.ToString() + ".";
          } else {
            msg += "[Unknown process identificator].";
          }
          logEntry.WriteEntry(msg, EventLogEntryType.SuccessAudit);
        }
      } catch {
        throw new MessagingException(MessagingException.Msg.InvalidWindowsEventLog, base.Name);
      }
    }

    protected sealed override Message ImplementsRetrive() {
      return null;
    }

    protected sealed override void ImplementsSend(Message message) {
      try {
        EventLogEntryType entryType = GetWindowsEventLogEntryType(message);
        string source = message.Body.GetType().Namespace;

        while (source != String.Empty) {
          if (WriteToLog(entryType, source, message)) {
            break;
          } else {
            source = GetNextSource(source);
          }
        }
        if (source == String.Empty) {
          if (!WriteToLog(entryType, DefaultEventLogSource, message)) {
            throw new MessagingException(MessagingException.Msg.CantSendMessageToWindowsEventLog);
          }
        }
      } catch (Exception innerException) {
        throw innerException;
      } // catch
    }

    #endregion Protected methods

    #region Private methods

    private EventLogEntryType GetWindowsEventLogEntryType(Message message) {
      object messageBody = message.Body;

      if (messageBody is EmpiriaException) {
        return EventLogEntryType.Error;
      } else {
        return EventLogEntryType.Information;
      }
    }

    private string GetNextSource(string currentSource) {
      if (currentSource.LastIndexOf('.') != -1) {
        return currentSource.Substring(0, currentSource.LastIndexOf('.') - 1);
      } else if (currentSource == "Empiria" || currentSource == ExecutionServer.LicenseName) {
        return DefaultEventLogSource;
      } else {
        return String.Empty;
      }
    }

    private bool WriteToLog(EventLogEntryType entryType, string source, Message message) {
      try {
        using (EventLog logEntry = new EventLog(base.Name)) {
          logEntry.Source = source;
          logEntry.WriteEntry(message.Body.ToString(), entryType);
        }
        return true;
      } catch {
        return false;
      }
    }

    #endregion Private methods

  } // class WindowsEventLogQueue

} // namespace Empiria.Messaging
