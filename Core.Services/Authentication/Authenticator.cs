/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Service provider                        *
*  Type     : Authenticator                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides authentication services.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Security;

namespace Empiria.Services.Authentication {

  /// <summary>Provides authentication services.</summary>
  internal class Authenticator {

    #region Constructors and fields

    private readonly UserCredentialsDto _credentials;

    internal Authenticator(UserCredentialsDto credentials) {
      Assertion.Require(credentials, nameof(credentials));

      _credentials = credentials;
    }

    #endregion Constructors and fields

    #region Methods

    internal IEmpiriaPrincipal Authenticate() {
      _credentials.AssertValidForAuthentication();

      _credentials.Entropy = SecurityTokenGenerator.PopToken(_credentials, SecurityTokenType.Login);

      IEmpiriaPrincipal principal = AuthenticationService.Authenticate(_credentials);

      Assertion.Require(principal, nameof(principal));

      return principal;
    }

    #endregion Methods

  }  // class Authenticator

}  // namespace Empiria.Services.Authentication
