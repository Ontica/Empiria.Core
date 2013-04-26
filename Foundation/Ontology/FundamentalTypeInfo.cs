﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : FundamentalTypeInfo                              Pattern  : Type metadata class               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a fundamental, built-in type or scalar type definition.                            *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/

namespace Empiria.Ontology {

  public sealed class FundamentalTypeInfo : MetaModelType {

    #region Constructors and parsers

    private FundamentalTypeInfo(int id)
      : base(MetaModelTypeFamily.FundamentalType, id) {

    }

    private FundamentalTypeInfo(string name)
      : base(MetaModelTypeFamily.FundamentalType, name) {

    }

    static public new FundamentalTypeInfo Parse(int id) {
      return MetaModelType.Parse<FundamentalTypeInfo>(id);
    }

    static public new FundamentalTypeInfo Parse(string name) {
      return MetaModelType.Parse<FundamentalTypeInfo>(name);
    }

    #endregion Constructors and parsers

  } // class FundamentalTypeInfo

} // namespace Empiria.Ontology