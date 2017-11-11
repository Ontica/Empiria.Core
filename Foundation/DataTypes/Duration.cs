/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : Duration                                         Pattern  : Value Type                        *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Data type with methods used to describe and control time duration.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      return new Duration(value);
    }

    static public Duration Empty {
      get {
        return new Duration(String.Empty);
      }
    }

    #endregion Constructors and parsers

    #region Properties

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

    private DurationType GetDurationType(string duration) {
      duration = duration.ToLowerInvariant();

      if (duration == "hours" || duration == "hour") {
        return DurationType.Hours;

      } else if (duration == "days" || duration == "day") {
        return DurationType.Days;

      } else if (duration == "work-days" || duration == "work-day" ||
                 duration == "working-days" || duration == "working-day") {
        return DurationType.WorkingDays;

      } else if (duration == "months" || duration == "month") {
        return DurationType.Months;

      } else if (duration == "years" || duration == "year") {
        return DurationType.Years;

      } else {
        throw Assertion.AssertNoReachThisCode();

      }
    }


    private string GetStringValue(int value, DurationType type) {
      switch (type) {
        case DurationType.Hours:
          return value + " hours";

        case DurationType.Days:
          return value + " days";

        case DurationType.WorkingDays:
          return value + " working-days";

        case DurationType.Months:
          return value + " months";

        case DurationType.Years:
          return value + " years";

        default:
          throw Assertion.AssertNoReachThisCode();
      }
    }


    private void Load() {
      string[] parts = _string_value.Split(' ');

      if (parts.Length == 2) {
        this.Value = int.Parse(parts[0]);
        this.DurationType = GetDurationType(parts[1]);
      }
    }


    public override string ToString() {
      return _string_value;
    }

    #endregion Methods

  } // class Duration

} // namespace Empiria.DataTypes
