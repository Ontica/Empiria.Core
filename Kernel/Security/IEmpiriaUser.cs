/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaUser                                     Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This interface contains the fields of EmpiriaIdentity and provides a separated interface for  *
*              integrate Empiria.Kernel.dll assembly with this assembly.                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.Security {

  public interface IEmpiriaUser : IIdentifiable {

    #region Members definition

    string UITheme { get; }

    string UserName { get; }

    bool VerifyElectronicSign(string esign);

    #endregion Members definition

  } //interface IEmpiriaUser

} //namespace Empiria.Security