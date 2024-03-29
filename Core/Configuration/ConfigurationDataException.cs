﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ConfigurationDataException                       Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a configuration parameter is not found or when the system   *
*              can't read or access it.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria {

  /// <summary>The exception that is thrown when a configuration parameter is not found or when the system
  /// can't read or access it.</summary>
  [Serializable]
  public class ConfigurationDataException : EmpiriaException {

    public enum Msg {
      ApplicationConfigFileNotExists,
      CantReadParameter,
      CantWriteParameter,
      ConfigurationFileLoadFailed,
      EnvironmentConfigFileNotExists,
      FileAlreadyProcessed,
      GlobalConfigFileNotExists,
      InvalidTypeName,
      ParameterNotExists,
      ValidTypeNotFoundInStackTrace,
      XmlConfigurationFileNotExists,
    }

    static private string resourceBaseName = "Empiria.Exceptions.KernelExceptionMsg";

    #region Constructors

    /// <summary>Initializes a new instance of ConfigurationDataException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ConfigurationDataException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {

    }

    /// <summary>Initializes a new instance of ConfigurationDataException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of objects to format into the exception message.</param>
    public ConfigurationDataException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {

    }

    #endregion Constructors

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class ConfigurationDataException

} // namespace Empiria
