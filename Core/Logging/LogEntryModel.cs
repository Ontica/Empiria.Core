/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  License  : Please read LICENSE.txt file      *
*  Type      : LoginModel                                       Pattern  : Data Transfer Object              *
*                                                                                                            *
*  Summary   : Data Transfer Object used to describe log entries.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Logging {

  /// <summary>Data Transfer Object used to describe log entries.</summary>
  public class LogEntryModel : ILogEntry {

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

  }  // class LogEntryModel

} // namespace Empiria.Logging
