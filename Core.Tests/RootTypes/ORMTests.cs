/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Tests                                        Component : Object-Relational Mapping Services    *
*  Assembly : Empiria.Core.Tests.dll                       Pattern   : Test class                            *
*  Type     : ORMTests                                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Empiria Object-Relational Mappping (ORM) services tests.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
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

    //[Fact]
    //public void MustCreateInnerDataObjectInstance() {
    //  var contact = Contact.Parse(-3);

    //  //TestInnerDataObject dataObject = contact.TestInnerDataObject;

    //  //Assert.NotNull(dataObject);
    //}

    //[Fact]
    //public void MustParseInnerDataObjectProperties() {
    //  var contact = Contact.Parse(-3);

    //  TestInnerDataObject dataObject = contact.TestInnerDataObject;

    //  Assert.Equal(dataObject.ShortName, "Administrador");
    //}

  }  // ORMTests

}  // namespace Empiria.Tests
