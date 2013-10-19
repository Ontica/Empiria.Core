/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaIdentity                                  Pattern  : Standard Class                    *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Sealed class that represents a Empiria System identity.                                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Web;
using System.Web.Security;

namespace Empiria.Security {

  public sealed class EmpiriaIdentity : IEmpiriaIdentity, IDisposable {
    
    #region Fields

    private EmpiriaUser user = null;
    private EmpiriaSession session = null;
    private bool isAuthenticated = false;
    private int regionId = 0;
    private bool disposed = false;

    static readonly private string globalApiKey = ConfigurationData.GetString("Empiria.Trade.API.Key"); 

    #endregion Fields

    #region Constructors and parsers

    private EmpiriaIdentity(EmpiriaUser user) {
      Assertion.RequireObject(user, "user");
      this.user = user;
    }

    static public EmpiriaIdentity Authenticate(string name, string password, string entropy, int regionId) {
      EmpiriaUser user = Empiria.Security.EmpiriaUser.Authenticate(name, password, entropy);

      if (user != null) {
        EmpiriaIdentity identity = new EmpiriaIdentity(user);

        identity.session = CreateSession(user.Id);
        identity.isAuthenticated = true;
        identity.regionId = regionId;
        ValidateIdentity(identity);
        return identity;
      } else {
        return null;
      }
    }

    // For Empiria Web-Api's authentication 
    static public EmpiriaPrincipal Authenticate(string clientID, string userName, string password, 
                                                string entropy,  int contextId) {
      if (!globalApiKey.Equals(clientID)) {
        new SecurityException(SecurityException.Msg.InvalidClientID, clientID);
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
        EmpiriaIdentity identity = new EmpiriaIdentity(user);

        identity.session = CreateSession(user.Id);
        identity.isAuthenticated = true;
        ValidateIdentity(identity);
        return identity;
      } else {
        return null;
      }
    }  

    static public EmpiriaIdentity AuthenticateGuest() {
      EmpiriaUser user = Empiria.Security.EmpiriaUser.AuthenticateGuest();

      if (user != null) {
        EmpiriaIdentity identity = new EmpiriaIdentity(user);

        identity.session = CreateSession(user.Id);
        identity.isAuthenticated = true;
        identity.regionId = 0;
        ValidateIdentity(identity);

        return identity;
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
        session.UpdateEndTime();
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

    static private EmpiriaSession CreateSession(int userId) {
      return new EmpiriaSession(userId,
                                HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : "Session-less",
                                HttpContext.Current.Request.UserHostAddress,
                                HttpContext.Current.Request.UserAgent);
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

    static private void ValidateIdentity(EmpiriaIdentity identity) {
      Assertion.Ensure((identity != null) && (identity.IsAuthenticated),
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));
    }

    static private bool ValidateReferrerHost(Uri referrer) {
      return true;
    }

    #endregion Private methods

  } // class EmpiriaIdentity

} // namespace Empiria.Security