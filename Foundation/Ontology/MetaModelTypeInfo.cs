/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : MetaModelTypeInfo                                Pattern  : Type metadata class               *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a meta-model type definition.                                                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
