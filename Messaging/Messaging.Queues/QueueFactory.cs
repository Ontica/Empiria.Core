/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Services                *
*  Namespace : Empiria.Messaging.Queues                         Assembly : Empiria.Messaging.dll             *
*  Type      : QueueFactory                                     Pattern  : Factory Class                     *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Class used for create messaging queues based on message type and content rules.               *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Messaging.Queues {

  /// <summary>Class used for create messaging queues based on message type and content rules.</summary>
  internal sealed class QueueFactory : IQueueFactory {

    #region Fields

    static private Queue defaultQueue = null;

    #endregion Fields

    #region Constructors and parsers

    public QueueFactory() {
      // no-op
    }

    #endregion Constructors and parsers

    #region Public methods

    public Queue GetDefaultQueue() {
      if (defaultQueue == null) {
        defaultQueue = CreateDefaultQueue();
      }
      if (!defaultQueue.IsStarted) {
        defaultQueue.Start();
      }
      return defaultQueue;
    }

    private Queue CreateDefaultQueue() {
      string defaultQueueTypeName = ConfigurationData.GetString("DefaultQueue.TypeName");
      string defaultQueueName = ConfigurationData.GetString("DefaultQueue.Name");

      switch (defaultQueueTypeName) {
        case "FileBasedQueue":
          return new FileBasedQueue(defaultQueueName);
        case "WindowsEventLogQueue":
          return new WindowsEventLogQueue(defaultQueueName);
        default:
          throw new MessagingException(MessagingException.Msg.InvalidDefaultQueueTypeName,
                                       defaultQueueTypeName);
      }
    }

    public Queue GetQueue(Message message) {
      // Retrive queue else return default queue
      return GetDefaultQueue();
    }

    #endregion Public methods

  } // class QueueFactory

} // namespace Empiria.Messaging.Queues