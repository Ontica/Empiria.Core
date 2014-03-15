/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : MetaModelTypeInfo                                Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a meta-model type definition.                                                      *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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