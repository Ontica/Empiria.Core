/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaSession                                  Pattern  : Separated Interface               *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : This interface contains the fields of the EmpiriaSession object and provides a separated      *
*              interface for integrate Empiria.Kernel.dll assembly with this assembly.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Security {

  public interface IEmpiriaSession : IIdentifiable {

    #region Members definition

    DateTime EndTime { get; }

    int ServerId { get; }

    DateTime StartTime { get; }

    string Token { get; }

    int UserId { get; }

    #endregion Members definition

  } //interface IEmpiriaSession

} //namespace Empiria.Security
