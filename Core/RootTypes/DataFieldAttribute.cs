/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : DataFieldAttribute                               Pattern  : Attribute class                   *
*                                                                                                            *
*  Summary   : Describes data source field information for automatic object loading.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Describes data source field information for automatic object loading.</summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class DataFieldAttribute : Attribute {

    /// <summary>Initializes a new instance of the <see cref="DataFieldAttribute"/> class.</summary>
    /// <param name="name">The name of the data item to map to the property or field.</param>
    public DataFieldAttribute(string name) {
      Assertion.AssertObject(name, "name");
      this.Name = name;
      this.IsOptional = true;
      this.Default = null;
    }

    /// <summary>The name of the data item to map to the property or field.</summary>
    public string Name {
      get;
      private set;
    }

    /// <summary>The default value of the data item. Returns null if not default value was set.</summary>
    public object Default {
      get;
      set;
    }

    /// <summary>Indicates if the data field is optional and therefore the Default value should be
    /// used in the abscence of data. IsOptional default value is true. Attempting to read an empty
    /// data field marked with IsOptional = false throws an exception.</summary>
    public bool IsOptional {
      get;
      set;
    }

  }  // class DataFieldAttribute

}  // namespace Empiria
