/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Json services                                Component : Json provider                         *
*  Assembly : Empiria.Core.dll                             Pattern   : Methods library                       *
*  Type     : JsonObject                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Allows data reading and parsing of JSON strings.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Reflection;

namespace Empiria.Json {

  public class JsonObject : IEnumerable {

    #region Fields

    private IDictionary<string, object> dictionary = null;

    #endregion Fields

    #region Constructors and parsers

    public JsonObject() {
      this.dictionary = new Dictionary<string, object>();
    }

    internal JsonObject(IDictionary<string, object> items) {
      Assertion.AssertObject(items, "items");

      this.dictionary = items;
    }

    static public JsonObject Parse(string jsonString) {
      if (String.IsNullOrWhiteSpace(jsonString)) {
        return JsonObject.Empty;
      }
      var dictionary = JsonConverter.ToDictionary(jsonString);

      return new JsonObject(dictionary);
    }

    static public JsonObject Parse(object instance) {
      if (instance == null) {
        return JsonObject.Empty;
      }
      var jsonString = JsonConverter.ToJson(instance);

      return JsonObject.Parse(jsonString);
    }

    static private readonly JsonObject _empty = new JsonObject() { IsEmptyInstance = true };
    static public JsonObject Empty {
      get {
        return _empty;
      }
    }

    #endregion Constructors and parsers

    #region Properties

    /// <summary>Returns true if the json structure contains one ore more items.</summary>
    public bool HasItems {
      get {
        return (this.dictionary.Count != 0);
      }
    }


    /// <summary>Returns true when the json structure is marked as the empty instance.</summary>
    public bool IsEmptyInstance {
      get;
      private set;
    }

    #endregion Properties

    #region Public methods


    /// <summary>Adds a new json item to the root of this json object.</summary>
    /// <param name="key">The key or name of the item to add.</param>
    /// <param name="value">The value of the added object.</param>
    public void Add(string key, object value) {
      Assertion.AssertObject(key, "key");
      Assertion.AssertObject(value, "value");

      dictionary.Add(key, value);
    }

    /// <summary>Adds a new json object to the json tree structure.</summary>
    /// <param name="key">The item key that describes the json object</param>
    /// <param name="jsonObject">The json object to add.</param>
    public void Add(string key, JsonObject jsonObject) {
      Assertion.AssertObject(key, "key");
      Assertion.AssertObject(jsonObject, "jsonObject");

      dictionary.Add(key, jsonObject.ToDictionary());
    }


    /// <summary>Adds a new json item to the root of this json object, in the case
    /// that condition is true and value is not null or empty.</summary>
    /// <param name="key">The key or name of the item to add.</param>
    /// <param name="value">The value of the object added</param>
    public void AddIf(string key, object value, bool condition) {
      Assertion.AssertObject(key, "key");
      if (!condition) {
        return;
      }

      Assertion.AssertObject(value, "value");

      if (HasEmptyOrNullValue(value)) {
        return;
      }

      dictionary.Add(key, value);
    }


    /// <summary>Adds a new json item to the root of this json object, in the case
    /// that value is not an empty value or null instance.</summary>
    /// <param name="key">The key or name of the item to add.</param>
    /// <param name="value">The value of the object added</param>
    public void AddIfValue(string key, object value) {
      Assertion.AssertObject(key, "key");

      if (HasEmptyOrNullValue(value)) {
        return;
      }

      dictionary.Add(key, value);
    }


