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

    #region Properties

    public int UserSessionId {
      get;
      set;
    } = -1;


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
      Assertion.AssertObject(this.Data, "Data");
    }

    #endregion Methods

  }  // class LogEntryModel

} // namespace Empiria.Logging
