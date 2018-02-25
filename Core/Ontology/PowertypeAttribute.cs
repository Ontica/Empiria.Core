/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : PowertypeAttribute                               Pattern  : Attribute class                   *
*                                                                                                            *
*  Summary   : Specifies the partitioned type for a Powertype type.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Ontology {

  /// <summary>Specifies the partitioned type for a Powertype type. This attribute should be
  /// another type (partitioned type).</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class PowertypeAttribute : Attribute {

    /// <summary>Initializes a new instance of the <see cref="PartitionedTypeAttribute"/> class.</summary>
    /// <param name="partitionedType">The partitioned type associated to this powertype type.</param>
    public PowertypeAttribute(Type partitionedType) {
      Assertion.AssertObject(partitionedType, "partitionedType");
      this.PartitionedType = partitionedType;
    }

    /// <summary>The type of the partitioned type.</summary>
    public Type PartitionedType {
      get;
      set;
    }

  }  // class PowertypeAttribute

}  // namespace Empiria.Ontology
