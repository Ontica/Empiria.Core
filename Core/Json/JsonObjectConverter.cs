/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Json Services                      Component : Json Converters                         *
*  Assembly : Empiria.Core.dll                           Pattern   : Newtonsoft Json Converter               *
*  Type     : JsonObjectConverter                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Newtonsoft converter that (de)serializes an Empiria JsonObject as a nested JSON object         *
*             instead of as an IEnumerable.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Empiria.Json {

  /// <summary>Newtonsoft converter that (de)serializes an Empiria JsonObject
  /// as a nested JSON object instead of as an IEnumerable.</summary>
  public class JsonObjectConverter : Newtonsoft.Json.JsonConverter {

    public override bool CanConvert(Type objectType) {
      return typeof(JsonObject).IsAssignableFrom(objectType);
    }


    public override object ReadJson(JsonReader reader, Type objectType,
                                    object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null) {
        return JsonObject.Empty;
      }

      JToken token = JToken.Load(reader);

      return JsonObject.Parse(token.ToString());
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      var jsonObject = (JsonObject) value;

      if (jsonObject == null || !jsonObject.HasItems) {
        writer.WriteStartObject();
        writer.WriteEndObject();
        return;
      }

      writer.WriteRawValue(jsonObject.ToString());
    }

  }  // class JsonObjectConverter

}  // namespace Empiria.Json
