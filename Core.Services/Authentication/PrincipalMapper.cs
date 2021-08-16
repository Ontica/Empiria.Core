/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Interface adapters                      *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Mapper class                            *
*  Type     : PrincipalMapper                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for principal instances.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;

using Empiria.Security;
using Empiria.Security.Claims;

namespace Empiria.Services.Authentication {

  /// <summary>Mapping methods for principal instances.</summary>
  static internal class PrincipalMapper {

    internal static PrincipalDto Map(EmpiriaPrincipal principal) {
      return new PrincipalDto {
        Identity = MapIdentity(principal.Identity),
        Claims = MapSecurityClaims(principal),
        Roles = principal.RolesArray,
        Permissions = principal.PermissionsArray
      };
    }

    #region Helper methods

    static private IdentityDto MapIdentity(EmpiriaIdentity identity) {
      return new IdentityDto {
        Name = identity.Name
      };
    }


    static private SecurityClaimDto[] MapSecurityClaims(EmpiriaPrincipal principal) {
      return new SecurityClaimDto[0];

      //Claim[] claims = ClaimsService.Claims(principal);

      //return claims.Select(x => MapToSecurityClaim(x))
      //             .ToArray();
    }


    static private SecurityClaimDto MapToSecurityClaim(Claim claim) {
      return new SecurityClaimDto {
        Type = claim.ClaimType.NamedKey,
        Value = claim.Value,
      };
    }


    #endregion Helper methods

  }  // class PrincipalMapper

}  // namespace Empiria.Services.Authentication
