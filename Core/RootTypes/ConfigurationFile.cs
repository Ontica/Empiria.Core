/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ConfigurationFile                                Pattern  : Singleton                         *
*                                                                                                            *
*  Summary   : Gets data items defined in the app.config or in a Json based configuration file.              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.IO;

using Empiria.Json;
using Empiria.Security;

namespace Empiria {

  /// <summary>Gets data items defined in the app.config or in a Json based configuration file.</summary>
  internal class ConfigurationFile {

    #region Fields

    private Dictionary<string, string> _settingsCache = new Dictionary<string, string>();

    #endregion Fields

    #region Constructors and parsers

    static private volatile ConfigurationFile _instance = null;
    static private object _syncRoot = new Object();
    private readonly Exception _loadFailedException;

    static private ConfigurationFile Instance {
      get {
        if (_instance == null) {
          lock (_syncRoot) {
            if (_instance == null) {
              _instance = new ConfigurationFile();
            }
          }
        }

        if (_instance._loadFailedException == null) {
          return _instance;
        } else {
          throw new ConfigurationDataException(ConfigurationDataException.Msg.ConfigurationFileLoadFailed,
                                             _instance._loadFailedException);
        }
      }
    }


    private ConfigurationFile() {
      try {
        this.Load();
        _loadFailedException = null;

      } catch (Exception e) {
        _loadFailedException = e;
      }
    }

    #endregion Constructors and parsers

    #region Internal methods

    static internal string TryGetValue(string typeName, string parameterName) {
      return Instance.TryGetSetting(typeName, parameterName);
    }

    #endregion Internal methods

    #region Private methods

    private void Load() {
      // First, load application settings directly from the app.config or the web.config
      this.LoadSettingsFromAppConfig();

      // Then, try to load application settings defined in empiria.config file, if any
      string appSettingsFileName = TryGetFileNameFromEmpiriaAppConfig();
      if (!String.IsNullOrWhiteSpace(appSettingsFileName)) {
        this.LoadSettingsFromJsonFile(appSettingsFileName);
      }

      // Try to load data from the environment configuration file
      string environmentFileName = this.TryGetSetting("Empiria", "EnvironmentConfigurationFile");
      if (!String.IsNullOrWhiteSpace(environmentFileName)) {
        if (ExistsFile(environmentFileName)) {
          this.LoadSettingsFromJsonFile(environmentFileName);
        } else {
          throw new ConfigurationDataException(ConfigurationDataException.Msg.EnvironmentConfigFileNotExists,
                                               environmentFileName);
        }
      }

      // Try to load data from global configuration file
      string globalFileName = this.TryGetSetting("Empiria", "GlobalConfigurationFile");
      if (!String.IsNullOrWhiteSpace(globalFileName)) {
        if (ExistsFile(globalFileName)) {
          this.LoadSettingsFromJsonFile(globalFileName);
        } else {
          throw new ConfigurationDataException(ConfigurationDataException.Msg.GlobalConfigFileNotExists,
                                               globalFileName);
        }
      }
    }

    private string ReadConfigurationValue(string typeName, string parameterName) {
      string key = BuildKey(typeName, parameterName);

      string keyValue = null;
      if (!_settingsCache.TryGetValue(key, out keyValue)) {
        keyValue = null;
      }
      if ((keyValue != null) && parameterName.StartsWith("§")) {
        keyValue = Cryptographer.Decrypt(keyValue, ExecutionServer.LicenseName);
      }
      return keyValue;
    }


    private string TryGetSetting(string typeName, string parameterName) {
      if (!(typeName.StartsWith("Empiria") || typeName.StartsWith(ExecutionServer.LicenseName))) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.InvalidTypeName,
                                             parameterName, typeName);
      }

      string tempTypeName = typeName.Substring(typeName.IndexOf('.') + 1);
      string retrivedValue = null;
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

    #endregion Private methods

    #region Auxiliary methods

    /// <summary>Builds a case-insensitive dictionary key.</summary>
    static private string BuildKey(string typeName, string key) {
      string temp = String.Empty;

      if (typeName.Length != 0) {
        temp = typeName + "." + key;
      } else {
        temp = key;
      }
      return temp.ToLowerInvariant();
    }

    static private void EnsureApplicationFileExists(string fileName) {
      if (!ExistsFile(fileName)) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.ApplicationConfigFileNotExists,
                                             fileName);
      }
    }

    /// <summary>Returns true if the given fileName string is a real path to a file.</summary>
    static private bool ExistsFile(string fileName) {
      if (String.IsNullOrWhiteSpace(fileName)) {
        return false;
      }
      return File.Exists(fileName);
    }

    /// <summary>Loads all the application settings from the app.config or web.config file.</summary>
    private void LoadSettingsFromAppConfig() {
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;

      foreach (var settingKey in appSettings.AllKeys) {
        string key = BuildKey("", settingKey);
        if (!_settingsCache.ContainsKey(key)) {
          _settingsCache.Add(key, appSettings[settingKey]);
        }
      }
    }

    /// <summary>Loads all settings defined in a JSON file.</summary>
    private void LoadSettingsFromJsonFile(string fileName) {
      var json = JsonObject.Parse(File.ReadAllText(fileName));

      // Load the settings from the JSON config file
      List<ConfigurationSetting> settingsList = json.GetList<ConfigurationSetting>("settings");
      foreach (var item in settingsList) {
        string key = BuildKey(item.TypeName, item.Key);
        if (!_settingsCache.ContainsKey(key)) {
          string replaceValue = EnvironmentVariables.TryGetValue(item.Value);
          _settingsCache.Add(key, replaceValue ?? item.Value);
        }
      }
    }

    /// <summary>Gets the configuration file name looking for the path string inside
    ///  the app.config or web.config files, or searching for a file with name
    ///  empiria.app.config inside the system's execution path. Returns null
    ///  if no config file was found.</summary>
    static private string TryGetFileNameFromEmpiriaAppConfig() {
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;

      // Seek for the filename under the 'EmpiriaConfigurationFile' key of the app.config file
      string configFileName = appSettings["EmpiriaConfigurationFile"];
      if (configFileName != null) {
        EnsureApplicationFileExists(configFileName);
        return configFileName;
      }

      // If not 'EmpiriaConfigurationFile' key, then look for a file with name "empiria.config.json"
      configFileName = ExecutionServer.GetFullFileNameFromCurrentExecutionPath("empiria.config.json");
      if (ExistsFile(configFileName)) {
        return configFileName;
      }
      return null;
    }

    #endregion Auxiliary methods

  } //class ConfigurationFile

} //namespace Empiria
