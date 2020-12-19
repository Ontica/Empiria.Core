/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Data processing services                     Component : Searching and filtering               *
*  Assembly : Empiria.Core.dll                             Pattern   : Utility class                         *
*  Type     : SearchExpression                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Contains methods to build search expressions.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Linq;

namespace Empiria {

  /// <summary>Contains methods to build search expressions.</summary>
  static public class SearchExpression {

    #region Public members

    static public string AllRecordsFilter {
      get {
        return "(1 = 1)";
      }
    }

    static public string NoRecordsFilter {
      get {
        return "(1 = 0)";
      }
    }


    static public string ParseAndLike(string fieldName, string keywords) {
      return ParseLikeExpressionUtility(fieldName, keywords, "AND", true);
    }


    static public string ParseAndLikeKeywords(string fieldName, string rawtext) {
      var keywords = EmpiriaString.BuildKeywords(rawtext);

      return ParseLikeExpressionUtility(fieldName, keywords, "AND", true);
    }


    static public string ParseAndLikeWithNoiseWords(string fieldName, string keywords) {
      return ParseLikeExpressionUtility(fieldName, keywords, "AND", false);
    }


    static public string ParseOrLike(string fieldName, string keywords) {
      return ParseLikeExpressionUtility(fieldName, keywords, "OR", true);
    }


    static public string ParseOrLikeKeywords(string fieldName, string rawtext) {
      var keywords = EmpiriaString.BuildKeywords(rawtext);

      return ParseLikeExpressionUtility(fieldName, keywords, "OR", true);
    }


    static public string ParseOrLikeWithNoiseWords(string fieldName, string keywords) {
      return ParseLikeExpressionUtility(fieldName, keywords, "OR", false);
    }


    static public string ParseBetweenDates(string fieldName, DateTime fromDate, DateTime toDate) {
      return ParseBetweenDates(fieldName, fromDate.ToShortDateString(), toDate.ToShortDateString());
    }


    static public string ParseBetweenDates(string fieldName, string fromDate, string toDate) {
      string dateWithTime1 = fromDate.CompareTo(toDate) <= 0 ?
                                  Format(fromDate + " 00:00:00") : Format(toDate + " 00:00:00");
      string dateWithTime2 = fromDate.CompareTo(toDate) <= 0 ?
                                  Format(toDate + " 23:59:59") : Format(fromDate + " 23:59:59");

      return ParseBetweenValues(fieldName, dateWithTime1, dateWithTime2);
    }


    static public string ParseBetweenValues(string fieldName, string fieldValue1, string fieldValue2) {
      if (fieldValue1.CompareTo(fieldValue2) < 0) {
        return "((" + Format(fieldValue1) + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(fieldValue2) + "))";

      } else if (fieldValue1.CompareTo(fieldValue2) > 0) {
        return "((" + Format(fieldValue2) + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(fieldValue1) + "))";

      } else {
        return "([" + fieldName + "] = " + Format(fieldValue1) + ")";

      }
    }


    static public string ParseDistinct(string fieldName, object fieldValue) {
      return "([" + fieldName + "] <> " + Format(fieldValue) + ")";
    }


    static public string ParseEquals(string fieldName, object fieldValue) {
      return "([" + fieldName + "] = " + Format(fieldValue) + ")";
    }


    static public string ParseInSet(string fieldName, string[] fieldValues) {
      string[] formattedValues = fieldValues.Select(x => Format(x)).ToArray();

      return ParseInSetUtility(fieldName, formattedValues);
    }


    static public string ParseInSet(string fieldName, ArrayList fieldValues) {
      string[] formattedValues = fieldValues.ToArray().Select(x => Format(x)).ToArray();

      return ParseInSetUtility(fieldName, formattedValues);
    }


    static public string ParseInSet(string fieldName, int[] fieldValues) {
      string[] formattedValues = fieldValues.Select(x => x.ToString()).ToArray();

      return ParseInSetUtility(fieldName, formattedValues);
    }


    static public string ParseInSet(string fieldName, string joinedValues, char separator) {
      string[] valuesArray = joinedValues.Split(separator);

      for (int i = 1; i < valuesArray.Length; i++) {
        valuesArray[i] = valuesArray[i - 1] + separator + valuesArray[i];
      }

      return ParseInSet(fieldName, valuesArray);
    }


    static public string ParseLike(string fieldName, string fieldValue) {
      if (EmpiriaString.IsEmpty(fieldValue)) {
        return String.Empty;
      }

      return "([" + fieldName + "] LIKE '%" + fieldValue + "%')";
    }

    #endregion Public members

    #region Helper methods


    static private string Format(object fieldValue) {
      if (fieldValue.GetType() == Type.GetType("System.String")) {
        return "'" + Prepare((string) fieldValue) + "'";

      } else if (fieldValue.GetType() == Type.GetType("System.DateTime")) {
        return "#" + fieldValue + "#";

      } else {
        return Prepare(fieldValue.ToString());

      }
    }


    static private string Prepare(string fieldValue) {
      return EmpiriaString.BuildKeywords(fieldValue);
    }

    static private string ParseInSetUtility(string fieldName, string[] fieldValues) {
      if ((fieldValues == null) || (fieldValues.Length == 0)) {
        return String.Empty;
      }

      if (fieldValues.Length == 1) {
        return "([" + fieldName + "] = " + fieldValues[0] + ")";
      }

      string temp = String.Empty;

      for (int i = 0; i < fieldValues.Length; i++) {
        temp += ((temp.Length != 0) ? "," : String.Empty) + fieldValues[i];
      }

      return "([" + fieldName + "] IN (" + temp + "))";
    }


    static private string ParseLikeExpressionUtility(string fieldName, string keywords,
                                                     string logicalOperator, bool removeNoiseWords) {
      if (EmpiriaString.IsEmpty(keywords)) {
        return String.Empty;
      }

      string[] fieldValues = EmpiriaString.BuildKeywords(keywords, removeNoiseWords).Split(' ');
      string temp = String.Empty;

      for (int i = 0; i < fieldValues.Length; i++) {
        if (temp.Length != 0) {
          temp += $" {logicalOperator} ";
        }
        temp += ParseLike(fieldName, fieldValues[i]);
      }

      if (temp.Length != 0) {
        return "(" + temp + ")";
      } else {
        return String.Empty;
      }
    }

    #endregion Helper methods

  } // struct SearchExpression

} // namespace Empiria