    /// <summary>Cleans the json object removing all empty objects from the given itemPath.</summary>
    /// <param name="itemPath">The item path to clean.</param>
    public void Clean(string itemPath) {
      Assertion.AssertObject(itemPath, "itemPath");

      if (RemoveIfEmptyOrNull(dictionary, itemPath)) {
        return;
      }

      if (dictionary.ContainsKey(itemPath) &&
          HasEmptyOrNullValue(itemPath)) {     // Fast-track case

        dictionary.Remove(itemPath);


      }

      string traversingPath = itemPath;

      while (true) {

        (string parentPath, string childKey) = GetParentChild(traversingPath);

        if (IsRoot(parentPath) && childKey.Length != 0) {

          RemoveIfEmptyOrNull(dictionary, childKey);

          if (dictionary.ContainsKey(childKey) &&
              HasEmptyOrNullValue(dictionary[childKey])) {

            dictionary.Remove(childKey);

          }
          return;

        } else if (IsRoot(parentPath) && childKey.Length == 0) {

          return;

        } else if (!IsRoot(parentPath)) {

          var parentNode = TryGetDictionaryValue(parentPath, false);

          if (parentNode != null) {
            var temp = (IDictionary<string, object>) parentNode;

            RemoveIfEmptyOrNull(temp, childKey);

          }

        }  // if

        traversingPath = parentPath;

      }   // while

    }


    /// <summary>Returns true if the json object has an item path.</summary>
    /// <param name="itemPath">The item path to search.</param>
    public bool Contains(string itemPath) {
      Assertion.AssertObject(itemPath, "itemPath");

      string[] pathMembers = SplitItemPath(itemPath);

      IDictionary<string, object> item = dictionary;

      for (int i = 0; i < pathMembers.Length; i++) {

        if (!item.ContainsKey(pathMembers[i])) {
          return false;
        }

        if (i == (pathMembers.Length - 1)) {  // The last item is the searched item in the path
          return true;
        }

        if (item[pathMembers[i]] is IDictionary<string, object>) {
          item = (IDictionary<string, object>) item[pathMembers[i]];

        } else {            // This item is a scalar (not a subtree), so the next item
          return false;     // in the path necessarily doesn't exist.

        }

      }  // for

      throw Assertion.AssertNoReachThisCode();
    }


    /// <summary>Searches for an item inside the JsonObject.</summary>
    /// <param name="itemPath">The item path to search.</param>
    /// <returns>The item relative to the searched path, or an exception if the object
    /// was not found or if the path is not well-formed.</returns>
    public T Get<T>(string itemPath) {
      Assertion.AssertObject(itemPath, "itemPath");

      return this.Find<T>(itemPath);
    }


    /// <summary>Extracts a new JsonObject from this instance given an itemPath.</summary>
    /// <param name="itemPath">The item path to search.</param>
    /// <param name="defaultValue">The default value if the searched item is not found.</param>
    /// <returns>The item relative to the searched path, the defaultValue if the object
    /// was not found or an exception if the path is not well-formed.</returns>
    public T Get<T>(string itemPath, T defaultValue) {
      Assertion.AssertObject(itemPath, "itemPath");

      return this.Find<T>(itemPath, defaultValue);
    }


    /// <summary>Searches for a string item inside the JsonObject and returns it cleaned
    /// without any whitespaces or control characters.</summary>
    /// <param name="itemPath">The item path to search</param>
    /// <returns>The item relative to the searched path, or an exception if the object
    /// was not found or if the path is not well-formed.</returns>
    public string GetClean(string itemPath, string defaultValue = null) {
      Assertion.AssertObject(itemPath, "itemPath");

      string value = String.Empty;

      if (defaultValue == null) {
        value = this.Find<string>(itemPath);
      } else {
        value = this.Find<string>(itemPath, defaultValue);
      }

      return EmpiriaString.Clean(value);
    }


    IEnumerator IEnumerable.GetEnumerator() {
      return dictionary.GetEnumerator();
    }


    /// <summary>Searches for a list of objects inside the JsonObject.</summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="listPath">The list path to search.</param>
    /// <returns>The list of objects relative to the searched path, or an exception if the list
    /// was not found or if the path is not well-formed.</returns>
    public List<T> GetList<T>(string listPath) {
      Assertion.AssertObject(listPath, "listPath");

      return GetList<T>(listPath, true);
    }

