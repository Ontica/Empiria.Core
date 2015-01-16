/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaIdentity                                  Pattern  : Standard Class                    *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Sealed class that represents a Empiria System identity.                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Security {

  public enum AuthenticationMode {
    None = -1,
    Basic = 1,
    Forms = 2,
    Realm = 3,
  }

  public enum SystemUser {
    Administrator = -3,
    Guest = -5,
    UnitTester = -4,
  }

  public sealed class EmpiriaIdentity : IEmpiriaIdentity {
    #region Constructors and parsers

    private EmpiriaIdentity(EmpiriaUser user) {
      this.User = user;
    }

    static public EmpiriaPrincipal Authenticate(string sessionToken) {
      Assertion.AssertObject(sessionToken, "sessionToken");

      EmpiriaPrincipal principal = EmpiriaPrincipal.TryParseWithToken(sessionToken);
      if (principal != null) {
        principal.Identity.SetAuthenticationType(AuthenticationMode.Realm);
        return principal;
      }
      EmpiriaSession session = EmpiriaSession.ParseActive(sessionToken);

      EmpiriaUser user = EmpiriaUser.Authenticate(session);

      var identity = new EmpiriaIdentity(user);
      identity.SetAuthenticationType(AuthenticationMode.Realm);
      return new EmpiriaPrincipal(identity, session);
    }

    static public EmpiriaPrincipal Authenticate(string clientAppKey,
                                                SystemUser systemUser, int contextId = -1) {
      Assertion.AssertObject(clientAppKey, "clientAppKey");

      var clientApplication = ClientApplication.ParseActive(clientAppKey);
      EmpiriaUser user = EmpiriaUser.Authenticate(systemUser);

      Assertion.AssertObject(user, "user");

      var identity = new EmpiriaIdentity(user);
      identity.SetAuthenticationType(AuthenticationMode.Basic);
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

      var identity = new EmpiriaIdentity(user);
      identity.SetAuthenticationType(AuthenticationMode.Basic);
      return new EmpiriaPrincipal(identity, clientApplication, contextId);
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

    IEmpiriaUser IEmpiriaIdentity.User {
      get { return this.User; }
    }

    public EmpiriaUser User {
      get;
      private set;
    }

    public int UserId {
      get {
        return this.User.Id;
      }
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
