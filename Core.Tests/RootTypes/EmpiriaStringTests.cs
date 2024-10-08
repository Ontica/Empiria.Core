﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : EmpiriaStringTest                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : EmpiriaString methods tests.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

namespace Empiria.Tests.RootTypes {

  /// <summary>EmpiriaString methods tests.</summary>
  public class EmpiriaStringTest {

    [Fact]
    public void Should_Divide_Exact_Long_String() {
      int size = 10;
      var s = "123456789012345678901234567890123456789012345678901234567890";

      var result = EmpiriaString.DivideLongString(s, size, "|");

      Assert.Equal(result.Length, s.Length + s.Length / size);

      string[] resultArray = result.Split('|');

      for (int i = 0; i < resultArray.Length; i++) {
        Assert.True(resultArray[i].Length <= size, $"string number {i} too long.");
      }
    }


    [Fact]
    public void Should_Divide_Long_String_With_Remainder() {
      int size = 7;

      var s = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

      var result = EmpiriaString.DivideLongString(s, size, "|");

      Assert.Equal(result.Length, s.Length + s.Length / size);

      string[] resultArray = result.Split('|');

      for (int i = 0; i < resultArray.Length - 1; i++) {
        Assert.True(resultArray[i].Length == size, $"Wrong long for string number {i + 1}: {resultArray[i]}");
      }

    }

  }  // EmpiriaStringTest

}  // namespace Empiria.Tests.RootTypes
