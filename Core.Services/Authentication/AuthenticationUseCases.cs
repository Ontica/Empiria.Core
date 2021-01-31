/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Use cases Layer                         *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Use case interactor                     *
*  Type     : AuthenticationUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for authenticate users.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Security;

namespace Empiria.Services.Authentication {

  /// <summary>Use cases for authenticate users.</summary>
  public class AuthenticationUseCases : UseCase {

    #region Constructors and parsers

    protected AuthenticationUseCases() {
      // no-op
    }

    static public AuthenticationUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<AuthenticationUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public EmpiriaPrincipal Authenticate(AuthenticationFields fields) {
      Assertion.AssertObject(fields, "fields");

      var authenticator = new Authenticator(fields);

      return authenticator.Authenticate();
    }


    public string GenerateAuthenticationToken(AuthenticationFields fields) {
      Assertion.AssertObject(fields, "fields");

      var authenticator = new Authenticator(fields);

      return authenticator.GenerateAuthenticationToken();
    }


    #endregion Use cases

  }  // class AuthenticationUseCases

}  // namespace Empiria.Core.Authentication
