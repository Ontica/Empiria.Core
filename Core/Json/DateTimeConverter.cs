/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Json Services                      Component : Json Converters                         *
*  Assembly : Empiria.Core.dll                           Pattern   : Newtonsoft Json Converter               *
*  Type     : DateTimeConverter                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Empiria JSON serialization class that writes empty strings for DateTime special values.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Newtonsoft.Json;

namespace Empiria.Json {

  ///<summary>Empiria JSON serialization class that writes empty strings for DateTime special values.</summary>
  public class DateTimeConverter : Newtonsoft.Json.Converters.DateTimeConverterBase {

    public override bool CanRead {
      get {
        return true;
      }
    }


    public override bool CanWrite {
      get {
        return true;
      }
    }


    public override bool CanConvert(Type objectType) {
      return objectType == typeof(DateTime);
    }


    public override object ReadJson(JsonReader reader, Type objectType,
                                    object existingValue, JsonSerializer serializer) {

      if (reader.TokenType == JsonToken.Date) {

        return (DateTime) reader.Value;

      } else if (reader.TokenType == JsonToken.String) {

        string stringValue = (string) reader.Value;

        if (stringValue.Length == 0) {

          return ExecutionServer.DateMaxValue;    // Empty strings map to Empiria Date special values

        } else {

          return DateTime.Parse(stringValue);
        }

      } else if (reader.TokenType == JsonToken.Null) {

        return ExecutionServer.DateMaxValue;

      } else {

        throw Assertion.EnsureNoReachThisCode(
                    $"Unhandled JSON token type '{reader.TokenType}' for DateTime deserialization.");
      }
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      DateTime date = (DateTime) value;

      if (ExecutionServer.IsMinOrMaxDate(date)) {

        serializer.Serialize(writer, String.Empty);   // Write empty strings for Empiria Date special values

      } else {

        serializer.Serialize(writer, date.ToString(serializer.DateFormatString));

      }
    }

  }  // class DateTimeConverter

} // namespace Empiria.Json
