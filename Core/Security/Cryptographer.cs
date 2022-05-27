/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Services                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Methods library                       *
*  Type     : Cryptographer                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Encryption, hashing and signing services.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.IO;

using System.Security;
using System.Security.Cryptography;
using System.Text;

using Empiria.Security.Claims;

namespace Empiria.Security {

  /// <summary>Encryption, hashing and signing services.</summary>
  static public class Cryptographer {

    #region Fields

    static private byte[] licenseKey = null;

    #endregion Fields

    #region Public methods


    static public void AssertValidPrivateKeyPassword(SecureString password) {
      Assertion.Require(password, "password");

      var privateKeyFilePath =
                    ClaimsService.GetClaimValue<string>(EmpiriaUser.Current,
                                                        ClaimType.ElectronicSignPrivateKeyFilePath);

      RSAProvider.GetProvider(privateKeyFilePath, password);
    }


    static public SecureString ConvertToSecureString(string source) {
      Assertion.Require(source, "source");

      var securedString = new SecureString();

      for (int i = 0; i < source.Length; i++) {
        securedString.AppendChar(source[i]);
      }

      return securedString;
    }


    static public string CreateHashCode(string text, string salt = "") {
      Assertion.Require(text, "text");
      salt = salt ?? String.Empty;

      StartEngine();

      byte[] data = Encoding.UTF8.GetBytes(salt + text + ExecutionServer.LicenseNumber +
                                           ConstructKey(salt + ExecutionServer.LicenseNumber));

      SHA512 sha = SHA512Cng.Create();

      return ConvertToString(sha.ComputeHash(data));
    }


    static public string CreateHashCode(byte[] bytesArray, string salt = "") {
      Assertion.Require(bytesArray, "bytesArray");
      Assertion.Require(bytesArray.Length != 0, "bytesArray can't be empty.");

      return CreateHashCode(Encoding.UTF8.GetString(bytesArray, 0, bytesArray.Length), salt);
    }


    /// <summary>Takes a ciphertext string and decrypts it using the giving public key.</summary>
    /// <param name="cipherText">Text string to be decrypted.</param>
    /// <param name="entropy">The entropy or salt string used to decrypt the text string.</param>
    static public string Decrypt(string cipherText, string entropy = "") {
      Assertion.Require(cipherText, "cipherText");
      entropy = entropy ?? "";

      return DecryptString(cipherText, entropy + ExecutionServer.LicenseNumber);
    }


    /// <summary>Takes a plaintext string and encrypts it.</summary>
    /// <param name="plainText">Text string to be encrypted.</param>
    /// <param name="salt">Optional string to use as encryption salt.</param>
    static public string Encrypt(string plainText, string salt = "") {
      Assertion.Require(plainText, "plainText");

      throw new NotImplementedException();

      //if (protectionMode == EncryptionMode.Standard || protectionMode == EncryptionMode.HashCode) {
      //  return Encrypt(protectionMode, plainText, String.Empty);
      //} else {
      //  throw new SecurityException(SecurityException.Msg.InvalidProtectionMode, protectionMode.ToString());
      //}
    }


    /// <summary>Takes a plaintext string and encrypts it with the giving public key.</summary>
    /// <param name="plainText">Text string to be encrypted.</param>
    /// <param name="entropy">The entropy string used to encrypt the text string.</param>
    static public string Encrypt(EncryptionMode protectionMode,
                                 string plainText, string entropy = "") {
      Assertion.Require(plainText, "plainText");
      entropy = entropy ?? "";

      if (entropy.Length != 0 &&
         (protectionMode == EncryptionMode.Standard || protectionMode == EncryptionMode.HashCode)) {
        Assertion.RequireFail($"Entropy parameter was provided but it not works " +
                              $"with encryption mode {protectionMode}.");
      }

      string s = String.Empty;

      switch (protectionMode) {
        case EncryptionMode.Standard:
          return EncryptString(plainText, ExecutionServer.LicenseNumber);

        case EncryptionMode.HashCode:
          s = EncryptString(plainText, ExecutionServer.LicenseNumber);

          return EncryptString(s, ExecutionServer.LicenseNumber);

        case EncryptionMode.EntropyKey:
          Assertion.Require(entropy, "entropy");

          return EncryptString(plainText, entropy + ExecutionServer.LicenseNumber);

        case EncryptionMode.EntropyHashCode:
          Assertion.Require(entropy, "entropy");

          s = EncryptString(plainText, entropy + ExecutionServer.LicenseNumber);
          s = CreateHashCode(s, entropy);

          return EncryptString(s, entropy + ExecutionServer.LicenseNumber);

        default:
          throw new SecurityException(SecurityException.Msg.InvalidProtectionMode, protectionMode.ToString());
      }
    }

