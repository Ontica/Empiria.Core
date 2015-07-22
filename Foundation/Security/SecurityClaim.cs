/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : SecurityClaim                                    Pattern  : Standard Class                    *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
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
      // Required by Empiria Framework.
    }

    static public SecurityClaim Parse(int id) {
      return BaseObjectFactory.Parse<SecurityClaim>(id);
    }

    static private SecurityClaim Parse(DataRow row) {
      return BaseObjectFactory.Parse<SecurityClaim>(row);
    }

    internal protected override void OnLoadObjectData(DataRow row) {
      this.ClaimType = SecurityClaimType.Parse((int) row["ClaimTypeId"]);
      this.ResourceType = (string) row["ResourceType"];
      this.ResourceId = (int) row["ResourceId"];
      this.Value = (string) row["ClaimValue"];
      this.Status = (ObjectStatus) Convert.ToChar((string) row["ClaimStatus"]);

      this.UniqueKey = this.BuildUniqueKey();
    }

    static internal string BuildUniqueKey(SecurityClaimType claimType,
                                          IIdentifiable resource, string claimValue) {
      string temp = String.Format("{0}~{1}~{2}~{3}", claimType.Key,
                                  resource.GetType().FullName, resource.Id, claimValue);

      return temp.ToLowerInvariant();
    }

    private string BuildUniqueKey() {
      string temp = String.Format("{0}~{1}~{2}~{3}", this.ClaimType.Key,
                                  this.ResourceType, this.ResourceId, this.Value);

      return temp.ToLowerInvariant();
    }

    #endregion Constructors and parsers

    #region Properties

    public SecurityClaimType ClaimType {
      get;
      private set;
    }

    private string ResourceType {
      get;
      set;
    }

    private int ResourceId {
      get;
      set;
    }

    internal string UniqueKey {
      get;
      private set;
    }

    public string Value {
      get;
      private set;
    }

    public ObjectStatus Status {
      get;
      private set;
    }

    #endregion Properties

  } // class SecurityClaim

} // namespace Empiria.Security
