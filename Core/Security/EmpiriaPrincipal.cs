/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaPrincipal                                 Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Represents the security context of the user or access account on whose behalf the Empiria     *
*              framework code is running, including that user's identity (EmpiriaIdentity) and any domain    *
*              roles to which they belong. This class can't be derived.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Security.Principal;

using Empiria.Collections;

namespace Empiria.Security {

  /// <summary>Represents the security context of the user or access account on whose behalf the Empiria
  /// framework code is running, including that user's identity (EmpiriaIdentity) and any domain
  /// roles to which they belong. This class can't be derived.</summary>
  public sealed class EmpiriaPrincipal : IPrincipal {

    #region Fields

    static private EmpiriaDictionary<string, EmpiriaPrincipal> principalsCache =
                                        new EmpiriaDictionary<string, EmpiriaPrincipal>(128);

    private string[] rolesArray = new string[0];

    #endregion Fields

    #region Constructors and parsers

    internal EmpiriaPrincipal(EmpiriaIdentity identity, EmpiriaSession session) {
      Assertion.AssertObject(session, "session");
      Assertion.AssertObject(identity, "identity");

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }
      this.Initialize(identity, ClientApplication.Parse(session.ClientAppId), session);
    }

    /// <summary>Initializes a new instance of the EmpiriaPrincipal class from an authenticated
    /// EmpiriaIdentity. Fails if identity represents a non authenticated EmpiriaIdentity.</summary>
    /// <param name="identity">Represents an authenticated Empiria user.</param>
    internal EmpiriaPrincipal(EmpiriaIdentity identity, ClientApplication clientApp,
                              int contextId = -1) {
      Assertion.AssertObject(identity, "identity");
      Assertion.AssertObject(clientApp, "clientApp");

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }
      this.Initialize(identity, clientApp, contextId: contextId);
    }

    static public EmpiriaPrincipal Current {
      get {
        return ExecutionServer.CurrentPrincipal as EmpiriaPrincipal;
      }
    }

    static internal EmpiriaPrincipal TryParseWithToken(string sessionToken) {
      EmpiriaPrincipal principal = null;
      if (principalsCache.ContainsKey(sessionToken)) {
        principal = principalsCache[sessionToken];
        principal.RefreshBeforeReturn();
      }
      return principal;
    }

    private void RefreshBeforeReturn() {
      this.Session.UpdateEndTime();
    }

    #endregion Constructors and parsers

    #region Public properties

    /// <summary>Gets the ClientApplication of the current principal.</summary>
    public ClientApplication ClientApp {
      get;
      private set;
    }

    public int ContextId {
      get;
      private set;
    }

    public AssortedDictionary ContextItems {
      get;
      private set;
    }

    /// <summary>Gets the IIdentity instance of the current principal.</summary>
    public EmpiriaIdentity Identity {
      get;
      private set;
    }

    IIdentity IPrincipal.Identity {
      get {
        return this.Identity;
      }
    }

    public string[] RolesArray {
      get {
        return rolesArray;
      }
    }

    public EmpiriaSession Session {
      get;
      private set;
    }

    public SecurityClaimList Claims {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    public void AssertClaim(SecurityClaimType claimType, string claimValue, string assertionFailMsg = null) {
      Assertion.AssertObject(claimValue, "claimValue");

      if (this.Claims.Contains(claimType, claimValue)) {
        return;
      }

      if (String.IsNullOrWhiteSpace(assertionFailMsg)) {
        assertionFailMsg = this.BuildAssertionClaimFailMsg(claimType, claimValue);
      }
      throw new SecurityException(SecurityException.Msg.PrincipalClaimNotFound, assertionFailMsg);
    }

    public void CloseSession() {
      try {
        this.Session.Close();
        principalsCache.Remove(this.Session.Token);
      } finally {
        // no-op
      }
    }

    /// <summary>Determines whether the current principal belongs to the specified role.</summary>
    /// <param name="role">The name of the role for which to check membership.</param>
    /// <returns>true if the current principal is a member of the specified role in the current domain;
    /// otherwise, false.</returns>
    public bool IsInRole(string role) {
      string[] users = SecurityData.GetUsersInRole(role);

      for (int i = 0; i < users.Length; i++) {
        if (users[i].Trim() == this.Identity.User.Id.ToString()) {
          return true;
        }
      }
      return false;
    }

    #endregion Public methods

    #region Private methods

    private string BuildAssertionClaimFailMsg(SecurityClaimType claimType, string claimValue) {
      return String.Format("Principal '{0}' doesn't have a security claim with value '{1}' of type '{2}'.",
                            this.Identity.User.UserName, claimValue, claimType.Key);
    }

    private void Initialize(EmpiriaIdentity identity, ClientApplication clientApp = null,
                            EmpiriaSession session = null, int contextId = -1) {
      this.Identity = identity;
      if (session != null) {
        this.ClientApp = ClientApplication.Parse(session.ClientAppId);
        this.ContextId = (contextId != -1) ? contextId : session.ContextId;
        this.Session = session;
      } else {
        Assertion.AssertObject(clientApp, "clientApp");
        this.ClientApp = clientApp;
        this.ContextId = contextId;
        this.Session = EmpiriaSession.Create(this);
      }
      LoadRolesArray(identity.User.Id);
      principalsCache.Insert(this.Session.Token, this);

      this.ContextItems = new AssortedDictionary();
      this.RefreshBeforeReturn();

      this.Claims = identity.User.Claims;
    }

    private void LoadRolesArray(int participantId) {
      //identity.User.GetRoles();
      rolesArray = new string[0];
    }

    #endregion Private methods

  } // class EmpiriaPrincipal

} // namespace Empiria.Security
