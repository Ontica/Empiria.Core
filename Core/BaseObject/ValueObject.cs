/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : ValueObject                                      Pattern  : Abstract Class                    *
*                                                                                                            *
*  Summary   : Wrapper for value type instances that can be C# value types or user defined immutable types.  *
*              For user defined immutable types, T should implement Equality operators, GetHashCode() and    *
*              ToString() methods in order to complaint with the value types design pattern.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Wrapper for value type instances that can be C# value types or user defined immutable types.
  /// For user defined immutable types, T should implement Equality operators, GetHashCode() and
  /// ToString() methods in order to complaint with the value types design pattern.</summary>
  /// <typeparam name="T">The type of the wrapped value type instance.</typeparam>
  public abstract class ValueObject<T> : IValueObject<T> {

    #region Fields

    private readonly T value;

    private bool isEmptyValue = false;
    private bool isUnknownValue = false;

    #endregion Fields

    #region Constructors and parsers

    protected ValueObject(T value) {
      Assertion.Require(value, "value");

      this.value = value;
    }

    #endregion Constructors and parsers

    #region Public properties

    public bool IsEmptyValue {
      get { return this.isEmptyValue; }
    }

    protected bool IsUnknownValue {
      get { return this.isUnknownValue; }
    }

    public T Value {
      get {
        return (this.value);
      }
    }

    #endregion Public properties

    #region Operators overloading

    static public bool operator ==(ValueObject<T> object1, ValueObject<T> object2) {
      return (object1.Equals(object2));
    }

    static public bool operator !=(ValueObject<T> object1, ValueObject<T> object2) {
      return (!object1.Equals(object2));
    }

    public override bool Equals(object obj) {
      if ((obj == null) || (obj.GetType() != this.GetType())) {
        return false;
      }
      return this.Equals((ValueObject<T>) obj);
    }

    public bool Equals(ValueObject<T> obj) {
      if (obj == null) {
        return false;
      }
      return (this.GetType() == obj.GetType()) &&
             (this.Value.ToString() == obj.Value.ToString());
    }

    public override int GetHashCode() {
      return this.Value.GetHashCode();
    }

    public override string ToString() {
      return value.ToString();
    }

    #endregion Operators overloading

    #region Public methods

    protected void MarkAsEmpty() {
      this.isEmptyValue = true;
    }

    protected void MarkAsUnknown() {
      this.isUnknownValue = true;
    }

    #endregion Public methods

  } // class ValueObject

} // namespace Empiria
