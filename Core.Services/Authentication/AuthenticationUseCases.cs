/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Use cases Layer                         *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Use case interactor                     *
*  Type     : AuthenticationUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for authenticate users.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

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

    public IEmpiriaPrincipal Authenticate(UserCredentialsDto credentials) {
      Assertion.Require(credentials, nameof(credentials));

      credentials.Entropy = SecurityTokenGenerator.PopToken(credentials, SecurityTokenType.Login);

      IEmpiriaPrincipal principal = AuthenticationService.Authenticate(credentials);

      Assertion.Require(principal, nameof(principal));

      EmpiriaLog.Operation(principal.Session, "UserAuthentication",
                           $"El usuario ingresó al sistema.");

      return principal;
    }


    public string GenerateAuthenticationToken(UserCredentialsDto credentials) {
      Assertion.Require(credentials, nameof(credentials));

      return SecurityTokenGenerator.GenerateToken(credentials, SecurityTokenType.Login);
    }


    public string GenerateNewCredentialsToken(UserCredentialsDto credentials) {
      Assertion.Require(credentials, nameof(credentials));

      return SecurityTokenGenerator.GenerateToken(credentials, SecurityTokenType.UpdateCredentials);
    }


    public void Logout() {
      if (!ExecutionServer.IsAuthenticated) {
        return;
      }
      ExecutionServer.CurrentPrincipal.Logout();
    }


    public PrincipalDto PrincipalData() {
      Assertion.Require(ExecutionServer.IsAuthenticated, "Unauthenticated user.");

      IEmpiriaPrincipal principal = ExecutionServer.CurrentPrincipal;

      return PrincipalMapper.Map(principal);
    }


    #endregion Use cases

  }  // class AuthenticationUseCases

}  // namespace Empiria.Core.Authentication
