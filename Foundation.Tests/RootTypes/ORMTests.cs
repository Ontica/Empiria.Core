using System;
using Xunit;

using Empiria.Contacts;

namespace Empiria {

  public class ORMTests {

    [Fact]
    public void MustLoadObjectDataFields() {
      var contact = Contact.Parse(-3);

      Assert.Equal(contact.Nickname, "ADMIN");
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

}  // namespace Empiria
