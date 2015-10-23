/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : TimeFrame                                        Pattern  : Value Type                        *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Value type that handles timeframe operations.                                                 *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  public struct TimeFrame {

    #region Fields

    private DateTime _startTime;
    private DateTime _endTime;

    #endregion Fields

    #region Constructors and parsers

    public TimeFrame(DateTime? startTime, DateTime? endTime)
                              : this(startTime ?? ExecutionServer.DateMinValue,
                                     endTime ?? ExecutionServer.DateMaxValue) {
    }

    public TimeFrame(DateTime startTime, DateTime endTime) {
      Assertion.Assert(startTime <= endTime, "startTime should be before or equal to endTime.");

      _startTime = startTime;
      _endTime = endTime;
    }

    static public TimeFrame Default {
      get {
        return new TimeFrame(ExecutionServer.DateMinValue, ExecutionServer.DateMaxValue);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public DateTime StartTime {
      get { return _startTime; }
    }

    public DateTime EndTime {
      get { return _endTime; }
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

    public bool IsInRange(DateTime date) {
      if (_endTime.TimeOfDay.Hours == 0) {
        if (_startTime <= date && date < _endTime.AddSeconds(86400)) {
          return true;
        }
      }
      return (_startTime <= date && date <= _endTime);
    }

    public override bool Equals(object o) {
      if (!(o is TimeFrame)) {
        return false;
      }
      TimeFrame temp = (TimeFrame) o;

      return ((this.StartTime == temp.StartTime) && (this.EndTime == temp.EndTime));
    }

    public override int GetHashCode() {
      return (this.ToString().GetHashCode());
    }

    public override string ToString() {
      return _startTime.ToString("yyyymmdd") + "." + _endTime.ToString("yyyymmdd");
    }

    #endregion Public methods

  } // class TimeFrame

} // namespace Empiria.DataTypes
