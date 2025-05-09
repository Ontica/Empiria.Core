/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                               Component : Services                                *
*  Assembly : Empiria.Core.dll                           Pattern   : Static service provider                 *
*  Type     : FieldPatcher                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains methods to patch fields data using default values.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Contains methods to patch fields data using default values.</summary>
  static public class FieldPatcher {

    #region Public methods

    public static string Clean(string fieldValue) {
      fieldValue = EmpiriaString.Clean(fieldValue);

      if (string.IsNullOrWhiteSpace(fieldValue)) {
        return string.Empty;
      }
      if (fieldValue.ToLowerInvariant() == "empty") {
        return string.Empty;
      }
      return fieldValue;
    }


    static public string PatchField(string newValue, string defaultValue) {
      if (!String.IsNullOrWhiteSpace(newValue)) {
        return newValue;
      }
      return defaultValue;
    }


    static public DateTime PatchField(DateTime newValue, DateTime defaultValue) {
      if (newValue != ExecutionServer.DateMaxValue &&
          newValue != ExecutionServer.DateMinValue &&
          newValue != DateTime.MinValue &&
          newValue != DateTime.MaxValue &&
          newValue != new DateTime()) {
        return newValue;
      }

      return defaultValue;
    }


    static public bool PatchField(bool? newValue, bool defaultValue) {
      if (newValue.HasValue) {
        return newValue.Value;
      }
      return defaultValue;
    }


    static public U PatchField<U>(int newValue, U defaultValue) where U : BaseObject {
      if (newValue > 0) {
        return BaseObject.ParseId<U>(newValue);
      }
      return defaultValue;
    }


    static public U PatchField<U>(string newValue, U defaultValue) where U : BaseObject {
      if (!String.IsNullOrWhiteSpace(newValue)) {
        return BaseObject.ParseKey<U>(newValue);
      }
      return defaultValue;
    }

    #endregion Public methods

  } //class FieldPatcher

} //namespace Empiria
