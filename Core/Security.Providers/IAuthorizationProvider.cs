/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Providers                    *
*  Assembly : Empiria.Core.dll                             Pattern   : Dependency inversion interface        *
*  Type     : IAuthorizationProvider                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Interface for subject authorization providers.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Security.Providers {

  /// <summary>Interface for subject authorization providers.</summary>
  public interface IAuthorizationProvider {

    bool IsSubjectActive(IIdentifiable subject);

    bool IsSubjectInRole(IIdentifiable subject, IClientApplication clientApp, string role);

  }  // interface IAuthorizationProvider

}  // namespace Empiria.Security.Providers
