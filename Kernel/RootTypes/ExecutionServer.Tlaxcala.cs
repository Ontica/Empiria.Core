/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ExecutionServer                                  Pattern  : Static Class                      *
*  Version   : 5.5   Date: 25/Jun/2014                          License  : GNU AGPLv3  (See license.txt)     *
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

    private const string EmpiriaPrincipalTag = "Empiria.Land.Tlaxcala.Principal";

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

    #region Constructors and parsers

    #endregion Constructors and parsers

    #region Public properties

    static public string CustomerName {
      get {
        if (!isStarted) {
          Start();
        }
        return customerName;
      }
    }

    static public string CustomerUrl {
      get {
        if (!isStarted) {
          Start();
        }
        return customerUrl;
      }
    }

    static public IEmpiriaIdentity CurrentIdentity {
      get {
        if (!isStarted) {
          Start();
        }
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
        if (!isStarted) {
          Start();
        }
        if (ServerType != ExecutionServerType.WebApiServer) {
          return HttpContext.Current.Session[EmpiriaPrincipalTag] as IEmpiriaPrincipal;
        } else {
          return HttpContext.Current.User as IEmpiriaPrincipal;
        }
      }
      set {
        if (!isStarted) {
          Start();
        }
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
        if (!isStarted) {
          Start();
        }
        if (CurrentIdentity != null) {
          return CurrentIdentity.Session;
        } else {
          return null;
        }
      }
    }

    static public string CurrentSessionToken {
      get {
        if (!isStarted) {
          Start();
        }
        if (CurrentSession != null) {
          return CurrentSession.Token;
        } else {
          return String.Empty;
        }
      }
    }

    static public IEmpiriaUser CurrentUser {
      get {
        if (!isStarted) {
          Start();
        }
        if (CurrentIdentity != null) {
          return CurrentIdentity.User;
        } else {
          return null;
        }
      }
    }

    static public int CurrentUserId {
      get {
        if (!isStarted) {
          Start();
        }
        if (CurrentUser != null) {
          return CurrentUser.Id;
        } else {
          return -5;
        }
      }
    }

    static public DateTime DateMaxValue {
      get {
        if (!isStarted) {
          Start();
        }
        return dateMaxValue;
      }
    }

    static public DateTime DateMinValue {
      get {
        if (!isStarted) {
          Start();
        }
        return dateMinValue;
      }
    }

    static public bool IsStarted {
      get { return isStarted; }
    }

    static public string LicenseName {
      get { return "Tlaxcala"; }
    }

    static public string LicenseNumber {
      get {
        if (!isStarted) {
          Start();
        }
        return licenseNumber;
      }
    }

    static public string LicenseSerialNumber {
      get {
        if (!isStarted) {
          Start();
        }
        return licenseSerialNumber;
      }
    }

    static public string Name {
      get {
        if (!isStarted) {
          Start();
        }
        return serverName;
      }
    }

    static public int OrganizationId {
      get {
        if (!isStarted) {
          Start();
        }
        return organizationId;
      }
    }

    static public int ServerId {
      get {
        if (!isStarted) {
          Start();
        }
        return serverId;
      }
    }

    static public ExecutionServerType ServerType {
      get { return executionServerType; }
    }

    static public string ServiceProvider {
      get {
        if (!isStarted) {
          Start();
        }
        return serviceProvider;
      }
    }

    static public string SupportUrl {
      get {
        if (!isStarted) {
          Start();
        }
        return supportUrl;
      }
    }

    #endregion Public properties

    #region Public methods

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

    #region Private methods

    static private readonly object locker = new object();
    static private void Start() {
      if (!IsStarted) {
        lock (locker) {
          if (!IsStarted) {
            Start(ExecutionServerType.WebApplicationServer);
          }
        }
      }
    }

    #endregion Private methods

  } // class ExecutionServer

} // namespace Empiria