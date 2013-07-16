/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : WebServer                                        Pattern  : Empiria Object Type               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Type that represents and manage Empiria deploy web servers information.                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

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