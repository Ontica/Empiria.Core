/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  License  : Please read LICENSE.txt file      *
*  Type      : LoggingException                                 Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a Empiria logging service fails.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria.Logging {

  /// <summary>The exception that is thrown when a Empiria logging service fails.</summary>
  [Serializable]
  public sealed class LoggingException : EmpiriaException {

    internal enum Msg {
      LoggingIssue
    }

    static private string resourceBaseName = "Empiria.RootTypes.KernelExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of LoggingException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="msgType">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    internal LoggingException(Msg msgType, Exception innerException) :
                              base(msgType.ToString(), GetMessage(msgType), innerException) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class LoggingException

} // namespace Empiria.Logging
