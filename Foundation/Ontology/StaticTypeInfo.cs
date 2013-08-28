﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : StaticTypeInfo                                   Pattern  : Type metadata class               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a static type used for general purpose method execution.                           *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

namespace Empiria.Ontology {

  /// <summary>Represents a static type used for general purpose method execution.</summary>
  public sealed class StaticTypeInfo : MetaModelType {

    #region Fields

    #endregion Fields

    #region Constructors and parsers

    private StaticTypeInfo(int id)
      : base(MetaModelTypeFamily.StaticType, id) {

    }

    private StaticTypeInfo(string name)
      : base(MetaModelTypeFamily.StaticType, name) {

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
      TypeMethodInfo[] array = new TypeMethodInfo[base.Methods.Count];

      base.Methods.Values.CopyTo(array, 0);

      return array;
    }

    #endregion Public properties

  } // class StaticTypeInfo

} // namespace Empiria.Ontology