    static public string GetSHA256(string text) {
      Assertion.Require(text, "text");


      byte[] data = Encoding.UTF8.GetBytes(text);

      SHA256 sha = SHA256.Create();

      return ConvertToString(sha.ComputeHash(data));
    }

    static public string SignText(string text, SecureString password) {
      Assertion.Require(text, "text");
      Assertion.Require(password, "password");

      var privateKeyFilePath =
                    ClaimsService.GetClaimValue<string>(EmpiriaUser.Current,
                                                        ClaimType.ElectronicSignPrivateKeyFilePath);

      RSACryptoServiceProvider rsa = RSAProvider.GetProvider(privateKeyFilePath, password);

      /// SHA256CryptoServiceProvider uses FIPS 140-2 (Federal Information Processing Standard)
      /// validated Crypto Service Provider.
      var hasher = new SHA256CryptoServiceProvider();

      byte[] textAsByteArray = Encoding.UTF8.GetBytes(text);

      byte[] signAsArray = rsa.SignData(textAsByteArray, hasher);

      return Convert.ToBase64String(signAsArray);
    }


    static public string SignTextWithSystemCredentials(string text) {
      Assertion.Require(text, "text");

      RSACryptoServiceProvider rsa = RSAProvider.GetSystemProvider();

      /// SHA256CryptoServiceProvider uses FIPS 140-2 (Federal Information Processing Standard)
      /// validated Crypto Service Provider.
      var hasher = new SHA256CryptoServiceProvider();

      StartEngine();

      byte[] textAsByteArray = Encoding.UTF8.GetBytes(text + ExecutionServer.LicenseNumber +
                                                      ConstructKey(ExecutionServer.LicenseNumber));

      byte[] signAsArray = rsa.SignData(textAsByteArray, hasher);

      return Convert.ToBase64String(signAsArray);
    }


    #endregion Public methods

    #region Private methods

    static private byte[] ConstructIV(string entropy) {
      byte[] result = new byte[16];
      byte[] key = ReadByteArray("IV");

      byte[] license = licenseKey;

      int x = 0;

      for (int i = 1; i < license.Length; i++) {
        x += (key[i] * ((license[license.Length - i - 1] % 7) + 1)) -
              (license[i] * ((key[key.Length - i - 1] % 13) + 1));
      }
      result[0] = (byte) (Math.Abs((x * 11) - 23) % 256);

      for (int i = 1; i < key.Length / 2; i++) {
        result[i] = (byte) (Math.Abs(( ((x + 53) * result[i - 1]) + key[i]) + (x * key[key.Length - i - 1])) % 256);

        x += Math.Abs((license[i] * key[key.Length - i - 1]) - result[i]);
      }

      if (entropy != null) {
        result = MergeSalt(result, entropy);
      }

      return result;
    }

    static private byte[] ConstructKey(string entropy) {
      byte[] result = new byte[32];
      byte[] key = ReadByteArray("Key");

      byte[] license = licenseKey;

      long x = 0;

      for (int i = 1; i < license.Length; i++) {
        x += (key[i] * ((license[license.Length - i - 1] % 7) + 1)) -
             (license[i] * ((key[key.Length - i - 1] %
             13) + 1));
      }
      result[0] = (byte) (Math.Abs((x * 3) - 11) % 256);

      for (int i = 1; i < key.Length / 2; i++) {
        result[i] = (byte) (Math.Abs(((x + 43) * result[i - 1] + key[i]) + ((x + 5) * key[key.Length - i - 1])) % 256);

        x += Math.Abs((license[i] * key[key.Length - i - 1]) - result[i]);
      }

      if (entropy != null) {
        result = MergeSalt(result, entropy);
      }

      return result;
    }


    static private string ConvertToString(byte[] data) {
      var sBuilder = new StringBuilder();

      for (int i = 0; i < data.Length; i++) {
        sBuilder.AppendFormat("{0:x2}", data[i]);
      }
      return sBuilder.ToString();
    }


    static private string DecryptString(string cipherText, string entropy) {
      var textConverter = new UTF8Encoding();
      var rijndael = new RijndaelManaged();

      StartEngine();

      rijndael.Padding = PaddingMode.Zeros;
      rijndael.Key = ConstructKey(entropy);
      rijndael.IV = ConstructIV(entropy);

      ICryptoTransform decryptor = rijndael.CreateDecryptor();

      var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText));
      var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

      byte[] cipherTextArray = new byte[cipherText.Length];
      cryptoStream.Read(cipherTextArray, 0, cipherTextArray.Length);

