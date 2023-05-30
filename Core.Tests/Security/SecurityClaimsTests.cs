/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Security Claims Tests                   *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Test class                              *
*  Type     : SecurityClaimsTests                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Security claims services tests.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;
using System.Threading;

using Empiria.Security;

namespace Empiria.Tests.Security {

  /// <summary>Security claims services tests.</summary>
  public class SecurityClaimsTests {

    [Fact]
    public void Authenticate() {
      string sessionToken = ConfigurationData.GetString("Testing.SessionToken");
      string userHostAddress = ConfigurationData.GetString("Testing.UserHostAddress");

      IEmpiriaPrincipal principal = AuthenticationService.Authenticate(sessionToken, userHostAddress);

      Thread.CurrentPrincipal = principal;

      Assert.NotNull(principal);
    }


    //[Fact]
    //public void MustGetClaimValue() {
    //  CommonMethods.Authenticate();

    //  var value = ClaimsService.GetClaimValue<string>(ExecutionServer.CurrentUser,
    //                                                  ClaimType.ElectronicSignPrivateKeyFilePath);

    //  Assert.NotEmpty(value);
    //}


    //[Fact]
    //public void MustHaveClaimValue() {
    //  CommonMethods.Authenticate();

    //  ClaimsService.EnsureClaim(ExecutionServer.CurrentUser, ClaimType.UserRole, "Tester");

    //  Assert.True(true);
    //}

  }  // SecurityClaimsTests

}  // namespace Empiria.Tests.Security
