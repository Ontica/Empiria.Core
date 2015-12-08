/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ConfigurationFile                                Pattern  : Static Class                      *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Gets data items defined in the app.config or in a Json based configuration file.              *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

      // First, load all settings directly from the app.config or the web.config
      configFile.LoadSettingsFromAppConfig();

      // Then, try to load the settings defined in the application settings file, if any
      string appSettingsFileName = TryGetFileNameFromAppConfig();
      if (!String.IsNullOrWhiteSpace(appSettingsFileName)) {
        configFile.LoadSettingsFromJsonFile(appSettingsFileName);
      }

      // Try load all settings defined in the solutions settings file, if any
      if (!String.IsNullOrWhiteSpace(configFile.SolutionSettingsFile)) {
        if (ExistsFile(configFile.SolutionSettingsFile)) {
          configFile.LoadSettingsFromJsonFile(configFile.SolutionSettingsFile);
        } else {
          throw new ConfigurationDataException(ConfigurationDataException.Msg.SolutionConfigFileNotExists,
                                               configFile.SolutionSettingsFile);
        }
      }
      return configFile;
    }

    #endregion Internal methods

    #region Private methods

    /// <summary>Holds the solution settings filename, if any.</summary>
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

    /// <summary>Builds a case-insensitive dictionary key.</summary>
    private string BuildKey(string typeName, string key) {
      string temp = String.Empty;

      if (typeName.Length != 0) {
        temp = typeName + "." + key;
      } else {
        temp = key;
      }
      return temp.ToLowerInvariant();
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
        string key = this.BuildKey("", settingKey);
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
        string key = this.BuildKey(item.TypeName, item.Key);
        if (!_settingsCache.ContainsKey(key)) {
          _settingsCache.Add(key, item.Value);
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

      // Look the filename under the 'SettingsConfigurationFile' key of the app.config file
      string configFile = appSettings["SettingsConfigurationFile"];
      if (configFile != null) {
        AssertApplicationFileExists(configFile);
        return configFile;
      }

      // If not 'SettingsConfigurationFile' key, then look for a file with name "empiria.config.json"
      configFile = GetFullFileNameFromCurrentExecutionPath("empiria.config.json");
      if (ExistsFile(configFile)) {
        return configFile;
      }
      return null;
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

    static private string GetFullFileNameFromCurrentExecutionPath(string fileName) {
      string baseExecutionPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

      return Path.Combine(baseExecutionPath, fileName);
    }

    private string ReadConfigurationValue(string typeName, string parameterName) {
      string key = this.BuildKey(typeName, parameterName);

      string keyValue = null;
      if (!_settingsCache.TryGetValue(key, out keyValue)) {
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
