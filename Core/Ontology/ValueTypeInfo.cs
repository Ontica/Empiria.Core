/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : ValueTypeInfo                                    Pattern  : Type metadata class               *
*                                                                                                            *
*  Summary   : Metatype used to describe ValueObject types.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;

namespace Empiria.Ontology {

  /// <summary>Metatype used to describe ValueObject types.</summary>
  public sealed class ValueTypeInfo : MetaModelType {

    #region Constructors and parsers

    private ValueTypeInfo() : base(MetaModelTypeFamily.ValueType) {

    }

    static public new ValueTypeInfo Parse(int id) {
      return MetaModelType.Parse<ValueTypeInfo>(id);
    }

    static public new ValueTypeInfo Parse(string name) {
      return MetaModelType.Parse<ValueTypeInfo>(name);
    }

    #endregion Constructors and parsers

    #region Public methods

    /// <summary>Returns a fixed list of value instances that are defined as part of the
    /// ValueTypeInfo configuration.</summary>
    /// <typeparam name="T">The type of the ValueObject instances.</typeparam>
    /// <typeparam name="U">The type of the stored instances.</typeparam>
    /// <param name="constructor">Method to call to build T instances from a U instance</param>
    /// <returns></returns>
    public FixedList<T> GetValuesList<T, U>(Func<U, T> constructor) where T : ValueObject<U> {
      var json = this.ExtensionData;

      List<U> list = json.GetList<U>("Values");

      List<T> valueObjectsList = list.ConvertAll<T>((x) => constructor.Invoke(x));

      return valueObjectsList.ToFixedList();
    }

    #endregion Public methods

  } // class ValueTypeInfo

} // namespace Empiria.Ontology
