/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : RuleTypeInfo                                     Pattern  : Type metadata class               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a rule type definition.                                                            *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
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
