/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : PartiesTests                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Party instances tests.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Parties;

namespace Empiria.Tests.Parties {

  /// <summary>Party instances tests.</summary>
  public class PartiesTests {

    [Fact]
    public void Should_Get_Empty_Party() {
      var sut = Party.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Person"), sut.PartyType);
    }


    [Fact]
    public void Should_Get_An_Organization() {
      var sut = Organization.Parse(PartiesTestingConstants.ORGANIZATION_ID);

      Assert.Equal(PartiesTestingConstants.ORGANIZATION_ID, sut.Id);
      Assert.True(sut.Name.Length != 0);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Organization"), sut.PartyType);
    }


    [Fact]
    public void Should_Get_An_Organization_Through_Party() {
      var sut = Party.Parse(PartiesTestingConstants.ORGANIZATION_ID);

      Assert.Equal(PartiesTestingConstants.ORGANIZATION_ID, sut.Id);
      Assert.True(sut.Name.Length != 0);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Organization"), sut.PartyType);
    }


    [Fact]
    public void Should_Get_An_Organizational_Unit() {
      var sut = OrganizationalUnit.Parse(PartiesTestingConstants.ORGANIZATIONAL_UNIT_ID);

      Assert.Equal(PartiesTestingConstants.ORGANIZATIONAL_UNIT_ID, sut.Id);
      Assert.True(sut.Name.Length != 0);
      Assert.Equal(PartyType.Parse("ObjectType.Party.OrganizationalUnit"), sut.PartyType);
      Assert.True(sut.Code.Length != 0);
    }


    [Fact]
    public void Should_Get_An_Organizational_Unit_Through_Party() {
      var sut = Party.Parse(PartiesTestingConstants.ORGANIZATIONAL_UNIT_ID);

      Assert.Equal(PartiesTestingConstants.ORGANIZATIONAL_UNIT_ID, sut.Id);
      Assert.True(sut.Name.Length != 0);
      Assert.Equal(PartyType.Parse("ObjectType.Party.OrganizationalUnit"), sut.PartyType);
      Assert.True(((OrganizationalUnit) sut).Code.Length != 0);
    }


    [Fact]
    public void Should_Get_A_Person() {
      var sut = Person.Parse(PartiesTestingConstants.PERSON_ID);

      Assert.Equal(PartiesTestingConstants.PERSON_ID, sut.Id);
      Assert.True(sut.Name.Length != 0);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Person"), sut.PartyType);
      Assert.True(sut.FirstName.Length != 0);
      Assert.True(sut.LastName.Length != 0);
    }


    [Fact]
    public void Should_Get_A_Person_Through_Party() {
      var sut = Party.Parse(PartiesTestingConstants.PERSON_ID);

      Assert.Equal(PartiesTestingConstants.PERSON_ID, sut.Id);
      Assert.True(sut.Name.Length != 0);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Person"), sut.PartyType);
      Assert.True(((Person) sut).FirstName.Length != 0);
      Assert.True(((Person) sut).LastName.Length != 0);
    }

  }  // PartiesTests

}  // namespace Empiria.Tests
