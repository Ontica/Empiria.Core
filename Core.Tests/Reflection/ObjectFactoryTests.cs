/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Test cases                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Unit tests                              *
*  Type     : ObjectFactoryTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : ObjectFactory unit tests.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Xunit;

using Empiria.Reflection;

namespace Empiria.Tests.Reflection {

  /// <summary>ObjectFactory unit tests.</summary>
  public class ObjectFactoryTests {

    [Fact]
    public void Should_Convert_A_String_Array_To_String_List() {
      var array = new string[] { "a", "b", "c" };

      var sut = ObjectFactory.Convert<List<string>>(array);

      Assert.Equal(array.Length, sut.Count);
    }

  }  // ObjectFactoryTests

}  // namespace Empiria.Tests.Reflection
