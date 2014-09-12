/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.dll                       *
*  Type      : Interest                                         Pattern  : Value Type                        *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Contains data about a financial interest rate and term.                                       *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Contains data about a financial interest rate and term.</summary>
  public class Interest : IExtensibleData {

    #region Constructors and parsers

    public Interest() {
      this.Rate = 0.0000m;
      this.RateType = InterestRateType.Empty;
      this.TermPeriods = 0;
      this.TermUnit = Unit.Empty;
    }

    static public Interest Parse(Empiria.Data.JsonObject json) {
      var interest = new Interest();

      /// OOJJOO. Try to send all data read/write methods to isolated types ???      
      // interest = DataReader.GetInterestData(json);

      interest.TermPeriods = json.Get<Int32>("TermPeriods", interest.TermPeriods);
      interest.TermUnit = json.Get<Unit>("TermUnitId", interest.TermUnit);
      interest.Rate = json.Get<Decimal>("InterestRate", interest.Rate);
      interest.RateType = json.Get<InterestRateType>("InterestRateTypeId", interest.RateType);

      return interest;
    }

    static private readonly Interest _empty = new Interest() {
      IsEmptyInstance = true
    };

    static public Interest Empty {
      get {
        return _empty;
      }
    }

    #endregion Constructors and parsers

    #region Properties

    public bool IsEmptyInstance {
      get;
      private set;
    }

    public decimal Rate {
      get;
      set;
    }

    public InterestRateType RateType {
      get; 
      set;
    }

    public int TermPeriods {
      get; 
      set;
    }

    public Unit TermUnit {
      get; 
      set;
    }

    #endregion Properties

    #region Methods
    
    public string ToJson() {
      return Empiria.Data.JsonConverter.ToJson(this);
    }

    #endregion Methods

  }  // class Interest

} // namespace Empiria.DataTypes
