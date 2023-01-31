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

using Empiria.Security.Items;

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
      Assertion.Require(session, "session");
      Assertion.Require(identity, "identity");

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }
      this.Initialize(identity, ClientApplication.Parse(session.ClientAppId), session);
    }

    /// <summary>Initializes a new instance of the EmpiriaPrincipal class from an authenticated
    /// EmpiriaIdentity. Fails if identity represents a non authenticated EmpiriaIdentity.</summary>
    /// <param name="identity">Represents an authenticated Empiria user.</param>
    internal EmpiriaPrincipal(EmpiriaIdentity identity, ClientApplication clientApp,
                              Json.JsonObject contextData = null) {
      Assertion.Require(identity, "identity");
      Assertion.Require(clientApp, "clientApp");

      if (!identity.IsAuthenticated) {
        throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
      }
      this.Initialize(identity, clientApp, contextData: contextData);
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

    public string[] PermissionsArray {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    public void CloseSession() {
      try {
        this.Session.Close();
        principalsCache.Remove(this.Session.Token);
      } finally {
        // no-op
      }
    }


    public bool HasDataAccessTo<T>(T entity) where T : IIdentifiable {
      FixedList<int> userIds = SecurityData.GetUsersWithDataAccessTo(typeof(T), entity);

      if (entity.UID == "NivelacionCuentasCompraventa") {
        return EmpiriaMath.IsMemberOf(ExecutionServer.CurrentUserId, new[] { 135, 1002, 1003, 2006, 3512, 3548 });

      } else if (entity.UID == "986e908e-cef5-4313-87f2-f2ba5510ca13") {
        return EmpiriaMath.IsMemberOf(ExecutionServer.CurrentUserId, new[] { 690, 1002, 1003, 1005, 1882, 1949, 2007 });

      } else if (entity.GetType().Name == "FinancialConceptGroup") {

        return EmpiriaMath.IsMemberOf(ExecutionServer.CurrentUserId, new[] { 135, 307, 918, 1002, 1002, 1003, 1004, 1005,
                                                                            1724, 1760, 1825, 1882, 1896, 1985, 3512, 3548 });
      }

      if (userIds.Count == 0) {
        return true;
      }

      return userIds.Contains(this.Identity.User.Id);
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

    private void Initialize(EmpiriaIdentity identity, ClientApplication clientApp = null,
                            EmpiriaSession session = null, Json.JsonObject contextData = null) {
      this.Identity = identity;

      if (session != null) {
        this.ClientApp = ClientApplication.Parse(session.ClientAppId);
        this.Session = session;

      } else {
        Assertion.Require(clientApp, "clientApp");
        this.ClientApp = clientApp;
        this.Session = EmpiriaSession.Create(this, contextData);
      }

      LoadRolesArray(identity.User.Id);

      this.PermissionsArray = GetPermissionsArray();

      principalsCache.Insert(this.Session.Token, this);

      this.ContextItems = new AssortedDictionary();

      this.RefreshBeforeReturn();
    }

    private string[] GetPermissionsArray() {
      var permissionsBuilder = new PermissionsBuilder(this.ClientApp, this.Identity);

      return permissionsBuilder.Build();
    }


    private void LoadRolesArray(int participantId) {
      rolesArray = new string[0];
    }

    #endregion Private methods

  } // class EmpiriaPrincipal

} // namespace Empiria.Security
