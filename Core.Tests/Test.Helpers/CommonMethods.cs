/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : General Test Helper Methods             *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Methods Library                         *
*  Type     : CommonMethods                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Auxiliary common methods used by test cases.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Security;

namespace Empiria.Tests {

  /// <summary>Auxiliary common methods used by test cases.</summary>
  static internal class CommonMethods {

    #region Auxiliary methods

    static internal void Authenticate() {
      string sessionToken = ConfigurationData.GetString("Testing.SessionToken");

      System.Threading.Thread.CurrentPrincipal = AuthenticationService.Authenticate(sessionToken);
    }

    #endregion Auxiliary methods

  }  // CommonMethods

}  // namespace Empiria.Tests
