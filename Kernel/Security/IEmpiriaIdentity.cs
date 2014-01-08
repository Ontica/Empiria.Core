/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaIdentity                                 Pattern  : Separated Interface               *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : This interface contains the fields of EmpiriaIdentity and provides a separated interface for  *
*              integrate Empiria.Kernel.dll assembly with this assembly.                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
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