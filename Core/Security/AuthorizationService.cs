/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization services                *
*  Assembly : Empiria.Core.dll                             Pattern   : Service provider                      *
*  Type     : AuthorizationService                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides user authorization services.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Security.Providers;

namespace Empiria.Security {

  static public class AuthorizationService {

    static public bool IsSubjectInRole(IIdentifiable subject, string role) {
      Assertion.Require(subject, nameof(subject));
      Assertion.Require(role, nameof(role));

      var provider = SecurityProviders.AuthorizationProvider();

      return provider.IsSubjectInRole(subject, ExecutionServer.CurrentPrincipal.ClientApp, role);
    }

  }  //class AuthorizationService

}  // namespace Empiria.Security
