/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : JsonObject                                       Pattern  : Standard Class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Empiria JSON object allows data reading and parsing of JSON strings.                          *
*                                                                                                            *
********************************* Copyright (c) 2013-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using System.Collections.Generic;
using Empiria.Reflection;

namespace Empiria.Data {

  public class JsonObject {

    #region Fields

    private IDictionary<string, object> dictionary = null;

    #endregion Fields

    #region Constructors and parsers

    public JsonObject() {
      dictionary = new Dictionary<string, object>(8);
    }

    static public JsonObject Empty {
      get { 
        return new JsonObject() {
          IsEmptyInstance = true
        };
      }
    }

    #endregion Constructors and parsers

    public bool IsEmptyInstance {
      get;
      private set;
    }

    static public JsonObject Parse(string jsonString) {
      if (String.IsNullOrWhiteSpace(jsonString)) {
        return JsonObject.Empty;
      }
      JsonObject json = new JsonObject();
      json.dictionary = JsonConverter.ToDictionary(jsonString);
      return json;
    }

    #region Public members

    /// <summary>Searches for an item inside the JsonObject.</summary>
    /// <param name="itemPath">The item path to search</param>
    /// <returns>The item relative to the searched path, or an exception if the object 
    /// was not found or if the path is not well-formed.</returns>
    public T Get<T>(string itemPath) {
      if (ObjectFactory.IsStorable(typeof(T))) {
        return this.FindAndParseObject<T>(itemPath, true, default(T));
      } else {
        return this.Find<T>(itemPath, true, default(T));
      }
    }

    /// <summary>Extracts a new JsonObject from this instance given a itemPath.</summary>
    /// <param name="itemPath">The item path to search</param>
    /// <param name="defaultValue">The default value if the searched item is not found.</param> 
    /// <returns>The item relative to the searched path, the defaultValue if the object 
    /// was not found or an exception if the path is not well-formed.</returns>
    public T Get<T>(string itemPath, T defaultValue) {
      if (ObjectFactory.IsStorable(typeof(T))) {
        return this.FindAndParseObject<T>(itemPath, false, defaultValue);
      } else {
        return this.Find<T>(itemPath, false, defaultValue);
      }
    }

    /// <summary>Extracts a new JsonObject from this instance given a itemPath.</summary>
    /// <param name="itemPath">The item path to search</param>
    /// <returns>The JsonObject relative to the searched path, or the JsonObject.Empty
    /// instance if the path is not found or an exception if the path is not well-formed.</returns>
    public JsonObject Slice(string itemPath) {
      return this.Slice(itemPath, false);
    }

    /// <summary>Extracts a new JsonObject from this instance given a itemPath.</summary>
    /// <param name="itemPath">The item path to search</param>
    /// <param name="required">Throws an exception if is true and the searched item is not found.</param>
    /// <returns>The JsonObject relative to the searched path, or the JsonObject.Empty
    /// instance if the required flag is false. Otherwise throws an exception.</returns>
    public JsonObject Slice(string itemPath, bool required) {
      Assertion.AssertObject(itemPath, "itemPath");

      object value = this.GetDictionaryValue(itemPath, required);  
      if (value == null && required) {
        // An exception should be thrown from the GetDictionaryValue call above.
        Assertion.AssertNoReachThisCode();
      }
      if (value == null) {
        return JsonObject.Empty;
      } else if (value is IDictionary<string, object>) {
        JsonObject slice = new JsonObject();
        slice.dictionary = (IDictionary<string, object>) value;
        return slice;
      } else {
        JsonObject slice = new JsonObject();
        slice.dictionary.Add(this.GetDictionaryKey(itemPath), value);
        return slice;
      }
    }

    #endregion Public members

    #region Private members

    private T Convert<T>(object value) {
      Assertion.AssertObject(value, "value");

      return (T) System.Convert.ChangeType(value, typeof(T));
    }

    private T Find<T>(string itemPath, bool required, T defaultValue) {
      Assertion.AssertObject(itemPath, "itemPath");

      object value = this.GetDictionaryValue(itemPath, required);
      if (value == null && required) {
        // An exception should be thrown from the GetDictionaryValue call above.
        Assertion.AssertNoReachThisCode();
      }
      if (value == null) {
        return defaultValue;
      } else {
        return this.Convert<T>(value);
      }
    }

    private T FindAndParseObject<T>(string itemPath, bool required, T defaultValue) {
      int objectId = this.Find<Int32>(itemPath, required, 0);
      if (objectId == 0 && required) {
        // An exception should be thrown from the GetDictionaryValue call above.
        Assertion.AssertNoReachThisCode();
      }
      if (objectId != 0) {
        return Empiria.Reflection.ObjectFactory.ParseObject<T>(objectId);
      } else {
        return defaultValue;
      }
    }

    private string GetDictionaryKey(string itemPath) {
      string[] pathMembers = this.SplitItemPath(itemPath);

      return pathMembers[pathMembers.Length - 1];
    }

    private object GetDictionaryValue(string itemPath, bool required) {
      string[] pathMembers = this.SplitItemPath(itemPath);

      IDictionary<string, object> item = dictionary;
      for (int i = 0; i < pathMembers.Length; i++) {
        if (!item.ContainsKey(pathMembers[i])) {
          if (!required) {
            return null;
          } else {
            throw new EmpiriaDataException(EmpiriaDataException.Msg.JsonPathItemNotFound,
                                           itemPath, pathMembers[i]);
          }
        }
        if (i == (pathMembers.Length - 1)) {  // The last item is the searched item in the path
          return item[pathMembers[i]];
        }
        if (item[pathMembers[i]] is IDictionary<string, object>) {
          item = (IDictionary<string, object>) item[pathMembers[i]];
        } else {   // This item is a scalar (not a subtree), so the next item
                   // in the path necessarily doesn't exists.
          throw new EmpiriaDataException(EmpiriaDataException.Msg.JsonPathItemNotFound,
                                         itemPath, pathMembers[i + 1]);
        }
      }  // for
      throw Assertion.AssertNoReachThisCode();
    }

    private string[] SplitItemPath(string itemPath) {
      itemPath = itemPath.Trim(' ');
      itemPath = itemPath.Replace("//", "/");
      itemPath = itemPath.Replace("/", ".");
      itemPath = itemPath.Trim('.');

      return itemPath.Split('.');
    }

    #endregion Private members

  }  // class JsonObject

}  // namespace Empiria.Data
