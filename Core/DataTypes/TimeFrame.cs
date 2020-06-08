/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Data Types                              *
*  Assembly : Empiria.Core.dll                           Pattern   : Structure                               *
*  Type     : TimeFrame                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a time frame or period with start and end times.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

namespace Empiria.DataTypes {

  /// <summary>Holds information about a time frame or period with start and end times.</summary>
  public struct TimeFrame {

    #region Constructors and parsers

    public TimeFrame(DateTime? startTime, DateTime? endTime)
                              : this(startTime ?? ExecutionServer.DateMinValue,
                                     endTime ?? ExecutionServer.DateMaxValue) {
    }


    public TimeFrame(DateTime startTime, DateTime endTime) {
      Assertion.Assert(startTime <= endTime, "startTime should be before or equal to endTime.");

      this.StartTime = startTime;
      this.EndTime = endTime;
    }


    static public TimeFrame Default {
      get {
        return new TimeFrame(ExecutionServer.DateMinValue, ExecutionServer.DateMaxValue);
      }
    }


    static public TimeFrame Parse(JsonObject json) {
      return new TimeFrame(json.Get<DateTime>("from"), json.Get<DateTime>("to"));
    }


    #endregion Constructors and parsers

    #region Public properties

    public DateTime StartTime {
      get;
    }


    public DateTime EndTime {
      get;
    }


    #endregion Public properties

    #region Operators overloading

    static public bool operator ==(TimeFrame periodA, TimeFrame periodB) {
      return ((periodA.StartTime == periodB.StartTime) && (periodA.EndTime == periodB.EndTime));
    }


    static public bool operator !=(TimeFrame periodA, TimeFrame periodB) {
      return !(periodA == periodB);
    }

    #endregion Operators overloading

    #region Public methods

    public override bool Equals(object o) {
      if (!(o is TimeFrame)) {
        return false;
      }

      TimeFrame temp = (TimeFrame) o;

      return ((this.StartTime == temp.StartTime) && (this.EndTime == temp.EndTime));
    }


    public bool Includes(DateTime date) {
      if (this.EndTime.TimeOfDay.Hours == 0) {
        if (this.StartTime <= date && date < this.EndTime.AddSeconds(86400)) {
          return true;
        }
      }

      return (this.StartTime <= date && date <= this.EndTime);
    }


    public override int GetHashCode() {
      return (this.ToString().GetHashCode());
    }


    public override string ToString() {
      return this.StartTime.ToString("yyyymmdd") + "." + this.EndTime.ToString("yyyymmdd");
    }


    #endregion Public methods

  } // struct TimeFrame

} // namespace Empiria.DataTypes
