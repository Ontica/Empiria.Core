/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : SecurityException                                Pattern  : Empiria Exception Class           *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : The exception that is thrown when a system security check fails.                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria.Security {

  /// <summary>The exception that is thrown when a system security check fails.</summary>
  [Serializable]
  public sealed class SecurityException : EmpiriaException {

    public enum Msg {
      CantChangePasswordOnUnauthenticatedUser,
      CantCloseSession,
      CantDecryptString,
      CantEncryptString,
      CantVerifyPasswordOnUnauthenticatedUser,
      EncryptedObjectWasAltered,
      GuestUserNotFound,
      InvalidAuthenticationClient,
      InvalidClientID,
      InvalidHttpMethod,
      InvalidProtectionMode,
      InvalidRequestPath,
      InvalidSingleSignOnToken,
      RequireExecuteMethod,
      UnauthenticatedIdentity,
      WrongAuthentication,
      WrongImpersonationToken,
      WrongRequestedVersionForDIF,
      IntegrityValidatorAssertFails,
      InvalidDIFDataItemDataType,
    }

    static private string resourceBaseName = "Empiria.Security.SecurityExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of SecurityException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public SecurityException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {
      try {
        base.Publish();
      } finally {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of SecurityException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public SecurityException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {
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

  } // class SecurityException

} // namespace Empiria.Security
