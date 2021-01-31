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

namespace Empiria.Security {

  /// <summary>Provides user authentication services.</summary>
  static public class AuthenticationService {

    #region Services

    static public EmpiriaPrincipal Authenticate(string sessionToken) {
      Assertion.AssertObject(sessionToken, "sessionToken");

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
                                                Json.JsonObject contextData = null) {
      Assertion.AssertObject(clientAppKey, "clientAppKey");
      Assertion.AssertObject(username, "username");
      Assertion.AssertObject(password, "password");

      var clientApplication = ClientApplication.ParseActive(clientAppKey);

      EmpiriaUser user = EmpiriaUser.Authenticate(username, password, entropy);

      Assertion.AssertObject(user, "user");

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Basic);

      return new EmpiriaPrincipal(identity, clientApplication, contextData);
    }


    static public EmpiriaPrincipal AuthenticateAnonymous(string clientAppKey, AnonymousUser anonymousUser,
                                                         Json.JsonObject contextData = null) {
      Assertion.AssertObject(clientAppKey, "clientAppKey");

      var clientApplication = ClientApplication.ParseActive(clientAppKey);

      EmpiriaUser user = EmpiriaUser.AuthenticateAnonymous(anonymousUser);

      Assertion.AssertObject(user, "user");

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Basic);

      return new EmpiriaPrincipal(identity, clientApplication, contextData);
    }

    #endregion Services

  } // class AuthenticationService

} // namespace Empiria.Security
