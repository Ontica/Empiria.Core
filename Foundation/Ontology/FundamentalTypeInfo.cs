/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : FundamentalTypeInfo                              Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a fundamental, built-in type or scalar type definition.                            *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

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