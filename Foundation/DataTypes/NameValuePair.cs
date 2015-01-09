/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : NameValuePair                                    Pattern  : Value object                      *
*  Version   : 2.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : String Name-Value pair value object.                                                          *
*                                                                                                            *
********************************* Copyright (c) 2009-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Json;

namespace Empiria {

  public interface IJsonParseable {

  }

}
namespace Empiria.DataTypes {

  /// <summary>String Name-Value pair value object.</summary>
  public struct NameValuePair : IJsonParseable {

    #region Fields

    private string name;
    private string value;

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

    static public NameValuePair Parse(JsonRoot jsonData) {
      Assertion.AssertObject(jsonData, "jsonData");

      return new NameValuePair(jsonData.GetItem<string>("Name"), 
                               jsonData.GetItem<string>("Value"));
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
