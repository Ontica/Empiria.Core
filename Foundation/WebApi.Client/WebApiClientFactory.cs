/* Empiria Extensions Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extensions Framework                   System   : Empiria Web API Services            *
*  Namespace : Empiria.WebApi.Client                          Assembly : Empiria.WebApi.Client.dll           *
*  Type      : WebApiClientFactory                            Pattern  : Factory class                       *
*  Version   : 1.1                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Methods to create WebApiClient instances which are included in a separated component.         *
*                                                                                                            *
********************************* Copyright (c) 2004-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Reflection;

namespace Empiria.WebApi.Client {

  /// <summary>Methods to create WebApiClient instances which are included in a separated component.</summary>
  static internal class WebApiClientFactory {

    static private IWebApiClient _defaultWebApiClient = null;
    static public IWebApiClient CreateWebApiClient() {
      if (_defaultWebApiClient == null) {
        Type type = ObjectFactory.GetType("Empiria.WebApi.Client",
                                          "Empiria.WebApi.Client.WebApiClient");
        _defaultWebApiClient = (IWebApiClient) ObjectFactory.CreateObject(type);
      }
      return _defaultWebApiClient;
    }

    static public IWebApiClient CreateWebApiClient(string baseAddress) {
      Assertion.AssertObject(baseAddress, "baseAddress");

      Type type = ObjectFactory.GetType("Empiria.WebApi.Client",
                                        "Empiria.WebApi.Client.WebApiClient");
      return (IWebApiClient) ObjectFactory.CreateObject(type,
                             new[] { typeof(string) }, new[] { baseAddress });
    }

  }  // interface WebApiClientFactory

}  // namespace Empiria.WebApi.Client
