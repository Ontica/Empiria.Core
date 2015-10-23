/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : RuleTypeInfo                                     Pattern  : Type metadata class               *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a rule type definition.                                                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  public sealed class RuleTypeInfo : MetaModelType {

    #region Constructors and parsers

    private RuleTypeInfo() : base(MetaModelTypeFamily.RuleType) {

    }

    static public new RuleTypeInfo Parse(int id) {
      return MetaModelType.Parse<RuleTypeInfo>(id);
    }

    static public new RuleTypeInfo Parse(string name) {
      return MetaModelType.Parse<RuleTypeInfo>(name);
    }

    #endregion Constructors and parsers

  } // class RuleTypeInfo

} // namespace Empiria.Ontology
