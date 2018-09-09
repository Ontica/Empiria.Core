/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Tests                                        Component : Security Services                     *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Test class                            *
*  Type     : AuthorizationServiceTests                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Authorization service tests.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

using Empiria.Security.Authorization;

namespace Empiria.Tests.Security {

  /// <summary>Authorization services tests.</summary>
  public class AuthorizationServiceTests {

    [Fact]
    public void ShouldRequestAnAuthorization() {
      CommonMethods.Authenticate();

      int initialCount = AuthorizationService.GetActive().Count;

      const string operationName = "TestOperation";
      string externalObjectUID = EmpiriaString.BuildRandomString(12, 36);

      var request = new AuthorizationRequest(operationName, externalObjectUID);

      Authorization authorization = AuthorizationService.Request(request);

      Assert.True(!authorization.IsEmptyInstance,
                  "Authorization must be not the empty instance.");

      Assert.Equal(AuthorizationStatus.Pending, authorization.Status);
      Assert.NotNull(authorization.Request);
      Assert.Equal(operationName, authorization.Request.Operation);
      Assert.Equal(externalObjectUID, authorization.Request.ExternalObjectUID);

      Assert.Equal(initialCount + 1, AuthorizationService.GetActive().Count);
    }

  }  // AuthorizationServiceTests

}  //namespace Empiria.Tests.Security
