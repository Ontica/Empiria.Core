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

    static readonly string OPERATION_NAME = "TestOperation";

    [Fact]
    public void ShouldRequestAnAuthorization() {
      CommonMethods.Authenticate();

      int initialCount = this.GetPendingAuthorizations().Count;

      string externalObjectUID = EmpiriaString.BuildRandomString(12, 36);

      var request = new AuthorizationRequest(OPERATION_NAME, externalObjectUID);

      Authorization authorization = Authorization.Create(request);

      Assert.True(!authorization.IsEmptyInstance,
                  "Authorization must be not the empty instance.");

      Assert.Equal(AuthorizationStatus.Pending, authorization.Status);
      Assert.NotNull(authorization.Request);
      Assert.Equal(OPERATION_NAME, authorization.Request.OperationName);
      Assert.Equal(externalObjectUID, authorization.Request.ExternalObjectUID);

      Assert.Equal(initialCount + 1, this.GetPendingAuthorizations().Count);
    }


    [Fact]
    public void ShouldAuthorizeAnAuthorizationRequest() {
      CommonMethods.Authenticate();

      FixedList<Authorization> authorizations = this.GetPendingAuthorizations();

      if (authorizations.Count == 0) {
        return;
      }

      int initialCount = authorizations.Count;

      Authorization authorization = authorizations[0];

      authorization.Authorize(10, "Authorized by me");

      Assert.Equal(AuthorizationStatus.Authorized, authorization.Status);
      Assert.Equal("Authorized by me", authorization.AuthorizationNotes);
      Assert.NotNull(authorization.Request);
      Assert.True(authorization.AuthorizationTime <= DateTime.Now,
                  $"Unexpected authorization time {authorization.AuthorizationTime}.");

      Assert.Equal(initialCount - 1, this.GetPendingAuthorizations().Count);
    }


    [Fact]
    public void ShouldApplyAnAuthorizationRequest() {
      CommonMethods.Authenticate();

      FixedList<Authorization> authorizations = Authorization.Authorized();

      if (authorizations.Count == 0) {
        return;
      }

      int initialCount = authorizations.Count;

      Authorization authorization = authorizations[0];

      authorization.Apply();

      Assert.Equal(AuthorizationStatus.Closed, authorization.Status);

      Assert.Equal(initialCount - 1, Authorization.Authorized().Count);
    }


    #region Utility methods

    private FixedList<Authorization> GetPendingAuthorizations() {
      return Authorization.Pending(x => x.Request.OperationName == OPERATION_NAME);
    }

    #endregion Utility methods

  }  // AuthorizationServiceTests

}  //namespace Empiria.Tests.Security
