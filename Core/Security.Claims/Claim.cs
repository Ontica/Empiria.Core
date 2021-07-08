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

using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Security.Claims {

  /// <summary>Represents a security claim. Claims are partitioned types of ClaimType.</summary>
  [PartitionedType(typeof(ClaimType))]
  public sealed class Claim : BaseObject {

    #region Constructors and parsers

    private Claim(ClaimType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public Claim Parse(int id) {
      return BaseObject.ParseId<Claim>(id);
    }


    static public Claim Parse(string uid) {
      return BaseObject.ParseKey<Claim>(uid);
    }


    static internal Claim Create(ClaimType claimType, IClaimsSubject subject,
                                 string claimValue, EntityStatus status) {

      var newClaim = claimType.CreateObject<Claim>();

      newClaim.Subject = subject;
      newClaim.Value = claimValue;
      newClaim.Status = status;

      ClaimsData.WriteSecurityClaim(newClaim);

      return newClaim;
    }


    #endregion Constructors and parsers

    #region Properties

    public ClaimType ClaimType {
      get {
        return (ClaimType) base.GetEmpiriaType();
      }
    }

    public IClaimsSubject Subject {
      get;
      private set;
    }


    [DataField("ClaimValue")]
    public string Value {
      get;
      private set;
    }


    [DataField("ClaimStatus", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      internal set;
    }

    #endregion Properties

  } // class Claim

} // namespace Empiria.Security.Claims
