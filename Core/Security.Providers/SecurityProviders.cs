/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Providers                    *
*  Assembly : Empiria.Core.dll                             Pattern   : Providers Factory                     *
*  Type     : SecurityProviders                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Factory that provides object instances used to access security external services.              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Reflection;

namespace Empiria.Security.Providers {

  /// <summary>Factory that provides object instances used to access security external services.</summary>
  static internal class SecurityProviders {

    static internal IAuthenticationProvider AuthenticationProvider() {
      Type type = ObjectFactory.GetType("Empiria.Security",
                                        "Empiria.Security.Services.AuthenticationServiceProvider");

      return (IAuthenticationProvider) ObjectFactory.CreateObject(type);
    }


    static internal IAuthorizationProvider AuthorizationProvider() {
      Type type = ObjectFactory.GetType("Empiria.Security",
                                        "Empiria.Security.Services.AuthorizationServiceProvider");

      return (IAuthorizationProvider) ObjectFactory.CreateObject(type);
    }


    static internal ICryptoServiceProvider CryptoServiceProvider() {
      Type type = ObjectFactory.GetType("Empiria.Security",
                                        "Empiria.Security.Services.CryptoServiceProvider");

      return (ICryptoServiceProvider) ObjectFactory.CreateObject(type);
    }

  }  // class SecurityProviders

}  // namespace Empiria.Security.Providers
