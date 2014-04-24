/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Services                *
*  Namespace : Empiria.Messaging.Queues                         Assembly : Empiria.Messaging.dll             *
*  Type      : WindowsEventLogQueue                             Pattern  : Messaging Queue Class             *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Class used for publish information in the Windows Event Log.                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Diagnostics;

namespace Empiria.Messaging.Queues {

  /// <summary>Class used for publish information in the Windows Event Log.</summary>
  //[EventLogPermissionAttribute(SecurityAction.Demand)]
  internal sealed class WindowsEventLogQueue : Queue {

    #region Fields

    private const string DefaultEventLogSource = "Empiria.Foundation";

    #endregion Fields

    #region Constructors and parsers

    internal WindowsEventLogQueue(string queueName)
      : base(queueName) {

    }

    #endregion Constructors and parsers

    #region Protected methods

    protected sealed override void ImplementsStart() {
      try {
        using (EventLog logEntry = new EventLog(base.Name)) {
          logEntry.Source = DefaultEventLogSource;
          string msg = "Se inicializó la cola de mensajes de Windows a las " + DateTime.Now.ToLongTimeString() +
                       " para el servidor Empiria del tipo '" + Empiria.ExecutionServer.ServerType.ToString() +
                       "' bajo el proceso con identificador PID = ";
          if (System.Diagnostics.Process.GetCurrentProcess() != null) {
            msg += System.Diagnostics.Process.GetCurrentProcess().Id.ToString() + ".";
          } else {
            msg += "[Id de Proceso desconocido].";
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

      //if (messageBodyType is Empiria.WarningException) {
      //  return EventLogEntryType.Warning;
      //} else if (messageBodyType is Empiria.EmpiriaException) {
      //  return EventLogEntryType.Error;
      //} else if (messageBodyType is Empiria.Messaging.IDocument) {
      //  return EventLogEntryType.Information;
      //} else if (messageBodyType is Empiria.Messaging.ICommand) {
      //  return EventLogEntryType.Information;
      //} else if (messageBodyType is Empiria.Security.IAuditTrail) {
      //  if (((Empiria.Security.IAuditTrail) messageBodyType).IsSuccess) {
      //    return EventLogEntryType.SuccessAudit;
      //  } else {
      //    return EventLogEntryType.FailureAudit;
      //  }
      //} else {
      //  return EventLogEntryType.Information;
      //}
      if (messageBody is EmpiriaException) {
        return EventLogEntryType.Error;
      } else {
        return EventLogEntryType.Information;
      }
    }

    private string GetNextSource(string currentSource) {
      if (currentSource.LastIndexOf('.') != -1) {
        return currentSource.Substring(0, currentSource.LastIndexOf('.') - 1);
      } else if (currentSource == "Empiria") {
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

} // namespace Empiria.Messaging.Queues