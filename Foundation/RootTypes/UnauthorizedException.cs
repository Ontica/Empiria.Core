/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Domain Services Layer             *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : UnauthorizedException                            Pattern  : Exception Class                   *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : The exception that is thrown when a web service operation call fails.                         *
*                                                                                                            *
********************************* Copyright (c) 2003-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  /// <summary>The exception that is thrown when a web service operation call fails.</summary>
  [Serializable]
  public sealed class UnauthorizedException : PlainTextException {

    #region Constructors and parsers

    /// <summary>Initializes a new instance of UnauthorizedException class with a specified error
    /// message.</summary>
    /// <param name="messageCode">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public UnauthorizedException(string messageCode, string message, params object[] args)
      : base(messageCode, message, args) {
    }

    /// <summary>Initializes a new instance of UnauthorizedException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="exception">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public UnauthorizedException(string messageCode, string message, Exception exception,
                                 params object[] args)
      : base(messageCode, message, exception, args) {
    }

    #endregion Constructors and parsers

  } // class UnauthorizedException

} // namespace Empiria
