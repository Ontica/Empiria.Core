/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : PartitionedTypeAttribute                         Pattern  : Attribute class                   *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Specifies a type whose subtypes are instances of a powertype.                                 *
*                                                                                                            *
********************************* Copyright (c) 2014-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  /// <summary>Specifies a type whose subtypes are instances of a powertype.</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class PartitionedTypeAttribute : Attribute {

    /// <summary>Initializes a new instance of the <see cref="PartitionedTypeAttribute"/> class.</summary>
    /// <param name="name">The type of the powertype type associated to this partitioned type.</param>
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
