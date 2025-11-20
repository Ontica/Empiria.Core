/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Strings                            Component : Services Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Static methods library                  *
*  Type     : EmpiriaString (partial)                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static partial class with more methods for string manipulation.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Text;

namespace Empiria {

  /// <summary>Static partial class with more methods for string manipulation.</summary>
  static public partial class EmpiriaString {

    #region Public methods

    static public string Combine(params object[] items) {
      var temp = new StringBuilder("||");

      foreach (var item in items) {
        if (item is null) {
          temp.Append("null");
        } else if (item is string) {
          temp.Append(item);
        } else if (item is int || item is decimal || item is float || item is double) {
          temp.Append(item.ToString());
        } else if (item is DateTime) {
          temp.Append(((DateTime) item).ToString("yyy-MM-dd HH:mm:ss"));
        } else if (item is Enum) {
          temp.Append((Enum) item).ToString();
        } else if (item is bool) {
          temp.Append((bool) item ? "true" : "false");
        } else {
          temp.Append(item.GetHashCode());
        }
        temp.Append("|");
      }

      temp.Append("|");

      return temp.ToString();
    }


    static public string DateLongString(DateTime source) {
      return source.ToString("dd \\de MMMM \\de yyyy");
    }


    static public string DateTimeString(DateTime date) {
      if (date.Date == ExecutionServer.DateMinValue) {
        return "Nunca";
      } else if (date.Date == ExecutionServer.DateMaxValue) {
        return "No determinada";
      } else if (date.Date == DateTime.Today) {
        return "Hoy";
      } else if (date.Date == DateTime.Today.AddDays(1)) {
        return "Mañana";
      } else if (date.Date == DateTime.Today.AddDays(-1)) {
        return "Ayer";
      } else {
        return date.ToString("dd/MMM/yyyy");
      }
    }


    static public string DivideLongString(string source, int maxLength, string divisionString) {
      if (String.IsNullOrWhiteSpace(source)) {
        return String.Empty;
      }

      if (source.Length <= maxLength) {
        return source;
      }

      int parts = (source.Length / maxLength) + 1;

      string result = String.Empty;

      for (int i = 0; i < parts; i++) {
        if (i != (parts - 1)) {
          result += source.Substring(i * maxLength, maxLength) + divisionString;
        } else {
          result += source.Substring(i * maxLength);
        }
      }

      return result;
    }

    #endregion Public methods

    #region Private methods

    static internal string ToString(object value) {
      if (value == null || value == DBNull.Value) {
        return string.Empty;

      } else if (value is string) {
        return (string) value;

      } else {
        return Convert.ToString(value);
      }
    }

    static public string ToString(FixedList<string> list) {
      string temp = string.Empty;

      foreach (var item in list) {
        temp += item + "\n";
      }

      return temp;
    }

    #endregion Private methods

  }  // class EmpiriaString

} // namespace Empiria
