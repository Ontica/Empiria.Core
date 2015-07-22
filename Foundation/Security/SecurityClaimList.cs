/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : SecurityClaimList                                Pattern  : Standard Class                    *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Holds the full collection of security claims for a given resource.                            *
*                                                                                                            *
********************************* Copyright (c) 2009-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Collections;

namespace Empiria.Security {

  /// <summary>Holds the full collection of security claims for a given resource.</summary>
  public sealed class SecurityClaimList : ObjectsCache<string, SecurityClaim> {
    #region Constructors and parsers

    public SecurityClaimList(IIdentifiable resource) {
      Assertion.AssertObject(resource, "resource");

      this.Resource = resource;
      this.ResourceType = resource.GetType().FullName;
      this.LoadClaimList();
    }

    #endregion Constructors and parsers

    #region Properties

    public string ResourceType {
      get;
      private set;
    }

    public IIdentifiable Resource {
      get;
      private set;
    }

    #endregion Properties

    #region Methods

    public bool Contains(SecurityClaimType claimType, string claimValue) {
      string cacheKey = SecurityClaim.BuildUniqueKey(claimType, this.Resource, claimValue);

      return (base.ContainsKey(cacheKey) &&
              base[cacheKey].Value.ToLowerInvariant() == claimValue.ToLowerInvariant());
    }

    private void LoadClaimList() {
      List<SecurityClaim> list = SecurityData.GetSecurityClaims(this.ResourceType, this.Resource);

      foreach(SecurityClaim claim in list) {
        base.Add(claim.UniqueKey, claim);
      }
    }

    #endregion Methods

  } // class EmpiriaClaim

} // namespace Empiria.Security
