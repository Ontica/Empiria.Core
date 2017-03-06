using System;
using Xunit;

namespace Empiria {

  public class ConfigurationDataTests {

    [Theory]
    [InlineData("ApplicationKey")]
    [InlineData("License.Name")]
    [InlineData("License.IsSpecial")]
    [InlineData("License.Number")]
    [InlineData("License.SerialNumber")]
    public void ShouldHaveMandatoryConfigData(string dataKey) {
      var value = ConfigurationData.Get<string>(dataKey);

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
      Assert.Throws(typeof(ConfigurationDataException),
                    () => ConfigurationData.Get<DateTime>("NotDefinedDateTimeValue"));
    }

  }  // ConfigurationDataTests

}  // namespace Empiria
