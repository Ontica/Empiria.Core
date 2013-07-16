/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Messaging Core Services           *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Kernel.dll                *
*  Type      : MessagingCoreException                           Pattern  : Empiria Exception Class           *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in messaging core services system.         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Reflection;

namespace Empiria.Messaging {

  /// <summary>The exception that is thrown when a problem occurs in messaging core services system.</summary>
  [Serializable]
  public sealed class MessagingCoreException : EmpiriaException {

    public enum Msg {
      CantCreateDefaultQueue,
      CantCreateQueueFactory,
      CantCreateQueueFactoryType,
      CantPublishMessage,
      CantPublishNullMessage,
      CantRetriveMessage,
      CantSendMessage,
      CantStartQueue,
      QueueNotStarted,
    }

    static private string resourceBaseName = "Empiria.RootTypes.KernelExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of MessagingCoreException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public MessagingCoreException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {

    }

    /// <summary>Initializes a new instance of MessagingCoreException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public MessagingCoreException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class MessagingCoreException

} // namespace Empiria.Messaging