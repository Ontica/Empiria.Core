/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : Assertion                                        Pattern  : Static Class                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This class allows assertion checking and automatic publishing of assertions fails.            *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Diagnostics;
using System.Reflection;

namespace Empiria {

  /// <summary>Static library that allows assertion checking and automatic publishing
  ///of assertions fails.</summary>
  static public class Assertion {

    #region Public methods

    /// <summary>Checks for an assertion and throws an AssertionFailException if it fails.</summary>
    /// <param name="assertion">The assertion to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the postcondition fails.</param>
    static public void Assert(bool assertion, string failsMessage) {
      if (!assertion) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                          GetSourceMethodName(), failsMessage);
      }
    }

    /// <summary>Checks for an assertion and throws an AssertionFailException if it fails.</summary>
    /// <param name="assertion">The assertion to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the postcondition fails.</param>
    static public void Assert(bool assertion, Exception onFailsException) {
      if (!assertion) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      }
    }

    /// <summary>Special assertion used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="instanceName">The name or identificator of the object in code.</param>
    static public void AssertObject(object instance, string instanceName) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertNotNullObjectFails,
                                          GetSourceMethodName(), instanceName);
      } else if ((instance is string) && (string.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertNotEmptyStringFails,
                                          GetSourceMethodName(), instanceName);
      }
    }

    /// <summary>Special assertion used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="instanceName">The name or identificator of the object in code.</param>
    static public void AssertObject(object instance, Exception onFailsException) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertNotNullObjectFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      } else if ((instance is string) && (string.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertNotEmptyStringFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      }
    }

    /// <summary>Checks for a postcondition and throws an AssertionFailException if it fails.</summary>
    /// <param name="postcondition">The postcondition to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the postcondition fails.</param>
    static public void Ensure(bool postcondition, string failsMessage) {
      if (!postcondition) {
        throw new AssertionFailsException(AssertionFailsException.Msg.EnsureFails,
                                          GetSourceMethodName(), failsMessage);
      }
    }

    /// <summary>Checks for a postcondition and throws an AssertionFailException if it fails.</summary>
    /// <param name="postcondition">The postcondition to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the postcondition fails.</param>
    static public void Ensure(bool postcondition, Exception onFailsException) {
      if (!postcondition) {
        throw new AssertionFailsException(AssertionFailsException.Msg.EnsureFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      }
    }

    /// <summary>Special postcondition used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="instanceName">The name or identificator of the object in code.</param>
    static public void EnsureObject(object instance, string instanceName) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.EnsureNotNullObjectFails,
                                          GetSourceMethodName(), instanceName);
      } else if ((instance is string) && (string.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.EnsureNotEmptyStringFails,
                                          GetSourceMethodName(), instanceName);
      }
    }

    /// <summary>Special postcondition used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="instanceName">The name or identificator of the object in code.</param>
    static public void EnsureObject(object instance, Exception onFailsException) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.EnsureNotNullObjectFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      } else if ((instance is string) && (string.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.EnsureNotEmptyStringFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      }
    }

    /// <summary>Checks for a precondition and throws an AssertionFailException if it fails.</summary>
    /// <param name="precondition">The precondition to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the precondition fails.</param>
    static public void Require(bool precondition, string failsMessage) {
      if (!precondition) {
        throw new AssertionFailsException(AssertionFailsException.Msg.RequireFails,
                                          GetSourceMethodName(), failsMessage);
      }
    }

    /// <summary>Checks for a precondition and throws an AssertionFailException if it fails.</summary>
    /// <param name="precondition">The precondition to check.</param>
    /// <param name="onFailsException">The exception that is throw if the precondition fails.</param>
    static public void Require(bool precondition, Exception onFailsException) {
      if (!precondition) {
        throw new AssertionFailsException(AssertionFailsException.Msg.RequireFails, onFailsException,
                                          GetSourceMethodName(), onFailsException.Message);
      }
    }

    /// <summary>Special precondition used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="postcondition">The precondition to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the precondition fails.</param>
    static public void RequireObject(object instance, string instanceName) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.RequireNotNullObjectFails,
                                          GetSourceMethodName(), instanceName);
      } else if ((instance is string) && (string.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.RequireNotEmptyStringFails,
                                          GetSourceMethodName(), instanceName);
      }
    }

    /// <summary>Special precondition used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="postcondition">The precondition to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the precondition fails.</param>
    static public void RequireObject(object instance, Exception onFailsException) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.RequireNotNullObjectFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      } else if ((instance is string) && (string.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.RequireNotEmptyStringFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      }
    }

    #endregion Public methods

    #region Private methods

    static private string GetSourceMethodName() {
      MethodBase sourceMethod = new StackFrame(2).GetMethod();
      ParameterInfo[] methodPars = sourceMethod.GetParameters();

      string methodName = String.Format("{0}.{1}", sourceMethod.DeclaringType, sourceMethod.Name);
      methodName += "(";
      for (int i = 0; i < methodPars.Length; i++) {
        methodName += String.Format("{0}{1} {2}", (i != 0) ? ", " : String.Empty,
                                    methodPars[i].ParameterType.Name, methodPars[i].Name);
      }
      methodName += ")";

      return methodName;
    }

    #endregion Private methods

  } //class Assertion

} //namespace Empiria
