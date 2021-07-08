/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                      Component : Interface adapters                    *
*  Assembly : Empiria.Core.Services.dll                    Pattern   : Data Transfer Object                  *
*  Type     : PrincipalDto                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO with data representing an authenticated user or principal.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Services.Authentication {

  /// <summary>Output DTO with data representing an authenticated user or principal.</summary>
  public class PrincipalDto {

    internal PrincipalDto() {
      // no-op
    }


    public SecurityClaimDto[] Claims {
      get; internal set;
    }


    public IdentityDto Identity {
      get; internal set;
    }


    public string[] Roles {
      get; internal set;
    }
    public string[] Permissions {
      get;
      internal set;
    }
  }  // class PrincipalDto



  public class IdentityDto {

    internal IdentityDto() {
      // no-op
    }

    public string Name {
      get;
      internal set;
    }
  }  // class IdentityDto



  public class SecurityClaimDto {

    internal SecurityClaimDto() {
      // no-op
    }

    public string Type {
      get;
      internal set;
    }
    public string Value {
      get;
      internal set;
    }
  }  // class SecurityClaimDto


}  // namespace Empiria.Services.Authentication
