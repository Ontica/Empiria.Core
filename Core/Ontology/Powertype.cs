/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : Powertype                                        Pattern  : Power type                        *
*                                                                                                            *
*  Summary   : A powertype is a an object type whose instances are subtypes of another object type, named    *
*              the partitioned type. Powertypes enable dynamic specialization. All descendents of this type  *
*              must be decorated with the PowerType attribute.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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

    #region Public methods

    /// <summary>Returns true if this powertype instance is subtype of T.</summary>
    public bool IsSubtypeOf<T>() where T : class {
      Type type = this.UnderlyingSystemType;

      return typeof(T) == type;
    }

    #endregion Public methods

  } // class Powertype

} // namespace Empiria.Ontology
