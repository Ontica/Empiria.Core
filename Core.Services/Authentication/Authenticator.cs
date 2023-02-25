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

using Empiria.Collections;

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

      _credentials.Entropy = BuildEntropyString();

      IEmpiriaPrincipal principal = AuthenticationService.Authenticate(_credentials);

      Assertion.Require(principal, "principal");

      return principal;
    }


    internal string GenerateAuthenticationToken() {
      _credentials.AssertValidForTokenGeneration();

      string rawToken = _credentials.GetRawToken();

      var tokenRandomSalt = StoreToken(rawToken);

      return Cryptographer.CreateHashCode(rawToken, tokenRandomSalt);
    }


    private string BuildEntropyString() {
      string rawToken = _credentials.GetRawToken();

      string tokenSalt = GetSaltFromGeneratedTokens(rawToken);

      RemoveTokenFromStore(rawToken);

      return Cryptographer.CreateHashCode(rawToken, tokenSalt);
    }

    #endregion Methods

    #region Token storage helpers

    private static EmpiriaDictionary<string, TokenSalt> _tokens = new EmpiriaDictionary<string, TokenSalt>(64);

    static private string GetSaltFromGeneratedTokens(string rawToken) {
      Assertion.Require(_tokens.ContainsKey(rawToken), "token");

      var storedSalt = _tokens[rawToken];

      Assertion.Require(storedSalt.GenerationTime.AddSeconds(5) > DateTime.Now,
                       "Authentication token has expired.");

      return storedSalt.Value;
    }


    private void RemoveTokenFromStore(string rawToken) {
      _tokens.Remove(rawToken);
    }


    private string StoreToken(string rawToken) {
      var salt = new TokenSalt();

      _tokens.Insert(rawToken, salt);

      return salt.Value;
    }


    sealed private class TokenSalt {

      internal DateTime GenerationTime {
        get;
      } = DateTime.Now;


      internal string Value {
        get;
      } = Guid.NewGuid().ToString();


    }  // class TokenSalt

    #endregion Token storage helpers

  }  // class Authenticator

}  // namespace Empiria.Services.Authentication
