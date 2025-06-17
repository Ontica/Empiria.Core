/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : PartyRoleTests                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for PartyRole instances.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Parties;

namespace Empiria.Tests.Parties {

  /// <summary>Unit tests for PartyRole instances.</summary>
  public class PartyRoleTests {

    [Fact]
    public void Should_Get_All_Party_Roles() {
      var sut = PartyRole.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Party_Roles() {
      var roles = PartyRole.GetList();

      foreach (var sut in roles) {
        Assert.NotNull(sut.Name);
        Assert.NotNull(sut.NamedKey);
        Assert.NotNull(sut.Category);
      }
    }


    [Fact]
    public void Should_Parse_Empty_PartyRole() {
      var sut = PartyRole.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(PartyRole.Parse("Empty"), sut);
      Assert.True(sut.Category.IsEmptyInstance);
    }

  }  // PartiesTests

}  // namespace Empiria.Tests.Parties
