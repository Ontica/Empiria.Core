/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain entity                         *
*  Type     : EmpiriaUser                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a system's user.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Security.Claims;

namespace Empiria.Security {

  /// <summary>Represents a system's user.</summary>
  public sealed class EmpiriaUser : BaseObject, IClaimsSubject {

    #region Constructors and parsers

    private EmpiriaUser() {
      // Required by Empiria Framework
    }

    static public EmpiriaUser Parse(int id) {
      return BaseObject.ParseId<EmpiriaUser>(id);
    }

    static private EmpiriaUser Parse(DataRow row) {
      return BaseObject.ParseDataRow<EmpiriaUser>(row);
    }

    static public EmpiriaUser Parse(string username, string email) {
      Assertion.AssertObject(username, "username");
      Assertion.AssertObject(email, "email");

      EmpiriaUser user = SecurityData.TryGetUserWithUserName(username);

      if (user == null) {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, username, email);
      }
      if (user.EMail.Equals(email)) {
        return user;
      } else {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, username, email);
      }
    }


    static public EmpiriaUser Current {
      get {
        if (ExecutionServer.IsAuthenticated) {
          return ExecutionServer.CurrentIdentity.User;
        } else {
          throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
        }
      }
    }

    static public bool Exists(string userName) {
      Assertion.AssertObject(userName, "userName");

      return (SecurityData.TryGetUserWithUserName(userName) != null);
    }

    #endregion Constructors and parsers

    #region Authenticate methods

    static internal EmpiriaUser Authenticate(string username, string password, string entropy) {
      Assertion.AssertObject(username, "username");
      Assertion.AssertObject(password, "password");
      Assertion.Assert(entropy != null, "entropy can't be null.");

      EmpiriaUser user = EmpiriaUser.GetUserWithCredentials(username, password, entropy);

      user.EnsureCanAuthenticate();
      user.IsAuthenticated = true;

      return user;
    }

    static internal EmpiriaUser Authenticate(EmpiriaSession activeSession) {
      Assertion.AssertObject(activeSession, "activeSession");

      if (!activeSession.IsStillActive) {
        throw new SecurityException(SecurityException.Msg.ExpiredSessionToken, activeSession.Token);
      }

      EmpiriaUser user = EmpiriaUser.Parse(activeSession.UserId);

      user.EnsureCanAuthenticate();
      user.IsAuthenticated = true;

      return user;
    }


    static internal EmpiriaUser AuthenticateAnonymous(AnonymousUser anonymousUser) {
      var anonymous = EmpiriaUser.Parse((int) anonymousUser);

      anonymous.EnsureCanAuthenticate();
      anonymous.IsAuthenticated = true;

      return anonymous;
    }


    #endregion Authenticate methods

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

    public string FirstName {
      get;
      private set;
    }

    public string LastName {
      get;
      private set;
    }

    public string EMail {
      get;
      private set;
    }

    public string OfficePhone {
      get;
      private set;
    }

    public string MobilePhone {
      get;
      private set;
    }

    string IClaimsSubject.ClaimsToken {
      get {
        return this.AsContact().UID;
      }
    }

    #endregion Public properties

    #region Public methods

    public Contact AsContact() {
      return Contact.Parse(this.Id);
    }

    internal JsonObject GetExtendedData() {
      var json = new JsonObject();

      json.Add("FirstName", this.FirstName);
      json.Add("LastName", this.LastName);
      json.Add("OfficePhone", this.OfficePhone);
      json.Add("MobilePhone", this.MobilePhone);

      return json;
    }

    #endregion Public methods

    #region Private methods

    private void EnsureCanAuthenticate() {
      if (!this.IsActive) {
        throw new SecurityException(SecurityException.Msg.NotActiveUser, this.UserName);
      }

      if (this.PasswordExpired) {
        throw new SecurityException(SecurityException.Msg.UserPasswordExpired, this.UserName);
      }
    }

    private void FillExtendedData(JsonObject extendedData) {
      this.FirstName = String.Empty;
      this.LastName = String.Empty;
      this.OfficePhone = String.Empty;
      this.MobilePhone = String.Empty;

      if (extendedData == null || extendedData.IsEmptyInstance) {
        return;
      }
      this.FirstName = extendedData.Get("FirstName", String.Empty);
      this.LastName = extendedData.Get("LastName", String.Empty);
      this.OfficePhone = extendedData.Get("OfficePhone", String.Empty);
      this.MobilePhone = extendedData.Get("MobilePhone", String.Empty);
    }


    static private EmpiriaUser GetUserWithCredentials(string username, string password, string entropy) {
      DataRow row = SecurityData.GetUserWithCredentials(username, password, entropy);

      return EmpiriaUser.Parse(row);
    }


    internal protected override void OnLoadObjectData(DataRow row) {
      this.UserName = (string) row["UserName"];
      this.IsAuthenticated = false;
      this.IsActive = ((EntityStatus) Convert.ToChar((string) row["ContactStatus"]) == EntityStatus.Active);
      this.PasswordExpired = false;
      this.EMail = (string) row["ContactEmail"];
      this.FullName = (string) row["ContactFullName"];

      var json = Json.JsonObject.Parse((string) row["ContactExtData"]);
      this.FillExtendedData(json);

    }

    #endregion Private methods

  } // class EmpiriaUser

} // namespace Empiria.Security
