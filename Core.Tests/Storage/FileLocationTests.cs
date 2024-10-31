/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : FileLocationTests                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FileLocation instances.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Storage;

namespace Empiria.Tests.Storage {

  /// <summary>Unit tests for FileLocation instances.</summary>
  public class FileLocationTests {

    [Fact]
    public void Should_Get_All_File_Locations() {
      var sut = BaseObject.GetFullList<FileLocation>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Empty_FileLocation() {
      var sut = FileLocation.Empty;

      Assert.Equal(-1, sut.Id);
      Assert.Equal("Empty", sut.UID);
      Assert.NotEmpty(sut.Name);
    }


    [Fact]
    public void Should_Parse_All_File_Locations() {
      var fileLocations = BaseObject.GetFullList<FileLocation>();

      foreach (var sut in fileLocations) {
        Assert.NotEmpty(sut.Name);
        Assert.NotEmpty(sut.BaseFileDirectory);
        Assert.NotEmpty(sut.BaseUrl);

      }
    }

  }  // class FileLocationTests

}  // namespace Empiria.Tests.Storage
