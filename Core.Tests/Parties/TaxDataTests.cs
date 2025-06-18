/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : TaxDataTests                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for TaxData type.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Parties;

namespace Empiria.Tests.Parties {

  /// <summary>Unit tests for TaxData type.</summary>
  public class TaxDataTests {

    [Fact]
    public void Should_Get_An_Organization_TaxData() {
      var org = Organization.Parse(PartiesTestingConstants.ORGANIZATION_ID);

      TaxData sut = org.TaxData;

      Assert.NotEmpty(sut.TaxCode);
      Assert.NotEmpty(sut.TaxEntityName);
      Assert.NotEmpty(sut.TaxRegimeCode);
      Assert.NotEmpty(sut.TaxZipCode);
    }


    [Fact]
    public void Should_Set_An_Organization_TaxData() {
      var org = Organization.Parse(PartiesTestingConstants.ORGANIZATION_ID);

      TaxDataFields fields = new TaxDataFields {
        TaxCode = "von981012rEa",
        TaxEntityName = "la Vía óntica",
        TaxRegimeCode = "01",
        TaxZipCode = "90567"
      };

      org.SetTaxData(fields);

      TaxData sut = org.TaxData;

      Assert.Equal(fields.TaxCode.ToUpperInvariant(), sut.TaxCode);
      Assert.Equal(fields.TaxEntityName.ToUpperInvariant(), sut.TaxEntityName);
      Assert.Equal(fields.TaxRegimeCode.ToUpperInvariant(), sut.TaxRegimeCode);
      Assert.Equal(fields.TaxZipCode.ToUpperInvariant(), sut.TaxZipCode);
    }


    [Fact]
    public void Should_Get_A_Person_TaxData() {
      var person = Person.Parse(PartiesTestingConstants.PERSON_ID);

      TaxData sut = person.TaxData;

      Assert.NotEmpty(sut.TaxCode);
      Assert.NotEmpty(sut.TaxEntityName);
      Assert.NotEmpty(sut.TaxRegimeCode);
      Assert.NotEmpty(sut.TaxZipCode);
    }

    [Fact]
    public void Should_Set_A_Person_TaxData() {
      var person = Person.Parse(PartiesTestingConstants.PERSON_ID);

      TaxDataFields fields = new TaxDataFields {
        TaxCode = "maur981012rEa",
        TaxEntityName = "mauricio ramÍrez vega",
        TaxRegimeCode = "01",
        TaxZipCode = "81031"
      };

      person.SetTaxData(fields);

      TaxData sut = person.TaxData;

      Assert.Equal(fields.TaxCode.ToUpperInvariant(), sut.TaxCode);
      Assert.Equal(fields.TaxEntityName.ToUpperInvariant(), sut.TaxEntityName);
      Assert.Equal(fields.TaxRegimeCode.ToUpperInvariant(), sut.TaxRegimeCode);
      Assert.Equal(fields.TaxZipCode.ToUpperInvariant(), sut.TaxZipCode);
    }

  }  // TaxDataTests

}  // namespace Empiria.Tests.Parties
