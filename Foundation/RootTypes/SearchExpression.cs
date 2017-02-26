/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Storage Services                  *
*  Namespace : Empiria.Storage                                  Assembly : Empiria.Foundation.dll            *
*  Type      : SearchExpression                                 Pattern  : Standard Class                    *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a searching expression.                                                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;

namespace Empiria {

  public struct SearchExpression {

    #region Fields

    private string expression;

    #endregion Fields

    #region Constructors and parsers

    public SearchExpression(string expression) {
      this.expression = expression;
    }

    static public SearchExpression Empty {
      get { return new SearchExpression(String.Empty); }
    }

    static public implicit operator string(SearchExpression expression) {
      return expression.ToString();
    }

    static public implicit operator SearchExpression(string expression) {
      return new SearchExpression(expression);
    }

    public static string AllRecordsFilter {
      get {
        return "(1 = 1)";
      }
    }

    static public string NoRecordsFilter {
      get {
        return "(1 = 0)";
      }
    }

    #endregion Constructors and parsers

    #region Static methods

    static public SearchExpression ParseAndLike(string fieldName, string searchKeywords) {
      if (String.IsNullOrEmpty(searchKeywords)) {
        return String.Empty;
      }
      string[] fieldValues = EmpiriaString.BuildKeywords(searchKeywords).Split(' ');
      string temp = String.Empty;

      for (int i = 0; i < fieldValues.Length; i++) {
        temp += ((temp.Length != 0) ? " AND " : String.Empty) +
                 ParseLike(fieldName, fieldValues[i]);
      }
      if (temp.Length != 0) {
        return "(" + temp + ")";
      } else {
        return String.Empty;
      }
    }

    static public SearchExpression ParseAndLikeWithNoiseWords(string fieldName, string searchKeywords) {
      if (String.IsNullOrEmpty(searchKeywords)) {
        return String.Empty;
      }
      string[] fieldValues = EmpiriaString.BuildKeywords(searchKeywords, false).Split(' ');
      string temp = String.Empty;

      for (int i = 0; i < fieldValues.Length; i++) {
        temp += ((temp.Length != 0) ? " AND " : String.Empty) +
                 ParseLike(fieldName, fieldValues[i]);
      }
      if (temp.Length != 0) {
        return "(" + temp + ")";
      } else {
        return String.Empty;
      }
    }

    static public SearchExpression ParseOrLike(string fieldName, string searchKeywords) {
      if (String.IsNullOrWhiteSpace(searchKeywords)) {
        return String.Empty;
      }
      string[] fieldValues = EmpiriaString.BuildKeywords(searchKeywords).Split(' ');
      string temp = String.Empty;

      for (int i = 0; i < fieldValues.Length; i++) {
        temp += ((temp.Length != 0) ? " OR " : String.Empty) +
                ParseLike(fieldName, (string) fieldValues[i]);
      }
      if (temp.Length != 0) {
        return "(" + temp + ")";
      } else {
        return String.Empty;
      }
    }

    static public SearchExpression ParseOrLikeWithNoiseWords(string fieldName, string searchKeywords) {
      if (String.IsNullOrWhiteSpace(searchKeywords)) {
        return String.Empty;
      }
      string[] fieldValues = EmpiriaString.BuildKeywords(searchKeywords, true).Split(' ');
      string temp = String.Empty;

      for (int i = 0; i < fieldValues.Length; i++) {
        temp += ((temp.Length != 0) ? " OR " : String.Empty) +
                ParseLike(fieldName, (string) fieldValues[i]);
      }
      if (temp.Length != 0) {
        return "(" + temp + ")";
      } else {
        return String.Empty;
      }
    }

    static public SearchExpression ParseBetweenDates(string fieldName, string dateValue1, string dateValue2) {
      if (dateValue1.CompareTo(dateValue2) < 0) {
        return "((" + Format(dateValue1 + " 00:00:00") + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(dateValue2 + " 23:59:59") + "))";
      } else if (dateValue1.CompareTo(dateValue2) > 0) {
        return "((" + Format(dateValue2 + " 00:00:00") + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(dateValue1 + " 23:59:59") + "))";
      } else {
        return "((" + Format(dateValue1 + " 00:00:00") + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(dateValue1 + " 23:59:59") + "))";
      }
    }

