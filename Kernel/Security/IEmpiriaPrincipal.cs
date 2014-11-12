/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaPrincipal                                Pattern  : Separated Interface               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This interface contains the fields of EmpiriaPrincipal and provides a separated interface for *
*              integrate Empiria.Kernel.dll assembly with this assembly.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Security.Principal;

using Empiria.Collections;

namespace Empiria.Security {

  public interface IEmpiriaPrincipal : IPrincipal, IDisposable {

    #region Members definition

    AssortedDictionary ContextItems { get; }

    IEmpiriaSession Session { get; }

    #endregion Members definition

  } //interface IEmpiriaPrincipal

} //namespace Empiria.Security
