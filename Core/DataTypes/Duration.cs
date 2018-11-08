/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : Duration                                         Pattern  : Value Type                        *
*                                                                                                            *
*  Summary   : Data type with methods used to describe and control time duration.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Data type with methods used to describe and control time duration.</summary>
  public class Duration {

    #region Fields

    private string _string_value = String.Empty;

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

    #region Methods

    private DurationType GetDurationType(string durationType) {
      durationType = durationType.ToLowerInvariant();

      if (durationType == "hours" || durationType == "hour") {
        return DurationType.Hours;

      } else if (durationType == "days" || durationType == "day") {
        return DurationType.CalendarDays;

      } else if (durationType == "business-days" || durationType == "business-day" ||
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


    public object ToJson() {
      return new {
        value = this.Value,
        type = this.DurationType.ToString()
      };
    }


    public override string ToString() {
      return _string_value;
    }

    #endregion Methods

  } // class Duration

} // namespace Empiria.DataTypes
