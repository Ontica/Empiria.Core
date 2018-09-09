/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization Services                *
*  Assembly : Empiria.Core.dll                             Pattern   : Service Provider                      *
*  Type     : AuthorizationService                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Static class that provides authorization services.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Authorization {

  /// <summary>Static class that provides authorization services.</summary>
  static public class AuthorizationService {

    #region Public methods

    /// <summary>Return all active authorizations where the current user is involved as a requester or
    /// who authorizes or approves the authorization request.</summary>
    /// <returns>A read only list of authorizations.</returns>
    static public FixedList<Authorization> GetActive() {
      return AuthorizationServiceData.GetActiveAuthorizations();
    }


    /// <summary>Creates an authorization object from a given authorization request.</summary>
    static public Authorization Request(AuthorizationRequest request) {
      Assertion.AssertObject(request, "request");

      var authorization = new Authorization(request);

      authorization.Save();

      return authorization;
    }

    #endregion Public methods

  } // class AuthorizationService

}  // namespace Empiria.Security.Authorization
