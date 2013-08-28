/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Types Library                *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : TimePeriod                                       Pattern  : Value Type                        *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Value type that handles datetime period operations.                                           *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

namespace Empiria {

  public struct TimePeriod {

    #region Fields

    private DateTime fromDate;
    private DateTime toDate;

    #endregion Fields

    #region Constructors and parsers

    public TimePeriod(DateTime fromDate, DateTime toDate) {
      this.fromDate = fromDate;
      this.toDate = toDate;
    }

    static public TimePeriod AllTime {
      get {
        return new TimePeriod(ExecutionServer.DateMinValue, ExecutionServer.DateMaxValue);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public DateTime FromDate {
      get { return fromDate; }
    }

    public DateTime ToDate {
      get { return toDate; }
    }

    #endregion Public properties

    #region Operators overloading

    static public bool operator ==(TimePeriod periodA, TimePeriod periodB) {
      return ((periodA.FromDate == periodB.FromDate) && (periodA.ToDate == periodB.ToDate));
    }

    static public bool operator !=(TimePeriod periodA, TimePeriod periodB) {
      return !(periodA == periodB);
    }

    #endregion Operators overloading

    #region Public methods

    public bool IsInRange(DateTime date) {
      if (this.toDate.TimeOfDay.Hours == 0) {
        if (this.fromDate <= date && date < this.toDate.AddSeconds(86400)) {
          return true;
        }
      }
      return (this.fromDate <= date && date <= this.toDate);
    }

    public override bool Equals(object o) {
      if (!(o is TimePeriod)) {
        return false;
      }
      TimePeriod temp = (TimePeriod) o;

      return ((this.FromDate == temp.FromDate) && (this.ToDate == temp.ToDate));
    }

    public override int GetHashCode() {
      return (this.ToString().GetHashCode());
    }

    public override string ToString() {
      return fromDate.ToString("yyyymmdd") + "." + toDate.ToString("yyyymmdd");
    }

    #endregion Public methods

  } // class TimePeriod

} // namespace Empiria