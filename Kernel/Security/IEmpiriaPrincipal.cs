/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaPrincipal                                Pattern  : Separated Interface               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This interface contains the fields of EmpiriaPrincipal and provides a separated interface for *
*              integrate Empiria.Kernel.dll assembly with this assembly.                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Security.Principal;

namespace Empiria.Security {

  public interface IEmpiriaPrincipal : IPrincipal, IDisposable {

    #region Members definition

    bool CanExecute(int typeId, char operationType);

    bool CanExecute(int typeId, char operationType, int instanceId);

    bool IsAuditable(int typeId, char operationType);

    #endregion Members definition

  } //interface IEmpiriaPrincipal

} //namespace Empiria.Security