/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Strings                            Component : Services Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Static methods library                  *
*  Type     : EmpiriaString (partial)                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static partial class with methods for string security.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using System.Text.RegularExpressions;

using Empiria.Security;

namespace Empiria {

  /// <summary>Static partial class with methods for string security.</summary>
  static public partial class EmpiriaString {

    #region Fields

    static private readonly string[] _unsafePatterns = {
        @"(""|\s|^)select\s+.+\s+from\s",
        @"(""|\s|^)insert\s+into\s",
        @"(""|\s|^)update\s+.+\s+set\s",
        @"(""|\s|^)delete\s+from\s",
        @"(""|\s|^)(drop|create|alter)\s+(table|database|schema|view|index|procedure|proc|package|function|trigger|sequence|
                                          synonym|user|login|role|column|constraint|type|default|rule|assembly|certificate|
                                          credential|queue|service|statistics|tablespace|profile|cluster|context|directory|
                                          materialized\s+view|package\s+body|partition\s+function|fulltext\s+index|database\s+link)\s",
        @"(""|\s|^)truncate\s+table\s",
        @"(""|\s|^)exec(ute)?\s+\S",
        @"(""|\s|^)(sysadmin|is_member|is_srvrolemember)",
        @"(""|\s|^)(sys\.)\w",
        @"(""|\s|^)(xp|sp|apd|do|qry|write|user|all|db)_\w",
        @"javascript:", @"vbscript:", @"data:text/html",
        @"\bon\w+\s*=",
        @"<\s*script\b", @"<\s*/\s*script\s*>",
        @"<\s*iframe\b", @"<\s*/\s*iframe\s*>",
        @"<\s*object\b", @"<\s*/\s*object\s*>",
        @"<\s*embed\b", @"<\s*applet\b", @"<\s*/\s*applet\s*>",
        @"<\s*meta\b", @"<\s*base\b",
        @"<\s*style\b", @"<\s*/\s*style\s*>", @"expression\s*\(",
        @"<\s*svg\b", @"<\s*/\s*svg\s*>", @"<\s*img\b",
        @"<\s*html\b", @"<\s*/\s*html\s*>", @"<\s*head\b", @"<\s*/\s*head\s*>",
        @"<\s*body\b", @"<\s*/\s*body\s*>",
        @"<\s*form\b", @"<\s*/\s*form\s*>", @"<\s*input\b",
        @"<\s*a\b", @"<\s*/\s*a\s*>", @"<\s*link\b", @"<\s*/\s*link\s*>",
        @"<\s*button\b", @"<\s*/\s*button\s*>",
        @"<\s*video\b", @"<\s*audio\b", @"<\s*source\b",
        @"function\s*\(", @"sub\s*\(", @"alert\s*\(", @"href\s*=",
        @"benchmark\s*\(", @"sleep\s*\(", @"waitfor\s+delay"
    };

    #endregion Fields

    #region Methods

    static public void EnsureIsSafe(string value, string message) {
      if (IsSafe(value)) {
        return;
      }

      EmpiriaLog.Critical(message);

      throw new SecurityException(SecurityException.Msg.UnsafeInput);
    }


    static public bool IsSafe(string source) {
      return !IsUnsafe(source);
    }


    static public bool IsUnsafe(string source) {
      if (string.IsNullOrWhiteSpace(source)) {
        return false;
      }

      return _unsafePatterns.Any(pattern => Regex.IsMatch(source, pattern,
                                                          RegexOptions.IgnoreCase,
                                                          TimeSpan.FromSeconds(5)));
    }

    #endregion Methods

  }  // class EmpiriaString

} // namespace Empiria
