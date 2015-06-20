/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria.Json                                     Assembly : Empiria.Kernel.dll                *
*  Type      : DataRowConverter                                 Pattern  : Newtonsoft Json Converter         *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides serialization services of DataRow and DataRowView objects into Json strings.         *
*                                                                                                            *
********************************* Copyright (c) 2013-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Empiria.Json {

  /// <summary>Converts DataRow and DataViewRow objects to JSON.</summary>
  public class DataRowConverter : Newtonsoft.Json.JsonConverter {

    public override bool CanConvert(Type objectType) {
      return typeof(DataRow).IsAssignableFrom(objectType) ||
             typeof(DataRowView).IsAssignableFrom(objectType);
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

    /// <summary>Writes the JSON representation of the DataRow or DatRowView.</summary>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      DataRow dataRow = this.GetDataRow(value);

      var resolver = serializer.ContractResolver as DefaultContractResolver;

      writer.WriteStartObject();
      foreach (DataColumn column in dataRow.Table.Columns) {
        object columnValue = dataRow[column];

        if (serializer.NullValueHandling == NullValueHandling.Ignore &&
            (columnValue == null || columnValue == DBNull.Value)) {
          continue;
        }
        writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
        serializer.Serialize(writer, columnValue);
      }
      writer.WriteEndObject();
    }

    private DataRow GetDataRow(object value) {
      Type objectType = value.GetType();

      if (typeof(DataRow).IsAssignableFrom(objectType)) {
        return (DataRow) value;
      } else if (typeof(DataRowView).IsAssignableFrom(objectType)) {
        return ((DataRowView) value).Row;
      } else {
        throw Assertion.AssertNoReachThisCode();
      }
    }

  }  // class DataViewConverter

}  // namespace Empiria.Json
