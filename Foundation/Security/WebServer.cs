/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : WebServer                                        Pattern  : Empiria Object Type               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Type that represents and manage Empiria deploy web servers information.                       *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Security {

  /// <summary>Type that represents and manage Empiria deploy web servers information</summary>
  public sealed class WebServer : GeneralObject, IEmpiriaServer {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.WebServer";

    static private WebServer currentServer = null;

    #endregion Fields

    #region Constructors and parsers

    public WebServer()
      : base(thisTypeName) {

    }

    private WebServer(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public WebServer Parse(int id) {
      return BaseObject.Parse<WebServer>(thisTypeName, id);
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

    public string WebSiteIPAddress {
      get { return base.GetAttribute<string>("WebSiteIPAddress"); }
    }

    public string WebSiteURL {
      get { return base.GetAttribute<string>("WebSiteURL"); }
    }

    public string WebServicesSiteIPAddress {
      get { return base.GetAttribute<string>("WebServicesSiteIPAddress"); }
    }

    public string WebServicesSiteURL {
      get { return base.GetAttribute<string>("WebServicesSiteURL"); }
    }

    #endregion Public properties

  } // class WebServer

} // namespace Empiria.Security