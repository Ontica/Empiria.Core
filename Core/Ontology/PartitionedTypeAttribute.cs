/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : PartitionedTypeAttribute                         Pattern  : Attribute class                   *
*                                                                                                            *
*  Summary   : Specifies a type whose subtypes are instances of a powertype.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Ontology {

  /// <summary>Specifies a type whose subtypes are instances of a powertype.</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class PartitionedTypeAttribute : Attribute {

    /// <summary>Initializes a new instance of the <see cref="PartitionedTypeAttribute"/> class.</summary>
    /// <param name="powertypeType">The type of the powertype type associated to this partitioned type.</param>
    public PartitionedTypeAttribute(Type powertypeType) {
      Assertion.AssertObject(powertypeType, "powertypeType");
      this.Powertype = powertypeType;
    }

    /// <summary>The type of the powertype type.</summary>
    public Type Powertype {
      get;
      set;
    }

  }  // class PartitionedTypeAttribute

}  // namespace Empiria.Ontology
