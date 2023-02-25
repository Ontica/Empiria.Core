/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Type Extension methods                  *
*  Type     : UserCredentialsDtoExtensions               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for UserCredentialsDto instances.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Security;

namespace Empiria.Services.Authentication {

  /// <summary>Extension methods for UserCredentialsDto instances.</summary>
  static internal class UserCredentialsDtoExtensions {

    static internal void AssertValidForAuthentication(this UserCredentialsDto credentials) {
      AssertValidForTokenGeneration(credentials);

      Assertion.Require(credentials.Password, "Password");
    }


    static internal void AssertValidForTokenGeneration(this UserCredentialsDto credentials) {
      Assertion.Require(credentials.UserID, "UserID");
      Assertion.Require(credentials.AppKey, "AppKey");
    }


    static internal string GetRawToken(this UserCredentialsDto credentials) {
      return $"/{credentials.UserID}/{credentials.AppKey}/{credentials.IpAddress}/";
    }

  }  // class UserCredentialsDtoExtensions

} // namespace Empiria.Services.Authentication
