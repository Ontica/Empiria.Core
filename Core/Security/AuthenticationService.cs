/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain service class                  *
*  Type     : AuthenticationService                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides user authentication services.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Security {

  /// <summary>Provides user authentication services.</summary>
  static public class AuthenticationService {

    #region Services

    static public EmpiriaPrincipal Authenticate(string sessionToken) {
      Assertion.Require(sessionToken, "sessionToken");

      EmpiriaPrincipal principal = EmpiriaPrincipal.TryParseWithToken(sessionToken);

      if (principal != null) {
        return principal;
      }

      EmpiriaSession session = EmpiriaSession.ParseActive(sessionToken);

      EmpiriaUser user = EmpiriaUser.Authenticate(session);

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Realm);

      return new EmpiriaPrincipal(identity, session);
    }


    static public EmpiriaPrincipal Authenticate(string clientAppKey, string username,
                                                string password, string entropy,
                                                JsonObject contextData = null) {
      Assertion.Require(clientAppKey, "clientAppKey");
      Assertion.Require(username, "username");
      Assertion.Require(password, "password");


      var clientApplication = ClientApplication.ParseActive(clientAppKey);

      EmpiriaUser user = EmpiriaUser.Authenticate(clientApplication,
                                                  username, password,
                                                  entropy);
      Assertion.Require(user, "user");

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Basic);

      return new EmpiriaPrincipal(identity, clientApplication, contextData);
    }

    #endregion Services

  } // class AuthenticationService

} // namespace Empiria.Security
