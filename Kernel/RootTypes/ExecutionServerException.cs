/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ExecutionServerException                         Pattern  : Exception Class                   *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : The exception that is thrown when ExecutionServer type operation fails.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>The exception that is thrown when ExecutionServer type operation fails.</summary>
  [Serializable]
  public sealed class ExecutionServerException : EmpiriaException {

    public enum Msg {
      CantReadExecutionServerProperty,
      InvalidLicense,
      NotStarted,
    }

    static private string resourceBaseName = "Empiria.RootTypes.KernelExceptionMsg";

    #region Constructors

    /// <summary>Initializes a new instance of ExecutionServerException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ExecutionServerException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {
      try {
        base.Publish();
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
        base.Publish();
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
