/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Kernel.dll                *
*  Type      : IEmpiriaServer                                   Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This interface contains the contract of a Empiria Server object and provides a separated      *
*              interface for integrate Empiria.Kernel.dll assembly with Empiria.dll assembly.                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/

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
