/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : Money                                            Pattern  : Value Type                        *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Value type that handles money operations.                                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

namespace Empiria.DataTypes {

  public struct Money {

    #region Fields

    private Currency currency;
    private decimal amount;

    #endregion Fields

    #region Constructors and parsers

    private Money(Money money) {
      this.currency = money.Currency;
      this.amount = money.Amount;
    }

    private Money(Currency currency, decimal amount) {
      this.currency = currency;
      this.amount = amount;
    }

    static public Money Parse(decimal amount) {
      return new Money(Currency.Default, amount);
    }

    static public Money Parse(Currency currency, decimal amount) {
      return new Money(currency, amount);
    }

    static public Money Empty {
      get { return new Money(Currency.Empty, 0m); }
    }

    static public Money Unknown {
      get { return new Money(Currency.Unknown, 0m); }
    }

    static public Money NoLegible {
      get { return new Money(Currency.NoLegible, 0m); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public decimal Amount {
      get { return amount; }
    }

    public Currency Currency {
      get {
        if (currency == null) {
          currency = Currency.Default;
        }
        return currency;
      }
    }

    #endregion Public properties

    #region Operators overloading

    static public Money operator +(Money moneyA, Money moneyB) {
      Money temp = new Money(moneyA);
      temp.amount += moneyB.Amount;

      return temp;
    }

    static public Money operator -(Money moneyA, Money moneyB) {
      Money temp = new Money(moneyA);
      temp.amount -= moneyB.Amount;

      return temp;
    }

    static public Money operator *(Money money, decimal scalar) {
      Money temp = new Money(money);
      temp.amount *= scalar;

      return temp;
    }

    static public Money operator *(decimal scalar, Money money) {
      Money temp = new Money(money);
      temp.amount *= scalar;

      return temp;
    }

    static public Money operator /(Money moneyA, decimal scalar) {
      Money temp = new Money(moneyA);
      temp.amount /= scalar;

      return temp;
    }

    static public bool operator ==(Money moneyA, Money moneyB) {
      return (moneyA.Currency.Equals(moneyB.Currency) && (moneyA.Amount == moneyB.Amount));
    }

    static public bool operator !=(Money moneyA, Money moneyB) {
      return !(moneyA == moneyB);
    }

    #endregion Operators overloading

    #region Public methods

    public override bool Equals(object o) {
      if (!(o is Money)) {
        return false;
      }
      Money temp = (Money) o;

      return (this.Currency.Equals(temp.Currency) && (this.Amount == temp.Amount));
    }

    public override int GetHashCode() {
      return (currency.GetHashCode() ^ amount.GetHashCode());
    }

    public override string ToString() {
      if (this.currency.Equals(Currency.Default)) {
        return amount.ToString("C2");
      } else if (this.currency.Equals(Currency.Empty)) {
        return amount.ToString();
      } else if (this.currency.Equals(Currency.NoLegible)) {
        return amount.ToString();
      } else {
        return amount.ToString("0,###.00##" + " " + currency.Abbreviation);
      }
    }

    #endregion Public methods

  } // class Money

} // namespace Empiria.DataTypes