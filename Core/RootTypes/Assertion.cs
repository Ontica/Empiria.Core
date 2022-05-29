/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core                                       Component : Assertions                              *
*  Assembly : Empiria.Core.dll                           Pattern   : Static class                            *
*  Type     : Assertion                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : This class allows assertion checking and automatic publishing of assertions fails.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Runtime.CompilerServices;

namespace Empiria {

  public interface IInvariant {

    bool Invariant();

  }


  /// <summary>Static library that allows assertion checking and automatic publishing
  ///of assertions fails.</summary>
  static public class Assertion {

    #region Methods

    static public void Ensure(bool postcondition) {
      if (postcondition) {
        return;
      }

      throw new ProgrammingException(ProgrammingException.Msg.PostconditionFailed);
    }


    static public void Ensure(bool postcondition, string message) {
      if (postcondition) {
        return;
      }

      throw new ProgrammingException(ProgrammingException.Msg.PostconditionFailed,
                                     message);
    }


    static public void Ensure(string nonEmptyStringPoscondition, string message) {
      nonEmptyStringPoscondition = EmpiriaString.Clean(nonEmptyStringPoscondition);

      if (!String.IsNullOrWhiteSpace(nonEmptyStringPoscondition)) {
        return;
      }

      throw new ProgrammingException(ProgrammingException.Msg.PreconditionFailed,
                                     message);
    }



    static public void Ensure(object nonEmptyObjectPostcondition, string message) {
      if (nonEmptyObjectPostcondition == null) {

        throw new ProgrammingException(ProgrammingException.Msg.PostconditionFailed,
                                       message);

      } else if (nonEmptyObjectPostcondition is string stringToCheck && (String.IsNullOrWhiteSpace(stringToCheck))) {

        throw new ProgrammingException(ProgrammingException.Msg.PostconditionFailed,
                                       message);
      }
    }


    static public void CheckInvariant(IInvariant instance, [CallerMemberName] string callerName = "") {
      if (instance.Invariant()) {
        return;
      }

      throw new ProgrammingException(ProgrammingException.Msg.InvariantFailed,
                                     callerName);

    }


    static public void EnsureFailed(string message) {
      throw new ProgrammingException(ProgrammingException.Msg.PostconditionFailed,
                                     message);
    }


    static public Exception EnsureNoReachThisCode() {
      throw new ProgrammingException(ProgrammingException.Msg.EnsureNoReachThisCode);
    }


    /// <summary>Used to protect code execution when the code flow reaches an invalid line.</summary>
    /// <returns>Returns an exception in order to use it in methods that return values. Simply use
    ///it as throw Assertion.AssertNoReachThisCode(); in place of a 'return value;' statement.</returns>
    static public Exception EnsureNoReachThisCode(string message) {
      throw new ProgrammingException(ProgrammingException.Msg.EnsureNoReachThisCode, message);
    }


    /// <summary>Checks a precondition and throws an AssertionFailException if it fails.</summary>
    /// <param name="precondition">The precondition to check.</param>
    /// <param name="message">Used to indicate the description of the exception
    /// if the precondition fails.</param>
    static public void Require(bool precondition, string message) {
      if (!precondition) {
        throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, message);
      }
    }

    /// <summary>Checks as a precondition if a string is null or empty or if it consists
    /// only in whitespaces after cleaning it. Throws an AssertionFailException if it fails.</summary>
    /// <param name="instance">The string instance to check.</param>
    /// <param name="messageOrName">A message or the variable name that holds the string object.</param>
    static public void Require(string instance, string messageOrName) {
      instance = EmpiriaString.Clean(instance);

      if (instance != null && !String.IsNullOrWhiteSpace(instance)) {
        return;
      }

      string message;

      if (messageOrName.Contains(" ")) {
        message = messageOrName;

      } else {
        message = String.Format("String variable '{0}' should be distinct to null or empty.",
                                messageOrName);
      }

      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                        message);
    }


    /// <summary>Precondition used to check if an object is not null.
    /// Throws an AssertionFailException if the object is null.</summary>
    /// <param name="requiredNotNullObject">The object to assert that is not null.</param>
    /// <param name="messageOrName">The message or when the the instance variable.</param>
    static public void Require(object instance, string messageOrName) {
      if (instance != null) {
        return;
      }

      string message;

      if (messageOrName.Contains(" ")) {
        message = messageOrName;

      } else {
        message = String.Format("Object variable '{0}' should be distinct to null.",
                                 messageOrName);
      }

      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails,
                                        message);
    }


    static public void RequireFail(string message) {
      throw new AssertionFailsException(AssertionFailsException.Msg.AssertFails, message);
    }


    static public void RequireRef(ref string instance, string messageOrName) {
      Require(instance, messageOrName);

      instance = EmpiriaString.Clean(instance);
    }


    #endregion Methods

  } //class Assertion

} //namespace Empiria
