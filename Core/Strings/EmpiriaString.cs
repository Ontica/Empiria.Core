/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Strings                            Component : Services Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Static methods library                  *
*  Type     : EmpiriaString                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static library for string manipulation.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Globalization;

using Empiria.Json;

namespace Empiria {

  static public partial class EmpiriaString {

    #region Public methods


    static public bool All(string source, string characterSet) {
      for (int i = 0; i < source.Length; i++) {
        if (!characterSet.Contains(source.Substring(i, 1))) {
          return false;
        }
      }
      return true;
    }


    static public bool AllDigits(string value) {
      foreach (char c in value) {
        if (!Char.IsDigit(c)) {
          return false;
        }
      }
      return true;
    }


    static public string BuildKeywords(params string[] words) {
      string temp = String.Join(" ", words).ToLowerInvariant();

      var keywords = EmpiriaString.BuildKeywords(temp, true);

      return Truncate(keywords, 4000);
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


    static public string BuildRandomString(int length) {
      return BuildRandomString(length, length);
    }


    static public string BuildRandomString(int minLength, int maxLength) {
      Assertion.Require(0 < minLength,
                      $"Parameter 'minLength' ({minLength}) must be greater than zero.");
      Assertion.Require(minLength == -1 || minLength <= maxLength,
                      $"Parameter 'minLength' ({minLength}) must be less or equal than " +
                      $"parameter 'maxLength' ({maxLength}).");

      var temp = String.Empty;

      int length = minLength;
      if (maxLength != -1) {
        length = EmpiriaMath.GetRandom(minLength, maxLength);
      }

      for (int i = 0; i < length; i++) {
        string value = EmpiriaMath.GetFullRandomDigitOrCharacter().ToString();

        if (EmpiriaMath.GetRandomBoolean()) {
          value = value.ToLowerInvariant();
        } else {
          value = value.ToUpperInvariant();
        }

        temp += value;
      }
      return temp;
    }


    /// <summary>Trims all excesive whitespaces and removes any control characters.</summary>
    static public string Clean(string value) {
      value = EmpiriaString.TrimControl(value);

      return EmpiriaString.TrimAll(value);
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

    static public bool ContainsAnyChar(string source, string characterSet) {
      for (int i = 0; i < characterSet.Length; i++) {
        if (source.Contains(characterSet.Substring(i, 1))) {
          return true;
        }
      }
      return false;
    }

    static public bool ContainsSegment(string source, string data, int segmentLength) {
      if (segmentLength <= 0) {
        return false;
      }
      if (data.Length < segmentLength) {
        return false;
      }
      if (source.Length < segmentLength) {
        return false;
      }

      for (int startIndex = 0; startIndex <= data.Length - segmentLength; startIndex++) {
        string segment = data.Substring(startIndex, segmentLength);
        if (source.Contains(segment)) {
          return true;
        }
      }
      return false;
    }

    static public T ConvertTo<T>(string source) {
      Assertion.Require(source, "source");

      if (typeof(T) == typeof(string)) {
        return (T) (object) source;

      } else if (typeof(T) == typeof(object)) {
        return (T) (object) source;

      } else if (typeof(T).IsPrimitive) {
        return (T) Convert.ChangeType(source, typeof(T));

      } else if (typeof(T).IsEnum) {
        return (T) Enum.Parse(typeof(T), source);

      } else if (JsonConverter.IsValidJson(source)) {
        return Empiria.Json.JsonConverter.ToObject<T>(source);

      } else {
        throw new EmpiriaStringException(EmpiriaStringException.Msg.CantConvertStringToTypeInstance,
                                         source, typeof(T).FullName);
      }
    }


    static public int CountOccurences(string source, char value) {
      return source.Split(value, value).Length - 1;
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

    static public string DateTimeString(object date) {
      if (date != null) {
        return DateTimeString((DateTime) date);
      } else {
        return DateTimeString(DateTime.MaxValue);
      }
    }


    static public string Duplicate(string source, int times) {
      string temp = String.Empty;

      for (int i = 0; i < times; i++) {
        temp += source;
      }

      return temp;
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


    static public string Format(string source, object[] arguments) {
      if (source != null && arguments != null && arguments.Length != 0) {
        return String.Format(source, arguments);
      } else {
        return source;
      }
    }


    static public string GetSection(string source, char delimiter, int sectionIndex) {
      string[] sections = source.Split(new char[] { delimiter });

      return sections[sectionIndex].Trim();
    }


    static public string IncrementCounter(string source, string prefix = "") {
      string counterPart = source.Replace(prefix, String.Empty);

      int counter = EmpiriaString.ToInteger(counterPart);

      counter++;

      return prefix + counter.ToString(Duplicate("0", counterPart.Length));
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


    static public bool IsDate(string source) {
      DateTime dischardedResult;

      return DateTime.TryParse(source, out dischardedResult);
    }

    static public bool NotIsDate(string source) {
      return !IsDate(source);
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
      string temp = TrimAll(source);

      if (String.IsNullOrWhiteSpace(source)) {
        return true;
      }

      for (int i = 0; i < temp.Length; i++) {
        if (!Char.IsWhiteSpace(temp[i])) {
          return false;
        }
      }
      return true;
    }


    static public bool NotIsEmpty(string source) {
      return !IsEmpty(source);
    }


    static public bool IsInList(string source, string firstValue, params string[] moreValues) {
      Assertion.Require(source, "source");
      Assertion.Require(firstValue, "firstValue");

      if (source == firstValue) {
        return true;
      }
      for (int i = 0; i < moreValues.Length; i++) {
        if (moreValues[i] == source) {
          return true;
        }
      }
      return false;
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
                                        '°', '%', '#', '¿', '!', '¡', '´', '`', '~', '|'};

      for (int i = 0; i < punctuations.Length; i++) {
        source = source.Replace(punctuations[i], ' ');
      }
      return TrimAll(source);
    }

    static public string RemoveEndPunctuation(string source) {
      char[] punctuations = new char[] {'.', ',', ';', ':', '=', '-', '_', '|'};

      string temp = source;

      for (int i = 0; i < punctuations.Length; i++) {
        temp = temp.TrimEnd(punctuations[i]);
      }
      return temp.TrimEnd();
    }


    static public bool Similar(string stringA, string stringB) {
      stringA = RemoveNoise(stringA).ToLowerInvariant();
      stringB = RemoveNoise(stringB).ToLowerInvariant();

      return (stringA.Equals(stringB));
    }


    static public bool StartsWith(string source, string firstValue, params string[] moreValues) {
      Assertion.Require(source, "source");
      Assertion.Require(firstValue, "firstValue");

      if (source.StartsWith(firstValue)) {
        return true;
      }
      for (int i = 0; i < moreValues.Length; i++) {
        if (source.StartsWith(moreValues[i])) {
          return true;
        }
      }
      return false;
    }


    static public string Truncate(string source, int maxLength) {
      if (String.IsNullOrEmpty(source)) {
        return source;
      }
      if (source.Length > maxLength) {
        return source.Substring(0, maxLength);
      } else {
        return source;
      }
    }


    public static string TruncateLast(string source, int maxLength) {
      if (String.IsNullOrEmpty(source)) {
        return source;
      }
      if (source.Length > maxLength) {
        return source.Substring(maxLength - 1);
      } else {
        return source;
      }
    }



    static public bool? TryToBoolean(string source) {
      source = source.ToUpperInvariant();

      if (source == "1" || source == "Y" || source == "T" || source == "S" || source == "V" ||
        source == "TRUE" || source == "SI" || source == "SÍ" || source == "VERDADERO") {
        return true;
      }

      if (source == "0" || source == "N" || source == "F" || source == "FALSE" ||
        source == "NO" || source == "FALSO") {
        return false;
      }

      return null;
    }

    static public bool ToBoolean(string source) {
      bool? value = TryToBoolean(source);

      if (!value.HasValue) {
        throw Assertion.EnsureNoReachThisCode($"No reconozco el valor '{source}' como del tipo de datos Boolean.");
      }

      return value.Value;
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
        throw new Exception($"No reconozco el valor {source} como del tipo de datos fecha.");
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
        throw new Exception($"No reconozco el valor {source} como del tipo de datos decimal.");
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
        throw new Exception($"No reconozco el valor {source} como del tipo de datos entero.");
      }
    }

    static public string ToProperNoun(string noun) {
      Assertion.Require(noun, "noun");

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
      if (String.IsNullOrWhiteSpace(source)) {
        return String.Empty;
      }

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
      if (source.Length == 0) {
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

    static private bool IsPrepositionOrConjuntion(string token) {
      string[] noiseTokens = new String[] { " ", "y", "o", "a", "e", "ó", "la", "el", "los", "las", "lo",
                                            "que", "con", "de", "del", "the", "and", "or", "of", "on",
                                            "in", "at", "for", "by", "to"};

      token = token.ToLowerInvariant();
      for (int i = 0; i < noiseTokens.Length; i++) {
        if (token == noiseTokens[i]) {
          return true;
        }
      }
      return false;
    }

    #endregion Private methods

  }  // class EmpiriaString

} // namespace Empiria
