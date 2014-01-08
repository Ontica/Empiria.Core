/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : EnumerationTypeInfo                              Pattern  : Type metadata class               *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Sealed class that represents an enumeration type definition.                                  *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
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
