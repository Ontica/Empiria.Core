/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : FundamentalTypeInfo                              Pattern  : Type metadata class               *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a fundamental, built-in type or scalar type definition.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
