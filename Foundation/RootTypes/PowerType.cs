/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : PowerType                                        Pattern  : Abstract Class                    *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : A power type is a an object type whose instances are subtypes of another object type, named   *
*              the partitioned type. Powertyping enables dynamic specialization.                             *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
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

    protected PowerType(string powerTypeName, int typeId)
      : base(typeId) {
      this.powerTypeInfo = PowerTypeInfo.Parse(powerTypeName);
      this.partitionedType = ObjectTypeInfo.Parse(typeId);
    }

    static public new U Parse<U>(int typeId) where U : PowerType<T> {
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse(typeId);
      if (typeInfo is U) {
        return (U) typeInfo;
      } else {
        return ObjectFactory.CreateObject<U>(new Type[] { typeof(int) }, new object[] { typeId });
      }
    }

    static public U Parse<U>(ObjectTypeInfo typeInfo) where U : PowerType<T> {
      if (typeInfo is U) {
        return (U) typeInfo;
      } else {
        return ObjectFactory.CreateObject<U>(new Type[] { typeof(int) }, new object[] { typeInfo.Id });
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public PowerTypeInfo PowerTypeInfo {
      get { return powerTypeInfo; }
    }

    public ObjectTypeInfo PartitionedType {
      get { return partitionedType; }
    }

    #endregion Public properties

    #region Public methods

    public T CreateInstance() {
      return (T) ObjectFactory.CreateObject(partitionedType.UnderlyingSystemType, new Type[] { typeof(string) },
                                            new object[] { partitionedType.Name });
    }

    protected ObjectList<U> GetLinks<U>(string name) where U : BaseObject {
      return powerTypeInfo.GetLinks<U>(partitionedType, name);
    }

    #endregion Public methods

  } // class PowerType

} // namespace Empiria