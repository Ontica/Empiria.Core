/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : JSON Data Services                *
*  Namespace : Empiria.Json                                     Assembly : Empiria.Kernel.dll                *
*  Type      : DateTimeConverter                                Pattern  : Json Serializer Class             *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Empiria JSON serialization class that writes empty strings for DateTime special values.       *
*                                                                                                            *
********************************* Copyright (c) 2013-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Empiria.Json {

  ///<summary>Empiria JSON serialization class that writes empty strings for DateTime special values.</summary>
  public class DateTimeConverter : Newtonsoft.Json.Converters.DateTimeConverterBase {

    public override bool CanConvert(Type objectType) {
      return objectType == typeof(DateTime);
    }

    public override bool CanRead {
      get { return false; }
    }

    public override bool CanWrite {
      get { return true; }
    }

    public override object ReadJson(JsonReader reader, Type objectType,
                                    object existingValue, JsonSerializer serializer) {
      throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      DateTime date = (DateTime) value;
      if (date != ExecutionServer.DateMinValue &&
          date != ExecutionServer.DateMaxValue) {
        serializer.Serialize(writer, date.ToString(serializer.DateFormatString));
      } else {
        // Write empty strings for Empiria Date special values
        serializer.Serialize(writer, String.Empty);
      }
    }

  }  // class DateTimeConverter

} // namespace Empiria.Json
