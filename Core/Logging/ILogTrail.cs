/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  License  : Please read LICENSE.txt file      *
*  Type      : ILogTrail                                        Pattern  : Separated interface               *
*                                                                                                            *
*  Summary   : Separated interface with trail logger's methods.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Logging {

  /// <summary>Separated interface with trail logger's methods.</summary>
  public interface ILogTrail {

    #region Members definition

    void Write(ILogEntry logEntry);

    void Write(ILogEntry[] logEntries);

    #endregion Members definition

  } // class ILogTrail

} // namespace Empiria.Logging
