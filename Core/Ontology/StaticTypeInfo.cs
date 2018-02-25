/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : StaticTypeInfo                                   Pattern  : Type metadata class               *
*                                                                                                            *
*  Summary   : Represents a static type used for general purpose method execution.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Ontology {

  /// <summary>Represents a static type used for general purpose method execution.</summary>
  public sealed class StaticTypeInfo : MetaModelType {

    #region Fields

    #endregion Fields

    #region Constructors and parsers

    private StaticTypeInfo() : base(MetaModelTypeFamily.StaticType) {

    }

    static public new StaticTypeInfo Parse(int id) {
      return MetaModelType.Parse<StaticTypeInfo>(id);
    }

    static public new StaticTypeInfo Parse(string name) {
      return MetaModelType.Parse<StaticTypeInfo>(name);
    }

    #endregion Constructors and parsers

    #region Public properties

    public TypeMethodInfo[] GetMethods() {
      return base.Methods;
    }

    #endregion Public properties

  } // class StaticTypeInfo

} // namespace Empiria.Ontology
