/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Domain Services Layer             *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : ResourceConflictException                        Pattern  : Exception Class                   *
*  Version   : 6.0        Date: Apr/2015                        License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : The exception that is thrown when a resource conflict occurs.                                 *
*                                                                                                            *
********************************* Copyright (c) 2003-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>The exception that is thrown when a resource conflict occurs.</summary>
  [Serializable]
  public sealed class ResourceConflictException : PlainTextException {

    #region Constructors and parsers

    /// <summary>Initializes a new instance of ResourceConflictException class with a specified error
    /// message.</summary>
    /// <param name="messageCode">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ResourceConflictException(string messageCode, string message,
                                     params object[] args) : base(messageCode, message, args) {
    }

    /// <summary>Initializes a new instance of ResourceConflictException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="exception">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ResourceConflictException(string messageCode, string message, Exception exception,
                                     params object[] args) : base(messageCode, message, exception, args) {
    }

    #endregion Constructors and parsers

  } // class ResourceConflictException

} // namespace Empiria
