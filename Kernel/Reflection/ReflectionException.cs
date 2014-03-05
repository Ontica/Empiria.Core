﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Reflection                               Assembly : Empiria.Kernel.dll                *
*  Type      : ReflectionException                              Pattern  : Empiria Exception Class           *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : The exception that is thrown when a reflection service problem occurs.                         *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria.Reflection {

  /// <summary>The exception that is thrown when a reflection service problem occurs.</summary>
  [Serializable]
  public sealed class ReflectionException : EmpiriaException {

    public enum Msg {
      ConstructorExecutionFails,
      MethodExecutionFails,

      ConditionalOptionNotDefined,
      ConstructorNotDefined,
      MethodNotFound,
      NotImplemented,
      ObjectPropertyNotFound,
      ParseMethodNotDefined,
      TypeNotDefined
    }

    static private string resourceBaseName = "Empiria.RootTypes.KernelExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of ReflectionException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public ReflectionException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {
      base.Publish();
    }

    /// <summary>Initializes a new instance of ReflectionException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public ReflectionException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {
      base.Publish();
    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class ReflectionException

} // namespace Empiria.Reflection