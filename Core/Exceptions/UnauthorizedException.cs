/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Exceptions Manager                         Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Exception Class                         *
*  Type     : UnauthorizedException                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : The exception that is thrown when authentication is required and has failed                    *
*             or has not yet been provided                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>The exception that is thrown when authentication is required and has failed
  ///  or has not yet been provided.</summary>
  [Serializable]
  public sealed class UnauthorizedException : PlainTextException {

    #region Constructors and parsers

    /// <summary>Initializes a new instance of UnauthorizedException class with a specified error
    /// message.</summary>
    /// <param name="messageCode">Used as a meaningful tag for the exception.</param>
    /// <param name="message">The textual description for the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public UnauthorizedException(string messageCode, string message, params object[] args)
      : base(messageCode, message, args) {
    }

    /// <summary>Initializes a new instance of UnauthorizedException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="messageCode">Used as a meaningful tag for the exception.</param>
    /// <param name="message">The textual description for the exception.</param>
    /// <param name="exception">The inner exception that generate this exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public UnauthorizedException(string messageCode, string message, Exception exception,
                                 params object[] args)
      : base(messageCode, message, exception, args) {
    }

    #endregion Constructors and parsers

  } // class UnauthorizedException

} // namespace Empiria
