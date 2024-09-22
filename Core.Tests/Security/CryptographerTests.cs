/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : CryptographerTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Cryptographer services tests.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Security;

namespace Empiria.Tests.Security {

  /// <summary>Cryptographer services tests.</summary>
  public class CryptographerTests {

    [Fact]
    public void Should_Encrypt_A_Short_Text_With_Entropy() {
      const string secret = "My protected message";
      const string entropy = "ABC123";

      string encrypted = Cryptographer.Encrypt(EncryptionMode.EntropyKey, secret, entropy);

      Assert.NotEqual(secret, encrypted);

      string decrypted = Cryptographer.Decrypt(encrypted, entropy);

      Assert.Equal(secret, decrypted);
    }


    [Fact]
    public void Should_Encrypt_The_Hash_Of_A_Short_Text_WithEntropy() {
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
    public void Should_Encrypt_A_Text_Using_Pure_AES() {
      const string secret = "My protected message";
      const string entropy = "Zw-@61737323313233ABCDEFGHZBwDEF12y3ABC-79134-YU5141";

      string encrypted = Cryptographer.Encrypt(EncryptionMode.Pure, secret, entropy);

      Assert.NotEqual(secret, encrypted);

      string decrypted = Cryptographer.Decrypt(encrypted, entropy, true);

      Assert.Equal(secret, decrypted);
    }

  }  // CryptographerTests

}  // namespace Empiria.Tests.Security
