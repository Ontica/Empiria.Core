/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : EnvironmentVariables                             Pattern  : Static Singleton Class            *
*                                                                                                            *
*  Summary   : Gets environment variables defined as key value pairs in files, which are used for            *
*              replace variable values used in configuration settings files.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.IO;

using Empiria.DataTypes;
using Empiria.Json;

namespace Empiria {

  /// <summary>Gets environment variables defined as key value pairs in files, which are used for replace
  /// variable values used in configuration settings files.</summary>
  internal class EnvironmentVariables {

    #region Fields

    static private Lazy<EnvironmentVariables> _instance = new Lazy<EnvironmentVariables>(() => EnvironmentVariables.Load());

    private Dictionary<string, string> _variables = new Dictionary<string, string>();

    #endregion Fields

    #region Constructors and parsers

    private EnvironmentVariables() {

    }

    static private EnvironmentVariables Load() {
      var environmentVariables = new EnvironmentVariables();

      string fileName = ExecutionServer.GetFullFileNameFromCurrentExecutionPath("empiria.environment.vars.json");

      if (File.Exists(fileName)) {
        environmentVariables.LoadVariablesFromJsonFile(fileName);
      }
      return environmentVariables;
    }

    #endregion Constructors and parsers

    #region Public methods

    static internal string TryGetValue(string variableName) {
      EnvironmentVariables configFile = _instance.Value;

      if (configFile._variables.ContainsKey(variableName)) {
        return configFile._variables[variableName];
      }
      return null;
    }

    #endregion Public methods

    #region Private methods

    private List<string> _loadedFiles = new List<string>(2);
    private void EnsureFileNameWasNotProcessedAndMarkDownThis(string fileName) {
      if (_loadedFiles.Contains(fileName)) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.FileAlreadyProcessed, fileName);
      } else {
        _loadedFiles.Add(fileName);
      }
    }


    /// <summary>Loads variables defined in a JSON file.</summary>
    private void LoadVariablesFromJsonFile(string fileName) {
      EnsureFileNameWasNotProcessedAndMarkDownThis(fileName);
      var json = JsonObject.Parse(File.ReadAllText(fileName));

      List<KeyValue> itemList = json.GetList<KeyValue>("variables");
      foreach (var item in itemList) {
        if (!_variables.ContainsKey(item.Key)) {
          _variables.Add(item.Key, item.Value);
        }
      }

      // Recursively load variables in other additional sources
      var additionalSources = json.GetList<string>("additionalSources", false);
      foreach (var additionalSource in additionalSources) {
        LoadVariablesFromJsonFile(additionalSource);
      }
    }

    #endregion Private methods

  } //class EnvironmentVariables

} //namespace Empiria
