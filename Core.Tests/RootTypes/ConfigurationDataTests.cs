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

namespace Empiria.Tests.RootTypes {

  /// <summary>Configuration data tests.</summary>
  public class ConfigurationDataTests {

    [Fact]
    public void Should_Get_ConfigDataValue_From_ExecutionServer() {
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
    public void Should_Have_Mandatory_ConfigData(string dataKey) {
      // Set Ok config file

      var value = ConfigurationData.Get<string>(dataKey);

      Assert.NotNull(value);

      Assert.DoesNotContain("{{", value);
      Assert.DoesNotContain("}}", value);
    }


    [Fact]
    public void Should_Return_DefaultValue_When_Not_ConfigData() {
      DateTime defaultValue = DateTime.Parse("2012-06-25");

      var data = ConfigurationData.Get<DateTime>("NotDefinedDateTimeValue", defaultValue);

      Assert.Equal(data, defaultValue);
    }


    [Fact]
    public void Sholud_Throw_Exception_When_NoData_And_Not_DefaultValue() {
      Assert.Throws<ConfigurationDataException>(() =>
                                                ConfigurationData.Get<DateTime>("NotDefinedDateTimeValue"));
    }


  }  // ConfigurationDataTests

}  // namespace Empiria.Tests.RootTypes
