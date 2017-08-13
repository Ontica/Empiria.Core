/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : Quantity                                         Pattern  : Value Type                        *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Value type that handles quantity data, a pair unit-amount data type.                          *
*                                                                                                            *
********************************* Copyright (c) 1999-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Value type that handles quantity data, a pair unit-amount data type.</summary>
  public struct Quantity {

    #region Fields

    private Unit unit;
    private decimal amount;

    #endregion Fields

    #region Constructors and parsers

    private Quantity(Quantity quantity) {
      this.unit = quantity.Unit;
      this.amount = quantity.Amount;
    }

    private Quantity(Unit unit, decimal amount) {
      this.unit = unit;
      this.amount = amount;
    }

    static public Quantity Parse(Unit unit, decimal amount) {
      return new Quantity(unit, amount);
    }

    public static Quantity One {
      get {
        return new Quantity(Unit.Empty, decimal.One);
      }
    }

    public static Quantity Zero {
      get {
        return new Quantity(Unit.Empty, decimal.Zero);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public decimal Amount {
      get { return amount; }
    }

    public Unit Unit {
      get {
        return unit;
      }
    }

    #endregion Public properties

    #region Operators overloading

    static public Quantity operator +(Quantity quantityA, Quantity quantityB) {
      Quantity temp = new Quantity(quantityA);

      temp.amount += quantityB.Amount;

      return temp;
    }

    static public Quantity operator -(Quantity quantityA, Quantity quantityB) {
      Quantity temp = new Quantity(quantityA);

      temp.amount -= quantityB.Amount;

      return temp;
    }

    static public Quantity operator *(Quantity quantity, decimal scalar) {
      Quantity temp = new Quantity(quantity);

      temp.amount *= scalar;

      return temp;
    }

    static public Quantity operator *(decimal scalar, Quantity quantity) {
      Quantity temp = new Quantity(quantity);

      temp.amount *= scalar;

      return temp;
    }

    static public Quantity operator /(Quantity quantity, decimal scalar) {
      Quantity temp = new Quantity(quantity);

      temp.amount /= scalar;

      return temp;
    }

    static public bool operator ==(Quantity quantityA, Quantity quantityB) {
      return (quantityA.Unit.Equals(quantityB.Unit) && (quantityA.Amount == quantityB.Amount));
    }

    static public bool operator !=(Quantity quantityA, Quantity quantityB) {
      return !(quantityA == quantityB);
    }

    #endregion Operators overloading

    #region Public methods

    public override bool Equals(object o) {
      if (!(o is Quantity)) {
        return false;
      }
      Quantity temp = (Quantity) o;

      return (this.Unit.Equals(temp.Unit) && (this.Amount == temp.Amount));
    }

    public override int GetHashCode() {
      return (unit.GetHashCode() ^ amount.GetHashCode());
    }

    public override string ToString() {
      return EmpiriaString.TrimAll(amount.ToString("#,#.00#") + " " + unit.Abbr);
    }

    #endregion Public methods

  } // struct Quantity

} // namespace Empiria.Quantity
