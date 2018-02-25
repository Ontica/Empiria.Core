using System;
using Xunit;

using Empiria.Ontology;

namespace Empiria {

  public class OntologyTests {

    [Fact]
    public void MustReadOntologyTypesData() {
      var typeInfo = ObjectTypeInfo.Parse(101);

      Assert.NotNull(typeInfo);
    }

  }  // OntologyTests

}  // namespace Empiria
