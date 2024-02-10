/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Object-relational mapping         *
*  Namespace : Empiria.ORM                                      License  : Please read LICENSE.txt file      *
*  Type      : DataMappingException                             Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in the OR/M mapping process.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria.ORM {

  /// <summary>The exception that is thrown when a problem occurs in the OR/M mapping process.</summary>
  [Serializable]
  public sealed class DataMappingException : EmpiriaException {

    public enum Msg {
      CannotGetDefaultValueforType,
      CannotInitializeObject,
      CannotMapDataValue,
      CannotParsePropertyForDefaultValue,
      MappingDataColumnNotFound,
      TypeMemberMappingFails,
    }

    static private string resourceBaseName = "Empiria.ORM.DataMappingExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of DataMappingException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public DataMappingException(Msg message, params object[] args) :
                                base(message.ToString(), GetMessage(message, args)) {
      base.Publish();
    }

    /// <summary>Initializes a new instance of DataMappingException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public DataMappingException(Msg message, Exception innerException, params object[] args) :
                                base(message.ToString(), GetMessage(message, args), innerException) {
      base.Publish();
    }


    static internal DataMappingException GetDataValueMappingException(object instance, DataMapping rule,
                                                                      Exception innerException) {
      throw new DataMappingException(Msg.CannotMapDataValue, innerException,
                                     GetExecutionData(instance, rule));
    }

    static internal DataMappingException GetDataValueMappingException(object instance, DataObjectMapping rule,
                                                                      Exception innerException) {
      throw new DataMappingException(Msg.CannotMapDataValue, innerException,
                                     GetExecutionData(instance, rule));
    }

    static internal DataMappingException GetInitializeObjectException(object instance, DataMapping rule,
                                                                      Exception innerException) {
      throw new DataMappingException(Msg.CannotInitializeObject, innerException,
                                     GetExecutionData(instance, rule));
    }

    static internal DataMappingException GetInitializeObjectException(object instance, DataObjectMapping rule,
                                                                  Exception innerException) {
      throw new DataMappingException(Msg.CannotInitializeObject, innerException,
                                     GetExecutionData(instance, rule));
    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetExecutionData(object instance, DataMapping rule) {
      string str = rule.GetExecutionData();

      str += String.Format("Instance Type: {0}\n", instance.GetType().FullName);
      if (instance is IIdentifiable) {
        str += String.Format("Instance Id: {0}\n", ((IIdentifiable) instance).Id);
      }
      return str;
    }

    static private string GetExecutionData(object instance, DataObjectMapping rule) {
      string str = rule.GetExecutionData();

      str += String.Format("Instance Type: {0}\n", instance.GetType().FullName);
      return str;
    }

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class DataMappingException

} // namespace Empiria.ORM
