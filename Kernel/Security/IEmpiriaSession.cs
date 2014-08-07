/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaSession                                  Pattern  : Separated Interface               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This interface contains the fields of the EmpiriaSession object and provides a separated      *
*              interface for integrate Empiria.Kernel.dll assembly with this assembly.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Security {

  public interface IEmpiriaSession : IIdentifiable {

    #region Members definition

    string ClientAddress { get; }

    string ClientEnvironment { get; }

    DateTime EndTime { get; }

    int ServerId { get; }

    DateTime StartTime { get; }

    string SystemSession { get; }

    string Token { get; }

    int UserId { get; }

    #endregion Members definition

  } //interface IEmpiriaSession

} //namespace Empiria.Security
