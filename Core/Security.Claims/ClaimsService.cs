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


    static public Claim[] Claims(EmpiriaPrincipal principal) {
      ClaimList applicationClaims = ClaimList.ParseFor(principal.ClientApp);

      Claim appRoles = applicationClaims.GetItem(ClaimType.ApplicationRoles);
      Claim appFeatures = applicationClaims.GetItem(ClaimType.ApplicationFeatures);

      return new Claim[0];
    }


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


    static public T GetClaimValue<T>(IClaimsSubject subject,
                                     ClaimType claimType) {

      var claimsList = ClaimList.ParseFor(subject);

      if (claimsList.Contains(claimType)) {
        return (T) (object) claimsList.GetItem(claimType).Value;
      }

      string msg = BuildClaimNotFoundMsg(subject, claimType);

      throw new SecurityException(SecurityException.Msg.EnsureClaimFailed, msg);
    }

    #endregion Services

    #region Private methods

    static private string BuildClaimNotFoundMsg(IClaimsSubject subject,
                                                ClaimType claimType) {
      return $"Subject of type {subject.GetType().FullName}' " +
             $"with token = {subject.ClaimsToken}, doesn't have a security claim " +
             $"of type '{claimType}'.";
    }

    static private string BuildClaimNotFoundMsg(IClaimsSubject subject,
                                                ClaimType claimType,
                                                string claimValue) {
      return $"Subject of type {subject.GetType().FullName}' " +
             $"with token = {subject.ClaimsToken}, doesn't have a security claim " +
             $"with value '{claimValue}' of type '{claimType}'.";
    }

    #endregion Private methods

  }  // class ClaimsService

}  // namespace Empiria.Security.Claims
