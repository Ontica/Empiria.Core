/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : EmpiriaPrincipal                                 Pattern  : Standard Class                    *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents the security context of the user or access account on whose behalf the Empiria     *
*              framework code is running, including that user's identity (EmpiriaIdentity) and any domain    *
*              roles to which they belong. This class can't be derived.                                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Security.Principal;

using Empiria.Collections;

namespace Empiria.Security {

  /// <summary>Represents the security context of the user or access account on whose behalf the Empiria
  /// framework code is running, including that user's identity (EmpiriaIdentity) and any domain
  /// roles to which they belong. This class can't be derived.</summary>
  public sealed class EmpiriaPrincipal : IEmpiriaPrincipal {

    #region Fields

    static private ObjectsCache<string, EmpiriaPrincipal> principalsCache =
                                        new ObjectsCache<string, EmpiriaPrincipal>(128);

    private string[] rolesArray = new string[0];
    private bool disposed = false;

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
      this.ContextItems = new AssortedDictionary();
      this.Session.UpdateEndTime();
    }

    ~EmpiriaPrincipal() {
      Dispose(false);
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

    IEmpiriaSession IEmpiriaPrincipal.Session {
      get {
        return this.Session;
      }
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

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>Determines whether the current principal belongs to the specified role.</summary>
    /// <param name="role">The name of the role for which to check membership.</param>
    /// <returns>true if the current principal is a member of the specified role in the current domain;
    /// otherwise, false.</returns>
    public bool IsInRole(string role) {
      string[] users = SecurityData.GetUsersInRole(role);

      for (int i = 0; i < users.Length; i++) {
        if (users[i].Trim() == this.Identity.UserId.ToString()) {
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

    private void Dispose(bool disposing) {
      try {
        if (!disposed) {
          disposed = true;
          this.Session.Close();
          principalsCache.Remove(this.Session.Token);
          if (disposing) {
            //this.InnerObject.Dispose();
          }
        }
      } finally {
        // no-op
      }
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
      LoadRolesArray(identity.UserId);
      principalsCache.Add(this.Session.Token, this);
      this.RefreshBeforeReturn();

      this.Claims = this.Identity.User.Claims;
    }

    private void LoadRolesArray(int participantId) {
      //identity.User.GetRoles();
      rolesArray = new string[0];
    }

    #endregion Private methods

  } // class EmpiriaPrincipal

} // namespace Empiria.Security
