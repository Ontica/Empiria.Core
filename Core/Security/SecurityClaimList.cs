/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain service class                  *
*  Type     : SecurityClaimList                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds the full collection of security claims for a given subject.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;
using System.Collections.Generic;

namespace Empiria.Security {

  /// <summary>Holds the full collection of security claims for a given resource.</summary>
  internal sealed class SecurityClaimList {

    #region Fields

    private EmpiriaDictionary<string, List<SecurityClaim>> internalList =
                                            new EmpiriaDictionary<string, List<SecurityClaim>>();

    #endregion Fields

    #region Constructors and parsers

    private SecurityClaimList(IClaimsSubject subject) {
      this.Subject = subject;

      this.LoadClaimList();
    }

    static public SecurityClaimList ParseFor(IClaimsSubject subject) {
      Assertion.AssertObject(subject, "subject");

      return new SecurityClaimList(subject);
    }

    #endregion Constructors and parsers

    #region Properties

    public int ResourceTypeId {
      get;
      private set;
    }


    public IClaimsSubject Subject {
      get;
      private set;
    }

    #endregion Properties

    #region Public methods

    public void ActivateSecure(SecurityClaimType claimType,
                               SecurityClaimType activationClaimType, string activationValue) {
      // 1) Assert that the resource has the activation token
      if (!this.ContainsSecure(activationClaimType, activationValue)) {
        throw new SecurityException(SecurityException.Msg.InvalidActivationToken, activationValue);
      }

      // 2) Get the claim attached to the resource
      var claim = SecurityData.GetPendingSecurityClaim(claimType, this.Subject);

      // 3) Activate the claim
      claim.Status = ObjectStatus.Active;
      SecurityData.WriteSecurityClaim(claim);

      // 4) Load the activated claim into the list
      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);
      if (!internalList.ContainsKey(cacheKey)) {
        internalList.Insert(cacheKey, new List<SecurityClaim>());
      }
      internalList[cacheKey].Add(claim);

      // 5) Remove the activation token used to activate the claim
      this.RemoveSecure(activationClaimType, activationValue);
    }


    public SecurityClaim Append(SecurityClaimType claimType, string claimValue) {
      return this.AppendUtil(claimType, claimValue, ObjectStatus.Active);
    }


    public SecurityClaim AppendSecure(SecurityClaimType claimType, string claimValue,
                                      ObjectStatus status = ObjectStatus.Active) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      var encryptedValue = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, claimValue, cacheKey);

      return this.AppendUtil(claimType, encryptedValue, status);
    }


    public bool Contains(SecurityClaimType claimType) {
      Assertion.AssertObject(claimType, "claimType");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      return internalList.ContainsKey(cacheKey);
    }


    public bool Contains(SecurityClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      return (internalList.ContainsKey(cacheKey) &&
              internalList[cacheKey].Exists( (x) => x.Value.ToLowerInvariant().Equals(claimValue.ToLowerInvariant())) );
    }


    public bool ContainsSecure(SecurityClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      var encryptedValue = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, claimValue, cacheKey);

      return this.Contains(claimType, encryptedValue);
    }


    public SecurityClaim GetItem(SecurityClaimType claimType) {
      Assertion.AssertObject(claimType, "claimType");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      if (!internalList.ContainsKey(cacheKey)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimTypeNotFound,
                                    claimType.Type, Subject.ClaimsToken);
      } else if (internalList[cacheKey].Count != 1) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimTypeIsMultiValue, claimType.Type);
      } else {
        return internalList[cacheKey][0];
      }
    }


    internal SecurityClaim GetItem(SecurityClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      if (!internalList.ContainsKey(cacheKey)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimTypeNotFound,
                                    claimType.Type, Subject.ClaimsToken);
      }

      SecurityClaim claim = internalList[cacheKey].Find((x) => x.Value.ToLowerInvariant()
                                                                .Equals(claimValue.ToLowerInvariant()));

      if (claim != null) {
        return claim;
      } else {
        throw new SecurityException(SecurityException.Msg.SubjectClaimNotFound,
                                    claimType.Type, Subject.ClaimsToken, claimValue);
      }
    }


    internal void RemoveSecure(SecurityClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      var encryptedValue = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode,
                                                 claimValue, cacheKey);

      SecurityClaim claim = this.GetItem(claimType, encryptedValue);

      SecurityData.RemoveClaim(claim);

      internalList[cacheKey].Remove(claim);
    }


    public void ReplaceSecure(SecurityClaimType claimType, string newClaimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(newClaimValue, "newClaimValue");

      SecurityClaim claim = this.GetItem(claimType);

      SecurityData.RemoveClaim(claim);

      this.AppendSecure(claimType, newClaimValue);
    }


    public void ReplaceSecure(SecurityClaimType claimType,
                              string oldClaimValue, string newClaimValue) {
      if (!this.ContainsSecure(claimType, oldClaimValue)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimNotFound,
                                    claimType.Type, Subject.ClaimsToken, oldClaimValue);
      }
      this.RemoveSecure(claimType, oldClaimValue);
      this.AppendSecure(claimType, newClaimValue);
    }


    public void ReplaceSecureWithToken(SecurityClaimType claimType, SecurityClaimType activationClaimType,
                                       string activationValue, string newClaimValue) {
      if (!this.ContainsSecure(activationClaimType, activationValue)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimNotFound,
                                    claimType.Type, Subject.ClaimsToken, activationValue);
      }
      this.AppendSecure(claimType, newClaimValue);
      this.RemoveSecure(activationClaimType, activationValue);
    }

    #endregion Public methods

    #region Private methods

    private SecurityClaim AppendUtil(SecurityClaimType claimType,
                                     string claimValue, ObjectStatus status) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Subject);

      if (!internalList.ContainsKey(cacheKey)) {
        internalList.Insert(cacheKey, new List<SecurityClaim>());
      }
      SecurityClaim claim = SecurityClaim.Create(claimType, this.Subject,
                                                 claimValue, status);
      internalList[cacheKey].Add(claim);

      return claim;
    }


    private void LoadClaimList() {
      List<SecurityClaim> fullList = SecurityData.GetSecurityClaims(this.Subject);

      foreach (SecurityClaim claim in fullList) {
        if (!internalList.ContainsKey(claim.UID)) {
          internalList.Insert(claim.UID, new List<SecurityClaim>());
        }
        internalList[claim.UID].Add(claim);
      }
    }

    #endregion Private methods

  } // class SecurityClaimList

} // namespace Empiria.Security
