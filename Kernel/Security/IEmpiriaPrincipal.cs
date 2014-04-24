/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaPrincipal                                Pattern  : Separated Interface               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This interface contains the fields of EmpiriaPrincipal and provides a separated interface for *
*              integrate Empiria.Kernel.dll assembly with this assembly.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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