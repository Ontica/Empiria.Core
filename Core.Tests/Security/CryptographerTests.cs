/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Cryptographer Tests                     *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Test class                              *
*  Type     : CryptographerTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Security claims services tests.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Security;

namespace Empiria.Tests.Security {

  /// <summary>Security claims services tests.</summary>
  public class CryptographerTests {

    [Fact]
    public void MustEncryptAShortTextWithEntropy() {
      const string secret = "My protected message";
      const string entropy = "ABC123";

      string encrypted = Cryptographer.Encrypt(EncryptionMode.EntropyKey, secret, entropy);

      Assert.NotEqual(secret, encrypted);

      string decrypted = Cryptographer.Decrypt(encrypted, entropy);

      Assert.Equal(secret, decrypted);
    }


    [Fact]
    public void MustEncryptTheHashOfAShortTextWithEntropy() {
      const string secret = "My protected message";
      const string entropy = "ABC123";

      string encrypted = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, secret, entropy);

      Assert.NotEqual(secret, encrypted);

      string decrypted = Cryptographer.Decrypt(encrypted, entropy);


      string expected = Cryptographer.Encrypt(EncryptionMode.EntropyKey, secret, entropy);
      expected = Cryptographer.CreateHashCode(expected, entropy);

      Assert.Equal(expected, decrypted);
    }

    [Fact]
    public void MustEncryptATextUsingPureAES() {
      const string secret = "My protected message";
      const string entropy = "Zw-@61737323313233ABCDEFGHZBwDEF";

      string encrypted = Cryptographer.Encrypt(EncryptionMode.Pure, secret, entropy);

      Assert.NotEqual(secret, encrypted);

      string decrypted = Cryptographer.Decrypt(encrypted, entropy, true);

      Assert.Equal(secret, decrypted);
    }

  }  // SecurityClaimsTests

}  // namespace Empiria.Tests.Security
