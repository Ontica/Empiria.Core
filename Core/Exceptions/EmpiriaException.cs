/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaException                                 Pattern  : Base Exception Class              *
*																																																						 *
*  Summary   : Base class for handling run-time exceptions in Empiria Backend Framework.                     *
*              All Empiria exception types needs be derivated from this class.                               *
*																																																						 *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;

namespace Empiria {

  /// <summary>Base class for handling run-time exceptions in Empiria Backend Framework.</summary>
  /// <remarks>All Empiria exception types needs be derivated from this class.</remarks>
  [Serializable]
  public abstract class EmpiriaException : Exception {

    #region Fields

    private readonly DateTime _timestamp = DateTime.Now;
    private string _exceptionTag = string.Empty;
    private string _processId = string.Empty;
    private bool _initialized = false;

    #endregion Fields

    #region Constructors and parsers

    /// <summary>Initializes a new instance of EmpiriaException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    protected EmpiriaException(string exceptionTag, string message) : base(message) {

      _exceptionTag = exceptionTag;

      InitializeEnvironmentInformation();
    }

    /// <summary>Initializes a new instance of EmpiriaException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">The inner exception that throws this exception.</param>
    protected EmpiriaException(string exceptionTag, string message, Exception innerException) :
                            base(message, innerException) {

      _exceptionTag = exceptionTag;

      InitializeEnvironmentInformation();
    }

    protected EmpiriaException(SerializationInfo info, StreamingContext context) : base(info, context) {

      InitializeEnvironmentInformation();
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Unique identificator tag or name for the exception.</summary>
    public string ExceptionTag {
      get {
        return _exceptionTag;
      }
    }

    /// <summary>Session ID where the exception was occured.</summary>
    public string SessionID {
      get {
        return ExecutionServer.CurrentSessionToken;
      }
    }

    /// <summary>Returns the .NET CLR Working Process ID that host the application.</summary>
    public string ProcessId {
      get {
        return _processId;
      }
    }

    /// <summary>Date and time when the exception was created.</summary>
    public DateTime Timestamp {
      get {
        return _timestamp;
      }
    }

    #endregion Public properties

    #region Public methods

    public override string ToString() {
      return GetExceptionText(this);
    }


    static protected string BuildMessage(string message, params object[] args) {
      if (args != null && args.Length != 0) {
        return string.Format(message, args);
      } else {
        return message;
      }
    }

    static protected string GetResourceMessage(string messageTag, string resourceBaseName,
                                               Assembly assembly, params object[] args) {
      ResourceManager resMgr = new ResourceManager(resourceBaseName, assembly);
      string temp = null;

      try {
        temp = resMgr.GetString(messageTag);
        temp = temp.Replace(@"\n", Environment.NewLine);

      } catch (Exception e) {
        EmpiriaLog.Error(e.Message);

      } finally {

        if (string.IsNullOrWhiteSpace(temp)) {
          temp = messageTag;

        } else if (args != null && args.Length != 0) {

          try {
            temp = string.Format(temp, args);

          } catch {
            // no-op
          }
        }
      }
      return temp;
    }

    #endregion Public methods

    #region Exception Publisher

    static private bool _publishErrors;
    static private DateTime _publishingErrorsStoppedSince;

    static EmpiriaException() {
      StartPublishingNextErrors();
    }

    /// <summary>Publish this exception.</summary>
    public void Publish() {
      if (!MustPublishErrors()) {
        // ToDo: Make something else with the error
        return;
      }

      StopPublishingNextErrors();

      try {

        EmpiriaLog.Error(this);

        StartPublishingNextErrors();

      } catch {

        StopPublishingNextErrors();

        // ToDo: Make something else with the error
      }
    }


    static private void StartPublishingNextErrors() {
      _publishErrors = true;
      _publishingErrorsStoppedSince = new DateTime(1900, 01, 01);
    }


    static private void StopPublishingNextErrors() {
      _publishErrors = false;
      _publishingErrorsStoppedSince = DateTime.Now;
    }


    static private bool MustPublishErrors() {
      const int SECONDS_AFTER_RETRY = 10;

      bool mustRetryPublishing =
            _publishingErrorsStoppedSince.Add(TimeSpan.FromSeconds(SECONDS_AFTER_RETRY)) < DateTime.Now;

      return _publishErrors || mustRetryPublishing;
    }

    #endregion Exception Publisher

    #region Helpers

    /// <summary>Creates and returns a string representation of the current exception.</summary>
    static private string GetExceptionText(Exception exception) {
      StringBuilder stringBuilder = new StringBuilder();

      Exception tempException = exception;
      int exceptionCount = 1;
      try {
        while (true) {
          stringBuilder.AppendFormat("{0}{0}", (exceptionCount != 1) ? Environment.NewLine : string.Empty);
          stringBuilder.AppendFormat("{1}) {2}{0}{0}", Environment.NewLine, exceptionCount.ToString(),
                                     exceptionCount == 1 ? "Exception Information" : "Inner Exception Information");
          stringBuilder.AppendFormat("ExceptionType: {0}", tempException.GetType().FullName);

          PropertyInfo[] exceptionProperties = tempException.GetType().GetProperties();
          foreach (PropertyInfo property in exceptionProperties) {

            if (property.Name != "InnerException" && property.Name != "StackTrace" && property.Name != "Data") {
              object propertyValue = property.GetValue(tempException, null);

              if (propertyValue != null) {
                stringBuilder.AppendFormat("{0}{1}: {2}", Environment.NewLine, property.Name, propertyValue);
              }
            }
          } // foreach

          if (tempException.InnerException == null) {
            break;
          } else {
            tempException = tempException.InnerException;
            exceptionCount++;
          }
        }  // while

        if (exception.StackTrace != null) {
          stringBuilder.AppendFormat("{0}{0}Exception Stack Trace:{0}", Environment.NewLine);
          stringBuilder.AppendFormat("{0}", exception.StackTrace);
        }
        if (Environment.StackTrace != null) {
          stringBuilder.AppendFormat("{0}{0}Environment Stack Trace:{0}", Environment.NewLine);
          stringBuilder.AppendFormat("{0}", Environment.StackTrace ?? "Empty stack trace");
        }
      } catch {
        //no-op
      }
      return stringBuilder.ToString();
    }

    static private string GetSourceMethodName() {

      MethodBase sourceMethod = new StackFrame(3).GetMethod();
      ParameterInfo[] methodPars = sourceMethod.GetParameters();

      string methodName = string.Format("{0}.{1}", sourceMethod.DeclaringType, sourceMethod.Name);
      methodName += "(";
      for (int i = 0; i < methodPars.Length; i++) {
        methodName += string.Format("{0}{1} {2}", (i != 0) ? ", " : string.Empty,
                                    methodPars[i].ParameterType.Name, methodPars[i].Name);
      }
      methodName += ")";

      return methodName;
    }

    /// <summary>Initialization function that gathers environment information.</summary>
    private void InitializeEnvironmentInformation() {
      const string unknownExceptionTag = "Unknown Exception";
      const string unknownProcessId = "Unknown Process Id";

      if (_initialized) {
        return;
      }
      if (string.IsNullOrEmpty(_exceptionTag)) {
        _exceptionTag = unknownExceptionTag;
      }
      try {
        base.Source = GetSourceMethodName();
      } catch {
        base.Source = unknownExceptionTag;
      }
      try {
        _processId = Process.GetCurrentProcess().Id.ToString();
      } catch {
        _processId = unknownProcessId;
      }
      _initialized = true;
    }

    #endregion Helpers

  } //class EmpiriaException

} //namespace Empiria
