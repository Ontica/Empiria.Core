/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaIdentity                                  Pattern  : Standard Class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Sealed class that represents a Empiria System identity.                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Web;
using System.Web.Security;

namespace Empiria.Security {

  public sealed class EmpiriaIdentity : IEmpiriaIdentity, IDisposable {
    
    #region Fields

    private EmpiriaUser user = null;
    private EmpiriaSession session = null;
    private bool isAuthenticated = false;
    private int regionId = -1;
    private bool disposed = false;

    #endregion Fields

    #region Constructors and parsers

    private EmpiriaIdentity(EmpiriaUser user) {
      Assertion.AssertObject(user, "user");
      this.user = user;
      this.isAuthenticated = true;
      this.session = EmpiriaSession.Create(this);
      this.EnsureValid();
    }

    static public EmpiriaIdentity Authenticate(string username, string password, string entropy, int regionId) {
      EmpiriaUser user = EmpiriaUser.Authenticate(username, password, entropy);

      if (user != null) {
        EmpiriaIdentity identity = new EmpiriaIdentity(user);
        identity.regionId = regionId;
        return identity;
      } else {
        return null;
      }
    }

    static internal EmpiriaIdentity Authenticate(EmpiriaSession session) {
      EmpiriaUser user = EmpiriaUser.Authenticate(session);

      if (user != null) {
        return new EmpiriaIdentity(user);
      } else {
        return null;
      }
    }

    static public EmpiriaPrincipal TryAuthenticate(string sessionToken) {
      EmpiriaSession session = EmpiriaSession.TryParseActive(sessionToken);
      if (session != null) {
        var identity = EmpiriaIdentity.Authenticate(session);
        if (identity != null) {
          return new EmpiriaPrincipal(identity);
        }
      }
      return null;
    }

    // For Empiria Web-Api's authentication 
    static public EmpiriaPrincipal Authenticate(string apiClientKey, string userName, string password,
                                                string entropy,  int contextId) {
      string globalApiKey = ConfigurationData.GetString("Empiria.Trade.API.Key"); 

      if (!globalApiKey.Equals(apiClientKey)) {
        new SecurityException(SecurityException.Msg.InvalidClientID, apiClientKey);
        return null;
      }
      var identity = EmpiriaIdentity.Authenticate(userName, password, entropy, contextId);
      if (identity != null) {
        return new EmpiriaPrincipal(identity);
      }
      return null;
    }

    static public EmpiriaIdentity Authenticate(Uri referrer, FormsAuthenticationTicket remoteTicket) {
      //CheckClient(referrer);
      EmpiriaUser user = EmpiriaUser.Authenticate(remoteTicket);
      if (user != null) {
        return new EmpiriaIdentity(user);
      } else {
        return null;
      }
    }  

    static public EmpiriaIdentity AuthenticateGuest() {
      EmpiriaUser user = EmpiriaUser.AuthenticateGuest();

      if (user != null) {
        return new EmpiriaIdentity(user);
      } else {
        throw new SecurityException(SecurityException.Msg.GuestUserNotFound);
      }
    }

    ~EmpiriaIdentity() {
      Dispose(false);
    }

    #endregion Constructors and parsers

    #region Public properties

    public string AuthenticationType {
      get {
        session.UpdateEndTime();
        return "EmpiriaLogon";
      }
    }

    public bool IsAuthenticated {
      get {
        session.UpdateEndTime();
        return isAuthenticated;
      }
    }

    public int CurrentRegionId {
      get {
        session.UpdateEndTime();
        return regionId;
      }
      set {
        session.UpdateEndTime();
        regionId = value;
      }
    }

    public string Name {
      get {
        session.UpdateEndTime();
        return user.UserName;
      }
    }

    IEmpiriaUser IEmpiriaIdentity.User {
      get { return this.User; }
    }

    public EmpiriaUser User {
      get {
        if (session != null) {
          session.UpdateEndTime();
        }
        return user;
      }
    }

    public int UserId {
      get {
        session.UpdateEndTime();
        return user.Id;
      }
    }

    public IEmpiriaSession Session {
      get {
        session.UpdateEndTime();
        return session;
      }
    }

    #endregion Public properties

    #region Public methods

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion Public methods

    #region Private methods

    private void EnsureValid() {
      Assertion.Assert(this.User != null,
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));
      Assertion.Assert(this.IsAuthenticated,
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));
      Assertion.Assert(this.Session != null,
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));
    }

    private void Dispose(bool disposing) {
      try {
        if (!disposed) {
          disposed = true;
          session.Close();
          ExecutionServer.DisposeSession();
        }
      } finally {

      }
    }

    static private bool ValidateReferrerHost(Uri referrer) {
      return true;
    }

    #endregion Private methods

  } // class EmpiriaIdentity

} // namespace Empiria.Security
