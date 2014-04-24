/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : OntologyException                                Pattern  : Empiria Exception Class           *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in Empiria Foundation Ontology Framework.  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria.Ontology {

  /// <summary>The exception that is thrown when a problem occurs in
  /// Empiria Foundation Ontology Framework.</summary>
  [Serializable]
  public sealed class OntologyException : EmpiriaException {

    public enum Msg {
      CannotParseObjectWithDataRow,
      ConvertionToTargetTypeFails,
      ObjectIdNotFound,
      ObjectNamedKeyNotFound,
      RelationMemberNameNotFound,
      TryToParseZeroObjectId,
      TypeAssociationInfoNotFound,
      TypeInfoFamilyNotMatch,
      TypeInfoNotFound,
      TypeMethodInfoNotFound,
      TypeNamedIdFieldNameNotDefined,
      UndefinedTypeInfoFamily,
      WrongAssociatedObjectFound,
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

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class OntologyException

} // namespace Empiria.Ontology
