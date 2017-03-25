/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaException                                 Pattern  : Base Exception Class              *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*																																																						 *
*  Summary   : Base class for handling run-time exceptions in Empiria Backend Framework.                     *
*              All Empiria exception types needs be derivated from this class.                               *
*																																																						 *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;

using Empiria.Json;

namespace Empiria {

  /// <summary>Base class for handling run-time exceptions in Empiria Backend Framework.</summary>
  /// <remarks>All Empiria exception types needs be derivated from this class.</remarks>
  [Serializable]
  public abstract class EmpiriaException : Exception {

    #region Fields

    private string exceptionTag = String.Empty;
    private DateTime timestamp = DateTime.Now;
    private Guid userSessionGuid = Guid.Empty;
    private string processId = String.Empty;
    private bool initialized = false;

    #endregion Fields

    #region Constructors and parsers

    /// <summary>Initializes a new instance of EmpiriaException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    public EmpiriaException(string exceptionTag, string message) : base(message) {
      this.exceptionTag = exceptionTag;
      this.timestamp = DateTime.Now;
      InitializeEnvironmentInformation();
    }

    /// <summary>Initializes a new instance of EmpiriaException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">The inner exception that throws this exception.</param>
    public EmpiriaException(string exceptionTag, string message, Exception innerException) :
                            base(message, innerException) {
      this.exceptionTag = exceptionTag;
      this.timestamp = DateTime.Now;
      InitializeEnvironmentInformation();
    }

    public EmpiriaException(SerializationInfo info, StreamingContext context) : base(info, context) {

    }

    static public JsonObject ToJson(Exception exception) {
      return new JsonObject() {
        new JsonItem("source", exception.Source == null ? "N/A" : exception.Source),
        new JsonItem("message", exception.Message),
      };
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Unique identificator tag or name for the exception.</summary>
    public string ExceptionTag {
      get {
        return exceptionTag;
      }
    }

    /// <summary>Session ID where the exception was occured.</summary>
    public string SessionID {
      get {
        return Empiria.ExecutionServer.CurrentSessionToken;
      }
    }

    /// <summary>Returns the .NET CLR Working Process ID that host the application.</summary>
    public string ProcessId {
      get {
        return processId;
      }
    }

    /// <summary>Date and time when the exception was created.</summary>
    public DateTime Timestamp {
      get {
        return timestamp;
      }
    }

    #endregion Public properties

    #region Public methods

    static public string GetHtmlString(Exception exception) {
      return EmpiriaException.GetHTMLString(exception);
    }

    static public string GetTextString(Exception exception) {
      string temp = EmpiriaException.GetHTMLString(exception);

      temp = temp.Replace("<u>", String.Empty);
      temp = temp.Replace("</u>", String.Empty);
      temp = temp.Replace("<b>", String.Empty);
      temp = temp.Replace("</b>", String.Empty);

      return temp;
    }

    /// <summary>Publish this exception.</summary>
    public void Publish() {
      EmpiriaLog.Error(this);
    }

    public string ToHtmlString() {
      return EmpiriaException.GetHTMLString(this);
    }

    public override string ToString() {
      return EmpiriaException.GetTextString(this);
    }

    #endregion Public methods

    #region Protected methods

    static protected string BuildMessage(string message, params object[] args) {
      if (args != null && args.Length != 0) {
        return String.Format(message, args);
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
        temp = temp.Replace(@"\n", System.Environment.NewLine);
      } catch (Exception e) {
        EmpiriaLog.Error(e.Message);
      } finally {
        if (String.IsNullOrEmpty(temp)) {
          temp = messageTag;
        } else if (args != null && args.Length != 0) {
          try {
            temp = String.Format(temp, args);
          } catch { // format exception
            // no-op
          }
        }
      }
      return temp;
    }

    #endregion Protected methods

    #region Private methods

    /// <summary>Creates and returns a string representation of the current exception.</summary>
    static private string GetHTMLString(Exception exception) {
      StringBuilder stringBuilder = new StringBuilder();

      Exception tempException = exception;
      int exceptionCount = 1;
      try {
        while (true) {
          stringBuilder.AppendFormat("{0}{0}", (exceptionCount != 1) ? Environment.NewLine : String.Empty);
          stringBuilder.AppendFormat("<b>{1}) <u>{2}</u></b>{0}{0}", Environment.NewLine, exceptionCount.ToString(),
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
          stringBuilder.AppendFormat("{0}{0}<u>Exception Stack Trace:</u>{0}", Environment.NewLine);
          stringBuilder.AppendFormat("{0}", exception.StackTrace);
        }
        if (System.Environment.StackTrace != null) {
          stringBuilder.AppendFormat("{0}{0}<u>Environment Stack Trace:</u>{0}", Environment.NewLine);
          stringBuilder.AppendFormat("{0}", System.Environment.StackTrace);
        }
      } catch {
        //no-op
      }
      return stringBuilder.ToString();
    }

    static private string GetSourceMethodName() {
      MethodBase sourceMethod = new StackFrame(3).GetMethod();
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

    /// <summary>Initialization function that gathers environment information.</summary>
    private void InitializeEnvironmentInformation() {
      const string unknownExceptionTag = "Unknown Exception";
      const string unknownProcessId = "Unknown Process Id";

      if (initialized) {
        return;
      }
      if (String.IsNullOrEmpty(this.exceptionTag)) {
        this.exceptionTag = unknownExceptionTag;
      }
      try {
        base.Source = GetSourceMethodName();
      } catch {
        base.Source = unknownExceptionTag;
      }
      try {
        processId = Process.GetCurrentProcess().Id.ToString();
      } catch {
        processId = unknownProcessId;
      }
      initialized = true;
    }

    #endregion Private methods

  } //class EmpiriaException

} //namespace Empiria
