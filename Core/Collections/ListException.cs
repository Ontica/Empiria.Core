﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.Collections                              License  : Please read LICENSE.txt file      *
*  Type      : ListException                                    Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a list or collection problem occurs.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria.Collections {

  /// <summary>The exception that is thrown when a list or collection problem occurs.</summary>
  [Serializable]
  public sealed class ListException : EmpiriaException {

    public enum Msg {
      ListIndexOutOfRange,
      ListKeyAlreadyExists,
      ListKeyNotFound,
    }

    static private string resourceBaseName = "Empiria.Exceptions.KernelExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of EmpiriaListException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ListException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {

    }

    /// <summary>Initializes a new instance of EmpiriaListException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ListException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class ListException

} // namespace Empiria.Collections
