/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : ValueObject                                      Pattern  : Abstract Class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Wrapper for value type instances that can be C# value types or user defined immutable types.  *
*              For user defined immutable types, T should implement Equality operators, GetHashCode() and    *
*              ToString() methods in order to complaint with the value types design pattern.                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      Assertion.AssertObject(value, "value");

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
      return (object1.Value.ToString() == object2.Value.ToString());
    }

    static public bool operator !=(ValueObject<T> object1, ValueObject<T> object2) {
      return (object1.Value.ToString() != object2.Value.ToString());
    }

    public override bool Equals(object obj) {
      if ((obj == null) || (obj.GetType() != this.GetType())) {
        return false;
      }
      return this == ((ValueObject<T>) obj);
    }

    public bool Equals(ValueObject<T> obj) {
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
