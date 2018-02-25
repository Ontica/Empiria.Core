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

using Empiria.Security;

namespace Empiria.DataTypes {

  public class Discount {

    #region Constructors and parsers

    private Discount() {
      this.DiscountType = DiscountType.Empty;
      this.Currency = Currency.Default;
      this.Amount = decimal.Zero;
      this.Authorization = Authorization.Empty;
    }

    static public Discount Parse(DiscountType discountType, decimal amount) {
      var discount = new Discount();
      discount.DiscountType = discountType;
      discount.Amount = amount;

      return discount;
    }

    static public Discount Parse(DiscountType discountType, decimal amount, int authorizationId) {
      var discount = new Discount();
      discount.DiscountType = discountType;
      discount.Amount = amount;
      discount.Authorization = Authorization.Parse(authorizationId, discount);
      return discount;
    }

    static public Discount Parse(DiscountType discountType, Currency currency, decimal amount) {
      var discount = new Discount();
      discount.DiscountType = discountType;
      discount.Currency = currency;
      discount.Amount = amount;

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
      set;
    }

    public decimal Amount {
      get;
      set;
    }

    public Currency Currency {
      get;
      set;
    }

    public Authorization Authorization {
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

    #region Public methods

    public Authorization Authorize() {
      throw new NotImplementedException();
    }

    #endregion Public methods

  } // class Discount

} // namespace Empiria.DataTypes
