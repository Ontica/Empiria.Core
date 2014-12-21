/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : TimePeriod                                       Pattern  : Value Type                        *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Value type that handles datetime period operations.                                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  public struct TimePeriod {

    #region Fields

    private DateTime _startTime;
    private DateTime _endTime;

    #endregion Fields

    #region Constructors and parsers

    public TimePeriod(DateTime startTime, DateTime endTime) {
      _startTime = startTime;
      _endTime = endTime;
    }

    static public TimePeriod Default {
      get {
        return new TimePeriod(ExecutionServer.DateMinValue, ExecutionServer.DateMaxValue);
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

    static public bool operator ==(TimePeriod periodA, TimePeriod periodB) {
      return ((periodA.StartTime == periodB.StartTime) && (periodA.EndTime == periodB.EndTime));
    }

    static public bool operator !=(TimePeriod periodA, TimePeriod periodB) {
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
      if (!(o is TimePeriod)) {
        return false;
      }
      TimePeriod temp = (TimePeriod) o;

      return ((this.StartTime == temp.StartTime) && (this.EndTime == temp.EndTime));
    }

    public override int GetHashCode() {
      return (this.ToString().GetHashCode());
    }

    public override string ToString() {
      return _startTime.ToString("yyyymmdd") + "." + _endTime.ToString("yyyymmdd");
    }

    #endregion Public methods

  } // class TimePeriod

} // namespace Empiria
