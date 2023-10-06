/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ExecutionServer                                  Pattern  : Singleton                         *
*                                                                                                            *
*  Summary   : Static class that returns Empiria current execution server information.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Threading;

using Empiria.Collections;
using Empiria.Contacts;
using Empiria.Reflection;
using Empiria.Security;

namespace Empiria {

  /// <summary>Static class that returns Empiria current execution server information.</summary>
  public sealed partial class ExecutionServer {

    #region Fields

    private string applicationKey = String.Empty;
    private string licenseName = String.Empty;
    private string licenseNumber = String.Empty;
    private string licenseSerialNumber = String.Empty;
    private bool isSpecialLicense = false;

    private DateTime dateMaxValue = DateTime.MaxValue;
    private DateTime dateMinValue = DateTime.MinValue;

    private string systemName = string.Empty;

    private int serverId = -1;

    private bool isDevelopmentServer = false;

    private bool isPassThroughServer = false;

    #endregion Fields

    #region Constructors and parsers

    static private volatile ExecutionServer _instance;
    static private object _syncRoot = new Object();
    private readonly Exception _startFailedException;

    static private ExecutionServer Instance {
      get {
        if (_instance == null) {
          lock (_syncRoot) {
            if (_instance == null) {
              _instance = new ExecutionServer();
            }
          }
        }

        if (_instance._startFailedException == null) {
          return _instance;
        } else {
          throw new ExecutionServerException(ExecutionServerException.Msg.StartFailed,
                                             _instance._startFailedException);
        }
      }
    }


    private ExecutionServer() {
      try {
        this.ExecuteStart();
        _startFailedException = null;

      } catch (Exception e) {
        _startFailedException = e;
      }
    }

    #endregion Constructors and parsers

    #region Public license properties

    static public string ApplicationKey {
      get {
        return Instance.applicationKey;
      }
    }

    static public bool IsSpecialLicense {
      get {
        return Instance.isSpecialLicense;
      }
    }

    static public string LicenseName {
      get {
        return Instance.licenseName;
      }
    }

    static public string LicenseNumber {
      get {
        return Instance.licenseNumber;
      }
    }

    static public string LicenseSerialNumber {
      get {
        return Instance.licenseSerialNumber;
      }
    }

    #endregion Public license properties

    #region Other public properties

    static public AssortedDictionary ContextItems {
      get {
        return CurrentPrincipal.ContextItems;
      }
    }


    static public Contact CurrentContact {
      get {
        return CurrentIdentity.User.Contact;
      }
    }


    static public IEmpiriaIdentity CurrentIdentity {
      get {
        return CurrentPrincipal.Identity;
      }
    }


    static public IEmpiriaPrincipal CurrentPrincipal {
      get {

        var principal = Thread.CurrentPrincipal;

        if (principal is IEmpiriaPrincipal) {
          return (IEmpiriaPrincipal) principal;
        }

        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }
    }

    static public string CurrentSessionToken {
      get {
        if (IsAuthenticated) {
          return CurrentPrincipal.Session.Token;
        } else {
          return String.Empty;
        }
      }
    }

    static public int CurrentUserId {
      get {
        if (IsAuthenticated) {
          return CurrentUser.Contact.Id;
        } else {
          return -1;
        }
      }
    }

    static public IEmpiriaUser CurrentUser {
      get {
        return ExecutionServer.CurrentPrincipal.Identity.User;
      }
    }

    static public DateTime DateMaxValue {
      get {
        return Instance.dateMaxValue;
      }
    }

    static public DateTime DateMinValue {
      get {
        return Instance.dateMinValue;
      }
    }


    static public bool IsAuthenticated {
      get {
        var principal = Thread.CurrentPrincipal;

        return (principal?.Identity != null && principal is IEmpiriaPrincipal &&
                principal.Identity.IsAuthenticated &&
                !String.IsNullOrWhiteSpace(((IEmpiriaPrincipal) principal)?.Session?.Token));
      }
    }

    static public bool IsDevelopmentServer {
      get {
        return Instance.isDevelopmentServer;
      }
    }

    static public bool IsPassThroughServer {
      get {
        return Instance.isPassThroughServer;
      }
    }

    static public int ServerId {
      get {
        return Instance.serverId;
      }
    }

    static public string SystemName {
      get {
        return Instance.systemName;
      }
    }

    [ThreadStatic]
    static public string UserHostAddress;

    #endregion Other public properties

    #region Static methods

    static public string GetFullFileNameFromCurrentExecutionPath(string fileName) {
      string baseExecutionPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

      return System.IO.Path.Combine(baseExecutionPath, fileName);
    }


    static public void Preload() {
      Thread thread = new Thread(new ThreadStart(DoPreload));

      thread.Start();
    }

    #endregion Static methods

    #region Private members

    static private void DoPreload() {
      try {
        var preloadConfig = ConfigurationData.Get("Preload.Configuration", string.Empty);

        if (preloadConfig.Length == 0) {
          return;
        }

        string[] preloadData = preloadConfig.Split(';');

        Type type = ObjectFactory.GetType(preloadData[0], preloadData[1]);

        string methodName = preloadData[2];

        Assertion.Require(type,
          $"Unrecognizable preloading type for assembly {preloadData[0]} and type name {preloadData[1]}.");

        Assertion.Require(methodName,
          $"Unrecognizable preloading method name {preloadData[2]}.");

        MethodInvoker.Execute(type, methodName);

      } catch (Exception e) {
        EmpiriaLog.Error(e);
      }
    }

    private void ExecuteStart() {
      try {
        var type = typeof(ExecutionServer);

        applicationKey = ConfigurationData.Get<string>(type, "ApplicationKey");
        licenseName = ConfigurationData.Get<string>(type, "License.Name");
        isSpecialLicense = ConfigurationData.Get(type, "License.IsSpecial", false);
        licenseNumber = ConfigurationData.Get<string>(type, "License.Number");
        licenseSerialNumber = ConfigurationData.Get<string>(type, "License.SerialNumber");

        systemName = ConfigurationData.Get<string>(type, "System.Name");

        dateMaxValue = ConfigurationData.Get(type, "DateTime.MaxValue", DateTime.Parse("2078-12-31"));
        dateMinValue = ConfigurationData.Get(type, "DateTime.MinValue", DateTime.Parse("1900-01-01"));

        serverId = ConfigurationData.Get<int>(type, "Server.Id");

        isDevelopmentServer = ConfigurationData.Get(type, "IsDevelopmentServer", true);
        isPassThroughServer = ConfigurationData.Get(type, "IsPassThroughServer", false);

        this.SetCustomFields();

      } catch (Exception innerException) {
        throw new ExecutionServerException(ExecutionServerException.Msg.CantReadExecutionServerProperty,
                                           innerException);
      }
    }

    #endregion Private members

  } // class ExecutionServer

} // namespace Empiria
