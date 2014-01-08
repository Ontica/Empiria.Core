/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Kernel.dll                *
*  Type      : Date                                             Pattern  : Static Data Type                  *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Performs datetime data operations.                                                            *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
using System;
using System.Globalization;

namespace Empiria.DataTypes {

  /// <summary>Performs datetime data operations.</summary>
  static public class Calendar {

    #region Public methods

    static public int DaysBetween(DateTime firstDate, DateTime lastDate) {
      return lastDate.Date.Subtract(firstDate.Date).Days;
    }

    static public int HoursBetween(DateTime firstTime, DateTime lastTime) {
      return firstTime.Subtract(lastTime.Date).Hours;
    }

    static public bool IsWeekendDate(DateTime date) {
      return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
    }

    static public bool IsNoLabourDate(DateTime date) {
      if (date.DayOfYear == 1) {
        return true;
      }
      if (date.Month == 12 && date.Day == 25) {
        return true;
      }
      if (date.Month == 9 && date.Day == 16) {
        return true;
      }
      return false;
    }

    static public bool IsDateTime(string source, string format) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        DateTime temp = DateTime.ParseExact(source, format, DateTimeFormatInfo.InvariantInfo);
        return true;
      } catch {
        return false;
      }
    }

    static public DateTime ToDateTime(string source, string format) {
      try {
        return DateTime.ParseExact(source, format, DateTimeFormatInfo.InvariantInfo);
      } catch {
        throw new Exception("No reconozco el valor de " + source + " como del tipo de datos fecha.");
      }
    }

    #endregion Public methods

  }  // class Date

} // namespace Empiria.DataTypes
