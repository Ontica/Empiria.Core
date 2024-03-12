﻿/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Service provider                        *
*  Type     : SecurityTokenGenerator                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Generates time-based tokens for authentication and other security-sensitive operations.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;
using Empiria.Security;

namespace Empiria.Services.Authentication {

  /// <summary>Describes a security token type.</summary>
  internal enum SecurityTokenType {

    Login,

    ElectronicSign,

    UpdateCredentials

  }


  /// <summary>Generates time-based tokens for authentication and other security-sensitive operations.</summary>
  internal class SecurityTokenGenerator {

    private static readonly int TOKEN_EXPIRATION_SECONDS =
                                          ConfigurationData.Get("SecurityToken.ExpirationSeconds", 10);

    private static readonly EmpiriaDictionary<string, TokenSalt> _tokens =
                                          new EmpiriaDictionary<string, TokenSalt>(64);

    #region Methods

    static internal string GenerateToken(UserCredentialsDto credentials, SecurityTokenType tokenType) {
      string rawToken = GetRawToken(credentials, tokenType);

      var tokenRandomSalt = StoreToken(rawToken);

      return Cryptographer.CreateHashCode(rawToken, tokenRandomSalt);
    }


    static internal string PopToken(UserCredentialsDto credentials, SecurityTokenType tokenType) {
      string rawToken = GetRawToken(credentials, tokenType);

      string tokenSalt = GetSaltFromGeneratedTokens(rawToken);

      RemoveTokenFromStore(rawToken);

      return Cryptographer.CreateHashCode(rawToken, tokenSalt);
    }

    #endregion Methods

    #region Helpers

    static private string GetRawToken(UserCredentialsDto credentials, SecurityTokenType tokenType) {
      Assertion.Require(credentials, nameof(credentials));
      Assertion.Require(credentials.AppKey, nameof(credentials.AppKey));
      Assertion.Require(credentials.UserID, nameof(credentials.UserID));
      Assertion.Require(credentials.UserHostAddress, nameof(credentials.UserHostAddress));

      return $"/{tokenType}/{credentials.AppKey}/{credentials.UserID}/{credentials.UserHostAddress}/";
    }


    static private string GetSaltFromGeneratedTokens(string rawToken) {
      Assertion.Require(_tokens.ContainsKey(rawToken), $"token '{rawToken}' not found.");

      var storedSalt = _tokens[rawToken];

      Assertion.Require(storedSalt.GenerationTime.AddSeconds(TOKEN_EXPIRATION_SECONDS) > DateTime.Now,
                       "Authentication token has expired.");

      return storedSalt.Value;
    }


    static private void RemoveTokenFromStore(string rawToken) {
      _tokens.Remove(rawToken);
    }


    static private string StoreToken(string rawToken) {
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


    #endregion Helpers

  }  // class SecurityTokenGenerator

}  // namespace Empiria.Services.Authentication
