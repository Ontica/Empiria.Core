/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : SecurityTests                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Security services tests.                                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using System.Threading;

using Empiria.Security;

namespace Empiria.Tests.Security {

  /// <summary>Security services tests.</summary>
  public class SecurityTests {

    [Fact]
    public void Should_Authenticate() {
      string sessionToken = ConfigurationData.GetString("Testing.SessionToken");
      string userHostAddress = ConfigurationData.GetString("Testing.UserHostAddress");

      IEmpiriaPrincipal principal = AuthenticationService.Authenticate(sessionToken, userHostAddress);

      Thread.CurrentPrincipal = principal;

      Assert.NotNull(principal);
    }

  }  // SecurityTests

}  // namespace Empiria.Tests.Security