    /// <summary>Searches for a list of objects inside the JsonObject.</summary>
    /// <typeparam name="T">The type of the list elements.</typeparam>
    /// <param name="listPath">The list path to search.</param>
    /// <param name="required">Throws an exception if is true and the searched item was not found.</param>
    /// <returns>The list of objects relative to the searched path, or an exception if the list
    /// was not found or if the path is not well-formed.</returns>
    public List<T> GetList<T>(string listPath, bool required) {
      Assertion.AssertObject(listPath, "listPath");

      List<object> objectsList;

      if (required) {
        objectsList = this.Get<List<object>>(listPath);
      } else {
        objectsList = this.Get<List<object>>(listPath, new List<object>());
      }

      if (ObjectFactory.IsIdentifiable(typeof(T))) {

        if (objectsList.Count == 0) {
          return new List<T>();
        }

        if (objectsList.TrueForAll((x) => EmpiriaString.IsInteger(Convert.ToString(x)))) {

          Assertion.Assert(ObjectFactory.HasParseWithIdMethod(typeof(T)),
                           $"Type {typeof(T).FullName} doesn't have defined a static Parse(int) method.");

          return objectsList.ConvertAll<T>((x) => ObjectFactory.InvokeParseMethod<T>(Convert.ToInt32(x)));

        } else {

          Assertion.Assert(ObjectFactory.HasParseWithStringMethod(typeof(T)),
                           $"Type {typeof(T).FullName} doesn't have defined a static Parse(string) method.");

          return objectsList.ConvertAll<T>((x) => ObjectFactory.InvokeParseMethod<T>(Convert.ToString(x)));
        }

      } else if (ObjectFactory.IsValueObject(typeof(T))) {

        return objectsList.ConvertAll<T>((x) =>
                                         ObjectFactory.InvokeParseMethod<T>(Convert.ToString(x)));

      } else if (ObjectFactory.HasJsonParser(typeof(T))) {

        return objectsList.ConvertAll<T>(
                  (x) => ObjectFactory.InvokeParseJsonMethod<T>(new JsonObject((IDictionary<string, Object>) x))
               );

      } else {

        try {
          return objectsList.ConvertAll<T>((x) => ObjectFactory.Convert<T>(x));
        } catch (Exception e) {
          throw new JsonDataException(JsonDataException.Msg.JsonListTypeConvertionFails, e,
                                      listPath, typeof(T).ToString());
        }

      }
    }


    /// <summary>Checks if there is an item in the json path with a none empty or null value.</summary>
    /// <param name="itemPath">The json path to the object to ckeck its value.</param>
    /// <returns>True if the Json structure has an object with a non empty
    /// or null value. False otherwise.</returns>
    public bool HasValue(string itemPath) {
      Assertion.AssertObject(itemPath, "itemPath");

      object value = this.TryGetDictionaryValue(itemPath, false);

      if (value == null) {
        return false;
      }

      return !HasEmptyOrNullValue(value);
    }


    /// <summary>Removes the item pointed by the itemPath.</summary>
    /// <param name="itemPath">The item path of the object to remove.</param>
    /// <returns>True if the object was located and removed, false otherwise.</returns>
    public bool Remove(string itemPath) {

      if (dictionary.ContainsKey(itemPath)) {     // Fast-track case
        dictionary.Remove(itemPath);

        return true;
      }

      string[] pathMembers = SplitItemPath(itemPath);

      IDictionary<string, object> item = dictionary;

      for (int i = 0; i < pathMembers.Length; i++) {

        if (!item.ContainsKey(pathMembers[i])) {

          return false;  // The item or the path to it do not exist, so there is nothing to remove

        } else {

          if (i == (pathMembers.Length - 1)) {  // Remove the last path item and finish

            item.Remove(pathMembers[i]);

            return true;

          } else {    // Do nothing. Just continue processing the next path item

            item = (IDictionary<string, object>) item[pathMembers[i]];

          }

        }

      }  // for

      return false;
    }


