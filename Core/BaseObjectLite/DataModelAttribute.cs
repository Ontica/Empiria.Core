﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : DataModelAttribute                               Pattern  : Attribute class                   *
*                                                                                                            *
*  Summary   : Describes a data model information linked to a type for automatic object loading.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Describes a data model information linked to a type for automatic object loading.</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class DataModelAttribute : Attribute {

    #region Constructors and parsers

    /// <summary>Initializes a new instance of the <see cref="DataModelAttribute"/> class.</summary>
    /// <param name="sourceName">The name of the data item to map to the property or field.</param>
    public DataModelAttribute(string sourceName, string idFieldName) {
      Assertion.Require(sourceName, "sourceName");
      Assertion.Require(idFieldName, "idFieldName");

      this.SourceName = sourceName;
      this.IdFieldName = idFieldName;
      this.KeyFieldName = String.Empty;
      this.NoCache = false;
    }

    /// <summary>Initializes a new instance of the <see cref="DataModelAttribute"/> class.</summary>
    /// <param name="sourceName">The name of the data item to map to the property or field.</param>
    /// <param name="idFieldName">The name of the field that holds the integer id of the instance.</param>
    /// <param name="keyFieldName">The name of the field that holds the UID or unique key of the instance.</param>
    public DataModelAttribute(string sourceName, string idFieldName, string keyFieldName) {
      Assertion.Require(sourceName, "sourceName");
      Assertion.Require(idFieldName, "idFieldName");
      Assertion.Require(keyFieldName, "keyFieldName");

      this.SourceName = sourceName;
      this.IdFieldName = idFieldName;
      this.KeyFieldName = keyFieldName;
      this.NoCache = false;
    }

    #endregion Constructors and parsers

    #region Properties

    /// <summary>The name of the field that holds the integer id of the instance.</summary>
    public string IdFieldName {
      get;
      private set;
    }

    /// <summary>The name of the field that holds the UID or unique key of the instance.</summary>
    public string KeyFieldName {
      get;
      private set;
    }

    /// <summary>The name of the data source.</summary>
    public string SourceName {
      get;
      private set;
    }

    /// <summary>Indicates if the instances need to be reloaded each time.</summary>
    public bool NoCache {
      get;
      set;
    }

    #endregion Properties

  }  // class DataModelAttribute

}  // namespace Empiria
