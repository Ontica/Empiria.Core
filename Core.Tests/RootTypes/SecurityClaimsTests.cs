﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Tests                                        Component : Security Services                     *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Test class                            *
*  Type     : SecurityClaimsTests                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Security claims services tests.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

using Empiria.Security;
using Empiria.Security.Claims;

namespace Empiria.Tests {

  /// <summary>Security claims services tests.</summary>
  public class SecurityClaimsTests {

    [Fact]
    public void MustHaveClaimValue() {
      CommonMethods.Authenticate();

      ClaimsService.EnsureClaim(EmpiriaUser.Current, ClaimType.UserID, "AutoTester");
    }


    [Fact]
    public void MustGetClaimValue() {
      CommonMethods.Authenticate();

      var value = ClaimsService.GetClaimValue<string>(EmpiriaUser.Current,
                                                      ClaimType.ElectronicSignPrivateKeyFilePath);

      Assert.NotEmpty(value);
    }

  }  // SecurityClaimsTests

}  //namespace Empiria.Tests
