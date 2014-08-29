/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : ValueTypeInfo                                    Pattern  : Type metadata class               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Metatype used to describe ValueObject types.                                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria.Ontology {

  /// <summary>Metatype used to describe ValueObject types.</summary>
  public sealed class ValueTypeInfo : MetaModelType {

    #region Constructors and parsers

    private ValueTypeInfo(int id) : base(MetaModelTypeFamily.ValueType, id) {

    }

    private ValueTypeInfo(string name) : base(MetaModelTypeFamily.ValueType, name) {

    }

    static public new ValueTypeInfo Parse(int id) {
      return MetaModelType.Parse<ValueTypeInfo>(id);
    }

    static public new ValueTypeInfo Parse(string name) {
      return MetaModelType.Parse<ValueTypeInfo>(name);
    }

    #endregion Constructors and parsers

    #region Public properties

    public new IList<TypeAttributeInfo> Attributes {
      get {
        return base.Attributes.Values;
      }
    }

    #endregion Public properties

    #region Public methods

    //public TypeAttributeInfo GetAttributeInfo(int id) {
    //  return base.GetRelationInfo<TypeAttributeInfo>(id);
    //}

    //public TypeAttributeInfo GetAttributeInfo(string name) {
    //  return base.GetRelationInfo<TypeAttributeInfo>(name);
    //}

    #endregion Public methods

  } // class ValueTypeInfo

} // namespace Empiria.Ontology
