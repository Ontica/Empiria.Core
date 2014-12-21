/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Json                                     Assembly : Empiria.Kernel.dll                *
*  Type      : JsonConverter                                    Pattern  : Static Class                      *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Empiria JSON serialization library. JSON operations are based on Json.NET.                    *
*                                                                                                            *
********************************* Copyright (c) 2013-2015. Ontica LLC, La Vía Óntica SC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Dynamic;

using Newtonsoft.Json;

/// ToDo List:    OOJJOO
/// Object slicing (include/exclude a list of properties)
/// Include private and public properties for serialize
/// Represent object references of IIdentifiable instances only by their Id 
/// excluding all (almost all) other fields.
/// Serialize DataOperation in order to support audit logs and (queue) db integration tasks
/// Create a new JSON string based on other JSON string removing or appending new fields
/// JSON Schema support ???
/// Merge two JSON objects
/// JSON to XML support?

namespace Empiria.Json {

  /// <summary>Empiria JSON serialization library. JSON operations are based on Json.NET.</summary>
  static public class JsonConverter {

    #region Fields

    static private Dictionary<string, Func<object, string>> jsonConverters =
                                                       new Dictionary<string, Func<object, string>>();

    #endregion Fields

    #region Public methods

    /// <summary>Adds a delegate which will be invoked to convert instances of a giving type to JSON.</summary>
    /// <param name="type">The type of objects that will be associated with the JSON convertion method.</param>
    /// <param name="converter">The method delegate that performs the object to JSON convertion.</param>
    /// <param name="useInDerivedTypes">Flag that indicates if the convertion rule will be applied also to 
    /// any derived instances of the giving type if there are not more specific rules for them.</param> 
    static public void AddConverter(Type type, Func<object, string> converter,
                                    bool useInDerivedTypes = true) {
      string formatType = BuildDictionaryKey(type);

      if (jsonConverters.ContainsKey(formatType)) {
        throw new JsonDataException(JsonDataException.Msg.JsonConverterForTypeAlreadyExists, formatType);
      }

      lock (jsonConverters) {
        if (!jsonConverters.ContainsKey(formatType)) {
          jsonConverters.Add(formatType, converter);
        }
      } // lock

    }

    /// <summary>
    /// Merges the items included in the JSON string into a loaded object, 
    /// retaining the object property values that are not contained in the JSON string.
    /// </summary>
    /// <typeparam name="T">The object type of the returned instance. 
    /// Implicit use for anonymous objects.</typeparam>
    /// <param name="json">The JSON string to merge into the object instance.</param>
    /// <param name="instance">The object instance of type T. Can be an anonymous object.</param>
    /// <returns>The object with the merged properties values</returns>
    static public T Merge<T>(string json, T instance) {
      JsonConvert.PopulateObject(json, instance, new JsonSerializerSettings());

      return instance;
    }

    static public void RemoveConverter(Type type) {
      string formatType = BuildDictionaryKey(type);

      lock (jsonConverters) {
        if (jsonConverters.ContainsKey(formatType)) {
          jsonConverters.Remove(formatType);
        } else {
          throw new JsonDataException(JsonDataException.Msg.JsonConverterForTypeNotFound, formatType);
        }
      }
    }

    /// <summary>Converts a JSON string into an object dictionary.</summary>
    /// <param name="jsonString">The JSON string to convert.</param>
    /// <returns>IDictionary with the same items and structure as the JSON string.</returns>
    static public IDictionary<string, object> ToDictionary(string jsonString) {
      return JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
    }

    /// <summary>Converts an object into a JSON string.</summary>
    /// <param name="json">The object to convert.</param>
    /// <returns>The JSON string with properties and values equals to the supplied 
    /// object public properties.</returns>
    static public string ToJson(object o) {
      string typeFullName = BuildDictionaryKey(o.GetType());

      if (jsonConverters.ContainsKey(typeFullName)) {
        return jsonConverters[typeFullName].Invoke(o);
      } else {    // Use the default JsonConvert serialization
        return JsonConvert.SerializeObject(o);
      }
    }

    static public JsonObject ToJsonObject(string jsonString) {
      return JsonObject.Parse(jsonString);
    }

    /// <summary>Converts an object into a indented JSON string.</summary>
    /// <param name="o">The object to convert.</param>
    /// <returns>The indented JSON string with properties and values equals to the supplied 
    /// object public properties.</returns>
    static public string ToJsonIndented(object o) {
      string typeFullName = BuildDictionaryKey(o.GetType());

      if (jsonConverters.ContainsKey(typeFullName)) {
        string json = jsonConverters[typeFullName].Invoke(o);
        return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json),
                                           Formatting.Indented);
      } else {
        return JsonConvert.SerializeObject(o, Formatting.Indented);
      }
    }

    /// <summary>Converts a JSON string into a dynamic ExpandoObject.</summary>
    /// <param name="json">The JSON string to convert.</param>
    /// <returns>A .Net ExpandoObject with properties similar to the JSON string structure.</returns>
    static public dynamic ToObject(string jsonString) {
      return JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
    }

    ///// <summary>Converts a JSON string into a dynamic ExpandoObject.</summary>
    ///// <param name="jsonString">The JSON string to convert.</param>
    ///// <returns>A .Net ExpandoObject with properties similar to the JSON string structure.</returns>
    //static public dynamic ToObject(string json, string schema) {
    //  return JsonConvert.DeserializeObject<ExpandoObject>(json);
    //}

    /// <summary>Converts a JSON string into an object instance of type T.</summary>
    /// <typeparam name="T">The type of the object returned by this method.</typeparam>
    /// <param name="jsonString">The JSON string to convert.</param>
    /// <returns>The object instance of type T with the properties obtained from the JSON structure.</returns>
    static public T ToObject<T>(string jsonString) {
      return JsonConvert.DeserializeObject<T>(jsonString);
    }

    /// <summary>Converts a JSON string into an object instance of type T</summary>
    /// <typeparam name="T">The object type of the returned instance. 
    /// Use implicit for anonymous objects.</typeparam> 
    /// <param name="jsonString">The JSON string to convert.</param>
    /// <param name="instance">The object instance of type T. Can be an anonymous object.</param>
    /// <returns>The object instance of type T with the properties obtained from the JSON structure.</returns>
    static public T ToObject<T>(string jsonString, T instance) {
      if (instance.GetType().Namespace == null) {
        return JsonConvert.DeserializeAnonymousType(jsonString, instance);
      } else {
        instance = JsonConvert.DeserializeObject<T>(jsonString);
        return instance;
      }
    }

    #endregion Public methods

    #region Private methods

    static private string BuildDictionaryKey(Type type) {
      return BuildDictionaryKey(type.FullName);
    }

    static private string BuildDictionaryKey(string formatType) {
      return formatType.ToUpperInvariant();
    }

    #endregion Private methods

  } // class JsonConverter

} // namespace Empiria.Json
