/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : User Management                            Component : Use cases Layer                         *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Use case interactor                     *
*  Type     : UserManagementUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for user management.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;


namespace Empiria.Services.UserManagement {

  /// <summary>Use cases for user management.</summary>
  public class UserManagementUseCases : UseCase {

    #region Constructors and parsers

    protected UserManagementUseCases() {
      // no-op
    }

    static public UserManagementUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<UserManagementUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<UserDescriptorDto> SearchUsers(string keywords) {
      var users = UserManagementData.SearchUsers(keywords);

      return users.Select(x => (UserDescriptorDto) x)
                  .ToFixedList();
    }

    #endregion Helpers

  }  // class UserManagementUseCases

}  // namespace Empiria.Services.UserManagement
