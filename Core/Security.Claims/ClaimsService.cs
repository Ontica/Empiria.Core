/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain service class                  *
*  Type     : ClaimsService                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides methods to create, verify and assign subject security claims.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Claims {

  /// <summary>Provides methods to create, verify and assign subject security claims.</summary>
  static public class ClaimsService {

    #region Services

    static public void EnsureClaim(IClaimsSubject subject,
                                   ClaimType claimType,
                                   string claimValue, string assertionFailMsg = "") {
      Assertion.AssertObject(subject, "subject");
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      var claimsList = ClaimList.ParseFor(subject);

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
                                                ClaimType claimType,
                                                string claimValue) {
      return $"Subject of type {subject.GetType().FullName}' " +
             $"with token = {subject.ClaimsToken}, doesn't have a security claim " +
             $"with value '{claimValue}' of type '{claimType}'.";
    }


    internal static T GetClaimValue<T>(IClaimsSubject subject,
                                       ClaimType claimType) {
      return (T) (object) @"E:\empiria.files\digital.certificates\prueba1.key";
    }

    #endregion Private methods

  }  // class ClaimsService

}  // namespace Empiria.Security.Claims
