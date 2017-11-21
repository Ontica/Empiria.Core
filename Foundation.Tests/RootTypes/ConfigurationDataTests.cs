using System;
using Xunit;

namespace Empiria {

  public class ConfigurationDataTests {

    //[Fact]
    //public void MustThrowAnErrorEveryTimeIfExecutionServerStartFailed() {
    //  // Remove ApplicationKey value from config file

    //  Assert.Throws(typeof(ExecutionServerException),
    //                () => ExecutionServer.DateMaxValue);

    //  Assert.Throws(typeof(ExecutionServerException),
    //                () => ExecutionServer.LicenseName);

    //  Assert.Throws(typeof(ExecutionServerException),
    //          () => ExecutionServer.LicenseSerialNumber);

    //  Assert.Throws(typeof(ExecutionServerException),
    //                () => ExecutionServer.DateMinValue);
    //}


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


    [Theory]
    [InlineData("§DataSource.Default")]
    public void ShouldReadProtectedSettings(string dataKey) {
      // Set Ok config file

      var value = ConfigurationData.Get<string>(typeof(Empiria.Data.DataReader), dataKey);

      Assert.NotNull(value);
    }

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

}  // namespace Empiria
