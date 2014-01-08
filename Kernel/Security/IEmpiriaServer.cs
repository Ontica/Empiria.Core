/* Empiria® Foundation Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaServer                                   Pattern  : Separated Interface               *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : This interface contains the contract of a Empiria Server object and provides a separated      *
*              interface for integrate Empiria.Kernel.dll assembly with Empiria.dll assembly.                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

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
