/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Tests                                        Component : Core Data Types                       *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Test class                            *
*  Type     : DateMethodsTests                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Date methods tests                                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

using Empiria.DataTypes;
using Empiria.Json;


namespace Empiria.Tests {

  /// <summary>Date methods tests.</summary>
  public class DateMethodsTests {

    [Theory]
    [InlineData("2017-12-30", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 17, 18, 240, 511 })]
    [InlineData("2018-01-01", new int[] { 0, 1, 2, 3, 77, 78, 79, 80, 1320 })]
    [InlineData("2018-10-23", new int[] { 0, 1, 2, 3, 45, 133, 240, 580, 1200, 1432 })]
    [InlineData("2019-03-03", new int[] { 10, 3570, 7800, 10000, 13750, 45000 })]
    public void MustAddWorkingDays(string dateString, int[] daysToAdd) {
      var calendar = EmpiriaCalendar.Default;

      var baseDate = DateTime.Parse(dateString);

      foreach (var days in daysToAdd) {
        DateTime calculatedDate = calendar.AddWorkingDays(baseDate, days);

        int actual = (int) calculatedDate.Subtract(baseDate).Days;

        int expected = calendar.WorkingDays(baseDate, calculatedDate) +
                       calendar.NonWorkingDays(baseDate, calculatedDate);

        Assert.Equal(expected, actual);

      }
    }



    [Theory]
    [InlineData("2017-12-30", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 17, 18, 240, 511 })]
    [InlineData("2018-01-01", new int[] { 0, 1, 2, 3, 77, 78, 79, 80, 1320 })]
    [InlineData("2018-10-23", new int[] { 0, 1, 2, 3, 45, 133, 240, 580, 1200, 1432 })]
    [InlineData("2019-03-03", new int[] { 10, 3570, 7800, 10000, 13750, 45000 })]
    public void MustSubstractWorkingDays(string dateString, int[] daysToSubstract) {
      var calendar = EmpiriaCalendar.Default;

      var baseDate = DateTime.Parse(dateString);

      foreach (var days in daysToSubstract) {
        DateTime calculatedDate = calendar.SubstractWorkingDays(baseDate, days);

        int calendarDays = (int) baseDate.Subtract(calculatedDate).Days;

        int expected = calendar.WorkingDays(calculatedDate, baseDate) +
                       calendar.NonWorkingDays(calculatedDate, baseDate);

        Assert.Equal(expected, calendarDays);

      }

    }

  }  // DateMethodsTests

}  // namespace Empiria.Tests
