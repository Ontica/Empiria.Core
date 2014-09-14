/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : Assertion                                        Pattern  : Static Class                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
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
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, onFailsException, 
                                          GetSourceMethodName(), onFailsException.Message);
      }
    }

    static public void AssertFail(string failsMessage) {
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                        GetSourceMethodName(), failsMessage);
    }

    /// <summary>Used to protect code execution when the code flow reaches an invalid line.</summary>
    /// <returns>Returns an exception in order to be used in methods that return values. Simply use
    ///it as throw Assertion.AssertNoReachThisCode(); in place of a 'return value;' statement.</returns>
    static public Exception AssertNoReachThisCode() {
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertNoReachThisCode);
    }

    /// <summary>Special assertion used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="messageOrInstanceName">A message or the name of the instance variable.</param>
    static public void AssertObject(object instance, string messageOrInstanceName) {
      if (instance == null) {
        string msg = String.Empty;
        if (messageOrInstanceName.Contains(" ")) {
          msg = messageOrInstanceName;
        } else {
          msg = String.Format("Unexpected null value for object variable '{0}'.",
                              messageOrInstanceName);
        }
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                          Assertion.GetSourceMethodName(), msg);
      }
    }

    static public void AssertObject(string instance, string messageOrInstanceName) {
      if (instance == null || String.IsNullOrWhiteSpace(instance)) {
        string msg = String.Empty;
        if (messageOrInstanceName.Contains(" ")) {
          msg = messageOrInstanceName;
        } else {
          msg = String.Format("Unexpected null or empty value for string variable '{0}'.", 
                              messageOrInstanceName);
        }
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                          Assertion.GetSourceMethodName(), msg);
      }
    }

    /// <summary>Special assertion used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="instanceName">The name or identificator of the object in code.</param>
    static public void AssertObject(object instance, Exception onFailsException) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                          onFailsException, GetSourceMethodName(), onFailsException.Message);
      } else if ((instance is string) && (String.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
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
