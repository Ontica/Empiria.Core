/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ConfigurationData                                Pattern  : Static Class                      *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Gets or sets the configuration parameters of an Empiria Framework type or types in customer   *
*              systems.                                                                                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Diagnostics;

namespace Empiria {

  /// <summary>Gets or sets the configuration parameters of an Empiria Framework type or types
  /// in customer systems.</summary>
  static public class ConfigurationData {

    #region Public methods

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
    /// <param name="parameter">Name of the configuration parameter.</param>
    static public bool GetBoolean(string typeName, string parameterName) {
      return bool.Parse(ReadValue(typeName, parameterName));
    }

    /// <summary>Returns the value of a Int32 configuration parameter belongs to the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    static public int GetInteger(string parameterName) {
      return int.Parse(ReadValue(GetCallerTypeName(), parameterName));
    }

    /// <summary>Returns the value of a Int32 configuration parameter belongs to the caller type.</summary>
    /// <param name="parameter">Name of the configuration parameter.</param>
    static public int GetInteger(string typeName, string parameterName) {
      return int.Parse(ReadValue(typeName, parameterName));
    }

    /// <summary>Returns the value of a string configuration parameter belongs to the caller type.</summary>
    /// <param name="parameter">Name of the configuration parameter.</param>
    static public string GetString(string parameterName) {
      return ReadValue(GetCallerTypeName(), parameterName);
    }

    /// <summary>Returns the value of a date-time configuration parameter belongs to the caller type.</summary>
    /// <param name="parameter">Name of the configuration parameter.</param>
    static public DateTime GetDateTime(string parameterName) {
      return DateTime.Parse(ReadValue(GetCallerTypeName(), parameterName));
    }

    /// <summary>Returns the value of a date-time configuration setting of the gived type.</summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="name">Name of the configuration setting.</param>
    static public DateTime GetDateTime(string typeName, string parameterName) {
      return DateTime.Parse(ReadValue(typeName, parameterName));
    }

    /// <summary>Returns the value of a string configuration setting of the gived type.</summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="name">Name of the configuration setting.</param>
    static public string GetString(string typeName, string parameterName) {
      return ReadValue(typeName, parameterName);
    }

    /// <summary>Stores the boolean value of a configuration parameter for the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    /// <param name="value">The boolean value of the parameter to store.</param>
    static public void SetBoolean(string parameterName, bool value) {
      WriteValue(GetCallerTypeName(), parameterName, value.ToString());
    }

    /// <summary>Stores the integer value of a configuration parameter for the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    /// <param name="value">The integer value of the parameter to store.</param>
    static public void SetInteger(string parameterName, int value) {
      WriteValue(GetCallerTypeName(), parameterName, value.ToString());
    }

    /// <summary>Stores the value of a configuration parameter for the caller type.</summary>
    /// <param name="parameterName">Name of the configuration parameter.</param>
    /// <param name="value">The value of the parameter to store.</param>
    static public void SetString(string parameterName, string value) {
      WriteValue(GetCallerTypeName(), parameterName, value);
    }

    #endregion Public methods

    #region Private methods

    static private string GetCallerTypeName() {
      string callerFullName = String.Empty;
      string stackString = String.Empty;
      StackTrace stack = new StackTrace();

      for (int i = 1; i < stack.FrameCount; i++) {
        callerFullName = stack.GetFrame(i).GetMethod().DeclaringType.FullName;
        if (!callerFullName.StartsWith("Empiria.ConfigurationData") &&
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

        string value = null;
        if (XmlConfigurationFile.Exists()) {
          value = XmlConfigurationFile.ReadValue(typeName, parameterName);
        } else {
          value = WindowsRegistryFile.ReadValue(typeName, parameterName);
        }
        Assertion.AssertObject(value, "ReadValue.Value");
        return value;
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

    static private void WriteValue(string typeName, string parameterName, string value) {
      try {
        Assertion.AssertObject(typeName, "typeName");
        Assertion.AssertObject(parameterName, "parameterName");
        Assertion.AssertObject(value, "value");

        if (XmlConfigurationFile.Exists()) {
          XmlConfigurationFile.WriteValue(typeName, parameterName, value);
        } else {
          WindowsRegistryFile.WriteValue(typeName, parameterName, value);
        }
      } catch (Exception innerException) {
        throw new ConfigurationDataException(ConfigurationDataException.Msg.CantWriteParameter,
                                             innerException, parameterName, typeName);
      }
    }

    #endregion Private methods

  } //class ConfigurationData

} //namespace Empiria
