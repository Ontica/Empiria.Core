/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : JSON Data Services                *
*  Namespace : Empiria.Json                                     Assembly : Empiria.Kernel.dll                *
*  Type      : ValueObjectConverter                             Pattern  : Json Serializer Class             *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Empiria JSON serialization class for IValueObject types.                                      *
*                                                                                                            *
********************************* Copyright (c) 2013-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Empiria.Reflection;

namespace Empiria.Json {

  /// <summary>Empiria JSON serialization class for IValueObject types.</summary>
  public class ValueObjectConverter : Newtonsoft.Json.JsonConverter {

    public override bool CanConvert(Type objectType) {
      return typeof(IValueObject).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType,
                                    object existingValue, JsonSerializer serializer) {
      var jsonObject = JValue.Load(reader);

      var stringValue = jsonObject.Value<String>();

      IValueObject valueObject = (IValueObject) ObjectFactory.InvokeParseMethod(objectType, stringValue);

      return valueObject;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      serializer.Serialize(writer, ((IValueObject) value).ToString());
    }

  }  // class ValueObjectConverter

} // namespace Empiria.Json
