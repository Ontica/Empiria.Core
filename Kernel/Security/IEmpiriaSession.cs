/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaSession                                  Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This interface contains the fields of the EmpiriaSession object and provides a separated      *
*              interface for integrate Empiria.Kernel.dll assembly with this assembly.                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
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