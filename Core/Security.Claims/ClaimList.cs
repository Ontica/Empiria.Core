/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : ClaimList                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds the full collection of security claims for a given subject.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Collections;
using Empiria.StateEnums;

namespace Empiria.Security.Claims {

  /// <summary>Holds the full collection of security claims for a given resource.</summary>
  internal sealed class ClaimList {

    #region Fields

    private EmpiriaDictionary<string, List<Claim>> internalList =
                                            new EmpiriaDictionary<string, List<Claim>>();

    #endregion Fields

    #region Constructors and parsers

    private ClaimList(IClaimsSubject subject) {
      this.Subject = subject;

      this.LoadClaimList();
    }

    static public ClaimList ParseFor(IClaimsSubject subject) {
      Assertion.AssertObject(subject, "subject");

      return new ClaimList(subject);
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

    public void ActivateSecure(ClaimType claimType,
                               ClaimType activationClaimType, string activationValue) {
      // 1) Assert that the resource has the activation token
      if (!this.ContainsSecure(activationClaimType, activationValue)) {
        throw new SecurityException(SecurityException.Msg.InvalidActivationToken, activationValue);
      }

      // 2) Get the claim attached to the resource
      var claim = ClaimsData.GetPendingSecurityClaim(claimType, this.Subject);

      // 3) Activate the claim
      claim.Status = EntityStatus.Active;
      ClaimsData.WriteSecurityClaim(claim);

      // 4) Load the activated claim into the list
      string cacheKey = BuildUniqueKey(claimType, this.Subject);
      if (!internalList.ContainsKey(cacheKey)) {
        internalList.Insert(cacheKey, new List<Claim>());
      }
      internalList[cacheKey].Add(claim);

      // 5) Remove the activation token used to activate the claim
      this.RemoveSecure(activationClaimType, activationValue);
    }


    public Claim Append(ClaimType claimType, string claimValue) {
      return this.AppendUtil(claimType, claimValue, EntityStatus.Active);
    }


    public Claim AppendSecure(ClaimType claimType, string claimValue,
                              EntityStatus status = EntityStatus.Active) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      var encryptedValue = FormerCryptographer.Encrypt(EncryptionMode.EntropyHashCode, claimValue, cacheKey);

      return this.AppendUtil(claimType, encryptedValue, status);
    }


    public bool Contains(ClaimType claimType) {
      Assertion.AssertObject(claimType, "claimType");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      return internalList.ContainsKey(cacheKey);
    }


    public bool Contains(ClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      return (internalList.ContainsKey(cacheKey) &&
              internalList[cacheKey].Exists( (x) => x.Value.ToLowerInvariant().Equals(claimValue.ToLowerInvariant())) );
    }


    public bool ContainsSecure(ClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      var encryptedValue = FormerCryptographer.Encrypt(EncryptionMode.EntropyHashCode, claimValue, cacheKey);

      return this.Contains(claimType, encryptedValue);
    }


    public Claim GetItem(ClaimType claimType) {
      Assertion.AssertObject(claimType, "claimType");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      if (!internalList.ContainsKey(cacheKey)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimTypeNotFound,
                                    claimType.DisplayName, Subject.ClaimsToken);
      } else if (internalList[cacheKey].Count != 1) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimTypeIsMultiValue,
                                    claimType.DisplayName);
      } else {
        return internalList[cacheKey][0];
      }
    }


    internal Claim GetItem(ClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      if (!internalList.ContainsKey(cacheKey)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimTypeNotFound,
                                    claimType.DisplayName, Subject.ClaimsToken);
      }

      Claim claim = internalList[cacheKey].Find((x) => x.Value.ToLowerInvariant()
                                                                .Equals(claimValue.ToLowerInvariant()));

      if (claim != null) {
        return claim;
      } else {
        throw new SecurityException(SecurityException.Msg.SubjectClaimNotFound,
                                    claimType.DisplayName, Subject.ClaimsToken, claimValue);
      }
    }


    internal void RemoveSecure(ClaimType claimType, string claimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      var encryptedValue = FormerCryptographer.Encrypt(EncryptionMode.EntropyHashCode,
                                                       claimValue, cacheKey);

      Claim claim = this.GetItem(claimType, encryptedValue);

      ClaimsData.RemoveClaim(claim);

      internalList[cacheKey].Remove(claim);
    }


    public void ReplaceSecure(ClaimType claimType, string newClaimValue) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(newClaimValue, "newClaimValue");

      Claim claim = this.GetItem(claimType);

      ClaimsData.RemoveClaim(claim);

      this.AppendSecure(claimType, newClaimValue);
    }


    public void ReplaceSecure(ClaimType claimType,
                              string oldClaimValue, string newClaimValue) {
      if (!this.ContainsSecure(claimType, oldClaimValue)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimNotFound,
                                    claimType.DisplayName, Subject.ClaimsToken, oldClaimValue);
      }
      this.RemoveSecure(claimType, oldClaimValue);
      this.AppendSecure(claimType, newClaimValue);
    }


    public void ReplaceSecureWithToken(ClaimType claimType, ClaimType activationClaimType,
                                       string activationValue, string newClaimValue) {
      if (!this.ContainsSecure(activationClaimType, activationValue)) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimNotFound,
                                    claimType.DisplayName, Subject.ClaimsToken, activationValue);
      }
      this.AppendSecure(claimType, newClaimValue);
      this.RemoveSecure(activationClaimType, activationValue);
    }

    #endregion Public methods

    #region Private methods

    private Claim AppendUtil(ClaimType claimType,
                             string claimValue, EntityStatus status) {
      Assertion.AssertObject(claimType, "claimType");
      Assertion.AssertObject(claimValue, "claimValue");

      string cacheKey = BuildUniqueKey(claimType, this.Subject);

      if (!internalList.ContainsKey(cacheKey)) {
        internalList.Insert(cacheKey, new List<Claim>());
      }
      Claim claim = Claim.Create(claimType, this.Subject, claimValue, status);
      internalList[cacheKey].Add(claim);

      return claim;
    }


    static private string BuildUniqueKey(ClaimType claimType,
                                         IClaimsSubject subject) {
      string temp = $"{claimType.Id}~{subject.ClaimsToken}";

      return temp.ToLowerInvariant();
    }


    private void LoadClaimList() {
      FixedList<Claim> fullList = ClaimsData.GetSecurityClaims(this.Subject);

      foreach (Claim claim in fullList) {
        string cacheKey = BuildUniqueKey(claim.ClaimType, this.Subject);

        if (!internalList.ContainsKey(cacheKey)) {
          internalList.Insert(cacheKey, new List<Claim>());
        }
        internalList[cacheKey].Add(claim);
      }
    }

    #endregion Private methods

  } // class ClaimList

} // namespace Empiria.Security.Claims
