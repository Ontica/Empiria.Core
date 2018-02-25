/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : KeyValue                                         Pattern  : Value Type                        *
*                                                                                                            *
*  Summary   : Key-value immutable value type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.DataTypes {

  /// <summary>Key-value immutable value type.</summary>
  public class KeyValue {

    #region Constructors and parsers

    public KeyValue(string key, string value) {
      Assertion.AssertObject(key, "key");
      Assertion.Assert(value != null, "value");

      this.Key = key;
      this.Value = value;
    }

    static KeyValue Parse(JsonObject json) {
      string key = json.Get<string>("key");
      string value = json.Get<string>("value");

      return new KeyValue(key, value);
    }

    #endregion Constructors and parsers

    #region Properties

    public string Key {
      get;
      private set;
    }

    public string Value {
      get;
      private set;
    }

    #endregion Properties

  }  // class KeyValue

} //namespace Empiria.DataTypes
