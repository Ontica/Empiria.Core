﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : PowertypeAttribute                               Pattern  : Attribute class                   *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Specifies the partitioned type for a Powertype type.                                          *
*                                                                                                            *
********************************* Copyright (c) 2014-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  /// <summary>Specifies the partitioned type for a Powertype type. This attribute should be 
  /// another type (partitioned type).</summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class PowertypeAttribute : Attribute {

    /// <summary>Initializes a new instance of the <see cref="PartitionedTypeAttribute"/> class.</summary>
    /// <param name="name">The type of the powertype type associated to this partitioned type.</param>
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