    static public SearchExpression ParseBetweenDates(string fieldName, DateTime dateValue1, DateTime dateValue2) {
      if (dateValue1.CompareTo(dateValue2) < 0) {
        return "((" + Format(dateValue1.ToShortDateString() + " 00:00:00") + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(dateValue2.ToShortDateString() + " 23:59:59") + "))";
      } else if (dateValue1.CompareTo(dateValue2) > 0) {
        return "((" + Format(dateValue2.ToShortDateString() + " 00:00:00") + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(dateValue1.ToShortDateString() + " 23:59:59") + "))";
      } else {
        return "((" + Format(dateValue1.ToShortDateString() + " 00:00:00") + " <= [" + fieldName + "]) AND ([" +
                fieldName + "] <= " + Format(dateValue1.ToShortDateString() + " 23:59:59") + "))";
      }
    }

    static public SearchExpression ParseBetweenValues(string fieldName, string fieldValue1, string fieldValue2) {
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

    static public SearchExpression ParseDistinct(string fieldName, object fieldValue) {
      return "([" + fieldName + "] <> " + Format(fieldValue) + ")";
    }

    static public SearchExpression ParseEquals(string fieldName, object fieldValue) {
      return "([" + fieldName + "] = " + Format(fieldValue) + ")";
    }

    static public SearchExpression ParseInSet(string fieldName, ArrayList fieldValues) {
      if ((fieldValues == null) || (fieldValues.Count == 0)) {
        return String.Empty;
      } else if (fieldValues.Count == 1) {
        return "([" + fieldName + "] = " + Format(fieldValues[0]) + ")";
      } else {
        string temp = String.Empty;
        for (int i = 0; i < fieldValues.Count; i++) {
          temp += ((temp.Length != 0) ? "," : String.Empty) + Format(fieldValues[i]);
        }
        return "([" + fieldName + "] IN (" + temp + "))";
      }
    }

    static public SearchExpression ParseInSet(string fieldName, string[] fieldValues) {
      if ((fieldValues == null) || (fieldValues.Length == 0)) {
        return String.Empty;
      } else if (fieldValues.Length == 1) {
        return "([" + fieldName + "] = " + Format(fieldValues[0]) + ")";
      } else {
        string temp = String.Empty;
        for (int i = 0; i < fieldValues.Length; i++) {
          temp += ((temp.Length != 0) ? "," : String.Empty) + Format(fieldValues[i]);
        }
        return "([" + fieldName + "] IN (" + temp + "))";
      }
    }

    static public SearchExpression ParseInSet(string fieldName, string joinedValues, char separator) {
      string[] valuesArray = joinedValues.Split(separator);

      for (int i = 1; i < valuesArray.Length; i++) {
        valuesArray[i] = valuesArray[i - 1] + separator + valuesArray[i];
      }

      return SearchExpression.ParseInSet(fieldName, valuesArray);
    }

    static public SearchExpression ParseLike(string fieldName, string fieldValue, bool searchAsIs = false) {
      if (String.IsNullOrEmpty(fieldValue)) {
        return String.Empty;
      } else if (searchAsIs) {
        return "([" + fieldName + "] LIKE '%" + fieldValue + "%')";
      } else {
        return "([" + fieldName + "] LIKE '%" + Prepare(fieldValue) + "%')";
      }
    }

    #endregion Static methods

    #region Public properties

    public bool IsEmpty {
      get { return (expression.Length == 0); }
    }

    #endregion Public properties

    #region Public instance methods

    public void AppendAnd(SearchExpression e) {
      if (IsEmpty) {
        this.expression = e.ToString();
      } else if (!e.IsEmpty) {
        this.expression += " AND " + e.ToString();
      } else {
        this.expression = SearchExpression.Empty;
      }
    }

    public void AppendOr(SearchExpression e) {
      if (IsEmpty) {
        this.expression = e.ToString();
      } else {
        this.expression += " OR " + e.ToString();
      }
    }

    public void SetNot() {
      if (IsEmpty) {
        this.expression = "NOT";
      } else {
        this.expression = "( NOT " + this.ToString() + ")";
      }
    }

    public override string ToString() {
      if (IsEmpty || expression.StartsWith("(")) {
        return expression;
      } else if (!IsEmpty) {
        return "(" + expression + ")";
      } else {
        return String.Empty;
      }
    }

    #endregion Public instance methods

    #region Private methods

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

    #endregion Private methods

  } // struct SearchExpression

} // namespace Empiria
