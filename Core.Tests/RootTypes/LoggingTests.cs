/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Logging services Tests                  *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Test class                              *
*  Type     : LoggingTests                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Logging service tests.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

namespace Empiria.Tests {

  /// <summary>Logging service tests.</summary>
  public class LoggingTests {

    [Fact]
    public void MustLogErrors() {
      var exception = new EntryPointNotFoundException();

      EmpiriaLog.Error(exception);

      // How to write the assertion?
      Assert.True(true);
    }

  }  // LoggingTests

}  // namespace Empiria.Tests
