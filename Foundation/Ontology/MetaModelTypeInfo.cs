/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : MetaModelTypeInfo                                Pattern  : Type metadata class               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a meta-model type definition.                                                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  public sealed class MetaModelTypeInfo : MetaModelType {

    #region Constructors and parsers

    private MetaModelTypeInfo() : base(MetaModelTypeFamily.MetaModelType) {

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
