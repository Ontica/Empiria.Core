/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology.Modeler                         Assembly : Empiria.dll                       *
*  Type      : DataMappingRules                                 Pattern  : Attribute class                   *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents data source field information for automatic object loading.                        *
*                                                                                                            *
********************************* Copyright (c) 2014-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  /// <summary>Represents data source field information for automatic object loading.</summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class DataFieldAttribute : Attribute {

    /// <summary>Initializes a new instance of the <see cref="DataFieldAttribute"/> class.</summary>
    /// <param name="name">The name of the data item to map to the property or field.</param>
    public DataFieldAttribute(string name) {
      Assertion.RequireObject(name, "name");
      this.Name = name;
    }

    /// <summary>The name of the data item to map to the property or field.</summary>
    public string Name {
      get;
      private set;
    }

  }  // class DataFieldAttribute

}  // namespace Empiria
