﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : JSON Data Services                *
*  Namespace : Empiria.Json                                     License  : Please read LICENSE.txt file      *
*  Type      : DateTimeConverter                                Pattern  : Json Serializer Class             *
*                                                                                                            *
*  Summary   : Empiria JSON serialization class that writes empty strings for DateTime special values.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Newtonsoft.Json;

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
        serializer.Serialize(writer, String.Empty);   // Write empty strings for Empiria Date special values

      }
    }

  }  // class DateTimeConverter

} // namespace Empiria.Json
