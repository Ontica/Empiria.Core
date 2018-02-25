/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : FundamentalTypeInfo                              Pattern  : Type metadata class               *
*                                                                                                            *
*  Summary   : Represents a fundamental, built-in type or scalar type definition.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Ontology {

  public sealed class FundamentalTypeInfo : MetaModelType {

    #region Constructors and parsers

    private FundamentalTypeInfo() : base(MetaModelTypeFamily.FundamentalType) {

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
