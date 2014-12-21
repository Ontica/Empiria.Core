﻿/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Core Services           *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : Publisher                                        Pattern  : Static Class                      *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Class used to publish or serialize objects to specific message queue.                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Reflection;

namespace Empiria.Messaging {

  /// <summary>Class used to publish or serialize objects to specific message queue.</summary>
  static public class Publisher {

    #region Fields

    static private Type queueFactoryType = null;
    static private IQueueFactory queueFactory = null;

    #endregion Fields

    #region Public methods

    static public void Publish(string text) {
      if (text == null || text.Length == 0) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantPublishNullMessage);
      }
      Message message = new Message(text);
      try {
        Queue queue = GetMessageQueue(message);
        queue.Send(message);
      } catch (Exception innerException) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantPublishMessage, innerException,
                                         message.Body.GetType().FullName, message.Body.ToString());
      }
    }

    static public void Publish(Exception exception) {
      if (exception == null) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantPublishNullMessage);
      }
      Message message = null;
      try {
        message = new Message(EmpiriaString.ExceptionString(exception));
        Queue queue = GetMessageQueue(message);
        queue.Send(message);
      } catch (Exception innerException) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantPublishMessage, innerException,
                                     message.Body.GetType().FullName, message.Body.ToString());
      }
    }

    static public void Publish(Message message) {
      if (message == null || message.Body == null) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantPublishNullMessage);
      }
      try {
        Queue queue = GetMessageQueue(message);
        queue.Send(message);
      } catch (Exception innerException) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantPublishMessage, innerException,
                                         message.Body.GetType().FullName, message.Body.ToString());
      }
    }

    static internal void Start() {
      try {
        queueFactoryType = ObjectFactory.GetType("Empiria.Messaging",
                                                 "Empiria.Messaging.Queues.QueueFactory");
      } catch (Exception innerException) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantCreateQueueFactoryType, innerException);
      }
      try {
        queueFactory = (IQueueFactory) ObjectFactory.CreateObject(queueFactoryType);
      } catch (Exception innerException) {
        throw new MessagingCoreException(MessagingCoreException.Msg.CantCreateQueueFactory, innerException,
                                     queueFactoryType.FullName);
      }
      try {
        queueFactory.GetDefaultQueue();
      } catch (Exception innerException) {
        queueFactoryType = null;
        queueFactory = null;
        throw new MessagingCoreException(MessagingCoreException.Msg.CantCreateDefaultQueue, innerException);
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
