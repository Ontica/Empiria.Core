/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaStringException                           Pattern  : Exception Class                   *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : The exception that is thrown when a EmpiriaString type operation fails.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>The exception that is thrown when a EmpiriaString type operation fails.</summary>
  [Serializable]
  public class EmpiriaStringException : EmpiriaException {

    public enum Msg {
      CantConvertStringToTypeInstance,
    }

    static private string resourceBaseName = "Empiria.RootTypes.KernelExceptionMsg";

    #region Constructors

    /// <summary>Initializes a new instance of EmpiriaStringException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public EmpiriaStringException(Msg message, params object[] args)
                                        : base(message.ToString(), GetMessage(message, args)) {
      try {

      } catch {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of EmpiriaStringException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public EmpiriaStringException(Msg message, Exception innerException, params object[] args)
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

  } // class EmpiriaStringException

} // namespace Empiria
