/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ExecutionServer                                  Pattern  : Static Class                      *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static class that returns Empiria current execution server information.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Threading;

using Empiria.Collections;
using Empiria.Security;

namespace Empiria {

  #region Enumerations

  public enum ExecutionServerType {
    WebApplicationServer = 1,
    WebServicesServer = 2,
    WindowsServiceServer = 3,
    WindowsApplication = 4,
    WebApiServer = 5,
    UnitTesting = 6,
  }

  #endregion Enumerations

  /// <summary>Static class that returns Empiria current execution server information.</summary>
  static public partial class ExecutionServer {

    #region Fields

    static private DateTime dateMaxValue = DateTime.MaxValue;
    static private DateTime dateMinValue = DateTime.MinValue;
    static private DateTime dateNullValue = DateTime.MinValue;

    static private string licenseNumber = null;
    static private string licenseSerialNumber = null;
    static private int serverId = 0;
    static private string supportUrl = null;
    static private bool isStarted = false;

    static private ExecutionServerType executionServerType = ExecutionServerType.WebApplicationServer;

    static private readonly object _locker = new object();

    #endregion Fields

    #region Public properties

    static public AssortedDictionary ContextItems {
      get {
        AssertIsStarted();

        return ExecutionServer.CurrentPrincipal.ContextItems;
      }
    }

    static public IEmpiriaIdentity CurrentIdentity {
      get {
        AssertIsStarted();

        return (IEmpiriaIdentity) ExecutionServer.CurrentPrincipal.Identity;
      }
    }

    static public IEmpiriaPrincipal CurrentPrincipal {
      get {
        AssertIsStarted();

        var principal = Thread.CurrentPrincipal;
        if (principal != null && principal is IEmpiriaPrincipal) {
          return (IEmpiriaPrincipal) principal;
        }
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }
    }

    static public int CurrentUserId {
      get {
        AssertIsStarted();

        if (IsAuthenticated) {
          return CurrentIdentity.User.Id;
        } else {
          return -1;
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

    static public DateTime DateNullValue {
      get {
        AssertIsStarted();

        return dateNullValue;
      }
    }

    static public bool IsAuthenticated {
      get {
        var principal = Thread.CurrentPrincipal;

        return (principal != null && principal is IEmpiriaPrincipal);
      }
    }

    static public bool IsPassThroughServer {
      get {
        return false;
      }
    }

    static public bool IsStarted {
      get { return isStarted; }
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

    static public int ServerId {
      get {
        AssertIsStarted();

        return serverId;
      }
    }

    static public ExecutionServerType ServerType {
      get { return executionServerType; }
    }

    static public string SupportUrl {
      get {
        AssertIsStarted();

        return supportUrl;
      }
    }

    #endregion Public properties

    #region Public methods

    static public void Start(ExecutionServerType serverType) {
      if (!IsStarted) {
        lock (_locker) {
          if (!IsStarted) {
            ExecuteStart(serverType);
          }
        }
      }
    }

    #endregion Public methods

    #region Private members

    static private void AssertIsStarted() {
      if (!IsStarted) {
        throw new ExecutionServerException(ExecutionServerException.Msg.NotStarted);
      }
    }

    static private void ExecuteStart(ExecutionServerType serverType) {
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
        dateMaxValue = ConfigurationData.GetDateTime("Empiria", "DateTime.MaxValue");
        dateMinValue = ConfigurationData.GetDateTime("Empiria", "DateTime.MinValue");
        dateNullValue = ConfigurationData.GetDateTime("Empiria", "DateTime.NullValue");

        serverId = ConfigurationData.GetInteger("Empiria", "Server.Id");
        supportUrl = ConfigurationData.GetString("Empiria", "Support.Url");

        ExecutionServer.SetCustomFields();

        isStarted = true;
      } catch (Exception innerException) {
        throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty,
                                           innerException);
      }
    }

    #endregion Private members

  } // class ExecutionServer

} // namespace Empiria
