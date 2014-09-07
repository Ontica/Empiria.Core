/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : Powertype                                        Pattern  : Abstract Class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : A powertype is a an object type whose instances are subtypes of another object type, named    *
*              the partitioned type. Powertypes enables dynamic specialization.                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria {

  /// <summary>A powertype is a an object type whose instances are subtypes of another object type, named 
  ///the partitioned type. Powertypes enables dynamic specialization.</summary>
  public abstract class Powertype<T> : ObjectTypeInfo where T : BaseObject {

    #region Fields

    private PowerTypeInfo powerTypeInfo = null;
    private ObjectTypeInfo partitionedType = null;

    #endregion Fields

    #region Constructors and parsers

    protected Powertype(string powerTypeName, int typeId) : base(typeId) {
      this.powerTypeInfo = PowerTypeInfo.Parse(powerTypeName);
      this.partitionedType = ObjectTypeInfo.Parse(typeId);
    }

    static public new U Parse<U>(int typeId) where U : Powertype<T> {
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse(typeId);
      if (typeInfo is U) {
        return (U) typeInfo;
      } else {
        return ObjectFactory.CreateObject<U>(new Type[] { typeof(int) }, new object[] { typeId });
      }
    }

    static public U Parse<U>(ObjectTypeInfo typeInfo) where U : Powertype<T> {
      if (typeInfo is U) {
        return (U) typeInfo;
      } else {
        return ObjectFactory.CreateObject<U>(new Type[] { typeof(int) }, new object[] { typeInfo.Id });
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public override bool IsPowerType {
      get { return true; }
    }

    public ObjectTypeInfo PartitionedType {
      get { return partitionedType; }
    }

    public PowerTypeInfo PowerTypeInfo {
      get { return powerTypeInfo; }
    }

    #endregion Public properties

    #region Public methods

    protected T CreateInstance() {
      return (T) ObjectFactory.CreateObject(partitionedType.UnderlyingSystemType, 
                                            new Type[] { typeof(ObjectTypeInfo) },
                                            new object[] { partitionedType });
    }

    // TODO: Review usage
    protected FixedList<U> GetLinks<U>(string name) where U : BaseObject {
      return powerTypeInfo.GetLinks<U>(partitionedType, name);
    }

    protected FixedList<U> GetTypeLinks<U>(string linkName) where U : MetaModelType {
      TypeAssociationInfo associationInfo = this.Associations[linkName];

      return associationInfo.GetTypeLinks<U>(this);
    }

    #endregion Public methods

  } // class Powertype

} // namespace Empiria
