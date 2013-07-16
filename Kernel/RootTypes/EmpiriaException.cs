/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaException                                 Pattern  : Empiria Base Exception Class      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Class for run-time exceptions in Empiria® Framework. All exception classes needs be           *
*              derivated of this class.                                                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;

namespace Empiria {

  /// <summary>Abstract class for run-time exceptions in Empiria® Framework.</summary>
  /// <remarks>Specialized exceptions must be managed through derived classes of this class.</remarks>
  [Serializable]
  public abstract class EmpiriaException : Exception {

    #region Fields

    private string exceptionTag = String.Empty;
    private DateTime timestamp = DateTime.Now;
    private string licenseName = String.Empty;
    private string serialNumber = String.Empty;
    private string userSessionToken = String.Empty;
    private string processId = String.Empty;
    private bool initialized = false;

    #endregion Fields

    #region Constructors and parsers

    /// <summary>Initializes a new instance of EmpiriaException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    public EmpiriaException(string exceptionTag, string message)
      : base(message) {
      this.exceptionTag = exceptionTag;
      this.timestamp = DateTime.Now;
    }

    /// <summary>Initializes a new instance of EmpiriaException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">The inner exception that throws this exception.</param>
    public EmpiriaException(string exceptionTag, string message, Exception innerException)
      : base(message, innerException) {
      this.exceptionTag = exceptionTag;
      this.timestamp = DateTime.Now;
    }

    public EmpiriaException(SerializationInfo info, StreamingContext context)
      : base(info, context) {

    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Unique identificator tag or name for the exception.</summary>
    public string ExceptionTag {
      get { return exceptionTag; }
    }

    /// <summary>LicenseName of Empiria product where the exception was occurred.</summary>
    public string LicenseName {
      get {
        if (!initialized) {
          InitializeEnvironmentInformation();
        }
        return licenseName;
      }
    }

    /// <summary>Serial number of the server license where the exception was ocurred.</summary>
    public string SerialNumber {
      get {
        if (!initialized) {
          InitializeEnvironmentInformation();
        }
        return serialNumber;
      }
    }

    /// <summary>Session token where the exception was ocurred.</summary>
    public string SessionGuid {
      get {
        if (!initialized) {
          InitializeEnvironmentInformation();
        }
        return userSessionToken;
      }
    }

    /// <summary>Returns the .NET CLR Working Process ID that host the Empiria Application.</summary>
    public string ProcessId {
      get { return processId; }
    }

    /// <summary>Date and time when the exception was created.</summary>
    public DateTime Timestamp {
      get { return timestamp; }
    }

    #endregion Public properties

    #region Public methods

    /// <summary>Publish this exception.</summary>    
    public void Publish() {
      if (ExecutionServer.IsStarted) {
        Empiria.Messaging.Message message = new Empiria.Messaging.Message(this);
        Empiria.Messaging.Publisher.Publish(message);
      }
    }

    public string ToString(bool xhtml) {
      if (xhtml) {
        string temp = EmpiriaString.ExceptionString(this);
        temp = temp.Replace(Environment.NewLine, "<br />");
        return temp;
      } else {
        return ToString();
      }
    }

    public override string ToString() {
      return EmpiriaString.ExceptionString(this);
    }

    #endregion Public methods

    #region Protected methods

    protected static string GetResourceMessage(string messageTag, string resourceBaseName,
                                               Assembly assembly, params object[] args) {
      ResourceManager resMgr = new ResourceManager(resourceBaseName, assembly);
      string temp = null;

      try {
        temp = resMgr.GetString(messageTag);
        temp = temp.Replace(@"\n", System.Environment.NewLine);
      } finally {
        if (String.IsNullOrEmpty(temp)) {
          temp = messageTag;
        } else if (args.Length != 0) {
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

    static private string GetSourceMethodName(int skipFrames) {
      MethodBase sourceMethod = new StackFrame(skipFrames).GetMethod();
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

    /// <summary>Initialization function that gathers environment information safely.</summary>    
    private void InitializeEnvironmentInformation() {
      const string unavailableTag = "Unavailable";

      if (initialized) {
        return;
      }
      if (String.IsNullOrEmpty(this.exceptionTag)) {
        this.exceptionTag = unavailableTag;
      }
      try {
        base.Source = GetSourceMethodName(4);
      } catch {
        base.Source = unavailableTag;
      }
      try {
        licenseName = ExecutionServer.LicenseName;
      } catch {
        licenseName = unavailableTag;
      }
      try {
        serialNumber = ExecutionServer.LicenseSerialNumber;
      } catch {
        serialNumber = unavailableTag;
      }
      try {
        userSessionToken = ExecutionServer.CurrentSessionToken;
      } catch {
        userSessionToken = String.Empty;
      }
      try {
        processId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
      } catch {
        processId = "[Id de proceso desconocido]";
      }
      try {
        base.HelpLink = ExecutionServer.SupportUrl + "?id=" +
                        this.GetType().FullName + "." + exceptionTag;
      } catch {
        base.HelpLink = "http://empiria.ontica.org/support/exceptions.aspx?id=" +
                        this.GetType().FullName + "." + exceptionTag;
      }
      initialized = true;
    }

    #endregion Private methods

  } //class EmpiriaException

} //namespace Empiria