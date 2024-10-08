﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : AssertionFailsException                          Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a code assertion fails.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Diagnostics;
using System.Reflection;

namespace Empiria {

  /// <summary>The exception that is thrown when a code assertion fails.</summary>
  [Serializable]
  public sealed class AssertionFailsException : EmpiriaException {

    internal enum Msg {
      AssertFails,
      AssertNoReachThisCode,
    }

    static private string resourceBaseName = "Empiria.Exceptions.KernelExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of AssertionFailsException class with a specified error
    /// message.</summary>
    /// <param name="msgType">Used to indicate the description of the exception.</param>
    internal AssertionFailsException(Msg msgType) :
                                     base(msgType.ToString(), GetMessage(msgType)) {
      try {
        this.MethodName = this.GetSourceMethodName();
        // base.Publish();
      } finally {
        // no-op
      }
    }

    /// <summary>Initializes a new instance of AssertionFailsException class with a specified error
    /// message.</summary>
    /// <param name="msgType">Used to indicate the description of the exception.</param>
    /// <param name="message">The assertion message.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    internal AssertionFailsException(Msg msgType, string message, params object[] args) :
                                     base(msgType.ToString(), BuildMessage(message, args)) {

      this.MethodName = this.GetSourceMethodName();
      // base.Publish();
    }

    /// <summary>Initializes a new instance of AssertionFailsException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="msgType">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    internal AssertionFailsException(Msg msgType, Exception innerException) :
                                     base(msgType.ToString(), GetMessage(msgType), innerException) {

      this.MethodName = this.GetSourceMethodName();
      // base.Publish();
    }

    #endregion Constructors and parsers

    #region Private methods

    public string MethodName {
      get;
      private set;
    }

    static private string GetMessage(Msg message) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly());
    }

    private string GetSourceMethodName() {
      try {
        return GetSourceMethodName(1);
      } catch {
        return "Undetermined source method";
      }
    }

    private string GetSourceMethodName(int skipFrames) {
      MethodBase sourceMethod = new StackFrame(skipFrames + 2).GetMethod();
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

  } // class AssertionFailsException

} // namespace Empiria
