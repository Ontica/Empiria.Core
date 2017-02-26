/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Logging Services                  *
*  Namespace : Empiria.Logging                                  Assembly : Empiria.Foundation.dll            *
*  Type      : ILogTrail                                        Pattern  : Separated interface               *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Separated interface with trail logger's methods.                                              *
*                                                                                                            *
********************************* Copyright (c) 2009-2017. Ontica LLC, La Vía Óntica SC, and contributors.  **/
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
