/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : PlainTextException                               Pattern  : Exception Class                   *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Abstract exception that allows set the exception's message in plain text.                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>Abstract exception that permits put the exception's message in plain text.</summary>
  [Serializable]
  public abstract class PlainTextException : EmpiriaException {

    #region Constructors and parsers

    /// <summary>Initializes a new instance of PlainTextException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    protected PlainTextException(string messageCode, string message, params object[] args)
      : base(messageCode, GetMessage(message, args)) {
    }

    /// <summary>Initializes a new instance of PlainTextException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="exception">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    protected PlainTextException(string messageCode, string message, Exception exception,
                                 params object[] args)
      : base(messageCode, GetMessage(message, args), exception) {
    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(string message, params object[] args) {
      if (args != null && args.Length != 0) {
        return String.Format(message, args);
      } else {
        return message;
      }
    }

    #endregion Private methods

  } // class PlainTextException

} // namespace Empiria
