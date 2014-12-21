/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : WebServer                                        Pattern  : Empiria Object Type               *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Type that represents and manage Empiria deploy web servers information.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Security {

  /// <summary>Type that represents and manage Empiria deploy web servers information</summary>
  public sealed class WebServer : GeneralObject, IEmpiriaServer {

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
