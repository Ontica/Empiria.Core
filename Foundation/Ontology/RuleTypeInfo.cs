/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : RuleTypeInfo                                     Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a rule type definition.                                                            *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  public sealed class RuleTypeInfo : MetaModelType {

    #region Constructors and parsers

    private RuleTypeInfo(int id)
      : base(MetaModelTypeFamily.RuleType, id) {

    }

    private RuleTypeInfo(string name)
      : base(MetaModelTypeFamily.RuleType, name) {

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
