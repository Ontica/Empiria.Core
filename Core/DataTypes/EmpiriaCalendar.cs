/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaCalendar                                  Pattern  : Storage Item Class                *
*                                                                                                            *
*  Summary   : Provides calendar operations mainly for working days.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.DataTypes {

  /// <summary>Provides calendar operations mainly for working days.</summary>
  public class EmpiriaCalendar {

    #region Constructors and parsers

    public EmpiriaCalendar(JsonObject json) {
      this.Holidays = json.GetList<DateTime>("holidays")
                          .ToFixedList();
    }


    static public EmpiriaCalendar Parse(string calendarName) {
      Assertion.AssertObject(calendarName, "calendarName");

      var storedJson = StoredJson.Parse("System.Calendars");

      JsonObject json = storedJson.Value.Slice(calendarName);

      return new EmpiriaCalendar(json);
    }


    static public EmpiriaCalendar Default {
      get {
        var calendarName = ConfigurationData.Get<string>("Default.Calendar.Name", "Default");

        return EmpiriaCalendar.Parse(calendarName);
      }
    }


    #endregion Constructors and parsers

    #region Public properties

    public FixedList<DateTime> Holidays {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods


    public DateTime AddWorkingDays(DateTime date, int days) {
      Assertion.Assert(days >= 0, "'days' parameter must be a non-negative number.");

      int workingDaysCounter = 0;
      DateTime datePointer = date.Date;

      while (true) {

        if (workingDaysCounter == days) {
          return datePointer;
        } else {
          datePointer = datePointer.AddDays(1);
        }

        if (IsWorkingDate(datePointer)) {
          workingDaysCounter++;
        }

      }  // while

    }


    public bool IsHoliday(DateTime date) {
      return this.Holidays.Contains(date.Date);
    }


    public bool IsNonWorkingDate(DateTime date) {
      return (this.IsWeekendDay(date) || this.IsHoliday(date));
    }


    public bool IsWeekendDay(DateTime date) {
      return (date.DayOfWeek == DayOfWeek.Saturday ||
              date.DayOfWeek == DayOfWeek.Sunday);
    }


    public bool IsWorkingDate(DateTime date) {
      return !this.IsNonWorkingDate(date);
    }


    public DateTime LastWorkingDate(DateTime date, bool includeDate = false) {
      date = date.Date;

      if (!includeDate) {
        date = date.AddDays(-1);
      }

      while (true) {
        if (this.IsWorkingDate(date)) {
          return date;
        } else {
          date = date.AddDays(-1);
        }
      }

    }


    public DateTime NextWorkingDate(DateTime date, bool includeDate = false) {
      date = date.Date;

      if (!includeDate) {
        date = date.AddDays(1);
      }

      while (true) {
        if (this.IsWorkingDate(date)) {
          return date;
        } else {
          date = date.AddDays(1);
        }
      }

    }


    public int NonWorkingDays(DateTime from, DateTime to) {
      Assertion.Assert(from <= to, "'from' parameter must be earlier or equal than 'to' parameter.");

      from = from.Date;
      to = to.Date;

      int nonWorkingDaysCounter = 0;
      DateTime datePointer = from;

      while (true) {

        if (datePointer == to) {
          return nonWorkingDaysCounter;
        } else {
          datePointer = datePointer.AddDays(1);
        }

        if (IsNonWorkingDate(datePointer)) {
          nonWorkingDaysCounter++;
        }

      }  // while

    }


    public int WorkingDays(DateTime from, DateTime to) {
      Assertion.Assert(from <= to, "'from' parameter must be earlier or equal than 'to' parameter.");

      from = from.Date;
      to = to.Date;

      int workingDaysCounter = 0;
      DateTime datePointer = from;

      while (true) {

        if (datePointer == to) {
          return workingDaysCounter;
        } else {
          datePointer = datePointer.AddDays(1);
        }

        if (IsWorkingDate(datePointer)) {
          workingDaysCounter++;
        }

      }  // while

    }


    public DateTime SubstractWorkingDays(DateTime date, int days) {
      Assertion.Assert(days >= 0, "'days' parameter must be a non-negative number.");

      int workingDaysCounter = 0;
      DateTime datePointer = date.Date;

      while (true) {

        if (workingDaysCounter == -1 * days) {
          return datePointer;
        } else {
          datePointer = datePointer.AddDays(-1);
        }

        if (IsWorkingDate(datePointer)) {
          workingDaysCounter--;
        }

      }  // while

    }


    #endregion Public methods

  } // class EmpiriaCalendar

} // namespace Empiria.DataTypes
