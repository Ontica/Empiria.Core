/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : OntologyTests                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Empiria Ontology types tests.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Ontology;

namespace Empiria.Tests.Ontology {

  /// <summary>Empiria Ontology types tests.</summary>
  public class OntologyTests {

    [Fact]
    public void Should_Read_Ontology_ObjectTypeInfo() {
      var typeInfo = ObjectTypeInfo.Parse(101);

      Assert.NotNull(typeInfo);
    }

  }  // OntologyTests

}  // namespace Empiria.Tests.Ontology
