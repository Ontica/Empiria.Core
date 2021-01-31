/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : User Management                            Component : Use cases Layer                         *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Use case interactor                     *
*  Type     : UserCredentialsUseCases                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for update user's access credentials.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Messaging;

using Empiria.Security;

using Empiria.Services.UserManagement.Providers;

namespace Empiria.Services.UserManagement {

  /// <summary>Use cases for update user's access credentials.</summary>
  public class UserCredentialsUseCases : UseCase {

    #region Constructors and parsers

    protected UserCredentialsUseCases() {
      // no-op
    }

    static public UserCredentialsUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<UserCredentialsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public void CreateUserPassword(string apiKey,
                                   string userName, string userEmail,
                                   string newPassword) {
      Assertion.AssertObject(apiKey, "apiKey");
      Assertion.AssertObject(userName, "userName");
      Assertion.AssertObject(userEmail, "userEmail");
      Assertion.AssertObject(newPassword, "newPassword");

      ChangePassword(apiKey, userName, userEmail, newPassword);

      var eventPayload = new {
        userName
      };

      EventNotifier.Notify(MessagingEvents.UserPasswordCreated, eventPayload);
    }


    public void ChangeUserPassword(string currentPassword, string newPassword) {
      Assertion.AssertObject(currentPassword, "currentPassword");
      Assertion.AssertObject(newPassword, "newPassword");

      var apiKey = ConfigurationData.GetString("Empiria.Security", "ChangePasswordKey");

      var userName = EmpiriaPrincipal.Current.Identity.User.UserName;
      var userEmail = EmpiriaPrincipal.Current.Identity.User.EMail;

      ChangePassword(apiKey, userName, userEmail, newPassword);

      var eventPayload = new {
        userName
      };

      EventNotifier.Notify(MessagingEvents.UserPasswordChanged, eventPayload);

      EMailServices.SendPasswordChangedWarningEMail();
    }

    #endregion Use cases

    #region Helpers

    static private void ChangePassword(string apiKey, string username,
                                       string email, string newPassword) {
      if (apiKey != ConfigurationData.GetString("ChangePasswordKey")) {
        throw new SecurityException(SecurityException.Msg.InvalidClientAppKey, apiKey);
      }

      EmpiriaUser user = EmpiriaUser.Parse(username, email);

      var helper = new PasswordStrength(user, newPassword);

      helper.VerifyStrength();

      UserManagementData.ChangePassword(username, newPassword);
    }

    #endregion Helpers

  }  // class UserCredentialsUseCases

}  // namespace Empiria.Core.Services
