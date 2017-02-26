/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  Assembly : Empiria.Foundation.dll            *
*  Type      : ILogEntry                                        Pattern  : Loose coupling interface          *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface that represents a log entry.                                                        *
*                                                                                                            *
********************************* Copyright (c) 1999-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    #region Methods

    void AssertIsValid();

    #endregion Methods

  }  // interface ILogEntry

}  // namespace Empiria.Logging
