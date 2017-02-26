/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : PartitionedTypeAttribute                         Pattern  : Attribute class                   *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Specifies a type whose subtypes are instances of a powertype.                                 *
*                                                                                                            *
********************************* Copyright (c) 2014-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
