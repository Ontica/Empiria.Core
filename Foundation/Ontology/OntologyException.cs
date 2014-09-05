/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : OntologyException                                Pattern  : Empiria Exception Class           *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in Empiria Foundation Ontology Framework.  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

using Empiria.Ontology.Modeler;

namespace Empiria.Ontology {

  /// <summary>The exception that is thrown when a problem occurs in
  /// Empiria Foundation Ontology Framework.</summary>
  [Serializable]
  public sealed class OntologyException : EmpiriaException {

    public enum Msg {
      CannotGetDefaultValueforType,
      CannotGetUnderlyingSystemType,
      CannotInitializeObject,
      CannotMapDataValue,
      CannotParseObjectWithDataRow,
      CannotParsePropertyForDefaultValue,
      ConvertionToTargetTypeFails,
      MappingDataColumnNotFound,
      ObjectIdNotFound,
      ObjectNamedKeyNotFound,
      RelationMemberNameNotFound,
      TryToParseZeroObjectId,
      TypeAssociationInfoNotFound,
      TypeInfoFamilyNotMatch,
      TypeInfoNotFound,
      TypeMemberMappingFails,
      TypeMethodInfoNotFound,
      TypeNamedIdFieldNameNotDefined,
      UndefinedTypeInfoFamily,
      UnderlyingTypeNotFound,
      WrongAssociatedObjectFound,
      WrongDefaultValueType,
    }

    static private string resourceBaseName = "Empiria.Ontology.OntologyExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of OntologyException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public OntologyException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {
      base.Publish();
    }

    /// <summary>Initializes a new instance of OntologyException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public OntologyException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {
      base.Publish();
    }


    static internal OntologyException GetDataValueMappingException(object instance, DataMapping rule,
                                                                   Exception innerException) {
      throw new OntologyException(OntologyException.Msg.CannotMapDataValue, innerException, 
                                  OntologyException.GetExecutionData(instance, rule));
    }

    static internal OntologyException GetInitializeObjectException(object instance, DataMapping rule,
                                                                   Exception innerException) {
      throw new OntologyException(OntologyException.Msg.CannotInitializeObject, innerException,
                                  OntologyException.GetExecutionData(instance, rule));
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

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class OntologyException

} // namespace Empiria.Ontology
