/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : StructureTypeInfo                                Pattern  : Type metadata class               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a structure type definition.                                                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

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

  } // class StructureTypeInfo

} // namespace Empiria.Ontology
