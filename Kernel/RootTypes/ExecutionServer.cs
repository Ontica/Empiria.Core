/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ExecutionServer                                  Pattern  : Static Class                      *
*  Version   : 6.0   Date: 23/Oct/2014                          License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Static class that returns Empiria current execution server information.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Web;

using Empiria.Security;

namespace Empiria {

  #region Enumerations

  public enum ExecutionServerType {
    WebApplicationServer = 1,
    WebServicesServer = 2,
    WindowsServiceServer = 3,
    WindowsApplication = 4,
    WebApiServer = 5,
  }

  #endregion Enumerations

  /// <summary>Static class that returns Empiria current execution server information.</summary>
  static public class ExecutionServer {

    #region Fields

    static private string customerName = null;
    static private string customerUrl = null;
    static private DateTime dateMaxValue = DateTime.MaxValue;
    static private DateTime dateMinValue = DateTime.MinValue;

    static private string licenseNumber = null;
    static private string licenseSerialNumber = null;
    static private string serverName = null;
    static private int serverId = 0;
    static private int organizationId = 0;
    static private string serviceProvider = null;
    static private string supportUrl = null;
    static private bool isStarted = false;

    static private ExecutionServerType executionServerType = ExecutionServerType.WebApplicationServer;

    #endregion Fields

    #region Public properties

    static public string CustomerName {
      get {
        AssertIsStarted();

        return customerName;
      }
    }

    static public string CustomerUrl {
      get {
        AssertIsStarted();

        return customerUrl;
      }
    }

    static public IEmpiriaIdentity CurrentIdentity {
      get {
        AssertIsStarted();

        if (ExecutionServer.CurrentPrincipal != null) {
          return ExecutionServer.CurrentPrincipal.Identity as IEmpiriaIdentity;
        } else {
          return null;
        }
      }
    }

    static public IEmpiriaPrincipal CurrentPrincipal {
      get {
        if (HttpContext.Current == null) {
          return null;
        }
        if (ServerType != ExecutionServerType.WebApiServer &&
            HttpContext.Current.Session == null) {
          return null;
        }

        AssertIsStarted();

        if (ServerType != ExecutionServerType.WebApiServer) {
          return HttpContext.Current.Session[EmpiriaPrincipalTag] as IEmpiriaPrincipal;
        } else {
          return HttpContext.Current.User as IEmpiriaPrincipal;
        }
      }
      set {
        AssertIsStarted();

        if (HttpContext.Current != null && ServerType != ExecutionServerType.WebApiServer) {
          HttpContext.Current.Session.Add(ExecutionServer.EmpiriaPrincipalTag, null);
        }
        if (ServerType != ExecutionServerType.WebApiServer) {
          HttpContext.Current.Session[EmpiriaPrincipalTag] = value;
        } else {
          HttpContext.Current.User = value;
        }
      }
    }

    static public IEmpiriaSession CurrentSession {
      get {
        AssertIsStarted();

        if (CurrentIdentity != null) {
          return CurrentIdentity.Session;
        } else {
          return null;
        }
      }
    }

    static public string CurrentSessionToken {
      get {
        AssertIsStarted();

        if (CurrentSession != null) {
          return CurrentSession.Token;
        } else {
          return String.Empty;
        }
      }
    }

    static public IEmpiriaUser CurrentUser {
      get {
        AssertIsStarted();

        if (CurrentIdentity != null) {
          return CurrentIdentity.User;
        } else {
          return null;
        }
      }
    }

    static public int CurrentUserId {
      get {
        AssertIsStarted();

        if (CurrentUser != null) {
          return CurrentUser.Id;
        } else {
          return -5;
        }
      }
    }

    static public DateTime DateMaxValue {
      get {
        AssertIsStarted();

        return dateMaxValue;
      }
    }

    static public DateTime DateMinValue {
      get {
        AssertIsStarted();

        return dateMinValue;
      }
    }

    static public bool IsStarted {
      get { return isStarted; }
    }

    static public string LicenseName {
      get {
        return "Tlaxcala";
      }
    }

    static public string LicenseNumber {
      get {
        AssertIsStarted();

        return licenseNumber;
      }
    }

    static public string LicenseSerialNumber {
      get {
        AssertIsStarted();

        return licenseSerialNumber;
      }
    }

    static public string Name {
      get {
        AssertIsStarted();

        return serverName;
      }
    }

    static public int OrganizationId {
      get {
        AssertIsStarted();

        return organizationId;
      }
    }

    static public int ServerId {
      get {
        AssertIsStarted();

        return serverId;
      }
    }

    static public ExecutionServerType ServerType {
      get { return executionServerType; }
    }

    static public string ServiceProvider {
      get {
        AssertIsStarted();

        return serviceProvider;
      }
    }

    static public string SupportUrl {
      get {
        AssertIsStarted();

        return supportUrl;
      }
    }

    #endregion Public properties

    #region Public methods

    static public void AssertSession() {
      Assertion.Assert(ExecutionServer.CurrentSession != null,
                "This operation needs an active system session. Please Sign-in with a valid user account.", 1);
    }

    static public bool IsWebServicesServer() {
      return (ServerType == ExecutionServerType.WebServicesServer ||
              ServerType == ExecutionServerType.WebApiServer);
    }

    static public void DisposeSession() {
      if (HttpContext.Current != null && HttpContext.Current.Session != null) {
        HttpContext.Current.Session.Remove(ExecutionServer.EmpiriaPrincipalTag);
      }
    }

    static public void Start(ExecutionServerType serverType) {
      if (isStarted) {
        return;
      }
      executionServerType = serverType;

      try {
        Messaging.Publisher.Start();
      } catch (Exception innerException) {
        isStarted = false;
        throw innerException;
      }
      try {
        licenseNumber = ConfigurationData.GetString("Empiria", "License.Number");
        licenseSerialNumber = ConfigurationData.GetString("Empiria", "License.SerialNumber");
        customerName = ConfigurationData.GetString("Empiria", "Customer.Name");
        customerUrl = ConfigurationData.GetString("Empiria", "Customer.Url");
        dateMaxValue = ConfigurationData.GetDateTime("Empiria", "DateMaxValue");
        dateMinValue = ConfigurationData.GetDateTime("Empiria", "DateMinValue");
        serverId = ConfigurationData.GetInteger("Empiria", "Server.ID");
        organizationId = ConfigurationData.GetInteger("Empiria", "Organization.ID");
        serverName = ConfigurationData.GetString("Empiria", "Server.Name");
        serviceProvider = ConfigurationData.GetString("Empiria", "Service.Provider");
        supportUrl = ConfigurationData.GetString("Empiria", "Support.Url");
        isStarted = true;
      } catch (Exception innerException) {
        throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty,
                                           innerException);
      }
    }

    #endregion Public methods

    #region Private members

    static private void AssertIsStarted() {
      if (!isStarted) {
        Start();
      }
    }

    static private string EmpiriaPrincipalTag {
      get {
        return String.Format("{0}.Empiria.Principal", LicenseName);
      }
    }

    static private readonly object _locker = new object();
    static private void Start() {
      if (!IsStarted) {
        lock (_locker) {
          if (!IsStarted) {
            Start(ExecutionServerType.WebApplicationServer);
          }
        }
      }
    }

    #endregion Private members

  } // class ExecutionServer

} // namespace Empiria
