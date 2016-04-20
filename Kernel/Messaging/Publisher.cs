/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : Publisher                                        Pattern  : Static Class                      *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Class used to publish or serialize objects to specific message queue.                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Messaging {

  /// <summary>Class used to publish or serialize objects to specific message queue.</summary>
  static public class Publisher {

    #region Fields

    static private IQueueFactory queueFactory = null;

    #endregion Fields

    #region Public methods

    static public void Publish(string text) {
      if (text == null || text.Length == 0) {
        throw new MessagingException(MessagingException.Msg.CantPublishNullMessage);
      }
      Message message = new Message(text);
      try {
        Queue queue = GetMessageQueue(message);
        queue.Send(message);
      } catch (Exception innerException) {
        throw new MessagingException(MessagingException.Msg.CantPublishMessage, innerException,
                                     message.Body.GetType().FullName, message.Body.ToString());
      }
    }

    static public void Publish(Exception exception) {
      if (exception == null) {
        throw new MessagingException(MessagingException.Msg.CantPublishNullMessage);
      }
      Message message = null;
      try {
        message = new Message(EmpiriaException.GetTextString(exception));
        Queue queue = GetMessageQueue(message);
        queue.Send(message);
      } catch (Exception innerException) {
        throw new MessagingException(MessagingException.Msg.CantPublishMessage, innerException,
                                     message.Body.GetType().FullName, message.Body.ToString());
      }
    }

    static public void Publish(Message message) {
      if (message == null || message.Body == null) {
        throw new MessagingException(MessagingException.Msg.CantPublishNullMessage);
      }
      try {
        Queue queue = GetMessageQueue(message);
        queue.Send(message);
      } catch (Exception innerException) {
        throw new MessagingException(MessagingException.Msg.CantPublishMessage, innerException,
                                     message.Body.GetType().FullName, message.Body.ToString());
      }
    }

    static public void Publish(object instance) {
      if (instance == null) {
        throw new MessagingException(MessagingException.Msg.CantPublishNullMessage);
      }
      try {
        var message = new Message(instance);
        Queue queue = GetMessageQueue(message);
        queue.Send(message);
      } catch (Exception innerException) {
        throw new MessagingException(MessagingException.Msg.CantPublishMessage, innerException,
                                     instance.GetType().FullName, Json.JsonObject.Parse(instance));
      }
    }

    static internal void Start() {
      try {
        queueFactory = new QueueFactory();
      } catch (Exception innerException) {
        throw new MessagingException(MessagingException.Msg.CantCreateQueueFactory, innerException,
                                     typeof(QueueFactory).FullName);
      }
    }

    #endregion Public methods

    #region Private methods

    static private Queue GetMessageQueue(Message message) {
      if (queueFactory == null) {
        Start();
      }
      return queueFactory.GetQueue(message);
    }

    #endregion Private methods

  } // class Publisher

} //namespace Empiria.Messaging
