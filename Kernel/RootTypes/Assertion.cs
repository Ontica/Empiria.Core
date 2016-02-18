/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : Assertion                                        Pattern  : Static Class                      *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : This class allows assertion checking and automatic publishing of assertions fails.            *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  /// <summary>Static library that allows assertion checking and automatic publishing
  ///of assertions fails.</summary>
  static public class Assertion {

    #region Public methods

    /// <summary>Checks for an assertion and throws an AssertionFailException if it fails.</summary>
    /// <param name="assertion">The assertion to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the assertion fails.</param>
    static public void Assert(bool assertion, string failsMessage, params object[] args) {
      if (!assertion) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, failsMessage, args);
      }
    }

    /// <summary>Checks for an assertion and throws an AssertionFailException if it fails.</summary>
    /// <param name="assertion">The assertion to check.</param>
    /// <param name="failsMessage">Used to indicate the description of the exception
    /// if the assertion fails.</param>
    static public void Assert(bool assertion, Exception onFailsException) {
      if (!assertion) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, onFailsException);
      }
    }

    static public void AssertFail(string failMessage, params object[] args) {
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, failMessage, args);
    }

    public static void AssertFail(Exception onFailException) {
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, onFailException);
    }

    /// <summary>Used to protect code execution when the code flow reaches an invalid line.</summary>
    /// <returns>Returns an exception in order to use it in methods that return values. Simply use
    ///it as throw Assertion.AssertNoReachThisCode(); in place of a 'return value;' statement.</returns>
    static public Exception AssertNoReachThisCode() {
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertNoReachThisCode);
    }

    /// <summary>Used to protect code execution when the code flow reaches an invalid line.</summary>
    /// <returns>Returns an exception in order to use it in methods that return values. Simply use
    ///it as throw Assertion.AssertNoReachThisCode(); in place of a 'return value;' statement.</returns>
    static public Exception AssertNoReachThisCode(string failMessage, params object[] args) {
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertNoReachThisCode,
                                        failMessage, args);
    }

    /// <summary>Special assertion used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="messageOrInstanceName">A message or the name of the instance variable.</param>
    static public void AssertObject(object instance, string messageOrInstanceName, params object[] args) {
      if (instance != null) {
        return;
      }
      string msg = String.Empty;
      if (messageOrInstanceName.Contains(" ")) {
        msg = messageOrInstanceName;
      } else {
        msg = String.Format("Object variable '{0}' should be distinct to null.",
                            messageOrInstanceName);
      }
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, msg, args);
    }

    static public void AssertObject(string instance, string messageOrInstanceName, params object[] args) {
      if (instance != null && !String.IsNullOrWhiteSpace(instance)) {
        return;
      }
      string msg = String.Empty;
      if (messageOrInstanceName.Contains(" ")) {
        msg = messageOrInstanceName;
      } else {
        msg = String.Format("String variable '{0}' should be distinct to null or empty.",
                            messageOrInstanceName);
      }
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, msg, args);
    }

    /// <summary>Special assertion used to check if an object is not null, and for strings, if not is
    /// empty too. Throws an AssertionFailException if the object is null or is an empty string.</summary>
    /// <param name="instance">The object to check.</param>
    /// <param name="instanceName">The name or identificator of the object in code.</param>
    static public void AssertObject(object instance, Exception onFailException) {
      if (instance == null) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                          onFailException);
      } else if ((instance is string) && (String.IsNullOrWhiteSpace((string) instance))) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                          onFailException);
      }
    }

    #endregion Public methods

  } //class Assertion

} //namespace Empiria
