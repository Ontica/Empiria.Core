/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Service provider                      *
*  Type     : AuthenticationService                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides user authentication services.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;
using Empiria.Security.Providers;

namespace Empiria.Security {

  /// <summary>Provides user authentication services.</summary>
  static public class AuthenticationService {

    #region Services

    static public IEmpiriaPrincipal Authenticate(string sessionToken, string userHostAddress) {
      Assertion.Require(sessionToken, nameof(sessionToken));
      Assertion.Require(userHostAddress, nameof(userHostAddress));

      var provider = SecurityProviders.AuthenticationProvider();

      return provider.Authenticate(sessionToken, userHostAddress);
    }


    static public IEmpiriaPrincipal Authenticate(UserCredentialsDto credentials) {
      Assertion.Require(credentials, nameof(credentials));

      var provider = SecurityProviders.AuthenticationProvider();

      return provider.Authenticate(credentials);
    }


    static public IClientApplication AuthenticateClientApp(string clientApplicationKey) {
      var provider = SecurityProviders.AuthenticationProvider();

      return provider.AuthenticateClientApp(clientApplicationKey);
    }

    #endregion Services

  } // class AuthenticationService

} // namespace Empiria.Security
