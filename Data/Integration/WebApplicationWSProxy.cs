/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         Assembly : Empiria.Data.dll                  *
*  Type      : WebApplicationWSProxy                            Pattern  : Web Service Proxy Class           *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Proxy type that allows communication to the user web application.                             *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

using Empiria.Security;

namespace Empiria.Data.Integration {

  /// <summary> Proxy type that allows communication to the user web application.</summary>
  [System.Web.Services.WebServiceBindingAttribute(Name = "WebApplicationServicesSoap", Namespace = "http://empiria.ontica.org/web.services/")]
  [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
  public class WebApplicationWSProxy : System.Web.Services.Protocols.SoapHttpClientProtocol {

    #region Constructors and Parsers

    public WebApplicationWSProxy(IEmpiriaServer server) {
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
