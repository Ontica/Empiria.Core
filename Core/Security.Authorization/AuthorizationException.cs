/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization Services                *
*  Assembly : Empiria.Core.dll                             Pattern   : Exception Class                       *
*  Type     : AuthorizationException                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : The exception that is thrown when an authorization service fails.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria.Security.Authorization {

  /// <summary>The exception that is thrown when an authorization service fails.</summary>
  [Serializable]
  public sealed class AuthorizationException : EmpiriaException {

    public enum Msg {
      AuthorizationWasExpired,
    }

    static private string resourceBaseName = "Empiria.Security.Authorization.AuthorizationExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of AuthorizationException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public AuthorizationException(Msg message, params object[] args):
                                        base(message.ToString(), GetMessage(message, args)) {
      try {
        base.Publish();
      } finally {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of AuthorizationException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public AuthorizationException(Msg message, Exception innerException, params object[] args) :
                                        base(message.ToString(), GetMessage(message, args), innerException) {
      try {
        base.Publish();
      } finally {
        // no-op
      }
    }

    #endregion Constructors and parsers

    #region Public methods

    static public string GetMessage(Msg message) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly());
    }

    #endregion Public methods

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class AuthorizationException

} // namespace Empiria.Security.Authorization
