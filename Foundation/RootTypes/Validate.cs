/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Domain Services Layer             *
*  Namespace : Empiria                                          Assembly : Empiria.Foundation.dll            *
*  Type      : Validate                                         Pattern  : Stateless Domain Service          *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides validation methods.                                                                  *
*                                                                                                            *
********************************* Copyright (c) 2003-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Json;

namespace Empiria {

  /// <summary>Provides validation methods.</summary>
  static public class Validate {

    static public void AlreadyExists(object value, string message) {
      if (value != null) {
        throw new ValidationException("Validate.AlreadyExists", message);
      }
    }

    static public void Fail(string message) {
      throw new ValidationException("Validate.Fail", message);
    }

    static public void HasValue(JsonObject json, string itemPath, string message) {
      Assertion.AssertObject(json, "json");
      Assertion.AssertObject(itemPath, "itemPath");

      if (!json.HasValue(itemPath)) {
        throw new ValidationException("Validate.HasValue", message);
      }
    }

    static public void IsNull(object value, string message) {
      if (value != null) {
        throw new ValidationException("Validate.IsNull", message);
      }
    }

    static public void IsTrue(bool value, string message) {
      if (!value) {
        throw new ValidationException("Validate.IsTrue", message);
      }
    }

    static public void NotNull(string value, string message) {
      if (!String.IsNullOrWhiteSpace(value)) {
        return;
      }
      if (message.Contains(" ")) {
        throw new ValidationException("Validate.NotNullOrEmpty", message);
      } else {
        throw new ValidationException("Validate.NotNullOrEmpty",
                                      message + " field can't be null or empty.");
      }
    }

    static public void NotNull(object value, string message) {
      if (value == null) {
        throw new ValidationException("Validate.NotNull", message);
      }
    }

  } // class Validate

} // namespace Empiria
