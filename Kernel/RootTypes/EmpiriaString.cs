/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaString                                    Pattern  : Static Data Type                  *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a string data type.                                                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Empiria {

  static public class EmpiriaString {

    #region Enumerations

    public enum DistanceAlgorithm {
      Levenshtein = 1,
      DamerauLevenshtein = 2,
      Jaro = 3,
      JaroWinkler = 4
    }

    #endregion Enumerations

    #region Fields

    private const decimal jaroWinklerDefaultPrefixAdjustmentScale = 0.1m;
    private const int jaroWinklerDefaultMaxPrefixDistance = 4;

    #endregion Fields

    #region Public methods

    /// <summary>Creates and returns a string representation of the current exception.</summary>
    static public string ExceptionString(Exception exception) {
      string temp = ExceptionHTMLString(exception);

      temp = temp.Replace("<u>", String.Empty);
      temp = temp.Replace("</u>", String.Empty);
      temp = temp.Replace("<b>", String.Empty);
      temp = temp.Replace("</b>", String.Empty);

      return temp;
    }

    /// <summary>Creates and returns a string representation of the current exception.</summary>
    static public string ExceptionHTMLString(Exception exception) {
      StringBuilder strInfo = new StringBuilder();
      Exception tempException = null;
      int exceptionCount = 1;

      tempException = exception;
      while (tempException != null) {
        strInfo.AppendFormat("{0}{0}", (exceptionCount != 1) ? Environment.NewLine : String.Empty);
        strInfo.AppendFormat("<b>{1}) <u>{2}</u></b>{0}{0}", Environment.NewLine, exceptionCount.ToString(),
                                                exceptionCount == 1 ? "Exception Information" : "Inner Exception Information");
        strInfo.AppendFormat("ExceptionType: {0}", tempException.GetType().FullName);

        PropertyInfo[] exceptionProperties = tempException.GetType().GetProperties();
        foreach (PropertyInfo property in exceptionProperties) {
          if (property.Name != "InnerException" && property.Name != "StackTrace" && property.Name != "Data") {
            object propertyValue = property.GetValue(tempException, null);
            if (propertyValue != null) {
              strInfo.AppendFormat("{0}{1}: {2}", Environment.NewLine, property.Name, propertyValue);
            }
          }  // if
        } // foreach
        if (tempException.StackTrace != null) {
          strInfo.AppendFormat("{0}{0}{1}) <u>Stack Trace Information</u>{0}{0}", Environment.NewLine,
                               exceptionCount.ToString() + ".1");
          strInfo.AppendFormat("{0}", tempException.StackTrace);
        }
        tempException = tempException.InnerException;
        exceptionCount++;
      } // while

      return strInfo.ToString();
    }

    static public string GetQuantityBulletName(decimal quantity) {
      if (quantity < 0m) {
        return "red.up.arrow.gif";
      } else if (quantity > 0m) {
        return "green.down.arrow.gif";
      } else {
        return "pixel.gif";
      }
    }

    static public string GetQuantityClassName(decimal quantity) {
      if (quantity < 0m) {
        return "negativeQty";
      } else if (quantity > 0m) {
        return "positiveQty";
      } else {
        return "zeroQty";
      }
    }

    static public string GetSection(string source, char delimiter, int sectionIndex) {
      string[] sections = source.Split(new char[] { delimiter });

      return sections[sectionIndex].Trim();
    }

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

    static public string BuildKeywords(params string[] words) {
      string temp = temp = String.Join(" ", words).ToLowerInvariant();
      return BuildKeywords(temp, true);
    }

    static public string BuildKeywords(string words, bool removeNoiseStrings) {
      words = TrimAll(words);
      if (String.IsNullOrEmpty(words)) {
        return String.Empty;
      }
      words = words.ToLowerInvariant();
      if (removeNoiseStrings) {
        words = RemoveNoiseStrings(RemoveAccents(RemovePunctuation(words)));
      } else {
        words = RemoveAccents(RemovePunctuation(words));
      }
      return words;
    }

    static public bool Contains(string source, string searchWords) {
      searchWords = TrimAll(searchWords);
      if (source.Length == 0 || searchWords.Length == 0) {
        return false;
      }
      string[] array = searchWords.Split(' ');
      for (int i = 0; i < array.Length; i++) {
        if (!source.Contains(TrimAll(array[i]))) {
          return false;
        }
      }
      return true;
    }

    static public bool ContainsAny(string source, string searchWords) {
      searchWords = TrimAll(searchWords);
      if (source.Length == 0 || searchWords.Length == 0) {
        return false;
      }
      string[] array = searchWords.Split(' ');
      for (int i = 0; i < array.Length; i++) {
        if (source.Contains(TrimAll(array[i]))) {
          return true;
        }
      }
      return false;
    }

    static public string RemoveNoiseStrings(string source) {
      string[] tokens = source.Split(' ');
      string temp = String.Empty;
      for (int i = 0; i < tokens.Length; i++) {
        string token = tokens[i];
        if (temp.IndexOf(token + " ") < 0 && !IsPrepositionOrConjuntion(token)) {
          temp += token + " ";
        }
      }
      return TrimAll(temp);
    }

    static public int DamerauLevenshteinDistance(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      int[][] distanceMatrix = new int[stringA.Length + 1][];

      for (int i = 0; i <= stringA.Length; i++) {
        distanceMatrix[i] = new int[stringB.Length + 1];
        distanceMatrix[i][0] = i;
      }
      for (int j = 0; j <= stringB.Length; j++) {
        distanceMatrix[0][j] = j;
      }
      for (int i = 1; i <= stringA.Length; i++) {
        for (int j = 1; j <= stringB.Length; j++) {
          distanceMatrix[i][j] = EmpiriaMath.Min(distanceMatrix[i - 1][j] + 1, distanceMatrix[i][j - 1] + 1,
                                                 distanceMatrix[i - 1][j - 1] + ((stringA[i - 1] == stringB[j - 1]) ? 0 : 1));

          // Transposition
          if ((i > 1) && (j > 1) && (stringA[i - 1] == stringB[j - 2]) && (stringA[i - 2] == stringB[j - 1])) {
            distanceMatrix[i][j] = Math.Min(distanceMatrix[i][j],
                                            distanceMatrix[i - 2][j - 2] + ((stringA[i - 1] == stringB[j - 1]) ? 0 : 1));
          }
        }
      }
      return distanceMatrix[stringA.Length][stringB.Length];
    }

    static public decimal DamerauLevenshteinProximityFactor(string stringA, string stringB) {
      int distance = DamerauLevenshteinDistance(stringA, stringB);

      return decimal.One - ((decimal) distance / Math.Max(stringA.Length, stringB.Length));
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

    static public string DateTimeString(object date) {
      if (date != null) {
        return DateTimeString((DateTime) date);
      } else {
        return DateTimeString(DateTime.MaxValue);
      }
    }

    static public string DivideLongString(string source, int maxLength, string divisionString) {
      if (String.IsNullOrWhiteSpace(source)) {
        return String.Empty;
      }
      if (source.Length <= maxLength) {
        return source;
      }
      int parts = (source.Length / maxLength);
      for (int i = 0; i < parts; i++) {
        source = source.Insert((i + 1) * maxLength, divisionString);
      }
      return source;
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

    static public string Exclude(string source, string excludeThis) {
      string[] excludeArray = excludeThis.Split(' ');
      int index = 0;

      source = source + " ";
      for (int i = 0; i < excludeArray.Length; i++) {
        index = source.IndexOf(excludeArray[i]);
        if (index > 0 && source.Substring(index, excludeArray[i].Length + 1) == (excludeArray[i] + " ")) {
          source = source.Replace(excludeArray[i], String.Empty);
        }
      }
      return TrimAll(source);
    }

    static public string FormatForScripting(string source) {
      source = source.Replace("''","\"").Replace("'", "´");
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

    static public bool IsBoolean(string source) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        bool sourceValue = ToBoolean(source);
        return true;
      } catch {
        return false;
      }
    }

    static public bool IsCurrency(string source) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        Decimal sourceValue = Decimal.Parse(source);
        return true;
      } catch {
        return false;
      }
    }

    static public bool IsCurrency(string source, string format) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        Decimal sourceValue = Decimal.Parse(source);
        if (source == sourceValue.ToString(format)) {
          return true;
        } else {
          return false;
        }
      } catch {
        return false;
      }
    }

    static public bool IsDateTime(string source, string format) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        DateTime temp = DateTime.ParseExact(source, format, DateTimeFormatInfo.InvariantInfo);
        return true;
      } catch {
        return false;
      }
    }

    static public bool IsDouble(string source) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        double sourceValue = double.Parse(source);
        return true;
      } catch {
        return false;
      }
    }

    static public bool IsDouble(string source, string format) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        double sourceValue = double.Parse(source);
        if (source == sourceValue.ToString(format)) {
          return true;
        } else {
          return false;
        }
      } catch {
        return false;
      }
    }

    static public bool IsEmpty(string source) {
      if (source == null) {
        return true;
      }
      for (int i = 0; i < source.Length; i++) {
        if (!Char.IsWhiteSpace(source[i])) {
          return false;
        }
      }
      return true;
    }

    static public bool IsFloat(string source) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        float sourceValue = float.Parse(source);
        return true;
      } catch {
        return false;
      }
    }

    static public bool IsFloat(string source, string format) {
      try {
        if (String.IsNullOrEmpty(source)) {
          return false;
        }
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        float sourceValue = float.Parse(source);
        if (source == sourceValue.ToString(format)) {
          return true;
        } else {
          return false;
        }
      } catch {
        return false;
      }
    }

    static public bool IsInList(string source, string format, string[] values) {
      try {
        string formatted = String.Empty;
        if (format != null && format.Length != 0) {
          formatted = String.Format("{0:" + format + "}", source);
        } else {
          formatted = source;
        }
        for (int i = 0; i < values.Length; i++) {
          if (values[i] == formatted) {
            return true;
          }
        }
        return false;
      } catch {
        return false;
      }
    }

    static public bool IsInteger(string source) {
      if (String.IsNullOrEmpty(source)) {
        return false;
      }
      int startSearchPosition = 0;
      if (source[0] == '-' || source[0] == '$') {
        startSearchPosition = 1;
      }
      for (int i = startSearchPosition; i < source.Length; i++) {
        if (!Char.IsDigit(source[i])) {
          return false;
        }
      }
      return true;
    }

    static public bool IsQuantity(string source) {
      if (String.IsNullOrEmpty(source)) {
        return false;
      }
      int startSearchPosition = 0;
      if (source[0] == '-' || source[0] == '$') {
        startSearchPosition = 1;
      }
      for (int i = startSearchPosition; i < source.Length; i++) {
        if (!(Char.IsDigit(source[i]) || source[i] == '.' || source[i] == ',')) {
          return false;
        }
      }
      return IsCurrency(source.Substring(startSearchPosition));
    }

    static public decimal JaroProximityFactor(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return decimal.Zero;
      }

      int distanceR = (Math.Max(stringA.Length, stringB.Length) / 2) - 1;
      string stringACommonCharsInB = GetCommonCharacters(stringA, stringB, distanceR);
      if (stringACommonCharsInB.Length == 0) {
        return decimal.Zero;
      }
      string stringBCommonCharsInA = GetCommonCharacters(stringB, stringA, distanceR);
      if (stringBCommonCharsInA.Length == 0) {
        return decimal.Zero;
      }
      int transpositions = 0;
      if (stringACommonCharsInB != stringBCommonCharsInA) {
        for (int i = 0, j = Math.Min(stringACommonCharsInB.Length, stringBCommonCharsInA.Length); i < j; i++) {
          if (stringACommonCharsInB[i] != stringBCommonCharsInA[i]) {
            transpositions++;
          }
        }
      }
      return ((stringACommonCharsInB.Length / (decimal) stringA.Length) + (stringBCommonCharsInA.Length / (decimal) stringB.Length) +
              ((stringACommonCharsInB.Length - (transpositions / 2.0m)) / (decimal) stringACommonCharsInB.Length)) / 3.0m;
    }

    static public decimal JaroWinklerProximityFactor(string stringA, string stringB) {
      return JaroWinklerProximityFactor(stringA, stringB, EmpiriaString.jaroWinklerDefaultPrefixAdjustmentScale,
                                        EmpiriaString.jaroWinklerDefaultMaxPrefixDistance);
    }

    static public decimal JaroWinklerProximityFactor(string stringA, string stringB,
                                                     decimal prefixAdjustmentScale, int maxPrefixDistance) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return decimal.Zero;
      }

      decimal jaroRatio = JaroProximityFactor(stringA, stringB);
      int prefixLength = JaroWinklerPrefixLength(stringA, stringB, maxPrefixDistance);

      return jaroRatio + (prefixLength * prefixAdjustmentScale) * (1.0m - jaroRatio);
    }

    static public int LevenshteinDistance(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      int[][] distanceMatrix = new int[stringA.Length + 1][];

      for (int i = 0; i <= stringA.Length; i++) {
        distanceMatrix[i] = new int[stringB.Length + 1];
        distanceMatrix[i][0] = i;
      }
      for (int j = 0; j <= stringB.Length; j++) {
        distanceMatrix[0][j] = j;
      }
      for (int i = 1; i <= stringA.Length; i++) {
        for (int j = 1; j <= stringB.Length; j++) {
          distanceMatrix[i][j] = EmpiriaMath.Min(distanceMatrix[i - 1][j] + 1, distanceMatrix[i][j - 1] + 1,
                                                 distanceMatrix[i - 1][j - 1] + ((stringA[i - 1] == stringB[j - 1]) ? 0 : 1));
        }
      }
      return distanceMatrix[stringA.Length][stringB.Length];
    }

    static public decimal LevenshteinProximityFactor(string stringA, string stringB) {
      int distance = LevenshteinDistance(stringA, stringB);

      return decimal.One - ((decimal) distance / Math.Max(stringA.Length, stringB.Length));
    }

    static public decimal MongeElkanProximityFactor(DistanceAlgorithm algorithm, string stringA, string stringB) {
      string[] stringArrayA = stringA.Split(' ');
      string[] stringArrayB = stringB.Split(' ');

      decimal accumulator = 0m;
      for (int i = 1; i <= stringArrayA.Length; i++) {
        decimal maximum = decimal.Zero;
        for (int j = 1; j <= stringArrayB.Length; j++) {
          switch (algorithm) {
            case DistanceAlgorithm.Levenshtein:
              maximum = Math.Max(maximum, LevenshteinProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1]));
              break;
            case DistanceAlgorithm.DamerauLevenshtein:
              maximum = Math.Max(maximum, DamerauLevenshteinProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1]));
              break;
            case DistanceAlgorithm.Jaro:
              maximum = Math.Max(maximum, JaroProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1]));
              break;
            case DistanceAlgorithm.JaroWinkler:
              maximum = Math.Max(maximum, JaroWinklerProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1],
                                                                     jaroWinklerDefaultPrefixAdjustmentScale,
                                                                     jaroWinklerDefaultMaxPrefixDistance));
              break;
          }
        }
        accumulator += maximum;
      }
      return accumulator / stringArrayA.Length;
    }

    static public NameValueCollection ParseQueryString(string queryString) {
      return ParseQueryString(queryString, '|', '=');
    }

    static public NameValueCollection ParseQueryString(string queryString, char itemsSeparator, char valuesSeparator) {
      NameValueCollection pars = new NameValueCollection(8);

      string[] stringItems = queryString.Split(itemsSeparator);
      for (int i = 0; i < stringItems.Length; i++) {
        pars.Add(stringItems[i].Split(valuesSeparator)[0], stringItems[i].Split(valuesSeparator)[1]);
      }

      return pars;
    }

    static public string Pluralize(int number, string singular, string plural) {
      if (number == 1) {
        return number.ToString("N0") + " " + singular;
      } else {
        return number.ToString("N0") + " " + plural;
      }
    }

    static public string RemoveAccents(string source) {
      source = source.Replace("ñ", "n");
      source = source.Replace("á", "a");
      source = source.Replace("à", "a");
      source = source.Replace("é", "e");
      source = source.Replace("è", "e");
      source = source.Replace("í", "i");
      source = source.Replace("́", "i");
      source = source.Replace("ó", "o");
      source = source.Replace("̣", "o");
      source = source.Replace("ú", "u");
      source = source.Replace("ù", "u");
      source = source.Replace("ü", "u");

      return source;
    }

    static public string RemoveNoise(string source) {
      return TrimAll(RemoveAccents(RemovePunctuation(TrimControl(source))));
    }

    static public string RemoveNoiseExtended(string source) {
      return TrimAll(RemoveNoiseStrings(RemoveAccents(RemovePunctuation(TrimControl(source)))));
    }

    static public string RemovePunctuation(string source) {
      char[] punctuations = new char[] {'.', ',', ';', ':', '"', '\'', '/', '\\', '>', '<', '=', '-', '_', 
                                        '?', '*', '$', '&', '+', '(', ')', '{', '}', '[', ']', '^', '¬',
                                        '°', '%', '#', '¿', '!', '¡', '`', '~', '|'};

      for (int i = 0; i < punctuations.Length; i++) {
        source = source.Replace(punctuations[i], ' ');
      }
      return TrimAll(source);
    }

    static public bool Similar(string stringA, string stringB) {
      stringA = RemoveNoise(stringA).ToLowerInvariant();
      stringB = RemoveNoise(stringB).ToLowerInvariant();

      return (stringA.Equals(stringB));
    }

    static public string SpeechInteger(int value) {
      if (value >= 2 && value <= 12) {
        return SpeechHundreds(value);
      } else {
        return value.ToString("N0");
      }
    }

    static public string SpeechMoney(decimal amount) {
      int cents = (int) ((amount * 100) % 100);
      string result = String.Empty;
      string currency = String.Empty;

      if (amount < 1) {
        return "(Cero pesos " + cents.ToString("00") + "/100 M.N.)";
      } else if (1 <= amount && amount < 2) {
        currency = "peso ";
      } else if (((int) (amount % 1000000)) == 0 && (amount >= 1000000)) {
        currency = "de pesos ";
      } else {
        currency = "pesos ";
      }

      if (amount == 0) {
        return "(Sin valor  Sin valor)";
      } else if ((amount >= 1000000m) && ((int) (amount / 1000000)) == 1) {
        result = SpeechHundreds((int) (amount / 1000000)) + " Millón ";
      } else if (amount > 1000000m) {
        result = SpeechHundreds((int) (amount / 1000000)) + " Millones ";
      }
      amount = amount % 1000000;
      if (amount >= 1000m) {
        result += SpeechHundreds((int) (amount / 1000)) + " Mil ";
      }
      if (amount > 0) {
        result += SpeechHundreds((int) (amount % 1000)) + " ";
      }
      result += currency + cents.ToString("00");
      result = "(" + result.Substring(0, 1) + result.Substring(1).ToLowerInvariant() + "/100 M.N.)";
      result = result.Replace("  ", " ");
      return result;
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

    static public bool ToBoolean(string source) {
      source = source.ToUpperInvariant();
      if (source == "1" || source == "Y" || source == "T" || source == "S" || source == "V" ||
        source == "TRUE" || source == "SI" || source == "SÍ" || source == "VERDADERO") {
        return true;
      }
      if (source == "0" || source == "N" || source == "F" || source == "FALSE" ||
        source == "NO" || source == "FALSO") {
        return false;
      }
      throw new Exception("No reconozco el valor " + source + " como del tipo de datos Boolean");
    }

    static public DateTime ToDate(string source) {
      return ToDateTime(source, "dd/MMM/yyyy");
    }

    static public DateTime ToDateTime(string source) {
      if (source.Contains(":")) {
        return ToDateTime(source, "dd/MMM/yyyy HH:mm");
      } else {
        return ToDate(source);
      }
    }

    static public DateTime ToDateTime(string source, string format) {
      try {
        source = source.Replace("-", "/");
        source = source.Replace("./", "/");
        source = source.Replace(".-", "-");
        return DateTime.ParseExact(source, format, new CultureInfo("es-US"));
      } catch {
        throw new Exception("No reconozco el valor " + source + " como del tipo de datos fecha.");
      }
    }

    static public DateTime ToDateTimeFull(string source) {
      return ToDateTime(source, "dd/MMM/yyyy HH:mm:ss");
    }

    static public DateTime ToDateTimeMax(string source) {
      return ToDateTime(source + " 23:59:59", "dd/MMM/yyyy HH:mm:ss");
    }

    static public decimal ToDecimal(string source) {
      try {
        source = TrimAll(source, ",", String.Empty);
        source = TrimAll(source, "$", String.Empty);
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        if (source.Length != 0) {
          return decimal.Parse(source);
        } else {
          return 0m;
        }
      } catch {
        throw new Exception("No reconozco el valor " + source + " como del tipo de datos decimal.");
      }
    }

    static public double ToDouble(string source) {
      try {
        source = TrimAll(source, ",", String.Empty);
        source = TrimAll(source, "$", String.Empty);
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        if (source.Length != 0) {
          return double.Parse(source);
        } else {
          return 0;
        }
      } catch {
        throw new Exception("No reconozco el valor " + source + " como del tipo de datos double.");
      }
    }

    static public float ToFloat(string source) {
      try {
        source = TrimAll(source, ",", String.Empty);
        source = TrimAll(source, "$", String.Empty);
        if (source.IndexOf(".") >= 0) {
          source = source.TrimStart("0".ToCharArray());
        }
        if (source.StartsWith(".")) {
          source = "0" + source;
        }
        if (source.Length != 0) {
          return float.Parse(source);
        } else {
          return 0;
        }
      } catch {
        throw new Exception("No reconozco el valor " + source + " como del tipo de datos float.");
      }
    }

    static public int ToInteger(string source) {
      try {
        source = TrimAll(source, ",", String.Empty);
        if (source.Length != 0) {
          return int.Parse(source);
        } else {
          return 0;
        }
      } catch {
        throw new Exception("No reconozco el valor " + source + " como del tipo de datos entero.");
      }
    }

    static public string ToProperNoun(string noun) {
      Assertion.AssertObject(noun, "noun");

      string[] nounParts = EmpiriaString.TrimAll(noun).Split(' ');
      string result = String.Empty;
      foreach(string nounPart in nounParts) {
        if (result.Length != 0) {
          result += ' '; 
        }
        if (IsPrepositionOrConjuntion(nounPart)) {
          result += nounPart;  // Prepositions and conjuntions are not capitalized.
        } else {
          result += nounPart.Substring(0, 1).ToUpperInvariant() + nounPart.Substring(1);
        }
      }
      return result;
    }

    static public string TrimAll(string source) {
      string temp = TrimAll(source, "  ", " ");

      return temp.Trim();
    }

    static public string TrimControl(string source) {
      string temp = source;

      for (int i = 0; i < temp.Length; i++) {
        if (Char.IsControl(temp[i])) {
          temp = temp.Replace(temp[i].ToString(), String.Empty);
        }
      }
      return temp;
    }

    static public string TrimSpacesAndControl(string source) {
      if (String.IsNullOrWhiteSpace(source)) {
        return String.Empty;
      }
      string temp = TrimControl(source);
      temp = temp.Replace("|", " ");
      temp = temp.Replace("<", " ");
      temp = temp.Replace(">", " ");
      //temp = temp.Replace("&", " ");
      temp = temp.Replace("\"", "´");
      temp = temp.Replace("\'", "´");

      temp = TrimAll(temp);
      if (!String.IsNullOrWhiteSpace(temp)) {
        return temp;
      } else {
        return String.Empty;
      }
    }

    static public string TrimAll(string source, string pattern, string replaceWith) {
      if (source == null) {
        return String.Empty;
      }
      while (true) {
        if (source.IndexOf(pattern) >= 0) {
          source = source.Replace(pattern, replaceWith);
        } else {
          break;
        }
      }
      return source;
    }

    #endregion Public methods

    #region Private methods

    static private string GetCommonCharacters(string stringA, string stringB, int distanceR) {
      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return String.Empty;
      }
      StringBuilder commonCharacters = new StringBuilder();
      StringBuilder stringBTemp = new StringBuilder(stringB);
      for (int i = 0; i < stringA.Length; i++) {
        for (int j = Math.Max(0, i - distanceR); j < Math.Min(i + distanceR, stringB.Length); j++) {
          if (stringBTemp[j] == stringA[i]) {
            commonCharacters.Append(stringA[i]);
            stringBTemp[j] = (char) 0;
            break;
          }
        }
      }
      return commonCharacters.ToString();
    }

    static private bool IsPrepositionOrConjuntion(string token) {
      string[] noiseTokens = new String[] { " ", "y", "o", "a", "e", "ó", "la", "el", "los", "las", "lo", 
                                            "que", "con", "de", "del" };

      token = token.ToLowerInvariant();
      for (int i = 0; i < noiseTokens.Length; i++) {
        if (token == noiseTokens[i]) {
          return true;
        }
      }
      return false;
    }

    static private int JaroWinklerPrefixLength(string stringA, string stringB, int minPrefixTestLength) {
      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return minPrefixTestLength;
      }

      int min = EmpiriaMath.Min(minPrefixTestLength, stringA.Length, stringB.Length);
      for (int i = 0; i < min; i++) {
        if (stringA[i] != stringB[i]) {
          return i;
        }
      }
      return min;
    }

    static private string PrepareForDistance(string source) {
      return TrimAll(RemoveNoiseExtended(source)).ToLowerInvariant();
    }

    static private string[,] LoadSpeechHundredsArray() {
      string[,] array = new string[10, 5];

      array[0, 0] = ""; array[0, 1] = "Un"; array[0, 2] = "Cien"; array[0, 3] = "y"; array[0, 4] = "";
      array[1, 0] = "Un"; array[1, 1] = "Diez"; array[1, 2] = "Ciento"; array[1, 3] = "Once"; array[1, 4] = "Veintiún";
      array[2, 0] = "Dos"; array[2, 1] = "Veinte"; array[2, 2] = "Doscientos"; array[2, 3] = "Doce"; array[2, 4] = "Veintidós";
      array[3, 0] = "Tres"; array[3, 1] = "Treinta"; array[3, 2] = "Trescientos"; array[3, 3] = "Trece"; array[3, 4] = "Veintitrés";
      array[4, 0] = "Cuatro"; array[4, 1] = "Cuarenta"; array[4, 2] = "Cuatrocientos"; array[4, 3] = "Catorce"; array[4, 4] = "Veinticuatro";
      array[5, 0] = "Cinco"; array[5, 1] = "Cincuenta"; array[5, 2] = "Quinientos"; array[5, 3] = "Quince"; array[5, 4] = "Veinticinco";
      array[6, 0] = "Seis"; array[6, 1] = "Sesenta"; array[6, 2] = "Seiscientos"; array[6, 3] = "Dieciseis"; array[6, 4] = "Veintiseis";
      array[7, 0] = "Siete"; array[7, 1] = "Setenta"; array[7, 2] = "Setecientos"; array[7, 3] = "Diecisiete"; array[7, 4] = "Veintisiete";
      array[8, 0] = "Ocho"; array[8, 1] = "Ochenta"; array[8, 2] = "Ochocientos"; array[8, 3] = "Dieciocho"; array[8, 4] = "Veintiocho";
      array[9, 0] = "Nueve"; array[9, 1] = "Noventa"; array[9, 2] = "Novecientos"; array[9, 3] = "Diecinueve"; array[9, 4] = "Veintinueve";

      return array;
    }

    static private string SpeechHundreds(int amount) {
      string[,] array = LoadSpeechHundredsArray();
      string result = String.Empty;

      int tens = (amount % 100) / 10;
      int units = amount % 10;

      if (amount == 100) {
        return array[0, 2];
      }
      if ((amount / 100) > 0) {
        result = array[(amount / 100), 2] + " ";
      }
      if ((tens == 0) && (units > 1)) {
        return result + array[amount % 10, 0];
      }
      if ((tens == 0) && (units == 1)) {
        return result + array[0, 1];
      }
      if ((tens == 0) && (units == 0) && (amount == 0)) {
        return array[0, 0];
      }
      if ((tens == 1) && (units != 0)) {
        return result + array[units, 3];
      }
      if ((tens == 1) && (units == 0)) {
        return result + array[1, 1];
      }
      if ((tens == 2) && (units != 0)) {
        return result + array[units, 4];
      }
      if ((tens == 2) && (units == 0)) {
        return result + array[2, 1];
      }
      if ((tens > 2) && (units != 0)) {
        return result + array[tens, 1] + " " + array[0, 3] + " " + array[units, 0];
      }
      if ((tens > 2) && (units == 0)) {
        return result + array[tens, 1];
      }
      return result; // if ((tens == 0) && (units == 0) && (amount != 0))  OR OTHERS
    }

    #endregion Private methods

  }  // class EmpiriaString

} // namespace Empiria
