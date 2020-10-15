/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Ontology Tests                          *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Test class                              *
*  Type     : OntologyTests                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Empiria Ontology types tests.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

namespace Empiria.Ontology.Tests {

  /// <summary>Empiria Ontology types tests</summary>
  public class OntologyTests {

    [Fact]
    public void MustReadOntologyTypesData() {
      var typeInfo = ObjectTypeInfo.Parse(101);

      Assert.NotNull(typeInfo);
    }

  }  // OntologyTests

}  // namespace Empiria.Ontology.Tests
