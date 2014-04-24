/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaIdentity                                 Pattern  : Separated Interface               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This interface contains the fields of EmpiriaIdentity and provides a separated interface for  *
*              integrate Empiria.Kernel.dll assembly with this assembly.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System.Security.Principal;

namespace Empiria.Security {

  public interface IEmpiriaIdentity : IIdentity {

    #region Members definition

    IEmpiriaSession Session { get; }

    IEmpiriaUser User { get; }

    int CurrentRegionId { get; set; }

    #endregion Members definition

  } //interface IEmpiriaIdentity

} //namespace Empiria.Security