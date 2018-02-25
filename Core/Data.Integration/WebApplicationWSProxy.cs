/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         License  : Please read LICENSE.txt file      *
*  Type      : WebApplicationWSProxy                            Pattern  : Web Service Proxy Class           *
*                                                                                                            *
*  Summary   : Proxy type that allows communication to the user web application.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Security;

namespace Empiria.Data.Integration {

  /// <summary> Proxy type that allows communication to the user web application.</summary>
  [System.Web.Services.WebServiceBindingAttribute(Name = "WebApplicationServicesSoap", Namespace = "http://empiria.ontica.org/web.services/")]
  [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
  public class WebApplicationWSProxy : System.Web.Services.Protocols.SoapHttpClientProtocol {

    #region Constructors and Parsers

    public WebApplicationWSProxy(WebServer server) {
      this.Url = server.WebSiteURL + "services/services.asmx";
    }

    #endregion Constructors and Parsers

    #region Public methods

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/UpdateCache", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public bool UpdateCache(string cachedObjectName) {
      object[] results = this.Invoke("UpdateCache", new object[] { cachedObjectName });

      return ((bool) results[0]);
    }

    #endregion Public methods

  } // class WebApplicationWSProxy

} // namespace Empiria.Data.Integration
