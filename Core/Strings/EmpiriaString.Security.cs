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

      string[] unsafePatterns =         {
          @"(""|\s|^)(select|insert|update|delete|drop|truncate|create|alter|exec|execute)\s",
          @"(""|\s|^)(sysadmin|is_member|is_srvrolemember)",
          @"(""|\s|^)(sys.)\S",
          @"(""|\s|^)(xp|sp|apd|do|qry|write|user|all|db)_\S",
          @"javascript:", @"vbscript:", @"onload\s*=",
          @"<script\s", @"</script>", @"function\s*\(", @"sub\s*\(", @"alert\s*\(",
          @"<html\s", @"</html>", @"<head\s", @"</head>", @"<body\s", @"</body>",
          @"<iframe\s", @"</iframe>", @"<form\s", @"</form>", @"<input\s",
          @"<a\s", @"</a>", @"<link\s", @"</link>",@"<button\s", @"</button>", @"href\s*=",
          @"benchmark\s*\(", @"sleep\s*\(", @"waitfor\s+delay"
      };

      return unsafePatterns.Any(pattern => Regex.IsMatch(source, pattern,
                                                         RegexOptions.IgnoreCase,
                                                         TimeSpan.FromSeconds(5)));
    }

    #endregion Methods

  }  // class EmpiriaString

} // namespace Empiria
