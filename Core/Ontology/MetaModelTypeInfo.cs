/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : MetaModelTypeInfo                                Pattern  : Type metadata class               *
*                                                                                                            *
*  Summary   : Represents a meta-model type definition.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
