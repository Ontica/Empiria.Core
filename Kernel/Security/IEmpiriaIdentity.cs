/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaIdentity                                 Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This interface contains the fields of EmpiriaIdentity and provides a separated interface for  *
*              integrate Empiria.Kernel.dll assembly with this assembly.                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
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