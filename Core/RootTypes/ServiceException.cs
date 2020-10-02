/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Exceptions Manager                         Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Exception Class                         *
*  Type     : ServiceException                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : The exception that is thrown when a service call or operation fails.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>The exception that is thrown when a service call or operation fails.</summary>
  [Serializable]
  public sealed class ServiceException : PlainTextException {

    #region Constructors and parsers

    /// <summary>Initializes a new instance of ServiceException class with a specified error
    /// message.</summary>
    /// <param name="messageCode">Used as a meaningful tag for the exception.</param>
    /// <param name="message">The textual description for the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ServiceException(string messageCode, string message, params object[] args)
      : base(messageCode, message, args) {
    }

    /// <summary>Initializes a new instance of ServiceException class with a specified error
    /// message.</summary>
    /// <param name="messageCode">Used as a meaningful tag for the exception.</param>
    /// <param name="message">The textual description for the exception.</param>
    /// <param name="exception">The inner exception that generate this exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ServiceException(string messageCode, string message, Exception exception,
                            params object[] args)
      : base(messageCode, message, exception, args) {
    }

    #endregion Constructors and parsers

  } // class ServiceException

} // namespace Empiria
