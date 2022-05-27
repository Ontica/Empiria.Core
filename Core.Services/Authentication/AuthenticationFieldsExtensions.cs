/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Type Extension methods                  *
*  Type     : AuthenticationFieldsExtensions             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for AuthetnticationField instances.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Security;

namespace Empiria.Services.Authentication {

  /// <summary>Extension methods for AuthenticationField instances.</summary>
  static internal class AuthenticationFieldsExtensions {

    static internal void AssertValidForAuthentication(this AuthenticationFields fields) {
      AssertValidForTokenGeneration(fields);

      Assertion.Require(fields.Password, "Password");

      ClientApplication.AssertIsActive(fields.AppKey);
    }


    static internal void AssertValidForTokenGeneration(this AuthenticationFields fields) {
      Assertion.Require(fields.UserID, "UserID");
      Assertion.Require(fields.AppKey, "AppKey");
    }


    static internal string GetRawToken(this AuthenticationFields fields) {
      return $"/{fields.UserID}/{fields.AppKey}/{fields.IpAddress}/";
    }

  }  // class AuthenticationFieldsExtensions

} // namespace Empiria.Services.Authentication