    /// <summary>Updates or adds a new JsonObject from this instance given an itemPath.</summary>
    /// <param name="itemPath">The item path to set (e.g. '/parent/child/item' or just 'item').</param>
    /// <param name="value">The value of the item to set.</param>
    public void Set(string itemPath, object value) {
      Assertion.AssertObject(itemPath, "itemPath");

      if (dictionary.ContainsKey(itemPath)) {   // Fast-track case
        dictionary[itemPath] = value;

        return;
      }

      string[] pathMembers = SplitItemPath(itemPath);

      IDictionary<string, object> node = dictionary;

      for (int i = 0; i < pathMembers.Length; i++) {

        if (!node.ContainsKey(pathMembers[i])) {

          if (i == (pathMembers.Length - 1)) {  // Add the item in the path with its value and finish

            node.Add(pathMembers[i], value);

            return;

          } else {    // Add a new Json node and continue processing the next path section

            var newNode = new JsonObject().ToDictionary();

            node.Add(pathMembers[i], newNode);

            node = newNode;
          }

        } else {    // Node contains pathMembers[i]

          if (i == (pathMembers.Length - 1)) {  // Replace the last path item with its value and finish

            node[pathMembers[i]] = value;

            return;

          } else {    // Do nothing. Just continue processing the next path item

            node = (IDictionary<string, object>) node[pathMembers[i]];
          }

        }

      }  // for

      throw Assertion.AssertNoReachThisCode();    // Code must exit from above code
    }


    /// <summary>Updates or adds a new JsonObject from this instance given an itemPath.</summary>
    /// <param name="itemPath">The item path to set (e.g. like '/parent/child/item' or just 'item').</param>
    /// <param name="jsonObject">The json object to add.</param>
    public void Set(string itemPath, JsonObject jsonObject) {
      Assertion.AssertObject(itemPath, "itemPath");
      Assertion.AssertObject(jsonObject, "jsonObject");

      Set(itemPath, jsonObject.ToDictionary());
    }


    /// <summary>Updates or adds a new JsonObject from this instance given an itemPath.
    /// If value has no value (null or empty), removes it from the structure if already exists.</summary>
    /// <param name="itemPath">The item path to set (e.g. like '/parent/child/item' or just 'item').</param>
    /// <param name="value">The value of the item to set.</param>
    public void SetIfValue(string itemPath, object value) {
      Assertion.AssertObject(itemPath, "itemPath");

      if (HasEmptyOrNullValue(value)) {  // Remove the item and clean up the path against empty objects

        this.Remove(itemPath);
        this.Clean(itemPath);

      } else {                           // If has value, just add or update it.

        this.Set(itemPath, value);
      }
    }

    /// <summary>Updates or adds a new JsonObject from this instance given an itemPath.</summary>
    /// <param name="itemPath">The item path to set (e.g. like '/parent/child/item' or just 'item').</param>
    /// <param name="jsonObject">The json object to add.</param>
    public void SetIfValue(string itemPath, JsonObject jsonObject) {
      Assertion.AssertObject(itemPath, "itemPath");
      Assertion.AssertObject(jsonObject, "jsonObject");

      SetIfValue(itemPath, jsonObject.ToDictionary());
    }


    /// <summary>Extracts a new JsonObject from this instance given an itemPath.</summary>
    /// <param name="itemPath">The item path to search. If starts with '@' then the object name
    /// is also included in the returned object, else only the item path contents.</param>
    /// <returns>The JsonObject relative to the searched path, or the JsonObject.Empty
    /// instance if the path is not found or an exception if the path is not well-formed.</returns>
    public JsonObject Slice(string itemPath) {
      Assertion.AssertObject(itemPath, "itemPath");

      return this.Slice(itemPath, false);
    }


