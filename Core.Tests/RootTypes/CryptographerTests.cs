/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Tests                                        Component : Security Services                     *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Test class                            *
*  Type     : SecurityClaimsTests                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Security claims services tests.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

using Empiria.Security;

namespace Empiria.Tests {

  /// <summary>Security claims services tests.</summary>
  public class CryptographerTests {

    [Fact]
    public void MustEncryptAShortTextWithEntropy() {
      const string secret = "My protected message";
      const string entropy = "ABC123";

      string encrypted = Cryptographer.Encrypt(EncryptionMode.EntropyKey, secret, entropy);

      Assert.NotEqual(encrypted, secret);

      string decrypted = Cryptographer.Decrypt(encrypted, entropy);

      Assert.Equal(decrypted, secret);
    }

    [Fact]
    public void MustEncryptTheHashOfAShortTextWithEntropy() {
      const string secret = "My protected message";
      const string entropy = "ABC123";

      string s = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, secret, entropy);

      Assert.NotEqual(s, secret);

      string decrypted = Cryptographer.Decrypt(s, entropy);

      Assert.Equal(decrypted, Cryptographer.CreateHashCode(secret, entropy));
    }

  }  // SecurityClaimsTests

}  //namespace Empiria.Tests
