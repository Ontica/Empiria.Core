/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : RuleTypeInfo                                     Pattern  : Type metadata class               *
*                                                                                                            *
*  Summary   : Represents a rule type definition.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
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
