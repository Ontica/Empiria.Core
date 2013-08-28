/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : StructureTypeInfo                                Pattern  : Type metadata class               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a structure type definition.                                                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Collections.Generic;

namespace Empiria.Ontology {

  public sealed class StructureTypeInfo : MetaModelType {

    #region Constructors and parsers

    private StructureTypeInfo(int id)
      : base(MetaModelTypeFamily.StructureType, id) {

    }

    private StructureTypeInfo(string name)
      : base(MetaModelTypeFamily.StructureType, name) {

    }

    static public new StructureTypeInfo Parse(int id) {
      return MetaModelType.Parse<StructureTypeInfo>(id);
    }

    static public new StructureTypeInfo Parse(string name) {
      return MetaModelType.Parse<StructureTypeInfo>(name);
    }

    #endregion Constructors and parsers

    #region Public properties

    public new IList<TypeAttributeInfo> Attributes {
      get {
        return base.Attributes.Values;
      }
    }

    #endregion Public properties

    #region Public methods

    //public TypeAttributeInfo GetAttributeInfo(int id) {
    //  return base.GetRelationInfo<TypeAttributeInfo>(id);
    //}

    //public TypeAttributeInfo GetAttributeInfo(string name) {
    //  return base.GetRelationInfo<TypeAttributeInfo>(name);
    //}

    #endregion Public methods

  } // class StructureTypeInfo

} // namespace Empiria.Ontology
