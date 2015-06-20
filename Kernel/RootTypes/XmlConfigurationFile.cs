/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : XmlConfigurationFile                             Pattern  : Static Class                      *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Writes and reads data elements using a Xml Configuration File.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using Empiria.Security;

namespace Empiria {

  /// <summary>Writes and reads data elements using a Xml Configuration File.</summary>
  static internal class XmlConfigurationFile {

    #region Fields

    private const string configurationfileName = "empiria.configuration.xml";
    private const string dbConnectionString = "DATASOURCE.";
    private const string impersonationTokenString = "IMPERSONATIONTOKEN.";

    static private Dictionary<string, string> parametersCache = null;

    #endregion Fields

    #region Internal methods

    static internal bool Exists() {
      if (parametersCache != null) {
        return true;
      }
      return File.Exists(GetFileName());
    }

    static internal string ReadValue(string typeName, string parameterName) {
      string retrivedValue = null;
      string tempTypeName = typeName;

      if (parameterName.ToUpperInvariant().StartsWith(dbConnectionString)) {   //Enforce DB key decryption
        parameterName = "§" + parameterName;
      } else if (parameterName.ToUpperInvariant().StartsWith(impersonationTokenString)) {   //Enforce DB key decryption
        parameterName = "§" + parameterName;
      }
      while (true) {
        if ((retrivedValue == null) && (tempTypeName != null)) {
          retrivedValue = ReadXmlConfigurationValue(tempTypeName, parameterName);
          if (retrivedValue == null) {   //Don´t search superkey if keyValue founded
            if (tempTypeName.IndexOf('.') != -1) {
              tempTypeName = tempTypeName.Substring(0, tempTypeName.LastIndexOf('.'));
            } else if (tempTypeName != String.Empty) {
              tempTypeName = String.Empty;
            } else if (tempTypeName == String.Empty) {
              tempTypeName = null;      //Key not found in the search tree. Flag the loop exit
            } // if else if
          } // if
        } else {
          break;
        } // if
      } // while
      if (String.IsNullOrEmpty(retrivedValue)) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.ParameterNotExists,
                                             parameterName, typeName);
      }
      return retrivedValue;
    }

    static internal void WriteValue(string typeName, string parameterName, string value) {
      WriteXmlConfigurationValue(typeName, parameterName, value);
    }

    #endregion Internal methods

    #region Private methods

    static private string GetFileName() {
      const string protocolPrefix = @"file:\";
      string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
      string configurationFilePath = System.IO.Path.GetDirectoryName(codeBase);

      if (configurationFilePath.StartsWith(protocolPrefix)) {
        configurationFilePath = configurationFilePath.Remove(0, protocolPrefix.Length);
      }
      return System.IO.Path.Combine(configurationFilePath, configurationfileName);
    }

    static private void LoadParametersCache() {
      string fileName = GetFileName();

      if (!Exists()) {
        parametersCache = null;
        throw new
            ConfigurationDataException(ConfigurationDataException.Msg.XmlConfigurationFileNotExists, fileName);
      }

      using (XmlTextReader xmlTextReader = new XmlTextReader(fileName)) {
        parametersCache = new Dictionary<string, string>(16);
        while (xmlTextReader.Read()) {
          if (xmlTextReader.NodeType == XmlNodeType.Element &&
              xmlTextReader.Name == "EmpiriaSetting") {
            string typeName = xmlTextReader.GetAttribute("type");
            string parameterName = xmlTextReader.GetAttribute("name");
            string parameterValue = xmlTextReader.GetAttribute("value");

            parametersCache.Add(typeName + "." + parameterName, parameterValue);
          }
        }
      }
    }

    static private string ReadXmlConfigurationValue(string typeName, string parameterName) {
      string keyValue = null;

      if (parametersCache == null) {
        LoadParametersCache();
      }
      if (!parametersCache.TryGetValue(typeName + "." + parameterName, out keyValue)) {
        keyValue = null;
      }
      if ((keyValue != null) && parameterName.StartsWith("§")) {
        keyValue = Cryptographer.Decrypt(keyValue, ExecutionServer.LicenseName);
      }
      return keyValue;
    }

    static private void WriteXmlConfigurationValue(string typeName, string name, string settingValue) {
      throw new NotImplementedException();
    }

    #endregion Private methods

  } //class XmlConfigurationFile

} //namespace Empiria
