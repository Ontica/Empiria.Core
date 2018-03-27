/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain service class                  *
*  Type     : SecurityClaimsService                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides methods to verify subject security claims.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  static public class SecurityClaimsService {

    #region Services

    static public void EnsureClaim(IClaimsSubject subject,
                                   SecurityClaimType claimType,
                                   string claimValue, string assertionFailMsg = "") {
      Assertion.AssertObject(subject, "subject");
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      var claimsList = SecurityClaimList.ParseFor(subject);

      if (claimsList.Contains(claimType, claimValue)) {
        return;
      }

      if (String.IsNullOrWhiteSpace(assertionFailMsg)) {
        assertionFailMsg = BuildClaimNotFoundMsg(subject, claimType, claimValue);
      }

      throw new SecurityException(SecurityException.Msg.EnsureClaimFailed, assertionFailMsg);
    }

    #endregion Services

    #region Private methods

    static private string BuildClaimNotFoundMsg(IClaimsSubject subject,
                                                SecurityClaimType claimType,
                                                string claimValue) {
      return $"Subject of type {subject.GetType().FullName}' " +
             $"with token = {subject.ClaimsToken}, doesn't have a security claim " +
             $"with value '{claimValue}' of type '{claimType}'.";
    }

    #endregion Private methods

  }  // class SecurityClaimsService

}  // namespace Empiria.Security