    /// <summary>Generates a new JsonObject from multiple itemPaths of this instance.</summary>
    /// <param name="itemPaths">Array with the item paths to include in the new JsonObject.
    /// If any of those paths start with '@', then that path's object name is also included in the
    /// returned object, otherwise the objects inside it are returned as direct items of the root.</param>
    /// <returns>The new JsonObject generated from the items path, or the JsonObject.Empty
    /// instance if any path was found or an exception if one of the paths are not well-formed.</returns>
    public JsonObject Slice(string[] itemPaths) {
      Assertion.AssertObject(itemPaths, "itemPaths");

      var root = new Dictionary<string, object>(itemPaths.Length);

      for (int i = 0; i < itemPaths.Length; i++) {
        var json = this.Slice(itemPaths[i]);

        foreach (var item in json.dictionary) {
          root.Add(item.Key, item.Value);
        }

      }

      return new JsonObject(root);
    }


    /// <summary>Extracts a new JsonObject from this instance given an itemPath.</summary>
    /// <param name="itemPath">The item path to search. If starts with '@' then the object name
    /// is included in the returned object, else only the item path contents.</param>
    /// <param name="required">Throws an exception if is true and the searched item was not found.</param>
    /// <returns>The JsonObject relative to the searched path, or the JsonObject.Empty
    /// instance if the required flag is false. Otherwise throws an exception.</returns>
    public JsonObject Slice(string itemPath, bool required) {
      Assertion.AssertObject(itemPath, "itemPath");

      bool includeitemNameInSlice = false;

      if (itemPath.StartsWith("@")) {
        includeitemNameInSlice = true;
        itemPath = itemPath.Substring(1);   // Remove the special character @
      }

      object value = this.TryGetDictionaryValue(itemPath, required);

      if (value == null && required) {
        // An exception should be thrown from the TryGetDictionaryValue call above.
        Assertion.AssertNoReachThisCode();
      }

      if (value == null) {
        return JsonObject.Empty;

      } else if (value is IDictionary<string, object> &&
                 !includeitemNameInSlice) {
        return new JsonObject((IDictionary<string, object>) value);

      } else {

        var dictionary = new Dictionary<string, object>(1);

        dictionary.Add(GetChildKey(itemPath), value);

        return new JsonObject(dictionary);

      }
    }


    /// <summary>Returns this Json object as a Json string.</summary>
    public override string ToString() {
      return this.ToString(false);
    }


    /// <summary>Returns this Json object as a Json string.</summary>
    /// <param name="indented">If true, returns the Json string in indented format. Default false.</param>
    public string ToString(bool indented = false) {
      if (this.dictionary.Count == 0) {
        return String.Empty;
      }

      if (indented) {
        return JsonConverter.ToJsonIndented(this.dictionary);
      } else {
        return JsonConverter.ToJson(this.dictionary);
      }
    }


    public dynamic ToObject() {
      return this.ToDictionary();
    }


    public IDictionary<string, object> ToDictionary() {
      var copy = new Dictionary<string, object>(this.dictionary);

      foreach (var item in this.dictionary) {

        if (item.Value is JsonObject) {
          copy[item.Key] = ((JsonObject) item.Value).ToDictionary();
        }

      }

      return copy;
    }


    #endregion Public methods

    #region Private methods

    private T Find<T>(string itemPath) {
      object value = this.TryGetDictionaryValue(itemPath, true);

      if (value == null) {
        // An exception should be thrown from the this.TryGetDictionaryValue call above.
        Assertion.AssertNoReachThisCode();
      }

      try {
        return ObjectFactory.Convert<T>(value);

      } catch (Exception e) {
        throw new JsonDataException(JsonDataException.Msg.JsonValueTypeConvertionFails, e,
                                    itemPath, value.ToString(), value.GetType().ToString(),
                                    typeof(T).ToString());

      }
    }


