/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : OperationSourceTests                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for OperationSource instances.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Parties;

namespace Empiria.Tests.Parties {

  /// <summary>Unit tests for OperationSource instances.</summary>
  public class OperationSourceTests {

    [Fact]
    public void Should_Get_Empty_OperationSource() {
      var sut = OperationSource.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.Equal(typeof(OperationSource), sut.GetEmpiriaType().UnderlyingSystemType);
    }


    [Fact]
    public void Should_Get_All_Operation_Sources() {
      var sut = OperationSource.GetList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

  }  // OperationSourceTests

}  // namespace Empiria.Tests.Parties
