/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Tests                                        Component : Ontology Services                     *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Test class                            *
*  Type     : OntologyTests                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Empiria Ontology types tests.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Xunit;

using Empiria.Ontology;

namespace Empiria.Tests {

  /// <summary>Empiria Ontology types tests</summary>
  public class OntologyTests {

    [Fact]
    public void MustReadOntologyTypesData() {
      var typeInfo = ObjectTypeInfo.Parse(101);

      Assert.NotNull(typeInfo);
    }

  }  // OntologyTests

}  // namespace Empiria.Tests
