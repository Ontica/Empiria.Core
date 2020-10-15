/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core Tests                         Component : Json Tests                              *
*  Assembly : Empiria.Core.Tests.dll                     Pattern   : Test class                              *
*  Type     : JsonObjectTests                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : JsonObject methods tests.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Xunit;

namespace Empiria.Json.Tests {

  /// <summary>JsonObject methods tests.</summary>
  public class JsonObjectTests {

    [Fact]
    public void SetMethodOverInnerItemMustWork() {
      const string addedValue = "Hello world";

      var json = new JsonObject();

      json.Set("/a/b/c/d", addedValue);   // Tested method

      string actual = json.Get<string>("/a/b/c/d");

      Assert.Equal(addedValue, actual);

      var dictionary = json.ToDictionary();

      var traversalNode = (IDictionary<string, object>) dictionary["a"];
      traversalNode = (IDictionary<string, object>) traversalNode["b"];
      traversalNode = (IDictionary<string, object>) traversalNode["c"];

      Assert.Equal(addedValue, traversalNode["d"]);

    }


    [Fact]
    public void SetIfValueMethodWithEmptyValueMustCleanThePath() {
      const string addedValue = "Hello world";

      var json = new JsonObject();

      json.Set("/a/b/c/d", addedValue);

      string actual = json.Get<string>("/a/b/c/d");

      Assert.Equal(addedValue, actual);

      json.SetIfValue("/a/b/c/d", String.Empty);   // Tested method

      var dictionary = json.ToDictionary();

      Assert.True(dictionary.Count == 0, "Json object must be empty");

    }

    [Fact]
    public void RemoveMethodOverRootItemMustWork() {
      const string addedValue = "Hello world";

      var json = new JsonObject();

      json.Set("a", addedValue);

      json.Remove("a");                   // Tested method

      var dictionary = json.ToDictionary();

      Assert.True(dictionary.Count == 0, "Json object must be empty");

    }

    [Fact]
    public void RemoveMethodOverInnerItemMustWork() {
      const string addedValue = "Hello world";

      var json = new JsonObject();

      json.Set("/a/b/c/d", addedValue);

      json.Remove("/a/b/c/d");            // Tested method

      var dictionary = json.ToDictionary();

      var traversalNode = (IDictionary<string, object>) dictionary["a"];
      traversalNode = (IDictionary<string, object>) traversalNode["b"];
      traversalNode = (IDictionary<string, object>) traversalNode["c"];

      Assert.True(traversalNode.Count == 0, "Node C must not have elements.");

    }


    [Fact]
    public void CleanMethodOverInnerItemMustWork() {
      const string addedValue = "Hello world";

      var json = new JsonObject();

      json.Set("/a/b/c/d", addedValue);

      json.Remove("/a/b/c/d");

      json.Clean("/a/b/c/d");             // Tested method

      var dictionary = json.ToDictionary();

      Assert.True(dictionary.Count == 0, "Json object must be empty");

    }


    [Fact]
    public void CleanMethodOverInnerItemWithSiblingsMustWork() {
      const string addedValue = "Hello world";

      var json = new JsonObject();

      json.Set("/a/b1/c1/d", addedValue);
      json.Set("/a/b2/c2/d", addedValue);

      var dictionary = json.ToDictionary();

      var traversalNode = (IDictionary<string, object>) dictionary["a"];

      // Verify that node a has two children
      Assert.Equal(2, traversalNode.Count);

      json.Remove("/a/b1/c1/d");

      json.Clean("/a/b1/c1/d");             // Tested method

      dictionary = json.ToDictionary();

      Assert.Equal(1, dictionary.Count);

      // Verify that branch 2 is intact
      traversalNode = (IDictionary<string, object>) dictionary["a"];
      traversalNode = (IDictionary<string, object>) traversalNode["b2"];
      traversalNode = (IDictionary<string, object>) traversalNode["c2"];

      Assert.Equal(addedValue, traversalNode["d"]);

    }

  }  // EmpiriaStringTest

}  // namespace Empiria.Json.Tests
