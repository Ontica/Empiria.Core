﻿/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ExecutionServer (Customized)                     Pattern  : Customized Static Class           *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static class that returns Empiria custom current execution server information.                *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Threading;

using Empiria.Collections;
using Empiria.Security;

namespace Empiria {

  /// <summary>Static class that returns Empiria custom current execution server information.</summary>
  static public partial class ExecutionServer {

    #region Public custom fields

    static private string customerName = null;
    static private string customerUrl = null;
    static private int organizationId = 0;
    static private string serviceProvider = null;
    static private string serverName = null;

    #endregion Public custom fields

    #region Public must custom properties

    static public bool IsSpecialLicense {
      get {
        return false;
      }
    }

    static public string LicenseName {
      get {
        return "Tlaxcala";
      }
    }

    #endregion Public custom properties

    #region Public custom additional properties

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

    static public int OrganizationId {
      get {
        AssertIsStarted();

        return organizationId;
      }
    }

    static public string ServerName {
      get {
        AssertIsStarted();

        return serverName;
      }
    }

    static public string ServiceProvider {
      get {
        AssertIsStarted();

        return serviceProvider;
      }
    }

    //OOJJOO: To deprecate
    static public string CurrentSessionToken {
      get {
        AssertIsStarted();

        if (IsAuthenticated) {
          return CurrentPrincipal.Session.Token;
        } else {
          return String.Empty;
        }
      }
    }

    static public bool IsDataSourceServer {
      get {
        AssertIsStarted();

        return ServerType == ExecutionServerType.WebApiServer ||
               ServerType == ExecutionServerType.WebServicesServer;
      }
    }

    #endregion Public custom additional properties

    #region Static methods called by the main partial class

    static private void SetCustomFields() {
      customerName = ConfigurationData.GetString("Empiria", "Customer.Name");
      customerUrl = ConfigurationData.GetString("Empiria", "Customer.Url");
      organizationId = ConfigurationData.GetInteger("Empiria", "Organization.Id");
      serverName = ConfigurationData.GetString("Empiria", "Server.Name");
    }

    #endregion Static methods called by the main partial class

  } // class ExecutionServer

} // namespace Empiria
