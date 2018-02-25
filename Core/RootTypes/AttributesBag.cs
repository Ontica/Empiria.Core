/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : AttributesBag                                    Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : (BETA) Attribute bag to hold BaseObject data.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria {

  /// <summary>(BETA) Attribute bag to hold BaseObject data. </summary>
  public class AttributesBag {

    internal enum DataType {
      Int32 = 1,
      String = 2,
      DateTime = 3,
      Decimal = 4,
      Object = 5,
    }

    #region Fields

    private int[] int32Values = new int[0];
    private DateTime[] dateTimeValues = new DateTime[0];
    private decimal[] decimalValues = new decimal[0];
    private string[] stringValues = new string[0];
    private object[] objectValues = new object[0];

    #endregion Fields

    #region Constructors and parsers

    internal AttributesBag(BaseObject instance, DataRow row) {
      if (attributeIndexes == null) {
        AttributesBag.InitializeRules(instance, row);
      }
      LoadAttributes(row);
    }

    #endregion Constructors and parsers

    #region Public methods

    public DateTime GetDateTime(string attributeName) {
      int index = attributeIndexes[attributeName];

      return dateTimeValues[index];
    }

    public decimal GetDecimal(string attributeName) {
      int index = attributeIndexes[attributeName];

      return decimalValues[index];
    }

    public Int32 GetInt32(string attributeName) {
      int index = attributeIndexes[attributeName];

      return int32Values[index];
    }

    public T GetObject<T>(string attributeName) {
      int index = attributeIndexes[attributeName];

      return (T) objectValues[index];
    }

    public string GetString(string attributeName) {
      int index = attributeIndexes[attributeName];

      return stringValues[index];
    }

    public void SetObject<T>(string attributeName, T value) {
      int index = attributeIndexes[attributeName];

      objectValues[index] = value;
    }

    #endregion Public methods

    #region Private methods

    static private Dictionary<string, int> attributeIndexes = null;
    static private Dictionary<string, DataType> attributeDataTypes = null;

    static private int int32Count = 0;
    static private int dateTimesCount = 0;
    static private int decimalsCount = 0;
    static private int stringsCount = 0;
    static private int objectsCount = 0;

    private static void InitializeRules(BaseObject instance, DataRow row) {
      DataColumnCollection columns = row.Table.Columns;

      attributeIndexes = new Dictionary<string, int>(columns.Count);
      attributeDataTypes = new Dictionary<string, DataType>(columns.Count);

      for (int i = 0; i < columns.Count; i++) {
        DataColumn column = columns[i];
        Type columnDataType = column.DataType;

        if (columnDataType == typeof(Int32)) {

          attributeIndexes.Add(column.ColumnName, int32Count);
          attributeDataTypes.Add(column.ColumnName, DataType.Int32);
          int32Count++;
        } else if (columnDataType == typeof(string)) {
          attributeIndexes.Add(column.ColumnName, stringsCount);
          attributeDataTypes.Add(column.ColumnName, DataType.String);
          stringsCount++;
        } else if (columnDataType == typeof(DateTime)) {
          attributeIndexes.Add(column.ColumnName, dateTimesCount);
          attributeDataTypes.Add(column.ColumnName, DataType.DateTime);
          dateTimesCount++;
        } else if (columnDataType == typeof(decimal)) {
          attributeIndexes.Add(column.ColumnName, decimalsCount);
          attributeDataTypes.Add(column.ColumnName, DataType.Decimal);
          decimalsCount++;
        }
      }  // for
    }

    private void LoadAttributes(DataRow row) {
      int32Values = new int[int32Count];
      dateTimeValues = new DateTime[dateTimesCount];
      decimalValues = new decimal[decimalsCount];
      stringValues = new string[stringsCount];
      objectValues = new object[objectsCount];

      foreach(var item in attributeIndexes) {
        DataType dataType = attributeDataTypes[item.Key];

        switch (dataType) {
          case DataType.Int32:
            int32Values[item.Value] = (int) row[item.Key];
            break;
          case DataType.String:
            stringValues[item.Value] = (string) row[item.Key];
            break;
          case DataType.DateTime:
            dateTimeValues[item.Value] = (DateTime) row[item.Key];
            break;
          case DataType.Decimal:
            decimalValues[item.Value] = (decimal) row[item.Key];
            break;
          case DataType.Object:
            objectValues[item.Value] = (int) row[item.Key];
            break;
        }
      }  // foreach
    }

    #endregion Private methods

  } // class AttributesBag

} // namespace Empiria

//static public Dictionary<string, object> DeepClone() {
//  if (baseStructure == null) {
//    baseStructure = GetBaseStructure();
//  }
//  using (var stream = new MemoryStream()) {
//    var formatter = new BinaryFormatter();
//    formatter.Serialize(stream, baseStructure);
//    stream.Position = 0;

//    return (Dictionary<string, object>) formatter.Deserialize(stream);
//  }
//}
