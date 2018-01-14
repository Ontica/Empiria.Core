using System;
using Xunit;

namespace Empiria {

  public class EmpiriaStringTest {

    [Fact]
    public void ShouldDivideLongStringExact() {
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
    public void ShouldDivideLongStringWithRemainder() {
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

}  // namespace Empiria