    private T Find<T>(string itemPath, T defaultValue) {
      object value = this.TryGetDictionaryValue(itemPath, false);

      // Return defaultValue when retrieved item is null
      if (value == null) {
        return defaultValue;
      }

      // Return defaultValue for empty strings values of some types;
      if (value is string && ((string) value).Length == 0) {
        var convertToType = typeof(T);

        if (convertToType == typeof(DateTime)) {
          return defaultValue;
        }
      }

      try {
        return ObjectFactory.Convert<T>(value);

      } catch (Exception e) {
        throw new JsonDataException(JsonDataException.Msg.JsonValueTypeConvertionFails, e,
                                    itemPath, value.ToString(), value.GetType().ToString(),
                                    typeof(T).ToString());
      }
    }


    private object TryGetDictionaryValue(string itemPath, bool required) {
      string[] pathMembers = SplitItemPath(itemPath);

      IDictionary<string, object> item = dictionary;
      for (int i = 0; i < pathMembers.Length; i++) {

        if (!item.ContainsKey(pathMembers[i])) {

          if (!required) {
            return null;

          } else {
            throw new JsonDataException(JsonDataException.Msg.JsonPathItemNotFound,
                                        itemPath, pathMembers[i]);
          }
        }

        if (i == (pathMembers.Length - 1)) {  // The last item is the searched item in the path
          return item[pathMembers[i]];
        }

        if (item[pathMembers[i]] is IDictionary<string, object>) {
          item = (IDictionary<string, object>) item[pathMembers[i]];

        } else {   // This item is a scalar (not a subtree), so the next item
                   // in the path necessarily doesn't exist.
          throw new JsonDataException(JsonDataException.Msg.JsonPathItemNotFound,
                                      itemPath, pathMembers[i + 1]);
        }

      }  // for

      throw Assertion.AssertNoReachThisCode();
    }

    #endregion Private methods

    #region Utility methods

    static private string GetChildKey(string itemPath) {
      string[] pathMembers = SplitItemPath(itemPath);

      return pathMembers[pathMembers.Length - 1];
    }


    static private (string parentPath, string childKey) GetParentChild(string childPath) {
      string[] pathMembers = SplitItemPath(childPath);

      if (pathMembers.Length > 1) {

        string temp = "";

        for (int i = 0; i < pathMembers.Length - 1; i++) {
          temp += "/" + pathMembers[i];
        }

        return (temp, pathMembers[pathMembers.Length - 1]);

      } else if (pathMembers.Length == 1) {
        return ("/", pathMembers[0]);

      } else if (pathMembers.Length == 0) {
        return ("/", String.Empty);

      } else {
        throw Assertion.AssertNoReachThisCode();

      }

    }


    static private bool HasEmptyOrNullValue(object item) {
      if (item == null) {
        return true;
      }

      if (item is String &&
          String.IsNullOrWhiteSpace((string) item)) {
        return true;
      }

      if (item is DateTime &&
          (((DateTime) item) == ExecutionServer.DateMaxValue ||
          ((DateTime) item) == ExecutionServer.DateMinValue)) {
        return true;
      }

      if (item is decimal &&
          ((decimal) item == 0)) {
        return true;
      }

      if (item is IDictionary<string, object> &&
          ((IDictionary<string, object>) item).Count == 0) {
        return true;
      }

      if (item is Array &&
         ((Array) item).Length == 0) {
        return true;
      }

      return false;
    }


    static private bool IsRoot(string itemPath) {
      Assertion.AssertObject(itemPath, "itemPath");

      return (itemPath == "/");
    }


    static private bool RemoveIfEmptyOrNull(IDictionary<string, object> node,
                                            string childKey) {
      if (node.ContainsKey(childKey) &&
          HasEmptyOrNullValue(node[childKey])) {

        node.Remove(childKey);

        return true;
      }

      return false;
    }


    static private string[] SplitItemPath(string itemPath) {
      itemPath = itemPath.Trim(' ');
      itemPath = itemPath.Replace("//", "/");
      itemPath = itemPath.Replace("/", ".");
      itemPath = itemPath.Trim('.');

      return itemPath.Split('.');
    }


    #endregion Utility methods

  }  // class JsonObject

}  // namespace Empiria.Json
