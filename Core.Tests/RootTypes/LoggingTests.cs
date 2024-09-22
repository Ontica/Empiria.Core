/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : LoggingTests                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Logging service tests.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

namespace Empiria.Tests.RootTypes {

  /// <summary>Logging service tests.</summary>
  public class LoggingTests {

    [Fact]
    public void Sholud_Log_Errors() {
      var exception = new EntryPointNotFoundException();

      EmpiriaLog.Error(exception);

      Assert.True(true);
    }

  }  // LoggingTests

}  // namespace Empiria.Tests.RootTypes
