/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : WindowsRegistryFile                              Pattern  : Static Class                      *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Writes and reads data elements using the Microsoft Windows OS Registry File.                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Security.Permissions;
using Empiria.Security;
using Microsoft.Win32;

namespace Empiria {

  /// <summary>Writes and reads data elements using the Microsoft Windows OS Registry File.</summary>
  [RegistryPermissionAttribute(SecurityAction.Demand, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Ontica",
                                                      Write = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Ontica")]
  static internal class WindowsRegistryFile {

    #region Fields

    private const string empiriaKey = @"SOFTWARE\Ontica\Empiria@";
    private const string dbConnectionString = "DATASOURCE.";
    private const string impersonationTokenString = "IMPERSONATIONTOKEN.";

    #endregion Fields

    #region Internal methods

    static internal string ReadValue(string typeName, string parameterName) {
      string retrivedValue = null;
      string tempTypeName = typeName;

      if (!typeName.StartsWith("Empiria")) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.InvalidTypeName, 
                                             parameterName, typeName);
      }
      if (parameterName.ToUpperInvariant().StartsWith(dbConnectionString)) {   //Enforce DB key decryption
        parameterName = "§" + parameterName;
      } else if (parameterName.ToUpperInvariant().StartsWith(impersonationTokenString)) {   //Enforce impersonation keys decryption
        parameterName = "§" + parameterName;
      }
      tempTypeName = tempTypeName.Substring(tempTypeName.IndexOf('.') + 1);
      while (true) {
        if ((retrivedValue != null) || (tempTypeName == null)) {
          break;
        }
        retrivedValue = TryReadRegistryValue(empiriaKey, tempTypeName, parameterName);
        if (retrivedValue != null) {  //Don´t search superkey if keyValue founded
          break;
        }
        if (tempTypeName.IndexOf('.') != -1) {
          tempTypeName = tempTypeName.Substring(0, tempTypeName.LastIndexOf('.'));
        } else if (tempTypeName != String.Empty) {
          tempTypeName = String.Empty;
        } else if (tempTypeName == String.Empty) {
          tempTypeName = null;      //Key not found in the search tree. Flag the loop exit
        } // if
      } // while
      if (retrivedValue == null) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.ParameterNotExists,
                                             parameterName, typeName);
      }
      return retrivedValue;
    }

    static internal void WriteValue(string typeName, string parameterName, string value) {
      WriteRegistryValue(empiriaKey, typeName.Substring(typeName.IndexOf('.') + 1), parameterName, value);
    }

    #endregion Internal methods

    #region Private members

    static private void CreateSubKey(string absoluteKey) {
      Registry.LocalMachine.CreateSubKey(absoluteKey);
      Registry.LocalMachine.Flush();
      Registry.LocalMachine.Close();
    }

    static private bool Is32BitsClient {
      get {
        return (IntPtr.Size == 4);
      }
    }

    static private string TryReadRegistryValue(string valueKey, string typeName, string parameterName) {
      string absoluteKey = String.Concat(valueKey, ExecutionServer.LicenseName, "\\", typeName);

      RegistryKey registryKey = null;
      try {
        if (WindowsRegistryFile.Is32BitsClient) {
          // In order to avoid reading on  HKLM\Software\Wow6432Node
          var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
          registryKey = baseKey.OpenSubKey(absoluteKey);
        } else {
          registryKey = Registry.LocalMachine.OpenSubKey(absoluteKey);
        }
        if (registryKey == null) {
          return null;
        }
        object keyValue = registryKey.GetValue(parameterName);
        if (keyValue == null) {
          return null;
        }
        if (parameterName.StartsWith("§")) {
          return Cryptographer.Decrypt(keyValue.ToString(), ExecutionServer.LicenseName);
        } else {
          return keyValue.ToString();
        }
      } finally {
        if (registryKey != null) {
          registryKey.Close();
        }
      }
    }

    static private void WriteRegistryValue(string valueKey, string typeName, string name, string settingValue) {
      string absoluteKey = null;

      absoluteKey = String.Concat(valueKey, ExecutionServer.LicenseName, "\\", typeName);
      if (name.ToUpperInvariant().StartsWith(dbConnectionString)) {   //Enforce database key encryption
        name = "§" + name;
      }
      if (name.ToUpperInvariant().StartsWith(impersonationTokenString)) {   //Enforce impersonation key encryption
        name = "§" + name;
      }
      if (name.StartsWith("§")) {
        settingValue = Cryptographer.Encrypt(EncryptionMode.EntropyKey, settingValue, ExecutionServer.LicenseName);
      }
      if (Registry.LocalMachine.OpenSubKey(absoluteKey) == null) {
        CreateSubKey(absoluteKey);
      }
      Registry.LocalMachine.OpenSubKey(absoluteKey, true).SetValue(name, settingValue);
      Registry.LocalMachine.Flush();
      Registry.LocalMachine.Close();
    }

    #endregion Private members

  } //class WindowsRegistryFile

} //namespace Empiria
