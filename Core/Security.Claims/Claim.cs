/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain service class                  *
*  Type     : SecurityClaim                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a security claim.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Security.Claims {

  /// <summary>Represents a security claim.</summary>
  public sealed class Claim : BaseObject {

    #region Constructors and parsers

    private Claim() {
      // Required by Empiria Framework
    }


    static public Claim Parse(int id) {
      return BaseObject.ParseId<Claim>(id);
    }


    static public Claim Parse(string uid) {
      return BaseObject.ParseKey<Claim>(uid);
    }


    static internal Claim Create(ClaimType claimType,
                                 IClaimsSubject subject, string claimValue,
                                 ObjectStatus status) {
      var newClaim = new Claim();

      newClaim.ClaimType = claimType;
      newClaim.Subject = subject;
      newClaim.Value = claimValue;
      newClaim.Status = status;

      ClaimsData.WriteSecurityClaim(newClaim);

      return newClaim;
    }

    static internal string BuildUniqueKey(ClaimType claimType,
                                          IClaimsSubject subject) {
      string temp = $"{claimType.Id}~{subject.ClaimsToken}";

      return temp.ToLowerInvariant();
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    public IClaimsSubject Subject {
      get;
      private set;
    }


    [DataField("ClaimExtData")]
    public JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("ClaimValue")]
    public string Value {
      get;
      private set;
    }


    public ClaimType ClaimType {
      //get {
      //  return (SecurityClaimType) base.GetEmpiriaType();
      //}
      get;
      private set;
    }


    [DataField("ClaimStatus", Default = ObjectStatus.Active)]
    public ObjectStatus Status {
      get;
      internal set;
    }

    #endregion Properties

  } // class Claim

} // namespace Empiria.Security.Claims
