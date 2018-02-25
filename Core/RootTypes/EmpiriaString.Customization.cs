/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaString                                    Pattern  : Static Data Type                  *
*                                                                                                            *
*  Summary   : Library for string manipulation.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria {

  static public partial class EmpiriaString {

    #region Public methods

    static public string BooleanString(bool source) {
      return ((source) ? "Sí" : "No");
    }

    static public string BuildDigitalString(params object[] items) {
      const string beginEndTag = "||";
      const string delimiter = "|";

      if (items == null || items.Length == 0) {
        return beginEndTag + beginEndTag;
      }
      string temp = beginEndTag;
      for (int i = 0; i < (items.Length - 1); i++) {
        temp += ConvertToDigitalString(items[i]) + delimiter;
      }
      temp += ConvertToDigitalString(items[items.Length - 1]);
      temp += beginEndTag;

      return temp;
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

    static public string[] DivideLongString(string source, int maxLength, int suggestedLines) {
      if (String.IsNullOrWhiteSpace(source)) {
        return new string[1] { String.Empty };
      }
      List<string> list = new List<string>();
      string[] words = null;

      if (source.IndexOf(" ") != -1) {
        words = source.Split(' ');
      } else {
        words = new string[(source.Length / maxLength) + 1];
        for (int i = 0; i < suggestedLines; i++) {
          words[i] = source.Substring(i * Math.Min(maxLength, source.Length - (i * suggestedLines)));
          int pos = Math.Min(maxLength, source.Length - (i * suggestedLines));
        }
      }

      for (int i = 0; i < words.Length; i++) {
        if (list.Count == 0) {
          list.Add(words[i]);
        } else {
          if ((list[list.Count - 1].Length + words[i].Length) < maxLength) {
            list[list.Count - 1] += " " + words[i];
          } else {
            list.Add(words[i]);
          }
        }
      }
      string[] listArray = new string[list.Count];
      list.CopyTo(listArray);
      return listArray;
    }

    static public string FormatForScripting(string source) {
      source = source.Replace("''", "\"").Replace("'", "´");
      source = source.Replace(System.Environment.NewLine, "\\n");

      return source.Replace(Char.ConvertFromUtf32(0x000A), "\\n");
    }

    static public string FormatTaxTag(string taxTag) {
      string temp = taxTag.Replace("-", String.Empty);
      temp = temp.Replace(" ", String.Empty);
      temp = EmpiriaString.TrimSpacesAndControl(temp);
      if (temp.Length == 12 || temp.Length == 13) {
        return temp;
      } else {
        return "XAXX010101000";
      }
    }

    static public string TimeSpanString(double seconds) {
      TimeSpan timespan = TimeSpan.FromSeconds(seconds);
      if (timespan == TimeSpan.Zero) {
        return "0.0 mins";
      } else if (timespan.TotalDays >= 1.0d) {
        return timespan.TotalDays.ToString("N2") + " días";
      } else if (timespan.TotalHours >= 1.0d) {
        return timespan.TotalHours.ToString("N2") + " hrs";
      } else if (timespan.TotalMinutes >= 1.0d) {
        return timespan.TotalMinutes.ToString("N2") + " min";
      } else {
        return timespan.TotalMinutes.ToString("N2") + " seg";
      }
    }

    static public string TimeSpanString(TimeSpan timespan) {
      if (timespan == TimeSpan.Zero) {
        return "0.0 min";
      } else if (timespan.TotalDays >= 1.0d) {
        return timespan.TotalDays.ToString("N2") + " días";
      } else if (timespan.TotalHours >= 1.0d) {
        return timespan.TotalHours.ToString("N2") + " hrs";
      } else {
        return timespan.TotalMinutes.ToString("N2") + " min";
      }
    }

    #endregion Public methods

    #region Private methods

    static private string ConvertToDigitalString(object text) {
      if (text is String) {
        return EmpiriaString.TrimAll((string) text);
      } else if (text is Int32 || text is Int16) {
        return Math.Abs((int) text).ToString("00000000") + ((((int) text) < 0) ? "-" : String.Empty);
      } else if (text is DateTime) {
        return ((DateTime) text).ToString(@"yyyy-MM-dd\THH:mm:ss");
      } else if (text is decimal) {
        return ((decimal) text).ToString("0.0000");
      } else if (text is bool) {
        return ((bool) text).ToString();
      } else if (text is char) {
        return ((char) text).ToString();
      } else if (text is Enum) {
        return ((Enum) text).ToString();
      }
      return String.Empty;
    }

    #endregion Private methods

  }  // class EmpiriaString

} // namespace Empiria
