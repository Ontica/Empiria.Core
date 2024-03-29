﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : JSON Data Services                *
*  Namespace : Empiria.Json                                     License  : Please read LICENSE.txt file      *
*  Type      : DataRowConverter                                 Pattern  : Newtonsoft Json Converter         *
*                                                                                                            *
*  Summary   : Provides serialization services of DataRow and DataRowView objects into Json strings.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

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
        writer.WritePropertyName((resolver != null) ?
                                  resolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
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
        throw Assertion.EnsureNoReachThisCode();
      }
    }

  }  // class DataViewConverter

}  // namespace Empiria.Json
