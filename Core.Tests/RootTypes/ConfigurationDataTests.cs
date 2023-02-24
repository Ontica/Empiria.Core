/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : ConfigurationData Tests                 *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Test class                              *
*  Type     : ConfigurationDataTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Configuration data tests.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

namespace Empiria.Tests {

  /// <summary>Configuration data tests.</summary>
  public class ConfigurationDataTests {

    [Fact]
    public void ShouldGetConfigDataValueFromExecutionServer() {
      // Set Ok config file

      var value = ExecutionServer.LicenseName;
      Assert.True(!String.IsNullOrWhiteSpace(value));

      value = ExecutionServer.LicenseSerialNumber;
      Assert.True(!String.IsNullOrWhiteSpace(value));
    }


    [Theory]
    [InlineData("ApplicationKey")]
    [InlineData("License.Name")]
    [InlineData("License.Number")]
    [InlineData("License.SerialNumber")]
    public void ShouldHaveMandatoryConfigData(string dataKey) {
      // Set Ok config file

      var value = ConfigurationData.Get<string>(dataKey);

      Assert.NotNull(value);

      Assert.DoesNotContain("{{", value);
      Assert.DoesNotContain("}}", value);
    }


    //[Theory]
    //[InlineData("§RSACryptoFile")]
    //public void ShouldReadProtectedSettings(string dataKey) {
    //  // Set Ok config file

    //  var value = ConfigurationData.Get<string>(typeof(Empiria.Security.ClientApplication), dataKey);

    //  Assert.NotNull(value);
    //}

    [Fact]
    public void MustReturnDefaultValueWhenNoConfigData() {
      DateTime defaultValue = DateTime.Parse("2012-06-25");

      var data = ConfigurationData.Get<DateTime>("NotDefinedDateTimeValue", defaultValue);

      Assert.Equal(data, defaultValue);
    }


    [Fact]
    public void ThrowWhenNoDataAndNoDefaultValue() {
      Assert.Throws<ConfigurationDataException>(() =>
                                                ConfigurationData.Get<DateTime>("NotDefinedDateTimeValue"));
    }


  }  // ConfigurationDataTests

}  // namespace Empiria.Tests
