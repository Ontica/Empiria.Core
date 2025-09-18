/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : SecurityException                                Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a system security check fails.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria.Security {

  /// <summary>The exception that is thrown when a system security check fails.</summary>
  [Serializable]
  public sealed class SecurityException : EmpiriaException {

    public enum Msg {
      CantDecodeEncryptedPrivateKey,
      CantWriteAuditTrail,
      ClientAppKeyIsSuspended,
      EnsureClaimFailed,
      ExpiredSessionToken,
      InvalidClientAppKey,
      InvalidDIFDataItemDataType,
      InvalidPrivateKeyFilePassword,
      InvalidProtectionMode,
      InvalidUserCredentials,
      InvalidUserHostAddress,
      MustChangePassword,
      PrivateKeyFileNotExists,
      SessionTokenNotFound,
      HSTSRequired,
      UnauthenticatedIdentity,
      UserAccountHasBeenBlocked,
      UserAccountIsSuspended,
      UserPasswordExpired,
      WrongDIFVersionRequested,
      WrongImpersonationToken,
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
