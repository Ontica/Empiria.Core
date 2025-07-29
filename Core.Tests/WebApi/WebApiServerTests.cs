/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Web Api                                    Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : WebApiServerTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for WebApiServer instances.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.WebApi;

namespace Empiria.Tests.WebApi {

  /// <summary>Unit tests for WebApiServer instances.</summary>
  public class WebApiServerTests {

    [Fact]
    public void Should_Parse_All_Web_Api_Servers() {
      var webApiServers = BaseObject.GetFullList<WebApiServer>();

      foreach (var sut in webApiServers) {
        Assert.NotEmpty(sut.Name);
        Assert.NotEmpty(sut.BaseAddress);
        Assert.NotNull(sut.Credentials);
        Assert.True(sut.Credentials.AppKey.Length != 0 || !sut.IsEmpiriaBased);
        Assert.NotEmpty(sut.Credentials.UserID);
        Assert.NotEmpty(sut.Credentials.Password);
        Assert.NotNull(sut.ConfigData);
        Assert.NotNull(sut.Headers);
      }
    }


    [Fact]
    public void Should_Parse_Empty_WebApiServer() {
      var sut = WebApiServer.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(typeof(WebApiServer), sut.GetType());
    }

  }  // WebApiServerTests

}  // namespace Empiria.Tests.WebApi
