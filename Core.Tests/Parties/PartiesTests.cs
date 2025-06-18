/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : PartiesTests                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for Party instances.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Parties;
using Empiria.Parties.Data;

namespace Empiria.Tests.Parties {

  /// <summary>Unit tests for Party instances.</summary>
  public class PartiesTests {

    [Fact]
    public void CleanParties() {
      var parties = BaseObject.GetFullList<Party>();

      foreach (var party in parties) {
        PartyDataService.CleanParty(party);
      }
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


    [Fact]
    public void Should_Parse_All_Parties() {
      var parties = BaseObject.GetFullList<Party>();

      foreach (var sut in parties) {
        Assert.NotNull(sut.Name);
      }
    }


    [Fact]
    public void Should_Parse_All_Persons() {
      var persons = BaseObject.GetFullList<Person>();

      foreach (var sut in persons) {
        Assert.NotEmpty(sut.FullName);
        Assert.NotEmpty(sut.FirstName);
        Assert.NotEmpty(sut.LastName);
        Assert.NotNull(sut.LastName2);
        Assert.NotNull(sut.TaxData);
      }
    }


    [Fact]
    public void Should_Parse_All_Organizations() {
      var organizations = BaseObject.GetFullList<Organization>();

      foreach (var sut in organizations) {
        Assert.NotEmpty(sut.Name);
        Assert.NotNull(sut.TaxData);
      }
    }


    [Fact]
    public void Should_Parse_All_OrganizationalUnits() {
      var organizationalUnits = BaseObject.GetFullList<OrganizationalUnit>();

      foreach (var sut in organizationalUnits) {
        Assert.NotEmpty(sut.Name);
        Assert.NotEmpty(sut.FullName);
        Assert.NotNull(sut.Code);
        Assert.NotNull(sut.Parent);
      }
    }


    [Fact]
    public void Should_Parse_Empty_Organization() {
      var sut = Organization.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Organization"), sut.PartyType);
    }


    [Fact]
    public void Should_Parse_Empty_OrganizationalUnit() {
      var sut = OrganizationalUnit.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(PartyType.Parse("ObjectType.Party.OrganizationalUnit"), sut.PartyType);
    }


    [Fact]
    public void Should_Parse_Empty_Party() {
      var sut = Party.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Person"), sut.PartyType);
    }


    [Fact]
    public void Should_Parse_Empty_Person() {
      var sut = Person.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(PartyType.Parse("ObjectType.Party.Person"), sut.PartyType);
    }

  }  // PartiesTests

}  // namespace Empiria.Tests.Parties
