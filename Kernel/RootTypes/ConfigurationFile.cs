/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ConfigurationFile                                Pattern  : Static Class                      *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Gets data elements using a Json based configuration file.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.IO;

using System.Xml;

using Empiria.Json;
using Empiria.Security;

namespace Empiria {

  /// <summary>Gets data elements using a Json-based configuration file.</summary>
  internal class ConfigurationFile {

    #region Fields

    private Dictionary<string, string> _settingsCache = new Dictionary<string, string>();

    #endregion Fields

    #region Constructors and parsers

    private ConfigurationFile() {

    }

    #endregion Constructors and parsers

    #region Internal methods

    static private Lazy<ConfigurationFile> _instance = new Lazy<ConfigurationFile>( () => ConfigurationFile.Load() );

    static internal string TryGetValue(string typeName, string parameterName) {
      ConfigurationFile configFile = _instance.Value;

      return configFile.TryGetSetting(typeName, parameterName);
    }

    static private ConfigurationFile Load() {
      var configFile = new ConfigurationFile();

      // Try load all settings defined in the application settings file
      string appSettingsFileName = TryGetFileNameFromAppConfig();
      if (!String.IsNullOrWhiteSpace(appSettingsFileName)) {
        configFile.LoadData(appSettingsFileName);
      }

      // Try load all settings defined in the solutions settings file, if any
      if (!String.IsNullOrWhiteSpace(configFile.SolutionSettingsFile)) {
        if (ExistsFile(configFile.SolutionSettingsFile)) {
          configFile.LoadData(configFile.SolutionSettingsFile);
        } else {
          throw new ConfigurationDataException(ConfigurationDataException.Msg.SolutionConfigFileNotExists,
                                               configFile.SolutionSettingsFile);
        }
      }

      return configFile;
    }

    #endregion Internal methods

    #region Private methods

    private string SolutionSettingsFile {
      get;
      set;
    }

    static private void AssertApplicationFileExists(string fileName) {
      if (!ExistsFile(fileName)) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.ApplicationConfigFileNotExists,
                                             fileName);
      }
    }

    static private bool ExistsFile(string fileName) {
      if (String.IsNullOrWhiteSpace(fileName)) {
        return false;
      }
      return File.Exists(fileName);
    }

    private void LoadData(string fileName) {
      var json = JsonObject.Parse(File.ReadAllText(fileName));

      // Load the settings from the JSON config file
      List<ConfigurationSetting> settingsList = json.GetList<ConfigurationSetting>("settings");
      foreach (var item in settingsList) {
        if (!_settingsCache.ContainsKey(item.TypeName + "." + item.Key)) {
          _settingsCache.Add(item.TypeName + "." + item.Key, item.Value);
        }
      }

      // Load the path of the parent config file, if any defined
      if (this.SolutionSettingsFile == null) {
        this.SolutionSettingsFile = json.Get<String>("solutionSettingsFile", String.Empty);
      }
    }

    /// <summary>Gets the configuration file name looking for the path string inside
    ///  the app.config or web.config files, or searching for a file with name
    ///  empiria.app.config inside the system's execution path. Returns null
    ///  if no config file was found.</summary>
    static private string TryGetFileNameFromAppConfig() {
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;

      string configFile = appSettings["ConfigFile"];
      if (configFile != null) {
        AssertApplicationFileExists(configFile);
        return configFile;
      }

      configFile = appSettings["EmpiriaConfigFile"];
      if (configFile != null) {
        AssertApplicationFileExists(configFile);
        return configFile;
      }

      configFile = GetFullFileNameFromCurrentExecutionPath("empiria.app.config");
      if (ExistsFile(configFile)) {
        return configFile;
      }
      return null;
    }

    private string TryGetSetting(string typeName, string parameterName) {
      string retrivedValue = null;
      string tempTypeName = typeName;

      while (true) {
        if ((retrivedValue == null) && (tempTypeName != null)) {
          retrivedValue = ReadConfigurationValue(tempTypeName, parameterName);
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
      if (!String.IsNullOrEmpty(retrivedValue)) {
        return retrivedValue;
      } else {
        return null;
      }
    }

    static private string GetFullFileNameFromCurrentExecutionPath(string fileName) {
      string baseExecutionPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

      return Path.Combine(baseExecutionPath, fileName);
    }

    private string ReadConfigurationValue(string typeName, string parameterName) {
      string keyValue = null;

      if (!_settingsCache.TryGetValue(typeName + "." + parameterName, out keyValue)) {
        keyValue = null;
      }
      if ((keyValue != null) && parameterName.StartsWith("§")) {
        keyValue = Cryptographer.Decrypt(keyValue, ExecutionServer.LicenseName);
      }
      return keyValue;
    }

    #endregion Private methods

  } //class ConfigurationFile

} //namespace Empiria
