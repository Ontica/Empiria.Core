/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                                 Component : Reflection services                   *
*  Assembly : Empiria.Core.dll                             Pattern   : Exception                             *
*  Type     : ReflectionException                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : The exception that is thrown when a reflection service problem occurs.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria.Reflection {

  /// <summary>The exception that is thrown when a reflection service problem occurs.</summary>
  [Serializable]
  public sealed class ReflectionException : EmpiriaException {

    public enum Msg {
      ConditionalOptionNotDefined,
      ConstructorExecutionFails,
      ConstructorNotDefined,
      MethodExecutionFails,
      MethodNotFound,
      NotImplemented,
      ObjectPropertyNotFound,
      ParseMethodNotDefined,
      TypeNotDefined,
    }

    static private string resourceBaseName = "Empiria.Exceptions.KernelExceptionMsg";

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
