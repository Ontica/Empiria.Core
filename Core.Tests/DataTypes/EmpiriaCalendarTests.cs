/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Data Types Tests                             Component : Test cases                            *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Unit tests                            *
*  Type     : EmpiriaCalendarTests                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : EmpiriaCalendar methods tests.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.DataTypes.Time;

namespace Empiria.Tests.DataTypes {

  /// <summary>EmpiriaCalendar methods tests.</summary>
  public class EmpiriaCalendarTests {

    [Theory]
    [InlineData("2017-12-30", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 17, 18, 240, 511 })]
    [InlineData("2018-01-01", new int[] { 0, 1, 2, 3, 77, 78, 79, 80, 1320 })]
    [InlineData("2018-10-23", new int[] { 0, 1, 2, 3, 45, 133, 240, 580, 1200, 1432 })]
    [InlineData("2019-03-03", new int[] { 10, 3570, 7800, 10000, 13750, 45000 })]
    public void Should_Add_Working_Days(string dateString, int[] daysToAdd) {
      var calendar = EmpiriaCalendar.Default;

      var baseDate = DateTime.Parse(dateString);

      foreach (var days in daysToAdd) {
        DateTime calculatedDate = calendar.AddWorkingDays(baseDate, days);

        int actual = calculatedDate.Subtract(baseDate).Days;

        int expected = calendar.WorkingDays(baseDate, calculatedDate) +
                       calendar.NonWorkingDays(baseDate, calculatedDate);

        Assert.Equal(expected, actual);
      }
    }


    [Fact]
    public void Should_Handle_Non_Working_Days_Periods() {
      var calendar = EmpiriaCalendar.Parse("ASEA-UGI");
      Assert.True(calendar.NonWorkingDaysPeriods.Count > 2,
                  "Invalid non working days periods");


      Assert.True(calendar.IsNonWorkingDate(ToDate("2020-05-18")),
                  "'2020-05-18' must be a non-working date.");
    }


    [Fact]
    public void Should_Handle_Non_Working_Days_Exceptions() {
      var calendar = EmpiriaCalendar.Parse("ASEA-UGI");

      Assert.True(calendar.IsWorkingDate(ToDate("2020-05-20")),
                  "'2020-05-20' must be a working date.");
    }


    [Fact]
    public void Should_Read_Stored_Calendar_Holidays() {
      var calendar = EmpiriaCalendar.Parse("ASEA");

      Assert.Contains(calendar.Holidays, x => x.Date.Equals(ToDate("2020-01-01")));
      Assert.Contains(calendar.Holidays, x => x.Date.Equals(ToDate("2020-09-16")));
    }


    [Theory]
    [InlineData("2017-12-30", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 17, 18, 240, 511 })]
    [InlineData("2018-01-01", new int[] { 0, 1, 2, 3, 77, 78, 79, 80, 1320 })]
    [InlineData("2018-10-23", new int[] { 0, 1, 2, 3, 45, 133, 240, 580, 1200, 1432 })]
    [InlineData("2019-03-03", new int[] { 10, 3570, 7800, 10000, 13750, 45000 })]
    public void Should_Substract_Working_Days(string dateString, int[] daysToSubstract) {
      var calendar = EmpiriaCalendar.Default;

      var baseDate = DateTime.Parse(dateString);

      foreach (var days in daysToSubstract) {
        DateTime calculatedDate = calendar.SubstractWorkingDays(baseDate, days);

        int calendarDays = baseDate.Subtract(calculatedDate).Days;

        int expected = calendar.WorkingDays(calculatedDate, baseDate) +
                       calendar.NonWorkingDays(calculatedDate, baseDate);

        Assert.Equal(expected, calendarDays);
      }
    }

    #region Helpers

    private DateTime ToDate(string dateString) {
      return DateTime.Parse(dateString).Date;
    }

    #endregion Helpers

  }  // EmpiriaCalendarTests

}  // namespace Empiria.Tests.DataTypes
