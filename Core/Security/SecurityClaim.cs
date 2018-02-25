/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : SecurityClaim                                    Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Represents a security claim.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Security {

  public sealed class SecurityClaim : BaseObject {

    #region Constructors and parsers

    private SecurityClaim() {
      // Required by Empiria Framework
    }


    static public SecurityClaim Parse(int id) {
      return BaseObject.ParseId<SecurityClaim>(id);
    }


    static public SecurityClaim Parse(string uid) {
      return BaseObject.ParseKey<SecurityClaim>(uid);
    }


    static internal SecurityClaim Create(SecurityClaimType claimType,
                                         IClaimsSubject subject, string claimValue,
                                         ObjectStatus status) {
      var newClaim = new SecurityClaim();

      newClaim.ClaimType = claimType;
      newClaim.Subject = subject;
      newClaim.Value = claimValue;
      newClaim.Status = status;

      SecurityData.WriteSecurityClaim(newClaim);

      return newClaim;
    }

    static internal string BuildUniqueKey(SecurityClaimType claimType,
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


    public SecurityClaimType ClaimType {
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

  } // class SecurityClaim

} // namespace Empiria.Security
