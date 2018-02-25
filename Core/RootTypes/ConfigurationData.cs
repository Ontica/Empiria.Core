/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ConfigurationData                                Pattern  : Static Class                      *
*                                                                                                            *
*  Summary   : Gets configuration values for the application or solution.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Diagnostics;

using Empiria.Reflection;

namespace Empiria {

  /// <summary>Gets configuration values for the application or solution.</summary>
  static public class ConfigurationData {

    #region Public methods

    /// <summary>Returns the value of the configuration data belonging to the caller type.</summary>
    /// <param name="key">The key name of the configuration value.</param>
    static public T Get<T>(string key) {
      Assertion.AssertObject(key, "key");

      string typeName = GetCallerTypeName();

      string valueAsString = TryReadValue(typeName, key);

      if (valueAsString != null) {
        return ObjectFactory.Convert<T>(valueAsString);

      } else {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.ParameterNotExists,
                                             key, typeName);

      }
    }


    /// <summary>Returns the value of the configuration data belonging for the given type.</summary>
    /// <param name="key">The key name of the configuration value.</param>
    static public T Get<T>(Type type, string key) {
      Assertion.AssertObject(type, "type");
      Assertion.AssertObject(key, "key");

      string valueAsString = TryReadValue(type.FullName, key);

      if (valueAsString != null) {
        return ObjectFactory.Convert<T>(valueAsString);

      } else {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.ParameterNotExists,
                                             key, type.FullName);

      }
    }


    /// <summary>Returns the value of the configuration data belonging to the caller type.</summary>
    /// <param name="key">The key name of the configuration value.</param>
    /// <param name="defaultValue">The default value to return if the key is not found.</param>
    static public T Get<T>(string key, T defaultValue) {
      Assertion.AssertObject(key, "key");

      string typeName = GetCallerTypeName();

      string valueAsString = TryReadValue(typeName, key);

      if (valueAsString != null) {
        return ObjectFactory.Convert<T>(valueAsString);

      } else {
        return defaultValue;

      }
    }


    /// <summary>Returns the value of the configuration data for the given type.</summary>
    /// <param name="key">The key name of the configuration value.</param>
    /// <param name="defaultValue">The default value to return if the key is not found.</param>
    static public T Get<T>(Type type, string key, T defaultValue) {
      Assertion.AssertObject(type, "type");
      Assertion.AssertObject(key, "key");

      string valueAsString = TryReadValue(type.FullName, key);

      if (valueAsString != null) {
        return ObjectFactory.Convert<T>(valueAsString);

      } else {
        return defaultValue;

      }
    }


    static public byte[] GetByteArray(string typeName, string parameterName, char separator) {
      string[] sbytes = ReadValue(typeName, parameterName).Split(separator);

      return ToByteArray(sbytes);
    }

    static public byte[] GetByteArray(string parameterName, char separator) {
      string[] sbytes = ReadValue(GetCallerTypeName(), parameterName).Split(separator);

      return ToByteArray(sbytes);
    }

    /// <summary>Returns the value of a boolean configuration parameter belongs to the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    static public bool GetBoolean(string parameterName) {
      return bool.Parse(ReadValue(GetCallerTypeName(), parameterName));
    }

    /// <summary>Returns the value of a boolean configuration parameter belongs to the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    static public bool GetBoolean(string typeName, string parameterName) {
      return bool.Parse(ReadValue(typeName, parameterName));
    }

    /// <summary>Returns the value of a Int32 configuration parameter belongs to the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    static public int GetInteger(string parameterName) {
      return int.Parse(ReadValue(GetCallerTypeName(), parameterName));
    }

    /// <summary>Returns the value of a Int32 configuration parameter belongs to the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    static public int GetInteger(string typeName, string parameterName) {
      return int.Parse(ReadValue(typeName, parameterName));
    }

    /// <summary>Returns the value of a string configuration parameter belongs to the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    static public string GetString(string parameterName) {
      return ReadValue(GetCallerTypeName(), parameterName);
    }

    /// <summary>Returns the value of a date-time configuration parameter belongs to the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    static public DateTime GetDateTime(string parameterName) {
      return DateTime.Parse(ReadValue(GetCallerTypeName(), parameterName));
    }

    /// <summary>Returns the value of a date-time configuration setting of the gived type.</summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="parameterName">Name of the configuration setting.</param>
    static public DateTime GetDateTime(string typeName, string parameterName) {
      return DateTime.Parse(ReadValue(typeName, parameterName));
    }

    /// <summary>Returns the value of a string configuration setting of the gived type.</summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="parameterName">Name of the configuration setting.</param>
    static public string GetString(string typeName, string parameterName) {
      return ReadValue(typeName, parameterName);
    }

    #endregion Public methods

    #region Private methods

    static private string GetCallerTypeName() {
      string callerFullName = String.Empty;
      string stackString = String.Empty;
      StackTrace stack = new StackTrace();

      for (int i = 1; i < stack.FrameCount; i++) {
        callerFullName = stack.GetFrame(i).GetMethod().DeclaringType.FullName;
        if (callerFullName != typeof(ConfigurationData).FullName &&
           (callerFullName.StartsWith("Empiria") || callerFullName.StartsWith(ExecutionServer.LicenseName))) {
          return callerFullName;
        } else {
          stackString += callerFullName + "\n";
        }
      }
      throw new ConfigurationDataException(ConfigurationDataException.Msg.ValidTypeNotFoundInStackTrace, stackString);
    }

    static private string ReadValue(string typeName, string parameterName) {
      try {
        Assertion.AssertObject(typeName, "typeName");
        Assertion.AssertObject(parameterName, "parameterName");

        // Try get the parameter's value from the application's configuration file
        var value = ConfigurationFile.TryGetValue(typeName, parameterName);
        if (value != null) {
          return value;
        }

        // If attempting to get a protected parameter failed,
        // then try to get the unprotected version of the same parameter
        if (parameterName.StartsWith("§")) {
          value = ConfigurationFile.TryGetValue(typeName, parameterName.Substring(1));
          if (value != null) {
            return value;
          }
        }

        Assertion.AssertObject(value, "ReadValue.Value");

        return value;
      } catch (Exception innerException) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.CantReadParameter,
                                             innerException, parameterName, typeName);
      }
    }

    static private string TryReadValue(string typeName, string parameterName) {
      try {
        Assertion.AssertObject(typeName, "typeName");
        Assertion.AssertObject(parameterName, "parameterName");

        // Try get the parameter's value from the application's configuration file
        return ConfigurationFile.TryGetValue(typeName, parameterName);

      } catch (Exception innerException) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.CantReadParameter,
                                             innerException, parameterName, typeName);
      }
    }

    static private byte[] ToByteArray(string[] sbytes) {
      byte[] byteArray = new byte[sbytes.Length];

      for (int i = 0; i < sbytes.Length; i++) {
        byteArray[i] = byte.Parse(sbytes[i].Trim());
      }
      return byteArray;
    }

    #endregion Private methods

  } //class ConfigurationData

} //namespace Empiria
