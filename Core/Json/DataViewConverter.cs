/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : JSON Data Services                *
*  Namespace : Empiria.Json                                     License  : Please read LICENSE.txt file      *
*  Type      : DataRowConverter                                 Pattern  : Newtonsoft Json Converter         *
*                                                                                                            *
*  Summary   : Provides serialization services of DataView objects into Json strings.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Newtonsoft.Json;

namespace Empiria.Json {

  /// <summary>Converts DataView objects to JSON.</summary>
  public class DataViewConverter : Newtonsoft.Json.JsonConverter {

    public override bool CanConvert(Type objectType) {
      return typeof(DataView).IsAssignableFrom(objectType);
    }

    public override bool CanRead {
      get {
        return false;
      }
    }

    public override bool CanWrite {
      get {
        return true;
      }
    }

    public override object ReadJson(JsonReader reader, Type objectType,
                                    object existingValue, JsonSerializer serializer) {
      throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      serializer.Serialize(writer, ((DataView) value).ToTable());
    }

  }  // class DataViewConverter

}  // namespace Empiria.Json
