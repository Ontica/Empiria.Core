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

using Empiria.Data;

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

    /// <summary>Returns a fixed list of value instances that are defined as part of the 
    /// ValueTypeInfo configuration.</summary>
    /// <typeparam name="T">The type of the ValueObject instances.</typeparam>
    /// <typeparam name="U">The type of the stored instances.</typeparam>
    /// <param name="constructor">Method to call to build T instances from a U instance</param>
    /// <returns></returns>
    public FixedList<T> GetValuesList<T, U>(Func<U, T> constructor) where T : ValueObject<U> {
      var json = JsonObject.Parse(this.ExtensionData);

      List<U> list = json.GetList<U>("Values");

      List<T> valueObjectsList = list.ConvertAll<T>((x) => constructor.Invoke(x));

      return valueObjectsList.ToFixedList();
    }

    #endregion Public methods

  } // class ValueTypeInfo

} // namespace Empiria.Ontology
