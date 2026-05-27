/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Json Services                      Component : Json Converters                         *
*  Assembly : Empiria.Core.dll                           Pattern   : Newtonsoft Json Converter               *
*  Type     : NamedEntityConverter                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Empiria JSON serialization class that writes BaseObject.INamedEntity instances                 *
*             to NamedEntityDto.                                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Newtonsoft.Json;

namespace Empiria.Json {

  ///<summary>Empiria JSON serialization class that writes BaseObject.INamedEntity instances
  ///to NamedEntityDto.</summary>
  public class NamedEntityConverter : Newtonsoft.Json.JsonConverter {


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


    public override bool CanConvert(Type objectType) {
      return typeof(BaseObject).IsAssignableFrom(objectType) &&
             typeof(INamedEntity).IsAssignableFrom(objectType);
    }


    public override object ReadJson(JsonReader reader, Type objectType,
                                    object existingValue, JsonSerializer serializer) {

      throw new NotImplementedException();
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      var namedEntity = (INamedEntity) value;

      serializer.Serialize(writer, namedEntity.MapToNamedEntity());
    }

  }  // class NamedEntityConverter

} // namespace Empiria.Json
