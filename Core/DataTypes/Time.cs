/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                              Component : Date Time                             *
*  Assembly : Empiria.Core.dll                             Pattern   : Methods library                       *
*  Type     : Time                                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Methods library used to manipulate time values as strings.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Methods library used to manipulate time values as strings.</summary>
  static public class Time {

    public static DateTime SetTimeToDate(DateTime dateTime, string timeString) {
      if (String.IsNullOrWhiteSpace(timeString)) {
        return dateTime.Date;
      }

      if (TimeSpan.TryParse(timeString, out var result)) {
        return dateTime.Date.Add(result);
      } else {
        throw new ValidationException("TimeString.Value",
                                      $"The time string has a wrong value: '{timeString}'.");
      }
    }

    static public string ToString(DateTime dateTime, TimeType timeType) {
      if (dateTime.TimeOfDay.TotalSeconds == 0) {
        return String.Empty;
      }

      if (timeType == TimeType.HoursMinutes) {
        return dateTime.TimeOfDay.ToString(@"hh\:mm");

      } else if (timeType == TimeType.HoursMinutesSeconds) {
        return dateTime.TimeOfDay.ToString(@"hh\:mm\:ss");

      } else {
        throw Assertion.AssertNoReachThisCode("Unhandled timeType value.");
      }
    }

  }  // class Time

}  // namespace Empiria.DataTypes
