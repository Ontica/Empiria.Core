/* Empiria Extensions ****************************************************************************************
*                                                                                                            *
*  Module   : Empiria User Management                    Component : Service Layer                           *
*  Assembly : Empiria.UserManagement.dll                 Pattern   : Services class                          *
*  Type     : UpdateUserCredentialsService               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Services for update user's access credentials.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Messaging;

using Empiria.Security;

using Empiria.UserManagement.Integration;

namespace Empiria.UserManagement.Services {

  /// <summary>Services for update user's access credentials.</summary>
  static public class UpdateUserCredentialsService {

    #region Services

    static public void CreateUserPassword(string apiKey,
                                          string userName, string userEmail,
                                          string newPassword, bool useSHA256) {
      Assertion.AssertObject(apiKey, "apiKey");
      Assertion.AssertObject(userName, "userName");
      Assertion.AssertObject(userEmail, "userEmail");
      Assertion.AssertObject(newPassword, "newPassword");

      UserManagementService.ChangePassword(apiKey, userName, userEmail,
                                           newPassword, useSHA256);

      var eventPayload = new {
        userName
      };

      EventNotifier.Notify(MessagingEvents.UserPasswordCreated, eventPayload);
    }


    static public void ChangeUserPassword(string currentPassword,
                                          string newPassword, bool useSHA256) {
      Assertion.AssertObject(currentPassword, "currentPassword");
      Assertion.AssertObject(newPassword, "newPassword");

      var apiKey = ConfigurationData.GetString("Empiria.Security", "ChangePasswordKey");

      var userName = EmpiriaPrincipal.Current.Identity.User.UserName;
      var userEmail = EmpiriaPrincipal.Current.Identity.User.EMail;

      UserManagementService.ChangePassword(apiKey, userName, userEmail,
                                           newPassword, useSHA256);

      var eventPayload = new {
        userName
      };

      EventNotifier.Notify(MessagingEvents.UserPasswordChanged, eventPayload);

      EMailServices.SendPasswordChangedWarningEMail();
    }


    #endregion Services

  }  // class UpdateUserCredentialsService

}  // namespace Empiria.UserManagement.Services
