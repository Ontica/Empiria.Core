/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : EnumerationTypeInfo                              Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Sealed class that represents an enumeration type definition.                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Ontology {

  /// <summary>Sealed class that represents an enumeration type definition.</summary>
  public sealed class EnumerationTypeInfo : MetaModelType {

    #region Constructors and parsers

    private EnumerationTypeInfo(int id)
      : base(MetaModelTypeFamily.EnumerationType, id) {

    }

    private EnumerationTypeInfo(string name)
      : base(MetaModelTypeFamily.EnumerationType, name) {

    }

    static public new EnumerationTypeInfo Parse(int id) {
      return MetaModelType.Parse<EnumerationTypeInfo>(id);
    }

    static public new EnumerationTypeInfo Parse(string name) {
      return MetaModelType.Parse<EnumerationTypeInfo>(name);
    }

    #endregion Constructors and parsers

  } // class EnumerationTypeInfo

} // namespace Empiria.Ontology
