/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  Assembly : Empiria.Foundation.dll            *
*  Type      : LoginModel                                       Pattern  : Data Transfer Object              *
*  Version   : 1.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Data Transfer Object used to describe log entries.                                            *
*                                                                                                            *
********************************* Copyright (c) 2014-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Logging {

  /// <summary>Data Transfer Object used to describe log entries.</summary>
  public class LogEntryModel : ILogEntry {

    #region Fields

    private const int TIMESTAMP_LOWER_BOUND_DAYS = 5;
    private const int TIMESTAMP_UPPER_BOUND_DAYS = 1;

    #endregion Fields

    #region Properties

    public string SessionToken {
      get;
      set;
    } = String.Empty;


    public DateTime Timestamp {
      get;
      set;
    } = DateTime.Now;

    public LogEntryType EntryType {
      get;
      set;
    } = LogEntryType.Unknown;


    public Guid TraceGuid {
      get;
      set;
    } = Guid.Empty;


    public string Data {
      get;
      set;
    } = String.Empty;

    #endregion Properties

    #region Methods

    public void AssertIsValid() {
      Assertion.Assert(this.SessionToken != null,
                       "Session token can't be null. Please send an empty string for no active sessions.");

      if (!Enum.IsDefined(typeof(LogEntryType), this.EntryType)) {
        Assertion.AssertFail("EntryType must have a valid value. Received value was {0}.", this.EntryType);
      }

      Assertion.Assert(this.EntryType != LogEntryType.Unknown,
                       "EntryType can't have the 'Unknown' value.");

      Assertion.Assert(this.EntryType != LogEntryType.Trace ||
                       this.TraceGuid != Guid.Empty,
                       "Trace logs must contain a non-empty TraceGuid value.");

      Assertion.Assert(DateTime.Now.AddDays(-1 * TIMESTAMP_LOWER_BOUND_DAYS) <= this.Timestamp &&
                       this.Timestamp <= DateTime.Now.AddDays(TIMESTAMP_UPPER_BOUND_DAYS),
                       "Timestamp value is out of bounds of the logging service time window.");

      Assertion.AssertObject(this.Data, "Data");

    }

    #endregion Methods

  }  // class LogEntryModel

} // namespace Empiria.Logging
