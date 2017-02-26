﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ExecutionServer                                  Pattern  : Static Class                      *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static class that returns Empiria current execution server information.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    static private string licenseName = null;
    static private bool? isSpecialLicense = false;
    static private string licenseNumber = null;
    static private string licenseSerialNumber = null;
    static private string applicationKey = null;

    static private DateTime dateMaxValue = DateTime.MaxValue;
    static private DateTime dateMinValue = DateTime.MinValue;

    static private int serverId = 0;
    static private string supportUrl = null;
    static private bool isStarted = false;
    static private bool isDevelopmentServer = false;

    static private ExecutionServerType executionServerType = ExecutionServerType.WebApplicationServer;

    #endregion Fields

    #region Public license properties

    static public string ApplicationKey {
      get {
        if (applicationKey != null) {
          return applicationKey;
        } else {
          throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty);
        }
      }
    }

    static public bool IsSpecialLicense {
      get {
        if (isSpecialLicense.HasValue) {
          return isSpecialLicense.Value;
        } else {
          throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty);
        }
      }
    }

    static public string LicenseName {
      get {
        if (licenseName != null) {
          return licenseName;
        } else {
          throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty);
        }
      }
    }

    static public string LicenseNumber {
      get {
        if (licenseNumber != null) {
          return licenseNumber;
        } else {
          throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty);
        }
      }
    }

    static public string LicenseSerialNumber {
      get {
        if (licenseSerialNumber != null) {
          return licenseSerialNumber;
        } else {
          throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty);
        }
      }
    }
    #endregion Public license properties

    #region Other public properties

    static public AssortedDictionary ContextItems {
      get {
        AssertIsStarted();

        return ExecutionServer.CurrentPrincipal.ContextItems;
      }
    }

    static public EmpiriaIdentity CurrentIdentity {
      get {
        AssertIsStarted();

        return ExecutionServer.CurrentPrincipal.Identity;
      }
    }

    static public EmpiriaPrincipal CurrentPrincipal {
      get {
        AssertIsStarted();

        var principal = Thread.CurrentPrincipal;

        if (principal != null && principal is EmpiriaPrincipal) {
          return (EmpiriaPrincipal) principal;
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

    static public bool IsAuthenticated {
      get {
        var principal = Thread.CurrentPrincipal;

        return (principal?.Identity != null && principal is EmpiriaPrincipal &&
                principal.Identity.IsAuthenticated &&
                !String.IsNullOrWhiteSpace(((EmpiriaPrincipal) principal)?.Session?.Token));
      }
    }

    static public bool IsDevelopmentServer {
      get {
        AssertIsStarted();

        return isDevelopmentServer;
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

    #endregion Other public properties

    #region Public methods

    static private readonly object _locker = new object();
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
        applicationKey = ConfigurationData.GetString("Empiria", "ApplicationKey");
        licenseName = ConfigurationData.GetString("Empiria", "License.Name");
        isSpecialLicense = ConfigurationData.GetBoolean("Empiria", "License.IsSpecial");
        licenseNumber = ConfigurationData.GetString("Empiria", "License.Number");
        licenseSerialNumber = ConfigurationData.GetString("Empiria", "License.SerialNumber");

        dateMaxValue = ConfigurationData.GetDateTime("Empiria", "DateTime.MaxValue");
        dateMinValue = ConfigurationData.GetDateTime("Empiria", "DateTime.MinValue");

        serverId = ConfigurationData.GetInteger("Empiria", "Server.Id");
        supportUrl = ConfigurationData.GetString("Empiria", "Support.Url");

        isDevelopmentServer = ConfigurationData.GetBoolean("Empiria", "IsDevelopmentServer");

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
