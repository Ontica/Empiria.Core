﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : Powertype                                        Pattern  : Power type                        *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : A powertype is a an object type whose instances are subtypes of another object type, named    *
*              the partitioned type. Powertypes enable dynamic specialization. All descendents of this type  *
*              must be decorated with the PowerType attribute.                                               *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  /// <summary>A powertype is a an object type whose instances are subtypes of another object type, named 
  /// the partitioned type. Powertypes enable dynamic specialization. All descendents of this type must
  /// be decorated with the PowerType attribute.</summary>
  public abstract class Powertype : ObjectTypeInfo {

    #region Constructors and parsers

    protected Powertype() {
      // Empiria powertype types always have this constructor.

    }

    #endregion Constructors and parsers

    #region Public properties

    public sealed override bool IsPowertype {
      get {
        return true;
      }
    }

    private Type _partitionedType = null;
    public Type PartitionedType {
      get {
        if (_partitionedType == null) {
          var attribute = (PowertypeAttribute) Attribute.GetCustomAttribute(this.GetType(),
                                                                            typeof(PowertypeAttribute));
          Assertion.AssertObject(attribute, "attribute");

          _partitionedType = attribute.PartitionedType;
        }
        return _partitionedType;
      }
    }

    #endregion Public properties

  } // class Powertype

} // namespace Empiria.Ontology