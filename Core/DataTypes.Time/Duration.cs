/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Enumeration Type                        *
*  Type     : Duration                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a time duration (e.g, 10 business days, 8 hours, 3 months, etc).                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes.Time {

  /// <summary>Describes a time duration (e.g, 10 business days, 8 hours, 3 months, etc).</summary>
  public class Duration {

    #region Fields

    private readonly string _string_value;

    #endregion Fields

    #region Constructors and parsers

    public Duration(int value, DurationType type) {
      _string_value = GetStringValue(value, type);

      this.Load();
    }

    private Duration(string value) {
      _string_value = EmpiriaString.TrimAll(value);

      this.Load();
    }

    static public Duration Parse(string value) {
      if (String.IsNullOrWhiteSpace(value)) {
        return Duration.Empty;
      }

      return new Duration(value);
    }

    static public Duration Empty {
      get {
        var emptyDuration = new Duration(String.Empty);

        emptyDuration.IsEmptyInstance = true;

        return emptyDuration;
      }
    }


    #endregion Constructors and parsers

    #region Properties

    public bool IsEmptyInstance {
      get;
      private set;
    } = false;


    public DurationType DurationType {
      get;
      private set;
    } = DurationType.Unknown;


    public int Value {
      get;
      private set;
    } = 0;


    #endregion Properties

    #region Public methods

    public double ToDays() {
      switch (this.DurationType) {
        case DurationType.Hours:
          return this.Value / 24.0;

        case DurationType.CalendarDays:
          return this.Value;

        case DurationType.BusinessDays:
          return this.Value;

        case DurationType.Months:
          return this.Value * 30;

        case DurationType.Years:
          return this.Value * 365;

        case DurationType.Unknown:
          return 0;

        case DurationType.NA:
          return 0;

        default:
          throw Assertion.AssertNoReachThisCode("Unrecognized");
      }
    }

    public object ToJson() {
      return new {
        value = this.Value,
        type = this.DurationType.ToString()
      };
    }

    public override string ToString() {
      return _string_value;
    }


    #endregion Public methods

    #region Private methods

    private DurationType GetDurationType(string durationType) {
      durationType = durationType.ToLowerInvariant();

      if (durationType == "hours" || durationType == "hour") {
        return DurationType.Hours;

      } else if (durationType == "days" || durationType == "day" ||
                 durationType == "calendarday" || durationType == "calendardays" ||
                 durationType == "calendar-day" || durationType == "calendar-days") {
        return DurationType.CalendarDays;

      } else if (durationType == "businessday" || durationType == "businessdays" ||
                 durationType == "workday" || durationType == "workdays" ||
                 durationType == "workingday" || durationType == "workingdays" ||
                 durationType == "business-days" || durationType == "business-day" ||
                 durationType == "work-days" || durationType == "work-day" ||
                 durationType == "working-days" || durationType == "working-day") {
        return DurationType.BusinessDays;

      } else if (durationType == "months" || durationType == "month") {
        return DurationType.Months;

      } else if (durationType == "years" || durationType == "year") {
        return DurationType.Years;

      } else if (durationType == "unknown") {
        return DurationType.Unknown;

      } else if (durationType == "not-available" || durationType == "NA") {
        return DurationType.NA;

      } else {
        throw Assertion.AssertNoReachThisCode($"Unrecognized duration type '{durationType}'.");

      }
    }

    private string GetStringValue(int value, DurationType type) {
      switch (type) {
        case DurationType.Hours:
          return value + " hours";

        case DurationType.CalendarDays:
          return value + " days";

        case DurationType.BusinessDays:
          return value + " business-days";

        case DurationType.Months:
          return value + " months";

        case DurationType.Years:
          return value + " years";

        case DurationType.Unknown:
          return value + " unknown";

        case DurationType.NA:
          return value + " not-available";

        default:
          throw Assertion.AssertNoReachThisCode("Unrecognized");
      }
    }

    private void Load() {
      string[] parts = _string_value.Split(' ');

      if (parts.Length == 2) {
        this.Value = int.Parse(parts[0]);
        this.DurationType = GetDurationType(parts[1]);
      }
    }

    #endregion Private methods

  } // class Duration

} // namespace Empiria.DataTypes.Time