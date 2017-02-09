/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaString                                    Pattern  : Static Data Type                  *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Library for string manipulation.                                                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Empiria {

  static public partial class EmpiriaString {

    #region Public methods

    static public string GetSection(string source, char delimiter, int sectionIndex) {
      string[] sections = source.Split(new char[] { delimiter });

      return sections[sectionIndex].Trim();
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

    public static string EncodeAsUrlIdentifier(string identifier) {
      Assertion.AssertObject(identifier, "identifier");
      Assertion.Assert(!identifier.Contains("¯") &&
                       !identifier.Contains("―") &&
                       !identifier.Contains("¢") &&
                       !identifier.Contains("§") &&
                       !identifier.Contains("¿") &&
                       !identifier.Contains("¤") &&
                       !identifier.Contains("±") &&
                       !identifier.Contains("÷") &&
                       !identifier.Contains("¦"),
                       "Identifier has one or more characters that makes it not suitable for encoding.");

      identifier = identifier.Replace("_", "¯");   // Protect underscores with macrons
      identifier = identifier.Replace("/", @"_");  // Replace slashes with underscores

      identifier = identifier.Replace("-", "―");   // Protect hypens with horizontal bars (U+2015)
      identifier = identifier.Replace(" ", "-");   // Replace spaces with hypens

      identifier = identifier.Replace("%", "¢");   // Protect percentage with cent sign
      identifier = identifier.Replace("&", "§");   // Replace ampersands with section signs
      identifier = identifier.Replace("?", "¿");   // Replace question ma__rks with initial question marks
      identifier = identifier.Replace("=", "¤");   // Replace equal with currency sign ¤
      identifier = identifier.Replace("+", "±");   // Replace plus with currency plus-minus
      identifier = identifier.Replace(":", "÷");   // Replace colons with division sign
      identifier = identifier.Replace(@"\", "¦");  // Replace back slashes with broken bars

      return identifier;
    }

    public static string DecodeUrlIdentifier(string identifier) {
      Assertion.AssertObject(identifier, "identifier");

      // Identifiers that starts with a '!' are not encoded, so return them as is without the '!' char.
      if (identifier.StartsWith("!")) {
        return identifier.Substring(1);
      }

      identifier = identifier.Replace("¦", @"\");  // Replace broken bars with back slashes
      identifier = identifier.Replace("÷", ":");   // Replace division sign with colons
      identifier = identifier.Replace("±", "+");   // Replace currency plus-minus with plus sign
      identifier = identifier.Replace("¤", "=");   // Replace currency sign ¤ with equal sign
      identifier = identifier.Replace("¿", "?");   // Replace initial question marks with question marks
      identifier = identifier.Replace("§", "&");   // Replace section signs with ampersands
      identifier = identifier.Replace("¢", "%");   // Protect cent signs with percentage symbols

      identifier = identifier.Replace("-", " ");   // Replace hypens with spaces
      identifier = identifier.Replace("―", "-");   // Replace protected horizontal bars (U+2015) with hypens

      identifier = identifier.Replace(@"_", "/");  // Replace underscores with slashes
      identifier = identifier.Replace("¯", "_");   // Replace protected macrons with underscores

      return identifier;
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

    static public string TrimIfLongThan(string text, int maxLength) {
      if (String.IsNullOrWhiteSpace(text)) {
        return String.Empty;
      }
      if (text.Length > (maxLength - 3)) {
        return text.Substring(0, maxLength - 4) + "...";
      } else {
        return text;
      }
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
