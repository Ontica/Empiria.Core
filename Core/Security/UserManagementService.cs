/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : User Management Services              *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain service class                  *
*  Type     : UserManagementService                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides methods to manage users, such as user activation and credentials management.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Provides methods to manage users, such as user activation
  /// and credentials management.</summary>
  static public class UserManagementService {

    #region Services

    static public void ChangePassword(string apiKey, string username, string email, string password) {
      if (apiKey != ConfigurationData.GetString("ChangePasswordKey")) {
        throw new SecurityException(SecurityException.Msg.InvalidClientAppKey, apiKey);
      }
      EmpiriaUser user = EmpiriaUser.Parse(username, email);

      VerifyPasswordStrengthRules(user, password);

      SecurityData.ChangePassword(username, password);
    }


    static public void VerifyPasswordStrengthRules(EmpiriaUser user, string newPassword) {
      var helper = new PasswordStrength(user, newPassword);

      helper.VerifyStrength();
    }

    #endregion Services

  }  // class UserManagementService

}  // namespace Empiria.Security


/*

    static public void Activate(string activationToken) {
      Assertion.AssertObject(activationToken, "activationToken");

      if (!this.Claims.ContainsSecure(SecurityClaimType.ActivationToken, activationToken)) {
        throw new SecurityException(SecurityException.Msg.InvalidActivationToken, activationToken);
      }
      SecurityData.ActivateUser(this);
      this.Claims.RemoveSecure(SecurityClaimType.ActivationToken, activationToken);
      this.IsActive = true;
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
      newUser.UserName = userName;
      newUser.FullName = fullName;
      newUser.EMail = email;
      newUser.Claims = new SecurityClaimList(newUser);
      newUser.FillExtendedData(extendedData);

      newUser.VerifyPasswordStrengthRules(password);

      newUser.Id = SecurityData.GetNextContactId();
      SecurityData.CreateUser(newUser, password, ObjectStatus.Pending);

      newUser.Claims.AppendSecure(SecurityClaimType.ActivationToken, activationToken);

      return newUser;
    }


    static public void ChangePassword(string currentPassword, string newPassword) {
      Assertion.AssertObject(currentPassword, "currentPassword");
      Assertion.AssertObject(newPassword, "newPassword");

      EmpiriaUser user = EmpiriaUser.GetUserWithCredentials(this.UserName, currentPassword);

      Assertion.Assert(user != null && user.Id == this.Id, "Invalid current user credentials");

      user.VerifyPasswordStrengthRules(newPassword);


      SecurityData.ChangePassword(this.UserName, newPassword);
    }


    static public string GetResetPasswordRequestToken() {
      // 1) Create a reset password request token
      string resetPasswordToken = Guid.NewGuid().ToString();

      // 2) Append the reset password request token to the user's claims collection
      this.Claims.AppendSecure(SecurityClaimType.ResetPasswordToken, resetPasswordToken);

      // 3) Return the reset pasword token
      return resetPasswordToken;
    }


    static public void ResetPassword(string resetPasswordToken, string newPassword) {
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

*/
