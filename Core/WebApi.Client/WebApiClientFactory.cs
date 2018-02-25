/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                   System   : Empiria Web API Services            *
*  Namespace : Empiria.WebApi.Client                          Assembly : Empiria.Core.dll                    *
*  Type      : WebApiClientFactory                            Pattern  : Factory class                       *
*                                                                                                            *
*  Summary   : Methods to create WebApiClient instances which are included in a separated component.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Reflection;

namespace Empiria.WebApi.Client {

  /// <summary>Methods to create WebApiClient instances which are included in a separated component.</summary>
  static internal class WebApiClientFactory {

    static private IWebApiClient _defaultWebApiClient = null;

    static public IWebApiClient CreateWebApiClient() {
      if (_defaultWebApiClient == null) {
        Type type = GetWebApiClientType();


        _defaultWebApiClient = (IWebApiClient) ObjectFactory.CreateObject(type);
      }
      return _defaultWebApiClient;
    }


    static public IWebApiClient CreateWebApiClient(string baseAddress) {
      Assertion.AssertObject(baseAddress, "baseAddress");

      Type type = GetWebApiClientType();

      return (IWebApiClient) ObjectFactory.CreateObject(type,
                             new[] { typeof(string) }, new[] { baseAddress });
    }


    static private Type GetWebApiClientType() {
      return ObjectFactory.GetType("Empiria.WebApi.Client",
                                    "Empiria.WebApi.Client.WebApiClient");
    }

  }  // interface WebApiClientFactory

}  // namespace Empiria.WebApi.Client
