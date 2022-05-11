/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                      Component : Interface adapters                    *
*  Assembly : Empiria.Core.Services.dll                    Pattern   : Data Transfer Object                  *
*  Type     : PrincipalDto                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO with data representing an authenticated user or principal.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Services.Authentication {

  /// <summary>Output DTO with data representing an authenticated user or principal.</summary>
  public class PrincipalDto {

    internal PrincipalDto() {
      // no-op
    }


    public IdentityDto Identity {
      get; internal set;
    }


    public string[] Permissions {
      get; internal set;
    }

  }  // class PrincipalDto



  public class IdentityDto {

    internal IdentityDto() {
      // no-op
    }

    public string Name {
      get; internal set;
    }

  }  // class IdentityDto

}  // namespace Empiria.Services.Authentication
