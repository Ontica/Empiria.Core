/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : EmpiriaUser                                      Pattern  : Ontology Relation Type            *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a system's user.                                                                   *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Security {

  public sealed class EmpiriaUser : BaseObject, IEmpiriaUser {

    #region Constructors and parsers

    private EmpiriaUser() {
      // Required by Empiria Framework.
    }

    static public EmpiriaUser Parse(int id) {
      return BaseObject.ParseId<EmpiriaUser>(id);
    }

    static private EmpiriaUser Parse(DataRow row) {
      return BaseObject.ParseDataRow<EmpiriaUser>(row);
    }

    static public EmpiriaUser Current {
      get {
        if (ExecutionServer.IsAuthenticated) {
          return ExecutionServer.CurrentIdentity.User as EmpiriaUser;
        } else {
          return null;
        }
      }
    }

    static internal EmpiriaUser Authenticate(string username, string password, string entropy) {
      EmpiriaUser user = EmpiriaUser.GetUserWithCredentials(username, password, entropy);
      if (!user.IsActive) {
        throw new SecurityException(SecurityException.Msg.NotActiveUser, username);
      }
      if (user.PasswordExpired) {
        throw new SecurityException(SecurityException.Msg.UserPasswordExpired, username);
      }
      user.IsAuthenticated = true;
      return user;
    }

    static internal EmpiriaUser Authenticate(EmpiriaSession activeSession) {
      Assertion.AssertObject(activeSession, "activeSession");

      if (!activeSession.IsStillActive) {
        throw new SecurityException(SecurityException.Msg.ExpiredSessionToken, activeSession.Token);
      }
      EmpiriaUser user = EmpiriaUser.Parse(activeSession.UserId);
      if (!user.IsActive) {
        throw new SecurityException(SecurityException.Msg.NotActiveUser, user.UserName);
      }
      if (user.PasswordExpired) {
        throw new SecurityException(SecurityException.Msg.UserPasswordExpired, user.UserName);
      }
      user.IsAuthenticated = true;
      return user;
    }

    static internal EmpiriaUser Authenticate(AnonymousUser systemUser) {
      EmpiriaUser user = EmpiriaUser.Parse((int) systemUser);
      if (!user.IsActive) {
        throw new SecurityException(SecurityException.Msg.NotActiveUser, systemUser.ToString());
      }
      if (user.PasswordExpired) {
        throw new SecurityException(SecurityException.Msg.UserPasswordExpired, systemUser.ToString());
      }
      user.IsAuthenticated = true;
      return user;
    }

    #endregion Constructors and parsers

    #region Public propertiese

    public bool IsActive {
      get;
      private set;
    }

    public bool IsAuthenticated {
      get;
      private set;
    }

    public bool PasswordExpired {
      get;
      private set;
    }

    public string UserName {
      get;
      private set;
    }

    public string FullName {
      get;
      private set;
    }

    public string EMail {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    static public void ChangePassword(string apiKey, string username, string password) {
      SecurityData.ChangePassword(apiKey, username, password);
    }

    #endregion Public methods

    #region Private methods

    static private EmpiriaUser GetUserWithCredentials(string username, string password, string entropy) {
      DataRow row = SecurityData.GetUserWithCredentials(username, password, entropy);

      return EmpiriaUser.Parse(row);
    }

    internal protected override void OnLoadObjectData(DataRow row) {
      this.UserName = (string) row["UserName"];
      this.IsAuthenticated = false;
      this.IsActive = ((ObjectStatus) Convert.ToChar((string) row["ContactStatus"]) == ObjectStatus.Active);
      this.PasswordExpired = false;
      this.EMail = (string) row["ContactEmail"];
      this.FullName = (string) row["ContactFullName"];
    }

    #endregion Private methods

  } // class EmpiriaUser

} // namespace Empiria.Security
