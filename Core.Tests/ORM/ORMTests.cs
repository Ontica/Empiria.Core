/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Object-Relational Mappping Tests        *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Test class                              *
*  Type     : ORMTests                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Empiria Object-Relational Mappping (ORM) services tests.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

using Empiria.Contacts;

namespace Empiria.Tests {

  /// <summary>Empiria Object-Relational Mappping (ORM) services tests.</summary>
  public class ORMTests {

    [Fact]
    public void MustLoadObjectDataFields() {
      var contact = Contact.Parse(-3);

      Assert.Equal("ADMIN", contact.Nickname);
    }

  }  // ORMTests

}  // namespace Empiria.Tests
