/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaIdentity                                  Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Sealed class that represents a Empiria System identity.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Security.Principal;

namespace Empiria.Security {

  public enum AuthenticationMode {
    None = -1,
    Basic = 1,
    Forms = 2,
    Realm = 3,
  }

  public enum AnonymousUser {
    Guest = -5,
    Tester = -4,
  }

  public sealed class EmpiriaIdentity : IIdentity {

    #region Constructors and parsers

    private EmpiriaIdentity(EmpiriaUser user, AuthenticationMode mode) {
      this.User = user;
      this.SetAuthenticationType(mode);
    }

    static public EmpiriaPrincipal Authenticate(string sessionToken) {
      Assertion.AssertObject(sessionToken, "sessionToken");

      EmpiriaPrincipal principal = EmpiriaPrincipal.TryParseWithToken(sessionToken);
      if (principal != null) {
        return principal;
      }
      EmpiriaSession session = EmpiriaSession.ParseActive(sessionToken);

      EmpiriaUser user = EmpiriaUser.Authenticate(session);

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Realm);
      identity.SetAuthenticationType(AuthenticationMode.Realm);

      return new EmpiriaPrincipal(identity, session);
    }

    static public EmpiriaPrincipal Authenticate(string clientAppKey,
                                                AnonymousUser systemUser, int contextId = -1) {
      Assertion.AssertObject(clientAppKey, "clientAppKey");

      var clientApplication = ClientApplication.ParseActive(clientAppKey);
      EmpiriaUser user = EmpiriaUser.Authenticate(systemUser);

      Assertion.AssertObject(user, "user");

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Basic);

      return new EmpiriaPrincipal(identity, clientApplication, contextId);
    }

    static public EmpiriaPrincipal Authenticate(string clientAppKey, string username, string password,
                                                string entropy = "", int contextId = -1) {
      Assertion.AssertObject(clientAppKey, "clientAppKey");
      Assertion.AssertObject(username, "username");
      Assertion.AssertObject(password, "password");

      var clientApplication = ClientApplication.ParseActive(clientAppKey);

      EmpiriaUser user = EmpiriaUser.Authenticate(username, password, entropy);

      Assertion.AssertObject(user, "user");

      var identity = new EmpiriaIdentity(user, AuthenticationMode.Basic);

      return new EmpiriaPrincipal(identity, clientApplication, contextId);
    }

    static public EmpiriaIdentity Current {
      get {
        return ExecutionServer.CurrentIdentity as EmpiriaIdentity;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string AuthenticationType {
      get;
      private set;
    }

    public bool IsAuthenticated {
      get;
      private set;
    }

    public string Name {
      get {
        return this.User.UserName;
      }
    }

    public EmpiriaUser User {
      get;
      private set;
    }

    #endregion Public properties

    #region Private methods

    private void EnsureValid() {
      Assertion.Assert(this.User != null,
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));
      Assertion.Assert(this.IsAuthenticated,
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));
    }

    private void SetAuthenticationType(AuthenticationMode authenticationMode) {
      switch (authenticationMode) {
        case AuthenticationMode.Basic:
          this.AuthenticationType = "Basic";
          this.IsAuthenticated = true;
          return;
        case AuthenticationMode.Realm:
          this.AuthenticationType = "Realm";
          this.IsAuthenticated = true;
          return;
        case AuthenticationMode.Forms:
          this.AuthenticationType = "Forms";
          this.IsAuthenticated = true;
          return;
        case AuthenticationMode.None:
          this.AuthenticationType = "None";
          this.IsAuthenticated = false;
          return;
        default:
          throw Assertion.AssertNoReachThisCode();
      }
    }

    #endregion Private methods

  } // class EmpiriaIdentity

} // namespace Empiria.Security
