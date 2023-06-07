/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Domain Services Layer             *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ResourceNotFoundException                        Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a resource (object instance, file, ...) was not found.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>The exception that is thrown when a resource
  /// (object instance, file, ...) was not found.</summary>
  [Serializable]
  public sealed class ResourceNotFoundException : PlainTextException {

    #region Constructors and parsers

    /// <summary>Initializes a new instance of ResourceNotFoundException class with a specified error
    /// message.</summary>
    /// <param name="messageCode">Used to indicate the description of the exception.</param>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ResourceNotFoundException(string messageCode, string message,
                                     params object[] args) : base(messageCode, message, args) {
    }

    /// <summary>Initializes a new instance of ResourceNotFoundException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="exception">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ResourceNotFoundException(string messageCode, string message, Exception exception,
                                     params object[] args) : base(messageCode, message, exception, args) {
    }

    #endregion Constructors and parsers

  } // class ResourceNotFoundException

} // namespace Empiria
