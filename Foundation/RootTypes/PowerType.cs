/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : PowerType                                        Pattern  : Abstract Class                    *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : A power type is a an object type whose instances are subtypes of another object type, named   *
*              the partitioned type. Powertyping enables dynamic specialization.                             *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
using System;

using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria {

  /// <summary>A power type is a an object type whose instances are subtypes of another object type, named 
  ///the partitioned type. Powertyping enables dynamic specialization.</summary>
  public abstract class PowerType<T> : ObjectTypeInfo where T : BaseObject {

    #region Fields

    private PowerTypeInfo powerTypeInfo = null;
    private ObjectTypeInfo partitionedType = null;

    #endregion Fields

    #region Constructors and parsers

    //protected internal PowerType(int id)
    //  : base(MetaModelTypeFamily.ObjectType, id) {

    //}

    //protected internal PowerType(string name)
    //  : base(MetaModelTypeFamily.ObjectType, name) {

    //}

    //static public new PowerType Parse(int id) {
    //  return MetaModelType.Parse<ObjectTypeInfo>(id);
    //}

    //static public new PowerType Parse(string name) {
    //  return MetaModelType.Parse<ObjectTypeInfo>(name);
    //}

    #endregion Constructors and parsers

    #region Constructors and parsers

    protected PowerType(string powerTypeName, int typeId)
      : base(typeId) {
      this.powerTypeInfo = PowerTypeInfo.Parse(powerTypeName);
      this.partitionedType = ObjectTypeInfo.Parse(typeId);
    }

    static public new U Parse<U>(int typeId) where U : PowerType<T> {
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse(typeId);
      if (typeInfo is U) {
        //Empiria.Messaging.Publisher.Publish("if (typeInfo is U) on typeid " + typeId);
        return (U) typeInfo;
      } else {
        //Empiria.Messaging.Publisher.Publish("NOT if (typeInfo is U) on typeid " + typeId);
        return ObjectFactory.CreateObject<U>(new Type[] { typeof(int) }, new object[] { typeId });
      }
    }

    static public U Parse<U>(ObjectTypeInfo typeInfo) where U : PowerType<T> {
      if (typeInfo is U) {
        //Empiria.Messaging.Publisher.Publish("if (typeInfo is U) on typeInfo " + typeInfo.Name);
        return (U) typeInfo;
      } else {
        //Empiria.Messaging.Publisher.Publish("NOT if (typeInfo is U) on typeInfo " + typeInfo.Name);
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

    public T CreateInstance() {
      return (T) ObjectFactory.CreateObject(partitionedType.UnderlyingSystemType, 
                                            new Type[] { typeof(string) },
                                            new object[] { partitionedType.Name });
    }

    protected ObjectList<U> GetLinks<U>(string name) where U : BaseObject {
      return powerTypeInfo.GetLinks<U>(partitionedType, name);
    }

    protected ObjectList<U> GetTypeLinks<U>(string linkName) where U : MetaModelType {
      TypeAssociationInfo associationInfo = this.Associations[linkName];

      return associationInfo.GetTypeLinks<U>(this);
    }

    #endregion Public methods

  } // class PowerType

} // namespace Empiria