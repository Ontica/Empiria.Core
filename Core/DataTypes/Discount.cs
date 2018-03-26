/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : Discount                                         Pattern  : Value Type                        *
*                                                                                                            *
*  Summary   : Value type that handles discount information.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  public class Discount {

    #region Constructors and parsers

    private Discount() {
      this.DiscountType = DiscountType.Empty;
      this.Currency = Currency.Default;
      this.Amount = decimal.Zero;
    }

    static public Discount Parse(DiscountType discountType, decimal amount) {
      var discount = new Discount() {
        DiscountType = discountType,
        Amount = amount
      };

      return discount;
    }

    static public Discount Parse(DiscountType discountType,
                                 Currency currency, decimal amount) {
      var discount = new Discount() {
        DiscountType = discountType,
        Currency = currency,
        Amount = amount
      };

      return discount;
    }

    static public Discount Empty {
      get {
        return Discount.Parse(DiscountType.Empty, Currency.Empty, 0m);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public DiscountType DiscountType {
      get;
      private set;
    }

    public decimal Amount {
      get;
      private set;
    }

    public Currency Currency {
      get;
      private set;
    }

    #endregion Public properties

    #region Operators overloading

    static public Discount operator +(Discount discountA, Discount discountB) {
      return Discount.Parse(discountA.DiscountType,
                            discountA.Amount + discountB.Amount);
    }

    static public Discount operator -(Discount discountA, Discount discountB) {
      return Discount.Parse(discountA.DiscountType,
                            discountA.Amount - discountB.Amount);
    }

    #endregion Operators overloading

  } // class Discount

} // namespace Empiria.DataTypes
