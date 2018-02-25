/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  License  : Please read LICENSE.txt file      *
*  Type      : LogEntryType                                     Pattern  : Enumeration                       *
*                                                                                                            *
*  Summary   : Describes a log entry type.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Logging {

  /// <summary>Describes a log entry type.</summary>
  public enum LogEntryType {

    /// <summary>Used for track no fatal errors. Typically the log data contains an exception object with
    /// its full trace and additional useful information.</summary>
    Error = 'E',

    /// <summary>Used for application debugging. Log entries must not be stored
    /// when them arise from production mode compiled clients.</summary>
    Debug = 'D',

    /// <summary>The entry describes a simple operation, message or object.</summary>
    Info = 'I',

    /// <summary>Describes an application critical or severe issue. Fatal logs entries could
    /// be reported to system managers using text or email messages.</summary>
    Critical = 'C',

    /// <summary>Used for application execution tracing.
    /// The entry must contain a trace guid in order to identificate the entries per trace.</summary>
    Trace = 'T',

    /// <summary>Used only for set a log entry type initial value.</summary>
    Unknown = 'U',

  }

} // namespace Empiria.Logging
