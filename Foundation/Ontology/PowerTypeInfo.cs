/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : PowerTypeInfo                                    Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a power type definition.                                                           *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  public sealed class PowerTypeInfo : MetaModelType {

    #region Constructors and parsers

    private PowerTypeInfo(int id)
      : base(MetaModelTypeFamily.PowerType, id) {

    }

    private PowerTypeInfo(string name)
      : base(MetaModelTypeFamily.PowerType, name) {

    }

    static public new PowerTypeInfo Parse(int id) {
      return MetaModelType.Parse<PowerTypeInfo>(id);
    }

    static public new PowerTypeInfo Parse(string name) {
      return MetaModelType.Parse<PowerTypeInfo>(name);
    }

    #endregion Constructors and parsers

    #region Public methods

    //public T GetLink<T>(ObjectTypeInfo partitionedType,
    //                    string name, T defaultValue = null) where T : BaseObject {
    //  TypeAssociationInfo association = base.GetRelation<TypeAssociationInfo>(name);

    //  return association.GetLink<T>(partitionedType, defaultValue);
    //}

    public ObjectList<T> GetLinks<T>(ObjectTypeInfo partitionedType, string name) where T : BaseObject {
      TypeAssociationInfo association = base.Associations[name];

      return association.GetLinks<T>(partitionedType);
    }

    public TypeMethodInfo[] GetMethods() {
      TypeMethodInfo[] array = new TypeMethodInfo[base.Methods.Count];

      base.Methods.Values.CopyTo(array, 0);

      return array;
    }

    #endregion Public methods

  } // class PowerTypeInfo

} // namespace Empiria.Ontology