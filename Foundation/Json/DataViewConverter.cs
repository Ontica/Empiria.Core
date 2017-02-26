/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : JSON Data Services                *
*  Namespace : Empiria.Json                                     Assembly : Empiria.Kernel.dll                *
*  Type      : DataRowConverter                                 Pattern  : Newtonsoft Json Converter         *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides serialization services of DataView objects into Json strings.                        *
*                                                                                                            *
********************************* Copyright (c) 2013-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
