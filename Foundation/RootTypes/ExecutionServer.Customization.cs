/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ExecutionServer (Customized)                     Pattern  : Customized Static Class           *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static class that returns Empiria custom current execution server information.                *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  /// <summary>Static class that returns Empiria custom current execution server information.</summary>
  public partial class ExecutionServer {

    #region Public custom fields

    private string customerName = null;
    private string customerUrl = null;
    private int organizationId = 0;
    private string serviceProvider = null;
    private string serverName = null;

    #endregion Public custom fields

    #region Public custom additional properties

    static public string CustomerName {
      get {
        return Instance.customerName;
      }
    }

    static public string CustomerUrl {
      get {
        return Instance.customerUrl;
      }
    }

    static public int OrganizationId {
      get {
        return Instance.organizationId;
      }
    }

    static public string ServerName {
      get {
        return Instance.serverName;
      }
    }

    static public string ServiceProvider {
      get {
        return Instance.serviceProvider;
      }
    }

    #endregion Public custom additional properties

    #region Static methods called from the main partial class

    private void SetCustomFields() {
      var type = typeof(ExecutionServer);

      this.customerName = ConfigurationData.Get<string>(type, "Customer.Name", String.Empty);
      this.customerUrl = ConfigurationData.Get<string>(type, "Customer.Url", String.Empty);
      this.organizationId = ConfigurationData.Get<int>(type, "Organization.Id");
      this.serverName = ConfigurationData.Get<string>(type, "Server.Name", String.Empty);
      this.serviceProvider = ConfigurationData.Get<string>(type, "Service.Provider", String.Empty);

    }

    #endregion Static methods called from the main partial class

  } // class ExecutionServer

} // namespace Empiria
