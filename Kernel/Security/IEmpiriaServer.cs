﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaServer                                   Pattern  : Separated Interface               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This interface contains the contract of a Empiria Server object and provides a separated      *
*              interface for integrate Empiria.Kernel.dll assembly with Empiria.dll assembly.                *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Security {

  public interface IEmpiriaServer : IIdentifiable {

    #region Members definition

    string WebSiteIPAddress { get; }

    string WebSiteURL { get; }

    string WebServicesSiteIPAddress { get; }

    string WebServicesSiteURL { get; }

    #endregion Members definition

  } //interface IEmpiriaServer

} //namespace Empiria.Security
