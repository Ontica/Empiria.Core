/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : PartyRelationCategoryTests                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for PartyRelationCategory instances.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Parties;

namespace Empiria.Tests.Parties {

  /// <summary>Unit tests for PartyRelationCategory instances.</summary>
  public class PartyRelationCategoryTests {

    [Fact]
    public void Should_Get_All_Party_Relation_Categories() {
      var sut = PartyRelationCategory.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Parse_All_Party_Relation_Categories() {
      var roles = PartyRelationCategory.GetList();

      foreach (var sut in roles) {
        Assert.NotNull(sut.Name);
        Assert.NotNull(sut.NamedKey);
      }
    }


    [Fact]
    public void Should_Parse_Empty_PartyRelationCategory() {
      var sut = PartyRelationCategory.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(PartyRelationCategory.Parse("Empty"), sut);
    }

  }  // PartyRelationCategoryTests

}  // namespace Empiria.Tests.Parties
