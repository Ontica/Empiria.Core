/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Services                *
*  Namespace : Empiria.Messaging.Queues                         Assembly : Empiria.Messaging.dll             *
*  Type      : FileBasedQueue                                   Pattern  : Messaging Queue Class             *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Class used for publish messages on file based queues.                                         *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Messaging.Queues {

  /// <summary>Class used for publish messages on file based queues.</summary>
  //[EventLogPermissionAttribute(SecurityAction.Demand)]
  internal sealed class FileBasedQueue : Queue {

    #region Fields

    #endregion Fields

    #region Constructors and parsers

    internal FileBasedQueue(string queueName)
      : base(queueName) {

    }

    #endregion Constructors and parsers

    #region Protected methods

    protected sealed override void ImplementsStart() {
      try {
        //using (EventLog logEntry = new EventLog(base.Name)) {
        //  logEntry.Source = DefaultEventLogSource;
        //  logEntry.WriteEntry("WindowsEventLogQueue started at " + DateTime.Now.ToString(),
        //                      EventLogEntryType.Information);
        //}
      } catch {
        throw new MessagingException(MessagingException.Msg.InvalidWindowsEventLog, base.Name);
      }
    }

    protected sealed override Message ImplementsRetrive() {
      return null;
    }

    protected sealed override void ImplementsSend(Message message) {

    }

    #endregion Protected methods

    #region Private methods

    #endregion Private methods

  } // class FileBasedQueue

} // namespace Empiria.Messaging.Queues