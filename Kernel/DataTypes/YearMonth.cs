/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Kernel.dll                *
*  Type      : YearMonth                                        Pattern  : Static Data Type                  *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Holds a year-month pair value.                                                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

namespace Empiria.DataTypes {

  /// <summary>Holds a year-month pair value.</summary>
  public struct YearMonth {

    #region Constructors and parsers

    public YearMonth(int year, int month) {
      Assertion.Assert(DateTime.MinValue.Year <= year && year <= DateTime.MaxValue.Year,
                       "Year value is out of bounds.");
      Assertion.Assert(1 <= month && month <= 12,
                      "Month value is out of bounds.");

      this.Year = year;
      this.Month = month;
    }

    /// <summary>Returns the year/month value corresponding to the past calendar month.</summary>
    static public YearMonth LastMonth {
      get {
        var lastMonthYear = DateTime.Today.AddMonths(-1);

        return new YearMonth(lastMonthYear.Year, lastMonthYear.Month);
      }
    }

    /// <summary>Returns the year/month value corresponding to the current calendar month.</summary>
    static public YearMonth ThisMonth {
      get {
        var today = DateTime.Today;

        return new YearMonth(today.Year, today.Month);
      }
    }

    /// <summary>Gets the list of all YearMonth pairs between two dates.</summary>
    /// <param name="initialDate">The initial date when the YearMonth list starts.</param>
    /// <param name="endDate">The fianl date when the YearMonth list ends.</param>
    /// <returns>A fixed list of YearMonths values.</returns>
    public static FixedList<YearMonth> GetList(DateTime initialDate, DateTime endDate) {
      var list = new List<YearMonth>();

      for (DateTime date = initialDate; date <= endDate; date = date.AddMonths(1)) {
        var temp = new YearMonth(date.Year, date.Month);
        list.Add(temp);
      }
      return list.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties

    /// <summary>The year value in four digits.</summary>
    public int Year {
      get;
      private set;
    }

    /// <summary>The month value starting with 1 for January and ending with 12 for December.</summary>
    public int Month {
      get;
      private set;
    }

    #endregion Properties

  }  // struct YearMonth

}  // namespace Empiria.DataTypes
