/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Data Types                      Component : Unit tests                            *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Test class                            *
*  Type     : EmpiriaCalendarTests                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : EmpiriaCalendar data type configuration loading and methods tests.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;


namespace Empiria.DataTypes.Tests {

  /// <summary>EmpiriaCalendar data type configuration loading and methods tests.</summary>
  public class EmpiriaCalendarTests {

    [Fact]
    public void ShouldReadStoredCalendarHolidays() {
      var calendar = EmpiriaCalendar.Parse("ASEA");

      Assert.Contains(calendar.Holidays, x => x.Date.Equals(ToDate("2020-01-01")));
      Assert.Contains(calendar.Holidays, x => x.Date.Equals(ToDate("2020-09-16")));
    }


    [Fact]
    public void ShouldHandleNonWorkingDaysPeriods() {
      var calendar = EmpiriaCalendar.Parse("ASEA-UGI");
      Assert.True(calendar.NonWorkingDaysPeriods.Count > 2,
                  "Invalid non working days periods");


      Assert.True(calendar.IsNonWorkingDate(ToDate("2020-05-18")),
                  "'2020-05-18' must be a non-working date.");
    }


    [Fact]
    public void ShouldHandleNonWorkingDaysExceptions() {
      var calendar = EmpiriaCalendar.Parse("ASEA-UGI");

      Assert.True(calendar.IsWorkingDate(ToDate("2020-05-20")),
                  "'2020-05-20' must be a working date.");
    }


    private DateTime ToDate(string dateString) {
      return DateTime.Parse(dateString).Date;
    }


  }  // EmpiriaCalendarTests

}  // namespace Empiria.Tests
