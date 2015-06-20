/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Services                *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : MessagingException                               Pattern  : Exception Class                   *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in messaging services system.              *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria.Messaging {

  /// <summary>The exception that is thrown when a problem occurs in messaging services system.</summary>
  [Serializable]
  public sealed class MessagingException : EmpiriaException {

    public enum Msg {
      CantCreateDefaultQueue,
      CantCreateQueueFactory,
      CantCreateQueueFactoryType,
      CantPublishMessage,
      CantPublishNullMessage,
      CantRetriveMessage,
      CantSendMessage,
      CantSendMessageToWindowsEventLog,
      CantStartQueue,
      EventLogNameNotFound,
      InvalidDefaultQueueTypeName,
      InvalidWindowsEventLog,
      QueueNotStarted,
    }

    static private string resourceBaseName = "Empiria.Messaging.MessagingExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of MessagingException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public MessagingException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {

    }

    /// <summary>Initializes a new instance of MessagingException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public MessagingException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class MessagingException

} // namespace Empiria.Messaging
