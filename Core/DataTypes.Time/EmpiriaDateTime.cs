/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Static Methods Library                  *
*  Type     : EmpiriaDateTime                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static methods library that performs date time operations.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Globalization;

namespace Empiria {

  /// <summary>Static methods library that performs date time operations.</summary>
  static public class EmpiriaDateTime {

    #region Properties

    static public DateTime NowWithoutSeconds {
      get {
        var now = DateTime.Now;

        return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
      }
    }

    #endregion Properties

    #region Methods

    static public int DaysTo(DateTime fromDate, DateTime toDate) {
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


    static public DateTime ToDateTime(string source, string format) {
      try {
        return DateTime.ParseExact(source, format, DateTimeFormatInfo.InvariantInfo);
      } catch {
        throw new Exception("No reconozco el valor de " + source + " como del tipo de datos fecha.");
      }
    }

    #endregion Methods

  }  // class EmpiriaDateTime

} // namespace Empiria
