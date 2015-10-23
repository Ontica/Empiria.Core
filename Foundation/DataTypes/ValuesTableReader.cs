/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : ValuesTableReader                                Pattern  : Data Type                         *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Performs column read operations over a general purpose values table.                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Performs column read operations over a general purpose values table.</summary>
  static public class ValuesTableReader {

    #region Public methods

    static public bool FindBoolean(string tableName, string columnName, decimal onRangeValue) {
      ValuesTableStucture tableStructure = ValuesTableStucture.Parse(tableName);

      object value = tableStructure.FindValue(columnName, onRangeValue);
      if (value != null) {
        return Convert.ToBoolean(value);
      } else {
        return false;
      }
    }

    static public DateTime FindDateTime(string tableName, string columnName, decimal onRangeValue) {
      ValuesTableStucture tableStructure = ValuesTableStucture.Parse(tableName);

      object value = tableStructure.FindValue(columnName, onRangeValue);
      if (value != null) {
        return Convert.ToDateTime(value);
      } else {
        return DateTime.MinValue;
      }
    }

    static public decimal FindDecimal(string tableName, string columnName, string onRangeValue) {
      ValuesTableStucture tableStructure = ValuesTableStucture.Parse(tableName);

      object value = tableStructure.FindValue(columnName, onRangeValue);
      if (value != null) {
        return Convert.ToDecimal(value);
      } else {
        return 0m;
      }
    }

    static public decimal FindDecimal(string tableName, string columnName, decimal onRangeValue) {
      ValuesTableStucture tableStructure = ValuesTableStucture.Parse(tableName);

      object value = tableStructure.FindValue(columnName, onRangeValue);
      if (value != null) {
        return Convert.ToDecimal(value);
      } else {
        return 0m;
      }
    }

    static public int FindInteger(string tableName, string columnName, decimal onRangeValue) {
      ValuesTableStucture tableStructure = ValuesTableStucture.Parse(tableName);

      object value = tableStructure.FindValue(columnName, onRangeValue);
      if (value != null) {
        return Convert.ToInt32(value);
      } else {
        return 0;
      }
    }

    static public string FindString(string tableName, string columnName, decimal onRangeValue) {
      ValuesTableStucture tableStructure = ValuesTableStucture.Parse(tableName);

      object value = tableStructure.FindValue(columnName, onRangeValue);
      if (value != null) {
        return Convert.ToString(value);
      } else {
        return String.Empty;
      }
    }

    #endregion Public methods

    #region Private methods

    #endregion Private methods

  }  // class ValuesTableReader

} // namespace Empiria.DataTypes
