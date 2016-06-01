/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : ConfigurationData                                Pattern  : Static Class                      *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Gets configuration values for the application or solution.                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Diagnostics;

namespace Empiria {

  /// <summary>Gets configuration values for the application or solution.</summary>
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

        // Try get the parameter's value form the app or solution configuration file
        string value = ConfigurationFile.TryGetValue(typeName, parameterName);
        if (value != null) {
          return value;
        }

        // If not found then get the value from the Windows Registry
        value = WindowsRegistryFile.GetValue(typeName, parameterName);

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

    #endregion Private methods

  } //class ConfigurationData

} //namespace Empiria
