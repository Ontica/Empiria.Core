/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Security Services                          Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Service provider                        *
*  Type     : SecurityTokenGenerator                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Generates time-based tokens for authentication and other security-sensitive operations.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;

namespace Empiria.Security {

  /// <summary>Describes a security token type.</summary>
  public enum SecurityTokenType {

    Login,

    ElectronicSign,

    UpdateCredentials

  }


  /// <summary>Interface that holds data used to generate security tokens.</summary>
  public interface ISecurityTokenData {

    string AppKey { get; }

    string UserID { get; }

    string UserHostAddress { get; }

  }


  /// <summary>Generates time-based tokens for authentication and other security-sensitive operations.</summary>
  public class SecurityTokenGenerator {

    private static readonly int TOKEN_EXPIRATION_SECONDS =
                                          ConfigurationData.Get("SecurityToken.ExpirationSeconds", 10);

    private static readonly EmpiriaDictionary<string, TokenSalt> _tokens =
                                          new EmpiriaDictionary<string, TokenSalt>(64);

    #region Methods

    static public string GenerateToken(ISecurityTokenData tokenData, SecurityTokenType tokenType) {
      string rawToken = GetRawToken(tokenData, tokenType);

      var tokenRandomSalt = StoreToken(rawToken);

      return Cryptographer.CreateHashCode(rawToken, tokenRandomSalt);
    }


    static public string PopToken(ISecurityTokenData tokenData, SecurityTokenType tokenType) {
      string rawToken = GetRawToken(tokenData, tokenType);

      string tokenSalt = GetSaltFromGeneratedTokens(rawToken);

      RemoveTokenFromStore(rawToken);

      return Cryptographer.CreateHashCode(rawToken, tokenSalt);
    }

    #endregion Methods

    #region Helpers

    static private string GetRawToken(ISecurityTokenData tokenData, SecurityTokenType tokenType) {
      Assertion.Require(tokenData, nameof(tokenData));
      Assertion.Require(tokenData.AppKey, nameof(tokenData.AppKey));
      Assertion.Require(tokenData.UserID, nameof(tokenData.UserID));
      Assertion.Require(tokenData.UserHostAddress, nameof(tokenData.UserHostAddress));

      return $"/{tokenType}/{tokenData.AppKey}/{tokenData.UserID}/{tokenData.UserHostAddress}/";
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

}  // namespace Empiria.Security
