/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : EmpiriaCalendar                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides calendar data to know working and non working days.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.DataTypes.Time {

  /// <summary>Provides calendar data to know working and non working days.</summary>
  public class EmpiriaCalendar {

    #region Constructors and parsers

    public EmpiriaCalendar(string calendarName, JsonObject json) {
      Assertion.AssertObject(json, "json");

      this.Name = calendarName;
      this.LoadJsonData(json);
    }


    static public EmpiriaCalendar Parse(string calendarName) {
      Assertion.AssertObject(calendarName, "calendarName");

      var storedJson = StoredJson.Parse("System.Calendars");

      JsonObject json = storedJson.Value.Slice(calendarName, true);

      return new EmpiriaCalendar(calendarName, json);
    }


    static public EmpiriaCalendar ParseOrDefault(string calendarName) {
      Assertion.AssertObject(calendarName, "calendarName");

      if (Exists(calendarName)) {
        return Parse(calendarName);
      } else {
        return Default;
      }
    }


    static public bool Exists(string calendarName) {
      Assertion.AssertObject(calendarName, "calendarName");

      var storedJson = StoredJson.Parse("System.Calendars");

      return storedJson.Value.Contains(calendarName);
    }


    static public EmpiriaCalendar Default {
      get {
        var calendarName = ConfigurationData.Get<string>("Default.Calendar.Name", "Default");

        return EmpiriaCalendar.Parse(calendarName);
      }
    }


    #endregion Constructors and parsers

    #region Public properties

    public string Name {
      get;
    }


    public FixedList<DateTime> Holidays {
      get;
      private set;
    }


    public FixedList<TimeFrame> NonWorkingDaysPeriods {
      get;
      private set;
    }


    public FixedList<DateTime> NonWorkingDaysExceptions {
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

    public bool IsHolidayOrWeekendDay(DateTime date) {
      return (this.Holidays.Contains(date.Date) || this.IsWeekendDay(date.Date));
    }

    public bool IsNonWorkingDate(DateTime date) {
      return (this.IsWeekendDay(date) || this.IsHoliday(date) ||
             (this.IncludedInANonWorkingDaysPeriod(date) &&
             !this.IsNonWorkingDateException(date)));
    }


    public bool IsWeekendDay(DateTime date) {
      return (date.DayOfWeek == DayOfWeek.Saturday ||
              date.DayOfWeek == DayOfWeek.Sunday);
    }


    public bool IsWorkingDate(DateTime date) {
      return !this.IsNonWorkingDate(date);
    }


    public DateTime LastNonHolidayOrWeekendDate(DateTime date, bool includeDate = false) {
      date = date.Date;

      if (!includeDate) {
        date = date.AddDays(-1);
      }

      while (true) {
        if (!this.IsHolidayOrWeekendDay(date)) {
          return date;
        } else {
          date = date.AddDays(-1);
        }
      }
    }


    public DateTime LastNonWeekendDate(DateTime date, bool includeDate = false) {
      date = date.Date;

      if (!includeDate) {
        date = date.AddDays(-1);
      }

      while (true) {
        if (!this.IsWeekendDay(date)) {
          return date;
        } else {
          date = date.AddDays(-1);
        }
      }
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


    public DateTime LastWorkingDateWithinMonth(int year, int month) {
      var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));

      var lastWorkingDate = this.LastWorkingDate(date, true);

      if (lastWorkingDate.Month == month) {
        return lastWorkingDate;
      }

      lastWorkingDate = LastNonHolidayOrWeekendDate(date, true);

      if (lastWorkingDate.Month == month) {
        return lastWorkingDate;
      }

      return LastNonWeekendDate(date, true);
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

    #region Private methods

    private bool IncludedInANonWorkingDaysPeriod(DateTime date) {
      foreach (var period in this.NonWorkingDaysPeriods) {
        if (period.Includes(date)) {
          return true;
        }
      }
      return false;
    }


    private bool IsNonWorkingDateException(DateTime date) {
      return this.NonWorkingDaysExceptions.Contains(date.Date);
    }


    private void LoadJsonData(JsonObject json) {

      this.Holidays = new FixedList<DateTime>();
      if (json.HasValue("holidays")) {
        this.Holidays = json.GetList<DateTime>("holidays")
                            .ToFixedList();

      }

      this.NonWorkingDaysPeriods = new FixedList<TimeFrame>();
      if (json.HasValue("nonWorkingPeriods")) {
        this.NonWorkingDaysPeriods = json.GetList<TimeFrame>("nonWorkingPeriods")
                                         .ToFixedList();
      }

      this.NonWorkingDaysExceptions = new FixedList<DateTime>();
      if (json.HasValue("nonWorkingExceptions")) {
        this.NonWorkingDaysExceptions = json.GetList<DateTime>("nonWorkingExceptions")
                                            .ToFixedList();
      }

    }


    #endregion Private methods

  } // class EmpiriaCalendar

} // namespace Empiria.DataTypes.Time
