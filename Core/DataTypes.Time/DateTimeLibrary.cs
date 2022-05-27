/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Static Data Type                        *
*  Type     : DateTimeLibrary                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static methods library that performs datetime operations.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Globalization;

namespace Empiria.DataTypes.Time {

  /// <summary>Static methods library that performs datetime operations.</summary>
  static public class DateTimeLibrary {

    #region Public methods

    static public int DaysTo(this DateTime fromDate, DateTime toDate) {
      if (fromDate <= toDate) {
        return toDate.Date.Subtract(fromDate.Date).Days;
      } else {
        return fromDate.Date.Subtract(toDate.Date).Days;
      }
    }

    static public int HoursTo(DateTime fromDate, DateTime toDate) {
      if (fromDate <= toDate) {
        return toDate.Subtract(fromDate).Hours;
      } else {
        return fromDate.Subtract(toDate).Hours;
      }
    }

    static public bool IsDateTime(string source, string format) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }

        DateTime.ParseExact(source, format, DateTimeFormatInfo.InvariantInfo);

        return true;

      } catch {
        return false;
      }
    }

    static public bool IsWeekendDate(this DateTime date) {
      return IsWeekendDate(date, EmpiriaCalendar.Default);
    }

    static public bool IsWeekendDate(this DateTime date, EmpiriaCalendar calendar) {
      return calendar.IsWeekendDay(date);
    }

    static public bool IsNonWorkingDate(this DateTime date) {
      return IsNonWorkingDate(date, EmpiriaCalendar.Default);
    }

    static public bool IsNonWorkingDate(this DateTime date, EmpiriaCalendar calendar) {
      Assertion.Require(calendar, "calendar");

      return (calendar.IsWeekendDay(date) || calendar.IsHoliday(date));
    }


    static public DateTime ToDateTime(string source, string format) {
      try {
        return DateTime.ParseExact(source, format, DateTimeFormatInfo.InvariantInfo);
      } catch {
        throw new Exception("No reconozco el valor de " + source + " como del tipo de datos fecha.");
      }
    }

    #endregion Public methods

  }  // class DateTimeLibrary

} // namespace Empiria.DataTypes.Time
