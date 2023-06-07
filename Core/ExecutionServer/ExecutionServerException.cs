/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ExecutionServerException                         Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when ExecutionServer type operation fails.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>The exception that is thrown when ExecutionServer type operation fails.</summary>
  [Serializable]
  public sealed class ExecutionServerException : EmpiriaException {

    public enum Msg {
      CantReadExecutionServerProperty,
      InvalidLicense,
      StartFailed,
    }

    static private string resourceBaseName = "Empiria.Exceptions.KernelExceptionMsg";

    #region Constructors

    /// <summary>Initializes a new instance of ExecutionServerException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ExecutionServerException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {
      try {

      } catch {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of ExecutionServerException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ExecutionServerException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {
      try {

      } catch {
        // no-op
      }
    }

    #endregion Constructors

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class ExecutionServerException

} // namespace Empiria
