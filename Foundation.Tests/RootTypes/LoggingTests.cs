using System;
using Xunit;

using Empiria.Ontology;

namespace Empiria {

  public class LoggingTests {

    [Fact]
    public void MustLogErrors() {
      var exception = new EntryPointNotFoundException();

      EmpiriaLog.Error(exception);

      // How to write the assertion?
    }

  }  // LoggingTests

}  // namespace Empiria
