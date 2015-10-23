/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : SecurityClaim                                    Pattern  : Standard Class                    *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a security claim.                                                                  *
*                                                                                                            *
********************************* Copyright (c) 2009-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Security {

  //[DataModel("SecurityClaims", "SecurityClaimId")]
  public sealed class SecurityClaim : BaseObject {

    #region Constructors and parsers

    private SecurityClaim() {
      // Required by Empiria Framework
    }

    static public SecurityClaim Parse(int id) {
      return BaseObjectFactory.Parse<SecurityClaim>(id);
    }

    static private SecurityClaim Parse(DataRow row) {
      return BaseObjectFactory.Parse<SecurityClaim>(row);
    }

    static internal SecurityClaim Create(SecurityClaimType claimType,
                                         IIdentifiable resource, string claimValue,
                                         ObjectStatus status) {
      var newClaim = new SecurityClaim();

      newClaim.ClaimType = claimType;
      newClaim.ResourceTypeId = GetResourceTypeId(resource);
      newClaim.ResourceId = resource.Id;
      newClaim.Value = claimValue;
      newClaim.Status = status;

      SecurityData.WriteSecurityClaim(newClaim);

      return newClaim;
    }

    internal protected override void OnLoadObjectData(DataRow row) {
      this.ClaimType = SecurityClaimType.Parse((int) row["ClaimTypeId"]);
      this.ResourceTypeId = (int) row["ResourceTypeId"];
      this.ResourceId = (int) row["ResourceId"];
      this.Value = (string) row["ClaimValue"];
      this.Status = (ObjectStatus) Convert.ToChar((string) row["ClaimStatus"]);
    }

    static internal string BuildUniqueKey(SecurityClaimType claimType,
                                          IIdentifiable resource) {
      string temp = String.Format("{0}~{1}~{2}", claimType.Key,
                                  GetResourceTypeId(resource), resource.Id);

      return temp.ToLowerInvariant();
    }

    #endregion Constructors and parsers

    #region Properties

    public SecurityClaimType ClaimType {
      get;
      private set;
    }

    internal int ResourceTypeId {
      get;
      private set;
    }

    internal int ResourceId {
      get;
      private set;
    }

    internal string UniqueKey {
      get {
        string temp = String.Format("{0}~{1}~{2}", this.ClaimType.Key, this.ResourceTypeId, this.ResourceId);

        return temp.ToLowerInvariant();
      }
    }

    public string Value {
      get;
      private set;
    }

    public ObjectStatus Status {
      get;
      internal set;
    }

    #endregion Properties

    /// Remove this smell
    static internal int GetResourceTypeId(IIdentifiable resource) {
      string resourceType = resource.GetType().FullName;

      if (resourceType == "Empiria.Security.EmpiriaUser") {
        return 2;
      } else {
        return 1;
      }
    }

  } // class SecurityClaim

} // namespace Empiria.Security
