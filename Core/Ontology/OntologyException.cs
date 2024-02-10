/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : OntologyException                                Pattern  : Exception Class                   *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in Empiria Ontology Framework.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

namespace Empiria.Ontology {

  /// <summary>The exception that is thrown when a problem occurs in Empiria Ontology Framework.</summary>
  [Serializable]
  public sealed class OntologyException : EmpiriaException {

    public enum Msg {
      CannotGetUnderlyingSystemType,
      CannotParseObjectWithDataRow,
      DefaultConstructorNotFound,
      ObjectIdNotFound,
      ObjectNamedKeyNotFound,
      PartitionedTypeAttributeMissed,
      PartitionedTypeConstructorNotFound,
      TryToParseZeroObjectId,
      TypeInfoFamilyNotMatch,
      TypeInfoNotFound,
      UndefinedTypeInfoFamily,
      UnderlyingTypeNotFound,
    }

    static private string resourceBaseName = "Empiria.Ontology.OntologyExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of OntologyException class with a specified error
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public OntologyException(Msg message, params object[] args) :
                             base(message.ToString(), GetMessage(message, args)) {
      base.Publish();
    }

    /// <summary>Initializes a new instance of OntologyException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public OntologyException(Msg message, Exception innerException, params object[] args) :
                             base(message.ToString(), GetMessage(message, args), innerException) {
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
