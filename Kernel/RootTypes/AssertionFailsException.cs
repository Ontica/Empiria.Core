/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : AssertionFailsException                          Pattern  : Exception Class                   *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : The exception that is thrown when a code assertion fails.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>The exception that is thrown when a code assertion fails. This exeptions always are
  /// replicated to the EventLog.</summary>
  [Serializable]
  public sealed class AssertionFailsException : EmpiriaException {

    internal enum Msg {
      AssertFails,
      AssertNoReachThisCode,
    }

    static private string resourceBaseName = "Empiria.RootTypes.KernelExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of AssertionFailsException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    internal AssertionFailsException(Msg message, params object[] args) :
                                     base(message.ToString(), GetMessage(message, args)) {
      try {
        base.Publish();
      } finally {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of AssertionFailsException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    internal AssertionFailsException(Msg message, Exception innerException, params object[] args) :
                                     base(message.ToString(), GetMessage(message, args), innerException) {
      try {
        base.Publish();
      } finally {
        // no-op
      }
    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class AssertionFailsException

} // namespace Empiria
