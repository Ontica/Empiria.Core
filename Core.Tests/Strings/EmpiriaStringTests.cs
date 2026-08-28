/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : EmpiriaStringTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Tests for EmpiriaString parsing and validation methods.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Globalization;
using System.Threading;

using Xunit;

namespace Empiria.Tests.Strings {

  /// <summary>Tests for EmpiriaString parsing and validation methods.</summary>
  public class EmpiriaStringTests {

    #region IsBoolean tests

    [Theory]
    [InlineData("1")]
    [InlineData("Y")]
    [InlineData("T")]
    [InlineData("S")]
    [InlineData("V")]
    [InlineData("TRUE")]
    [InlineData("true")]
    [InlineData("SI")]
    [InlineData("SÍ")]
    [InlineData("VERDADERO")]
    [InlineData("0")]
    [InlineData("N")]
    [InlineData("F")]
    [InlineData("FALSE")]
    [InlineData("false")]
    [InlineData("NO")]
    [InlineData("FALSO")]
    public void IsBoolean_Should_Return_True_For_Valid_Values(string value) {
      Assert.True(EmpiriaString.IsBoolean(value));
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("maybe")]
    [InlineData("2")]
    [InlineData("perhaps")]
    [InlineData("verdad")]
    public void IsBoolean_Should_Return_False_For_Invalid_Values(string value) {
      Assert.False(EmpiriaString.IsBoolean(value));
    }

    #endregion IsBoolean tests

    #region IsCurrency tests

    [Theory]
    [InlineData("100")]
    [InlineData("100.50")]
    [InlineData(".50")]
    [InlineData("0.50")]
    [InlineData("-100")]
    [InlineData("0")]
    public void IsCurrency_Should_Return_True_For_Valid_Values(string value) {
      Assert.True(EmpiriaString.IsCurrency(value));
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("100.50.25")]
    [InlineData("a1000")]
    public void IsCurrency_Should_Return_False_For_Invalid_Values(string value) {
      Assert.False(EmpiriaString.IsCurrency(value));
    }


    [Theory]
    [InlineData(null, "N2", false)]
    [InlineData("", "N2", false)]
    [InlineData("abc", "N2", false)]
    [InlineData("100", "N2", false)]
    [InlineData("100.5", "N2", false)]
    public void IsCurrency_With_Format_Should_Return_False_When_Not_Matching(string value,
                                                                             string format,
                                                                             bool expected) {
      Assert.Equal(expected, EmpiriaString.IsCurrency(value, format));
    }


    [Theory]
    [InlineData("100.00",   "N2", "es-MX", true)]
    [InlineData("1,000.00", "N2", "es-MX", true)]
    [InlineData("-50.2",    "N2", "es-MX", false)]
    [InlineData("100.00",   "N2", "en-US", true)]
    [InlineData("1,000.00", "N2", "en-US", true)]
    [InlineData("-50.2",    "N2", "en-US", false)]
    public void IsCurrency_With_Format_And_Culture_Should_Match(string value, string format,
                                                                string culture, bool expected) {
      var savedCulture = Thread.CurrentThread.CurrentCulture;
      try {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        Assert.Equal(expected, EmpiriaString.IsCurrency(value, format));
      } finally {
        Thread.CurrentThread.CurrentCulture = savedCulture;
      }
    }

    #endregion IsCurrency tests

    #region IsDateTime tests

    [Theory]
    [InlineData("25/Jan/2024", "dd/MMM/yyyy")]
    [InlineData("01/Feb/2000", "dd/MMM/yyyy")]
    public void IsDateTime_Should_Return_True_For_Valid_Values(string value, string format) {
      Assert.True(EmpiriaString.IsDateTime(value, format));
    }


    [Theory]
    [InlineData(null, "dd/MMM/yyyy")]
    [InlineData("", "dd/MMM/yyyy")]
    [InlineData("2024-01-25", "dd/MMM/yyyy")]
    [InlineData("25/13/2024", "dd/MM/yyyy")]
    [InlineData("not-a-date", "dd/MMM/yyyy")]
    public void IsDateTime_Should_Return_False_For_Invalid_Values(string value, string format) {
      Assert.False(EmpiriaString.IsDateTime(value, format));
    }

    #endregion IsDateTime tests

    #region IsDouble tests

    [Theory]
    [InlineData("100")]
    [InlineData("100.50")]
    [InlineData(".50")]
    [InlineData("0.50")]
    [InlineData("-100.75")]
    [InlineData("0")]
    public void IsDouble_Should_Return_True_For_Valid_Values(string value) {
      Assert.True(EmpiriaString.IsDouble(value));
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("100.50.25")]
    [InlineData("x100.5")]
    public void IsDouble_Should_Return_False_For_Invalid_Values(string value) {
      Assert.False(EmpiriaString.IsDouble(value));
    }


    [Theory]
    [InlineData(null, "N2", false)]
    [InlineData("", "N2", false)]
    [InlineData("abc", "N2", false)]
    [InlineData("100", "N2", false)]
    [InlineData("100.5", "N2", false)]
    public void IsDouble_With_Format_Should_Return_False_When_Not_Matching(string value,
                                                                           string format,
                                                                           bool expected) {
      Assert.Equal(expected, EmpiriaString.IsDouble(value, format));
    }


    [Theory]
    [InlineData("100.00",    "N2", "es-MX", true)]
    [InlineData("1,000.00",  "N2", "es-MX", true)]
    [InlineData("-50.2500",  "N4", "es-MX", true)]
    [InlineData("-50.2",     "N2", "es-MX", false)]
    [InlineData("100.00",    "N2", "en-US", true)]
    [InlineData("1,000.00",  "N2", "en-US", true)]
    [InlineData("-50.2500",  "N4", "en-US", true)]
    [InlineData("-50.2",     "N2", "en-US", false)]
    public void IsDouble_With_Format_And_Culture_Should_Match(string value, string format,
                                                              string culture, bool expected) {
      var savedCulture = Thread.CurrentThread.CurrentCulture;
      try {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        Assert.Equal(expected, EmpiriaString.IsDouble(value, format));
      } finally {
        Thread.CurrentThread.CurrentCulture = savedCulture;
      }
    }

    #endregion IsDouble tests

  }  // class EmpiriaStringTests

}  // namespace Empiria.Tests.Strings
