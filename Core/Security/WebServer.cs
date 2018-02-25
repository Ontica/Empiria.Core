/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : WebServer                                        Pattern  : Empiria Object Type               *
*                                                                                                            *
*  Summary   : Type that represents and manage Empiria deploy web servers information.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Type that represents and manage Empiria deploy web servers information</summary>
  public sealed class WebServer : GeneralObject {

    #region Fields

    static private WebServer currentServer = null;

    #endregion Fields

    #region Constructors and parsers

    private WebServer() {
      // Required by Empiria Framework.
    }

    static public WebServer Parse(int id) {
      return BaseObject.ParseId<WebServer>(id);
    }

    static public WebServer Current {
      get {
        if (currentServer == null) {
          currentServer = WebServer.Parse(ExecutionServer.ServerId);
        }
        return currentServer;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField(ExtensionDataFieldName + ".WebSiteIPAddress")]
    public string WebSiteIPAddress {
      get;
      private set;
    }

    [DataField(ExtensionDataFieldName + ".WebSiteURL", IsOptional = false)]
    public string WebSiteURL {
      get;
      private set;
    }

    [DataField(ExtensionDataFieldName + ".WebServicesSiteIPAddress")]
    public string WebServicesSiteIPAddress {
      get;
      private set;
    }

    [DataField(ExtensionDataFieldName + ".WebServicesSiteURL", IsOptional = false)]
    public string WebServicesSiteURL {
      get;
      private set;
    }

    #endregion Public properties

  } // class WebServer

} // namespace Empiria.Security