      return textConverter.GetString(cipherTextArray).Trim('\0');
    }

    static private string EncryptString(string plainText, string salt) {
      var textConverter = new UTF8Encoding();
      var rijndael = new RijndaelManaged();

      StartEngine();

      rijndael.Padding = PaddingMode.Zeros;
      rijndael.Key = ConstructKey(salt);
      rijndael.IV = ConstructIV(salt);

      ICryptoTransform encryptor = rijndael.CreateEncryptor();

      var memoryStream = new MemoryStream();
      var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

      byte[] plainTextArray = textConverter.GetBytes(plainText);

      cryptoStream.Write(plainTextArray, 0, plainTextArray.Length);
      cryptoStream.FlushFinalBlock();

      return Convert.ToBase64String(memoryStream.ToArray());
    }

    static private string GetLicenseHashCode(string license) {
      char[] licenseArray = license.ToCharArray();
      string hashCode = String.Empty;

      int x = licenseArray[0] * 3;

      for (int i = 1; i < licenseArray.Length; i++) {
        switch (i % 3) {
          case 0:
            if ((x % 3) != 2) {
              x = 65 + (x % 26);
              hashCode += Convert.ToChar(x);
            } else {
              x = 49 + (x % 9);
              hashCode += Convert.ToChar(x);
            }
            x += licenseArray[i] * 3;
            break;

            case 1:
            x += licenseArray[i] * 5;
            break;
          case 2:
            x += licenseArray[i] * 7;
            break;
        } // switch
      }  // for

      if ((x % 3) != 2) {
        x = 65 + (x % 26);
        hashCode += Convert.ToChar(x);
      } else {
        x = 48 + (x % 10);
        hashCode += Convert.ToChar(x);
      }
      return hashCode;
    }

    static private string GetSerialNumber() {
      return ExecutionServer.LicenseSerialNumber;
    }

    static private byte[] MergeSalt(byte[] byteArray, string salt) {
      byte[] result = new byte[byteArray.Length];

      for (int i = 0; i < byteArray.Length; i++) {
        result[i] = (byte) ((byteArray[i] + salt[i % salt.Length]) % 255);
      }

      return result;
    }

    static private byte[] ReadByteArray(string name) {
      string[] temp = ReadString(name).Split(',');

      byte[] bytes = new byte[temp.Length];

      for (int i = 0; i < temp.Length; i++) {
        bytes[i] = byte.Parse(temp[i].Trim());
      }

      return bytes;
    }

    static private string ReadString(string name) {
      string data = ConfigurationData.GetString(name);

      if (!data.StartsWith("@")) {
        byte[] bytes = Convert.FromBase64String(data);
        byte[] entropy = ASCIIEncoding.ASCII.GetBytes(name + "-" + ExecutionServer.LicenseSerialNumber);

        byte[] returnBytes = ProtectedData.Unprotect(bytes, entropy, DataProtectionScope.LocalMachine);

        return ASCIIEncoding.ASCII.GetString(returnBytes);

      } else {    // If starts with '@' then it contains the file path
        string fileName = data.Substring(1);

        string path = String.Empty;

        if (fileName.StartsWith("~")) {
          path = ExecutionServer.GetFullFileNameFromCurrentExecutionPath(fileName.Substring(1));
        } else {
          path = fileName;
        }
        return File.ReadAllText(path);
      }
    }

    static private void SetLicenseKey() {
      byte[] result = new byte[32];
      byte[] key = ReadByteArray("LKey");

      string licenseName = ExecutionServer.LicenseName;
      string license = ExecutionServer.LicenseNumber;

      int x = 0;

      for (int i = 1; i < license.Length; i++) {
        x += licenseName[i % licenseName.Length] +
              (key[i] * ((license[license.Length - i - 1] % 5) + 1)) -
              (license[i] * ((key[key.Length - i - 1] % 3) + 1));
      }

      result[0] = (byte) (Math.Abs(x) % 255);

      for (int i = 1; i < key.Length / 2; i++) {
        result[i] = (byte) ((((x * result[i - 1]) + key[i]) + (x * key[key.Length - i - 1])) % 255);
        x += license[i] + license[license.Length - i - 1];
      }

      licenseKey = result;
    }

    static private void StartEngine() {
      if (licenseKey != null) {
        return;
      }

      string license = ExecutionServer.LicenseNumber.Replace("-", String.Empty);

      if (license.Length != 32) {
        throw new ExecutionServerException(ExecutionServerException.Msg.InvalidLicense);
      }

      if (GetLicenseHashCode(license.Substring(8)) != license.Substring(0, 8)) {
        throw new ExecutionServerException(ExecutionServerException.Msg.InvalidLicense);
      }

      if (GetSerialNumber() != ExecutionServer.LicenseSerialNumber) {
        throw new ExecutionServerException(ExecutionServerException.Msg.InvalidLicense);
      }

      SetLicenseKey();
    }

    #endregion Private methods

  } //class Cryptographer

} //namespace Empiria.Security
