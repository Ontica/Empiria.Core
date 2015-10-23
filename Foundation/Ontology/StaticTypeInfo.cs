/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : StaticTypeInfo                                   Pattern  : Type metadata class               *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a static type used for general purpose method execution.                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
