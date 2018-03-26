﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : Cryptographer                                    Pattern  : Static Class                      *
*                                                                                                            *
*  Summary   : Provides encryption and decryption services for strings, objects and files.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Empiria.Security {

  public enum EncryptionMode {
    Unprotected = 0,
    Standard = 1,
    EntropyKey = 2,
    HashCode = 3,
    EntropyHashCode = 4,
  }

  /// <summary>Provides encryption and decryption services for strings, objects and files.</summary>
  static public class Cryptographer {

    #region Fields

    static private byte[] licenseKey = null;

    #endregion Fields

    #region Public methods

    static private string ConvertToString(byte[] data) {
      StringBuilder sBuilder = new StringBuilder();

      for (int i = 0; i < data.Length; i++) {
        sBuilder.AppendFormat("{0:x2}", data[i]);
      }
      return sBuilder.ToString();
    }

    static public string CreateDigitalSign(string text) {
      return CreateDigitalSign(text, String.Empty);
    }

    static public string CreateDigitalSign(string text, string entropy) {
      Assertion.AssertObject(text, "text");

      RSACryptoServiceProvider rsa = CryptoServices.GetRSACryptoServiceProvider();
      SHA256Managed hasher = new SHA256Managed();

      StartEngine();

      byte[] data = Encoding.UTF8.GetBytes(text + ExecutionServer.LicenseNumber +
                                           ConstructKey(ExecutionServer.LicenseNumber + entropy));
      byte[] array = rsa.SignData(data, hasher);

      return Convert.ToBase64String(array);
    }

    static public string CreateHashCode(string text) {
      return CreateHashCode(text, String.Empty);
    }

    static public string CreateHashCode(string text, string entropy) {
      Assertion.AssertObject(text, "text");

      StartEngine();

      byte[] data = Encoding.UTF8.GetBytes(text + ExecutionServer.LicenseNumber +
                                           ConstructKey(ExecutionServer.LicenseNumber + entropy));


      SHA256 sha = SHA256.Create();

      return ConvertToString(sha.ComputeHash(data));
    }

    static public string CreateHashCode(byte[] bytesArray, string entropy) {
      Assertion.AssertObject(bytesArray, "bytesArray");

      StartEngine();

      if (String.IsNullOrWhiteSpace(entropy)) {
        bytesArray = Encoding.UTF8.GetBytes(bytesArray + entropy + ExecutionServer.LicenseNumber +
                                            ConstructKey(ExecutionServer.LicenseNumber + entropy));
      }

      SHA256 sha = SHA256.Create();

      return ConvertToString(sha.ComputeHash(bytesArray));
    }

    /// <summary>Takes a ciphertext string and decrypts it.</summary>
    /// <param name="cipherText">Text string to be decrypted.</param>
    static public string Decrypt(string cipherText) {
      Assertion.AssertObject(cipherText, "cipherText");

      return DecryptString(cipherText, ExecutionServer.LicenseNumber);
    }

    /// <summary>Takes a ciphertext string and decrypts it using the giving public key.</summary>
    /// <param name="cipherText">Text string to be decrypted.</param>
    /// <param name="entropy">The public key used to decrypt the text string.</param>
    static public string Decrypt(string cipherText, string entropy) {
      Assertion.AssertObject(cipherText, "cipherText");
      Assertion.AssertObject(entropy, "entropy");

      return DecryptString(cipherText, entropy + ExecutionServer.LicenseNumber);
    }

    /// <summary>Takes a plaintext string and encrypts it.</summary>
    /// <param name="plainText">Text string to be encrypted.</param>
    static public string Encrypt(EncryptionMode protectionMode, string plainText) {
      Assertion.AssertObject(plainText, "plainText");

      if (protectionMode == EncryptionMode.Standard || protectionMode == EncryptionMode.HashCode) {
        return Encrypt(protectionMode, plainText, String.Empty);
      } else {
        throw new SecurityException(SecurityException.Msg.InvalidProtectionMode, protectionMode.ToString());
      }
    }

    /// <summary>Takes a plaintext string and encrypts it with the giving public key.</summary>
    /// <param name="plainText">Text string to be encrypted.</param>
    /// <param name="entropy">The entropy string used to encrypt the text string.</param>
    static public string Encrypt(EncryptionMode protectionMode, string plainText, string entropy) {
      Assertion.AssertObject(plainText, "plainText");

      string s = String.Empty;

      switch (protectionMode) {
        case EncryptionMode.Standard:
          return EncryptString(plainText, ExecutionServer.LicenseNumber);
        case EncryptionMode.HashCode:
          s = EncryptString(plainText, ExecutionServer.LicenseNumber);
          s = CreateHashCode(s);
          return EncryptString(s, ExecutionServer.LicenseNumber);
        case EncryptionMode.EntropyKey:
          Assertion.AssertObject(entropy, "entropy");

          return EncryptString(plainText, entropy + ExecutionServer.LicenseNumber);
        case EncryptionMode.EntropyHashCode:
          Assertion.AssertObject(entropy, "entropy");
          s = EncryptString(plainText, entropy + ExecutionServer.LicenseNumber);
          s = CreateHashCode(s, entropy);
          return EncryptString(s, entropy + ExecutionServer.LicenseNumber);
        default:
          throw new SecurityException(SecurityException.Msg.InvalidProtectionMode, protectionMode.ToString());
      }
    }

    static public string GetMD5HashCode(string source) {
      MD5 md5 = MD5.Create();
      byte[] inputBytes = Encoding.ASCII.GetBytes(source);
      byte[] hash = md5.ComputeHash(inputBytes);


      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < hash.Length; i++) {
        sb.Append(hash[i].ToString("X2"));
      }
      return sb.ToString().ToLower();
    }

    #endregion Public methods

    #region Private methods

    static private byte[] ConstructIV(string entropy) {
      byte[] result = new byte[16];
      byte[] key = ReadByteArray("IV");

      byte[] license = licenseKey;

      int x = 0;
      unchecked {
        for (int i = 1; i < license.Length; i++) {
          x += (key[i] * ((license[license.Length - i - 1] % 7) + 1)) -
               (license[i] * ((key[key.Length - i - 1] % 13) + 1));
        }
        result[0] = (byte) (Math.Abs((x * 11) - 23) % 255);
        for (int i = 1; i < key.Length / 2; i++) {
          result[i] = (byte) ((((x + 53) * result[i - 1]) + key[i]) + (x * key[key.Length - i - 1]) % 255);
          x += Math.Abs((license[i] * key[key.Length - i - 1]) - result[i]);
        }
      }
      if (entropy != null) {
        result = ParseEntropyKey(entropy, result);
      }
      return result;
    }

    static private byte[] ConstructKey(string entropy) {
      byte[] result = new byte[32];
      byte[] key = ReadByteArray("Key");

      byte[] license = licenseKey;

      int x = 0;
      unchecked {
        for (int i = 1; i < license.Length; i++) {
          x += (key[i] * ((license[license.Length - i - 1] % 7) + 1)) -
            (license[i] * ((key[key.Length - i - 1] % 13) + 1));
        }
        result[0] = (byte) (Math.Abs((x * 3) - 11) % 255);
        for (int i = 1; i < key.Length / 2; i++) {
          result[i] = (byte) ((((x + 43) * result[i - 1]) + key[i]) + ((x + 5) * key[key.Length - i - 1]) % 255);
          x += Math.Abs((license[i] * key[key.Length - i - 1]) - result[i]);
        }
      }
      if (entropy != null) {
        result = ParseEntropyKey(entropy, result);
      }
      return result;
    }

    static private string DecryptString(string cipherText, string entropy) {
      UTF8Encoding textConverter = new UTF8Encoding();
      RijndaelManaged rijndael = new RijndaelManaged();

      StartEngine();
      rijndael.Padding = PaddingMode.Zeros;
      rijndael.Key = ConstructKey(entropy);
      rijndael.IV = ConstructIV(entropy);

      ICryptoTransform decryptor = rijndael.CreateDecryptor();
      MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText));
      CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

      byte[] cipherTextArray = new byte[cipherText.Length];
      cryptoStream.Read(cipherTextArray, 0, cipherTextArray.Length);

      return textConverter.GetString(cipherTextArray).Trim('\0');
    }

    static private string EncryptString(string plainText, string entropy) {
      UTF8Encoding textConverter = new UTF8Encoding();
      RijndaelManaged rijndael = new RijndaelManaged();

      StartEngine();
      rijndael.Padding = PaddingMode.Zeros;
      rijndael.Key = ConstructKey(entropy);
      rijndael.IV = ConstructIV(entropy);
      ICryptoTransform encryptor = rijndael.CreateEncryptor();
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

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

    static private byte[] ParseEntropyKey(string publicKey, byte[] byteArray) {
      byte[] result = new byte[byteArray.Length];

      if (publicKey.IndexOf('.') != -1) {
        publicKey = publicKey.Substring(0, publicKey.IndexOf('.'));
      }
      unchecked {
        for (int i = 0; i < byteArray.Length; i++) {
          result[i] = (byte) (byteArray[i] + publicKey[i % publicKey.Length] % 255);
        }
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
      unchecked {
        for (int i = 1; i < license.Length; i++) {
          x += licenseName[i % licenseName.Length] +
               (key[i] * ((license[license.Length - i - 1] % 5) + 1)) -
               (license[i] * ((key[key.Length - i - 1] % 3) + 1));
        }
        result[0] = (byte) (Math.Abs(x) % 255);
        for (int i = 1; i < key.Length / 2; i++) {
          result[i] = (byte) (((x * result[i - 1]) + key[i]) + (x * key[key.Length - i - 1]) % 255);
          x += license[i] + license[license.Length - i - 1];
        }
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

    #region Inner Class CryptoServices

    static private class CryptoServices {

      static internal RSACryptoServiceProvider GetRSACryptoServiceProvider() {
        string privateKeyFileName = ConfigurationData.GetString("§RSACryptoFile");
        string path = String.Empty;

        if (privateKeyFileName.StartsWith("~")) {
          path = ExecutionServer.GetFullFileNameFromCurrentExecutionPath(privateKeyFileName.Substring(1));
        } else {
          path = privateKeyFileName;
        }
        Byte[] bytes = System.IO.File.ReadAllBytes(path);

        return CryptoServices.DecodeEncryptedPrivateKeyInfo(bytes);
      }

      //------- Parses binary asn.1 EncryptedPrivateKeyInfo; returns RSACryptoServiceProvider ---
      static private RSACryptoServiceProvider DecodeEncryptedPrivateKeyInfo(byte[] encpkcs8) {
        // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
        // this byte[] includes the sequence byte and terminal encoded null
        byte[] OIDpkcs5PBES2 = { 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0D };
        byte[] OIDpkcs5PBKDF2 = { 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0C };
        byte[] OIDdesEDE3CBC = { 0x06, 0x08, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x03, 0x07 };
        byte[] seqdes = new byte[10];
        byte[] seq = new byte[11];
        byte[] salt;
        byte[] IV;
        byte[] encryptedpkcs8;
        byte[] pkcs8;

        int saltsize, ivsize, encblobsize, iterations;

        // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
        MemoryStream mem = new MemoryStream(encpkcs8);
        int lenstream = (int) mem.Length;
        BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
        byte bt = 0;
        ushort twobytes = 0;

        try {

          twobytes = binr.ReadUInt16();
          if (twobytes == 0x8130) { //data read as little endian order (actual data order for Sequence is 30 81)
            binr.ReadByte();  //advance 1 byte
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();  //advance 2 bytes
          } else {
            return null;
          }

          twobytes = binr.ReadUInt16(); //inner sequence
          if (twobytes == 0x8130) {
            binr.ReadByte();
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();
          }

          seq = binr.ReadBytes(11);   //read the Sequence OID
          if (!CompareByteArrays(seq, OIDpkcs5PBES2)) { //is it a OIDpkcs5PBES2 ?
            return null;
          }

          twobytes = binr.ReadUInt16(); //inner sequence for pswd salt
          if (twobytes == 0x8130) {
            binr.ReadByte();
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();
          }

          twobytes = binr.ReadUInt16();  //inner sequence for pswd salt
          if (twobytes == 0x8130) {
            binr.ReadByte();
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();
          }

          seq = binr.ReadBytes(11);    //read the Sequence OID
          if (!CompareByteArrays(seq, OIDpkcs5PBKDF2)) {  //is it a OIDpkcs5PBKDF2 ?
            return null;
          }

          twobytes = binr.ReadUInt16();
          if (twobytes == 0x8130) {
            binr.ReadByte();
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();
          }

          bt = binr.ReadByte();
          if (bt != 0x04) {    //expect octet string for salt
            return null;
          }
          saltsize = binr.ReadByte();
          salt = binr.ReadBytes(saltsize);

          bt = binr.ReadByte();
          if (bt != 0x02) {  //expect an integer for PBKF2 interation count
            return null;
          }

          int itbytes = binr.ReadByte();  //PBKD2 iterations should fit in 2 bytes.
          if (itbytes == 1) {
            iterations = binr.ReadByte();
          } else if (itbytes == 2) {
            iterations = 256 * binr.ReadByte() + binr.ReadByte();
          } else {
            return null;
          }
          twobytes = binr.ReadUInt16();
          if (twobytes == 0x8130) {
            binr.ReadByte();
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();
          }

          seqdes = binr.ReadBytes(10);    //read the Sequence OID
          if (!CompareByteArrays(seqdes, OIDdesEDE3CBC)) {  //is it a OIDdes-EDE3-CBC ?
            return null;
          }

          bt = binr.ReadByte();
          if (bt != 0x04) {  //expect octet string for IV
            return null;
          }
          ivsize = binr.ReadByte();  // IV byte size should fit in one byte (24 expected for 3DES)
          IV = binr.ReadBytes(ivsize);

          bt = binr.ReadByte();
          if (bt != 0x04) {    // expect octet string for encrypted PKCS8 data
            return null;
          }

          bt = binr.ReadByte();

          if (bt == 0x81) {
            encblobsize = binr.ReadByte();  // data size in next byte
          } else if (bt == 0x82) {
            encblobsize = 256 * binr.ReadByte() + binr.ReadByte();
          } else {
            encblobsize = bt;               // we already have the data size
          }

          encryptedpkcs8 = binr.ReadBytes(encblobsize);

          SecureString secpswd = GetSecPswd();
          pkcs8 = DecryptPBDK2(encryptedpkcs8, salt, IV, secpswd, iterations);
          if (pkcs8 == null) {          // probably a bad pswd entered.
            return null;
          }

          //----- With a decrypted pkcs #8 PrivateKeyInfo blob, decode it to an RSA ---
          RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8);
          return rsa;
        } catch (Exception) {
          return null;
        } finally {
          binr.Close();
        }
      }

      static private bool CompareByteArrays(byte[] a, byte[] b) {
        if (a.Length != b.Length) {
          return false;
        }
        int i = 0;
        foreach (byte c in a) {
          if (c != b[i]) {
            return false;
          }
          i++;
        }
        return true;
      }

      //  ------  Uses PBKD2 to derive a 3DES key and decrypts data --------
      static public byte[] DecryptPBDK2(byte[] edata, byte[] salt, byte[] IV,
                                        SecureString secpswd, int iterations) {
        CryptoStream decrypt = null;

        IntPtr unmanagedPswd = IntPtr.Zero;
        byte[] psbytes = new byte[secpswd.Length];
        unmanagedPswd = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
        Marshal.Copy(unmanagedPswd, psbytes, 0, psbytes.Length);
        Marshal.ZeroFreeGlobalAllocAnsi(unmanagedPswd);

        try {
          Rfc2898DeriveBytes kd = new Rfc2898DeriveBytes(psbytes, salt, iterations);
          TripleDES decAlg = TripleDES.Create();
          decAlg.Key = kd.GetBytes(24);
          decAlg.IV = IV;
          MemoryStream memstr = new MemoryStream();
          decrypt = new CryptoStream(memstr, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
          decrypt.Write(edata, 0, edata.Length);
          decrypt.Flush();
          decrypt.Close();  // this is REQUIRED.
          byte[] cleartext = memstr.ToArray();
          return cleartext;
        } catch (Exception e) {
          throw new SecurityException(SecurityException.Msg.CantDecryptString, e.Message);
        }
      }

      //------- Parses binary asn.1 PKCS #8 PrivateKeyInfo; returns RSACryptoServiceProvider ---
      static public RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8) {
        // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
        // this byte[] includes the sequence byte and terminal encoded null
        byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
        byte[] seq = new byte[15];
        // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
        MemoryStream mem = new MemoryStream(pkcs8);
        int lenstream = (int) mem.Length;
        BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
        byte bt = 0;
        ushort twobytes = 0;

        try {

          twobytes = binr.ReadUInt16();
          if (twobytes == 0x8130) { //data read as little endian order (actual data order for Sequence is 30 81)
            binr.ReadByte();  //advance 1 byte
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();  //advance 2 bytes
          } else {
            return null;
          }

          bt = binr.ReadByte();
          if (bt != 0x02) {
            return null;
          }
          twobytes = binr.ReadUInt16();

          if (twobytes != 0x0001) {
            return null;
          }

          seq = binr.ReadBytes(15);    //read the Sequence OID
          if (!CompareByteArrays(seq, SeqOID)) {  //make sure Sequence for OID is correct
            return null;
          }
          bt = binr.ReadByte();
          if (bt != 0x04) {  //expect an Octet string
            return null;
          }
          bt = binr.ReadByte();    //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
          if (bt == 0x81) {
            binr.ReadByte();
          } else {
            if (bt == 0x82) {
              binr.ReadUInt16();
            }
          }
          //------ at this stage, the remaining sequence should be the RSA private key

          byte[] rsaprivkey = binr.ReadBytes((int) (lenstream - mem.Position));
          RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
          return rsacsp;
        } catch (Exception) {
          return null;
        } finally { binr.Close(); }

      }

      //------- Parses binary ans.1 RSA private key; returns RSACryptoServiceProvider  ---
      static public RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey) {
        byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

        // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
        MemoryStream mem = new MemoryStream(privkey);
        BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
        byte bt = 0;
        ushort twobytes = 0;
        int elems = 0;
        try {
          twobytes = binr.ReadUInt16();
          if (twobytes == 0x8130) { //data read as little endian order (actual data order for Sequence is 30 81)
            binr.ReadByte();  //advance 1 byte
          } else if (twobytes == 0x8230) {
            binr.ReadInt16();  //advance 2 bytes
          } else {
            return null;
          }
          twobytes = binr.ReadUInt16();
          if (twobytes != 0x0102) { //version number
            return null;
          }
          bt = binr.ReadByte();
          if (bt != 0x00) {
            return null;
          }

          //------  all private key components are Integer sequences ----
          elems = GetIntegerSize(binr);
          MODULUS = binr.ReadBytes(elems);

          elems = GetIntegerSize(binr);
          E = binr.ReadBytes(elems);

          elems = GetIntegerSize(binr);
          D = binr.ReadBytes(elems);

          elems = GetIntegerSize(binr);
          P = binr.ReadBytes(elems);

          elems = GetIntegerSize(binr);
          Q = binr.ReadBytes(elems);

          elems = GetIntegerSize(binr);
          DP = binr.ReadBytes(elems);

          elems = GetIntegerSize(binr);
          DQ = binr.ReadBytes(elems);

          elems = GetIntegerSize(binr);
          IQ = binr.ReadBytes(elems);

          // ------- create RSACryptoServiceProvider instance and initialize with public key -----
          RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
          RSAParameters RSAparams = new RSAParameters();
          RSAparams.Modulus = MODULUS;
          RSAparams.Exponent = E;
          RSAparams.D = D;
          RSAparams.P = P;
          RSAparams.Q = Q;
          RSAparams.DP = DP;
          RSAparams.DQ = DQ;
          RSAparams.InverseQ = IQ;
          RSA.ImportParameters(RSAparams);
          return RSA;
        } catch (Exception) {
          return null;
        } finally { binr.Close(); }
      }

      static private int GetIntegerSize(BinaryReader binr) {
        byte bt = 0;
        byte lowbyte = 0x00;
        byte highbyte = 0x00;
        int count = 0;

        bt = binr.ReadByte();
        if (bt != 0x02) {    //expect integer
          return 0;
        }
        bt = binr.ReadByte();

        if (bt == 0x81) {
          count = binr.ReadByte();  // data size in next byte
        } else {
          if (bt == 0x82) {
            highbyte = binr.ReadByte();  // data size in next 2 bytes
            lowbyte = binr.ReadByte();
            byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
            count = BitConverter.ToInt32(modint, 0);
          } else {
            count = bt;    // we already have the data size
          }
        }

        while (binr.ReadByte() == 0x00) {  //remove high order zeros in data
          count -= 1;
        }
        binr.BaseStream.Seek(-1, SeekOrigin.Current);    //last ReadByte wasn't a removed zero, so back up a byte
        return count;
      }

      static private SecureString GetSecPswd() {
        SecureString password = new SecureString();
        string s = ConfigurationData.GetString("§RSACryptoFilePwd");

        for (int i = 0; i < s.Length; i++) {
          password.AppendChar(s[i]);
        }
        return password;
      }

    } // inner class CryptoServices

    #endregion Inner Class CryptoServices

  } //class Cryptographer

} //namespace Empiria.Security