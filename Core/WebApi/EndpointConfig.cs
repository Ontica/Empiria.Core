/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                                 Component : Web Api Services                      *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain class                          *
*  Type     : EndpointConfig                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes an endpoint used to invoke a web API from a client app.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Security;

namespace Empiria.WebApi {

  /// <summary>Describes an endpoint used to invoke a web API from a client app.</summary>
  public class EndpointConfig : GeneralObject {

    #region Constructors and parsers

    private EndpointConfig() {
      // Required by Empiria Framework.
    }


    /// <summary>Parses an Http endpoint given its numerical id.</summary>
    static public EndpointConfig Parse(int id) {
      return BaseObject.ParseId<EndpointConfig>(id);
    }


    /// <summary>Gets the full list of available Http API endpoints.</summary>
    static public FixedList<EndpointConfig> GetList(ClientApplication clientApplication) {
      var endpoints = BaseObject.GetList<EndpointConfig>();

      endpoints = EndpointConfig.GetFilteredListForClientApplication(endpoints, clientApplication);

      return endpoints.ToFixedList();
    }


    static private List<EndpointConfig> GetFilteredListForClientApplication(List<EndpointConfig> endpointsList,
                                                                            ClientApplication clientApplication) {
      var defaultWebApiAddress = clientApplication.WebApiAddresses.Find((x) => x.Name == "Default");

      Assertion.AssertObject(defaultWebApiAddress.Name,
                             "ClientApplication doesn't have a default web api server address.");

      foreach (var endpoint in endpointsList) {
        var itemWebApiName = endpoint.ApiName;

        if (itemWebApiName == "*") {
          endpoint.BaseAddress = defaultWebApiAddress.Value;

        } else {
          var apiAddress = clientApplication.WebApiAddresses.Find((x) => x.Name == itemWebApiName);

          if (apiAddress.Name != null) {
            endpoint.BaseAddress = apiAddress.Value;
          } else {
            endpoint.BaseAddress = defaultWebApiAddress.Value;
          }

        }
      }
      return endpointsList;
    }

    #endregion Constructors and parsers

    #region Properties

    /// <summary>Unique ID string for the Http Endpoint.</summary>
    public string UID {
      get {
        return base.NamedKey;
      }
    }


    /// <summary>The base address of the http endpoint.</summary>
    public string BaseAddress {
      get;
      private set;
    }


    /// <summary>The Http relative endpoint that provides the service.</summary>
    [DataField(ExtensionDataFieldName + ".path")]
    public string Path {
      get;
      private set;
    }


    /// <summary>Array with the path parameters.</summary>
    //[DataField(ExtensionDataFieldName + ".parameters", Default = "HttpEndpoint.DefaultHeaders")]
    public string[] Parameters {
      get;
      private set;
    } = new string[0];


    /// <summary>HTTP method used to invoke the call.
    /// Posible return values are GET, POST, PUT, PATCH or DELETE.</summary>
    [DataField(ExtensionDataFieldName + ".method", Default = "GET")]
    public string Method {
      get;
      private set;
    }


    /// <summary>Description of the service provided by the endpoint.</summary>
    public string Description {
      get {
        return base.Name;
      }
    }


    /// <summary>Indicates if the service must be requested with a valid Authorization header.</summary>
    [DataField(ExtensionDataFieldName + ".isProtected", Default = true)]
    public bool IsProtected {
      get;
      private set;
    }


    /// <summary>Array with any additional request headers.</summary>
    //[DataField(ExtensionDataFieldName + ".headers", Default = "HttpEndpoint.DefaultHeaders")]
    public string[] Headers {
      get;
      private set;
    } = new string[0];


    /// <summary>The response's payload type.</summary>
    [DataField(ExtensionDataFieldName + ".payloadType")]
    public string PayloadType {
      get;
      private set;
    }


    /// <summary>The response's payload data file name.</summary>
    [DataField(ExtensionDataFieldName + ".payloadDataField", Default = "data")]
    public string PayloadDataField {
      get;
      private set;
    }


    /// <summary>Array with any additional request headers.</summary>
    [DataField(ExtensionDataFieldName + ".responseDataType")]
    public string ResponseDataType {
      get;
      private set;
    }


    /// <summary>The API unique identificator</summary>
    [DataField(ExtensionDataFieldName + ".api")]
    private string ApiName {
      get;
      set;
    }

    #endregion Properties

  } // class ServiceDirectoryItem

} // namespace Empiria.WebApi
