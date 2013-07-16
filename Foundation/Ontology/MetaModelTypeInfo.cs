/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : MetaModelTypeInfo                                Pattern  : Type metadata class               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a meta-model type definition.                                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

namespace Empiria.Ontology {

  public sealed class MetaModelTypeInfo : MetaModelType {

    #region Constructors and parsers

    private MetaModelTypeInfo(int id)
      : base(MetaModelTypeFamily.MetaModelType, id) {

    }

    private MetaModelTypeInfo(string name)
      : base(MetaModelTypeFamily.MetaModelType, name) {

    }

    static public new MetaModelTypeInfo Parse(int id) {
      return MetaModelType.Parse<MetaModelTypeInfo>(id);
    }

    static public new MetaModelTypeInfo Parse(string name) {
      return MetaModelType.Parse<MetaModelTypeInfo>(name);
    }

    #endregion Constructors and parsers

  } // class MetaModelTypeInfo

} // namespace Empiria.Ontology