/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : NameValuePair                                    Pattern  : Value object                      *
*                                                                                                            *
*  Summary   : String Name-Value pair value object.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.DataTypes {

  /// <summary>String Name-Value pair value object.</summary>
  public struct NameValuePair {

    #region Fields

    private readonly string name;
    private readonly string value;

    #endregion Fields

    #region Constructors and parsers

    private NameValuePair(string name, string value) {
      this.name = name;
      this.value = value;
    }

    static public NameValuePair Parse(string name, string value) {
      Assertion.AssertObject(name, "name");
      Assertion.AssertObject(value, "value");

      return new NameValuePair(name, value);
    }

    static public NameValuePair Parse(JsonObject json) {
      Assertion.AssertObject(json, "json");

      return new NameValuePair(json.Get<string>("Name"), json.Get<string>("Value"));
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Name {
      get { return name; }
    }

    public string Value {
      get {
        return value;
      }
    }

    #endregion Public properties

    #region Public methods

    public override bool Equals(object o) {
      if (!(o is NameValuePair)) {
        return false;
      }
      NameValuePair temp = (NameValuePair) o;

      return (this.Name == temp.Name) && (this.Value == temp.Value);
    }

    public override int GetHashCode() {
      return (this.Name + this.Value).GetHashCode();
    }

    public override string ToString() {
      return this.Name + " [" + this.Value + "]";
    }

    #endregion Public methods

  } // struct NameValuePair

} // namespace Empiria.DataTypes
