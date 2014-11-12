/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaUser                                      Pattern  : Ontology Relation Type            *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a human user.                                                                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Web.Security;

using Empiria.Contacts;

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

    static internal EmpiriaUser Authenticate(SystemUser systemUser) {
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

    static public EmpiriaUser Current {
      get {
        if (ExecutionServer.IsAuthenticated) {
          return ExecutionServer.CurrentIdentity.User as EmpiriaUser;
        } else {
          return null;
        }
      }
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

    protected override void OnLoadObjectData(DataRow row) {
      this.UserName = (string) row["UserName"];
      this.IsAuthenticated = false;
      this.IsActive = ((GeneralObjectStatus) Convert.ToChar((string) row["ContactStatus"]) == GeneralObjectStatus.Active);
      this.PasswordExpired = false;
      this.EMail = (string) row["ContactEmail"];
      this.FullName = (string) row["ContactFullName"];
    }

    #endregion Private methods

  } // class EmpiriaUser

} // namespace Empiria.Security
