/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  License  : Please read LICENSE.txt file      *
*  Type      : ILogEntry                                        Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : Interface that represents a log entry.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Logging {

  /// <summary>Interface that represents a log entry.</summary>
  public interface ILogEntry {

    #region Properties

    string SessionToken {
      get;
    }


    DateTime Timestamp {
      get;
    }


    LogEntryType EntryType {
      get;
    }


    Guid TraceGuid {
      get;
    }


    string Data {
      get;
    }

    #endregion Properties

  }  // interface ILogEntry

}  // namespace Empiria.Logging
