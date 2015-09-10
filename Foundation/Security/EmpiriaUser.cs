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

using Empiria.Json;

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

    static public EmpiriaUser Parse(string userName, string email) {
      Assertion.AssertObject(userName, "userName");
      Assertion.AssertObject(email, "email");

      EmpiriaUser user = SecurityData.TryGetUserWithUserName(userName);

      if (user == null) {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, userName, email);
      }
      if (user.EMail.Equals(email)) {
        return user;
      } else {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, userName, email);
      }
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

    static public EmpiriaUser Create(string userName, string fullName,
                                     string email, string password, string activationToken,
                                     JsonObject extendedData) {
      Assertion.AssertObject(userName, "userName");
      Assertion.AssertObject(fullName, "fullName");
      Assertion.AssertObject(email, "email");
      Assertion.AssertObject(password, "password");
      Assertion.AssertObject(activationToken, "activationToken");
      Assertion.AssertObject(extendedData, "extendedData");

      var newUser = new EmpiriaUser();
      newUser.Id = SecurityData.GetNextContactId();
      newUser.UserName = userName;
      newUser.FullName = fullName;
      newUser.EMail = email;
      newUser.Claims = new SecurityClaimList(newUser);
      newUser.FillExtendedData(extendedData);

      SecurityData.CreateUser(newUser, password, ObjectStatus.Pending);

      newUser.Claims.AppendSecure(SecurityClaimType.ActivationToken, activationToken);

      return newUser;
    }

    static public bool Exists(string userName) {
      Assertion.AssertObject(userName, "userName");

      return (SecurityData.TryGetUserWithUserName(userName) != null);
    }

    #endregion Constructors and parsers

    #region Authenticate methods

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

    public SecurityClaimList Claims {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    public void Activate(string activationToken) {
      Assertion.AssertObject(activationToken, "activationToken");
      if (!this.Claims.ContainsSecure(SecurityClaimType.ActivationToken, activationToken)) {
        throw new SecurityException(SecurityException.Msg.InvalidActivationToken, activationToken);
      }
      SecurityData.ActivateUser(this);
      this.Claims.RemoveSecure(SecurityClaimType.ActivationToken, activationToken);
      this.IsActive = true;
    }

    static public void ChangePassword(string apiKey, string username, string password) {
      if (apiKey != ConfigurationData.GetString("ChangePasswordKey")) {
        throw new SecurityException(SecurityException.Msg.InvalidClientAppKey, apiKey);
      }
      SecurityData.ChangePassword(username, password);
    }

    public void ChangePassword(string currentPassword, string newPassword) {
      Assertion.AssertObject(currentPassword, "currentPassword");
      Assertion.AssertObject(newPassword, "newPassword");

      EmpiriaUser user = EmpiriaUser.GetUserWithCredentials(this.UserName, currentPassword);

      Assertion.Assert(user != null && user.Id == this.Id, "Invalid current user credentials");

      SecurityData.ChangePassword(this.UserName, newPassword);
    }

    internal JsonObject GetExtendedData() {
      var json = new JsonObject() {
        new JsonItem("FirstName", this.FirstName),
        new JsonItem("LastName", this.LastName),
        new JsonItem("OfficePhone", this.OfficePhone),
        new JsonItem("MobilePhone", this.MobilePhone),
      };
      return json;
    }

    public string GetResetPasswordRequestToken() {
      // 1) Create a reset password request token
      string resetPasswordToken = Guid.NewGuid().ToString();

      // 2) Append the reset password request token to the user's claims collection
      this.Claims.AppendSecure(SecurityClaimType.ResetPasswordToken, resetPasswordToken);

      // 3) Return the reset pasword token
      return resetPasswordToken;
    }

    public void ResetPassword(string resetPasswordToken, string newPassword) {
      Assertion.AssertObject(resetPasswordToken, "resetPasswordToken");
      Assertion.AssertObject(newPassword, "newPassword");

      // 1) Assert the reset password token is in the user's claims collection
      if (!this.Claims.ContainsSecure(SecurityClaimType.ResetPasswordToken, resetPasswordToken)) {
        throw new SecurityException(SecurityException.Msg.InvalidResetPasswordToken, resetPasswordToken);
      }

      // 2) Change the password
      SecurityData.ChangePassword(this.UserName, newPassword);

      // 3) Remove the reset password token from the user's claims collection
      this.Claims.RemoveSecure(SecurityClaimType.ResetPasswordToken, resetPasswordToken);
    }

    #endregion Public methods

    #region Private methods

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

    static private EmpiriaUser GetUserWithCredentials(string username, string password, string entropy = "") {
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

      var json = Json.JsonObject.Parse((string) row["ContactExtData"]);
      this.FillExtendedData(json);

      this.Claims = new SecurityClaimList(this);
    }

    #endregion Private methods

  } // class EmpiriaUser

} // namespace Empiria.Security
