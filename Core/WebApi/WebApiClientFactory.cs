/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                                 Component : Web Api Services                      *
*  Assembly : Empiria.Core.dll                             Pattern   : Factory                               *
*  Type     : WebApiClientFactory                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Methods to create WebApiClient instances which are included in a separated component.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Reflection;

namespace Empiria.WebApi {

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
      Assertion.Require(baseAddress, "baseAddress");

      Type type = GetWebApiClientType();

      return (IWebApiClient) ObjectFactory.CreateObject(type,
                             new[] { typeof(string) }, new[] { baseAddress });
    }


    static private Type GetWebApiClientType() {
      return ObjectFactory.GetType("Empiria.WebApi.Client",
                                   "Empiria.WebApi.Client.WebApiClient");
    }

  }  // interface WebApiClientFactory

}  // namespace Empiria.WebApi
