/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Value type                            *
*  Type     : EmpiriaPrincipal                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents the security context of the user or access account on whose behalf the Empiria      *
*             framework code is running, including that user's identity (EmpiriaIdentity) and any domain     *
*             roles to which they belong. This class can't be derived.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;

using System.Security.Principal;

using Empiria.Collections;
using Empiria.Json;

using Empiria.Security.Providers;

namespace Empiria.Security {

  /// <summary>Represents the security context of the user or access account on whose behalf the Empiria
  /// framework code is running, including that user's identity (EmpiriaIdentity) and any domain
  /// roles to which they belong. This class can't be derived.</summary>
  public sealed class EmpiriaPrincipal : IPrincipal {

    #region Fields

    static private EmpiriaDictionary<string, EmpiriaPrincipal> principalsCache =
                                        new EmpiriaDictionary<string, EmpiriaPrincipal>(128);

    private readonly Lazy<SecurityObjects> _securityObjects;

    #endregion Fields

    #region Constructors and parsers

    internal EmpiriaPrincipal(EmpiriaIdentity identity, IClientApplication clientApp, EmpiriaSession session) {
      Assertion.Require(identity, nameof(identity));
      Assertion.Require(clientApp, nameof(clientApp));
      Assertion.Require(session, nameof(session));

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }

      this.Initialize(identity, clientApp, session);

      _securityObjects = new Lazy<SecurityObjects>(() => new SecurityObjects(this));
    }

    /// <summary>Initializes a new instance of the EmpiriaPrincipal class from an authenticated
    /// EmpiriaIdentity. Fails if identity represents a non authenticated EmpiriaIdentity.</summary>
    /// <param name="identity">Represents an authenticated Empiria user.</param>
    internal EmpiriaPrincipal(EmpiriaIdentity identity, IClientApplication clientApp,
                              JsonObject contextData = null) {
      Assertion.Require(identity, nameof(identity));
      Assertion.Require(clientApp, nameof(clientApp));

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }

      this.Initialize(identity, clientApp, contextData);

      _securityObjects = new Lazy<SecurityObjects>(() => new SecurityObjects(this));
    }


    static public EmpiriaPrincipal Current {
      get {
        return ExecutionServer.CurrentPrincipal;
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

    #endregion Constructors and parsers

    #region Properties

    /// <summary>Gets the ClientApplication of the current principal.</summary>
    public IClientApplication ClientApp {
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


    private FixedList<IObjectAccessRule> ObjectAccessRules {
      get {
        return _securityObjects.Value.ObjectAccessRules;
      }
    }


    public FixedList<string> Permissions {
      get {
        return _securityObjects.Value.Permissions;
      }
    }


    private FixedList<string> Roles {
      get {
        return _securityObjects.Value.Roles;
      }
    }


    public EmpiriaSession Session {
      get;
      private set;
    }

    #endregion Properties

    #region Methods

    public void CloseSession() {
      try {
        this.Session.Close();
        principalsCache.Remove(this.Session.Token);
      } finally {
        // no-op
      }
    }


    public bool HasDataAccessTo<T>(T entity) where T : IIdentifiable {

      var rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().Name &&
                                            x.ObjectsUIDs.Contains(entity.UID));
      if (rules.Count != 0) {
        return true;
      }

      rules = ObjectAccessRules.FindAll(x => x.TypeName == entity.GetType().Name &&
                                            !x.ObjectsUIDs.Contains(entity.UID));

      if (rules.Count != 0) {
        return false;
      }

      if (entity.UID == "NivelacionCuentasCompraventa") {

        return EmpiriaMath.IsMemberOf(ExecutionServer.CurrentUserId, new[] { 135, 1002, 1003, 2006, 3512, 3548 });
      }

      return true;
    }


    /// <summary>Determines whether the current principal belongs to the specified role.</summary>
    /// <param name="role">The name of the role for which to check membership.</param>
    /// <returns>true if the current principal is a member of the specified role in the current domain;
    /// otherwise, false.</returns>
    public bool IsInRole(string role) {
      return this.Roles.Contains(role);
    }


    #endregion Methods

    #region Helpers

    private void Initialize(EmpiriaIdentity identity, IClientApplication clientApp,
                            JsonObject contextData = null) {
      Assertion.Require(identity, nameof(identity));
      Assertion.Require(clientApp, nameof(clientApp));

      this.Identity = identity;
      this.ClientApp = clientApp;

      this.Session = EmpiriaSession.Create(this, contextData);

      principalsCache.Insert(this.Session.Token, this);

      this.ContextItems = new AssortedDictionary();

      this.RefreshBeforeReturn();
    }


    private void Initialize(EmpiriaIdentity identity, IClientApplication clientApp,
                            EmpiriaSession session) {
      Assertion.Require(identity, nameof(identity));
      Assertion.Require(session, nameof(session));

      this.Identity = identity;

      this.ClientApp = clientApp;

      this.Session = session;

      principalsCache.Insert(this.Session.Token, this);

      this.ContextItems = new AssortedDictionary();

      this.RefreshBeforeReturn();
    }

    private void RefreshBeforeReturn() {
      this.Session.UpdateEndTime();
    }

    #endregion Helpers


    sealed private class SecurityObjects {

      internal SecurityObjects(EmpiriaPrincipal principal) {
        var provider = SecurityProviders.PermissionsProvider();

        this.Roles = provider.GetRoles(principal.Identity, principal.ClientApp);

        this.Permissions = provider.GetFeaturesPermissions(principal.Identity, principal.ClientApp);

        this.ObjectAccessRules = provider.GetObjectAccessRules(principal.Identity, principal.ClientApp);
      }


      internal FixedList<IObjectAccessRule> ObjectAccessRules {
        get;
      }

      internal FixedList<string> Permissions {
        get;
      }

      internal FixedList<string> Roles {
        get;
      }

    }  // inner class SecurityObjects

  } // class EmpiriaPrincipal

} // namespace Empiria.Security
