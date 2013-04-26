﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Integration Services         *
*  Namespace : Empiria.Data.Integration                         Assembly : Empiria.Data.dll                  *
*  Type      : DataIntegratorProxyFactory                       Pattern  : Web Service Proxy Class           *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Proxy type that allows communication to data integration web services.                        *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;
using Empiria.Reflection;
using Empiria.Security;

namespace Empiria.Data.Integration {

  /// <summary>Proxy type that allows communication to data integration web services.</summary>
  [System.Web.Services.WebServiceBindingAttribute(Name = "DataIntegrationServicesSoap", Namespace = "http://empiria.ontica.org/web.services/")]
  [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
  public class DataIntegratorWSProxy : System.Web.Services.Protocols.SoapHttpClientProtocol {

    #region Constructors and Parsers

    static private IEmpiriaServer currentServer = null;

    static internal IEmpiriaServer CurrentServer {
      get {
        if (currentServer == null) {
          currentServer = GetIntegrationServer(ExecutionServer.ServerId);
        }
        return currentServer;
      }
    }

    internal DataIntegratorWSProxy(Empiria.Security.IEmpiriaServer targetServer) {
      this.Url = targetServer.WebServicesSiteURL + "data.integration/services.asmx";
    }

    static internal void SynchronizeServerCaches(string cachedObjectName) {
      try {
        if (ExecutionServer.ServerType == ExecutionServerType.WebServicesServer) {
          using (WebApplicationWSProxy proxy = new WebApplicationWSProxy(CurrentServer)) {
            proxy.UpdateCache(cachedObjectName);
          }
        }
      } catch (Exception innerException) {
        new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "SynchronizeServerCaches", "This");
      }
    }

    static internal IEmpiriaServer GetIntegrationServer(int serverId) {
      try {
        if (serverId == 0) {
          return null;
        }

        Type serverType = ObjectFactory.GetType("Empiria.Foundation", "Empiria.Security.WebServer");

        return (IEmpiriaServer) ObjectFactory.ParseObject(serverType, serverId);
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.CantParseDataIntegrationServer, innerException, serverId);
      }
    }

    #endregion Constructors and Parsers

    #region Public methods

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/CountData", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int CountData(string dataOperationMessage) {
      try {
        object[] results = this.Invoke("CountData", new object[] { dataOperationMessage });

        return ((int) results[0]);
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "CountData", this.Url);
      }
    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/CreateObjectId", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int CreateObjectId(string sourceName) {
      try {
        object[] results = this.Invoke("CreateObjectId", new object[] { sourceName });

        return ((int) results[0]);
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "CreateObjectId", this.Url);
      }
    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/Execute", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int Execute(string singleSignOnTokenMessage, string dataOperationMessage) {
      try {
        object[] results = this.Invoke("Execute", new object[] { singleSignOnTokenMessage, dataOperationMessage });

        return ((int) results[0]);
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "Execute", this.Url);
      }
    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/ExecuteList", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int ExecuteList(string singleSignOnTokenMessage, string[] dataOperationMessages) {
      try {
        object[] results = this.Invoke("ExecuteList", new object[] { singleSignOnTokenMessage, dataOperationMessages });

        return ((int) results[0]);
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "ExecuteList", this.Url);
      }
    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/GetFieldValue", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public object GetFieldValue(string dataOperationMessage, string fieldName) {
      try {
        object[] results = this.Invoke("GetFieldValue", new object[] { dataOperationMessage, fieldName });

        return results[0];
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "GetFieldValue", this.Url);
      }
    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/GetScalar", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public object GetScalar(string dataOperationMessage) {
      try {
        object[] results = this.Invoke("GetScalar", new object[] { dataOperationMessage });

        return results[0];
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "GetScalar", this.Url);
      }
    }

    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://empiria.ontica.org/web.services/GetDataTable", RequestNamespace = "http://empiria.ontica.org/web.services/", ResponseNamespace = "http://empiria.ontica.org/web.services/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public DataTable GetDataTable(string dataOperationMessage, string filter, string sort) {
      try {
        object[] results = this.Invoke("GetDataTable", new object[] { dataOperationMessage, filter, sort });

        return ((DataTable) results[0]);
      } catch (Exception innerException) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.DataIntegrationWSProxyException, innerException, "GetDataTable", this.Url);
      }
    }

    #endregion Public methods

  } // class DataIntegratorWSProxy

} // namespace Empiria.Data.Integration