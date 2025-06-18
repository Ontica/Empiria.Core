/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                               Component : Services                                *
*  Assembly : Empiria.Core.dll                           Pattern   : Static service provider                 *
*  Type     : Patcher                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides methods to patch fields and properties data using default values.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria {

  /// <summary>Provides methods to patch fields and properties data using default values.</summary>
  static public class Patcher {

    #region Public methods

    static public string CleanUID(string uidFieldValue) {
      uidFieldValue = EmpiriaString.Clean(uidFieldValue);

      if (string.IsNullOrWhiteSpace(uidFieldValue)) {
        return string.Empty;
      }
      if (uidFieldValue.ToLowerInvariant() == "empty") {
        return string.Empty;
      }
      return uidFieldValue;
    }


    static public string Patch(string newValue, string defaultValue) {
      if (!String.IsNullOrWhiteSpace(newValue)) {
        return newValue;
      }
      return defaultValue;
    }


    static public DateTime Patch(DateTime newValue, DateTime defaultValue) {
      if (newValue != ExecutionServer.DateMaxValue &&
          newValue != ExecutionServer.DateMinValue &&
          newValue != DateTime.MinValue &&
          newValue != DateTime.MaxValue &&
          newValue != new DateTime()) {
        return newValue;
      }

      return defaultValue;
    }


    static public bool Patch(bool? newValue, bool defaultValue) {
      if (newValue.HasValue) {
        return newValue.Value;
      }
      return defaultValue;
    }


    static public int Patch(int? newValue, int defaultValue) {
      if (newValue.HasValue) {
        return newValue.Value;
      }
      return defaultValue;
    }


    static public decimal Patch(decimal? newValue, decimal defaultValue) {
      if (newValue.HasValue) {
        return newValue.Value;
      }
      return defaultValue;
    }


    static public U Patch<U>(int newValue, U defaultValue) where U : BaseObject {
      if (newValue > 0) {
        return BaseObject.ParseId<U>(newValue);
      }
      return defaultValue;
    }


    static public U Patch<U>(string newValue, U defaultValue) where U : BaseObject {
      if (!String.IsNullOrWhiteSpace(newValue)) {
        return BaseObject.ParseKey<U>(newValue);
      }
      return defaultValue;
    }


    static public string PatchClean(string newValue, string defaultValue) {
      string cleaned = EmpiriaString.Clean(newValue);

      return Patch(cleaned, defaultValue);
    }

    #endregion Public methods

  } //class Patcher

} //namespace Empiria
