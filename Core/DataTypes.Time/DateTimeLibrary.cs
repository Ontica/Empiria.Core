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

    #endregion Public methods

  }  // class DateTimeLibrary

} // namespace Empiria.DataTypes.Time
