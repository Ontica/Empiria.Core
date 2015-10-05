/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Domain Services Layer             *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : Validate                                         Pattern  : Stateless Domain Service          *
*  Version   : 1.0        Date: May/2015                        License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides validation methods.                                                                  *
*                                                                                                            *
********************************* Copyright (c) 2003-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  /// <summary>Provides validation methods.</summary>
  static public class Validate {

    static public void IsTrue(bool value, string message,
                              params object[] messageArgs) {
      if (!value) {
        throw new ValidationException("Validate.IsTrue", message, messageArgs);
      }
    }

    static public void NotNull(string value, string message,
                               params object[] messageArgs) {
      if (!String.IsNullOrWhiteSpace(value)) {
        return;
      }
      if (message.Contains(" ")) {
        throw new ValidationException("Validate.NotNullOrEmpty", message, messageArgs);
      } else {
        throw new ValidationException("Validate.NotNullOrEmpty",
                                      message + " field can't be null or empty.");
      }
    }

    static public void NotNull(object value, string message,
                               params object[] messageArgs) {
      if (value == null) {
        throw new ValidationException("Validate.NotNull", message, messageArgs);
      }
    }

  } // class Validate

} // namespace Empiria
