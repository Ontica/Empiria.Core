/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Kernel Types                      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EmpiriaString (Partial)                          Pattern  : Static Data Type                  *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Contains methods that gets textual representation of currencies, integers and dates.          *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria {

  /// <summary>Contains methods that gets textual representation of currencies, integers and dates.</summary>
  static public partial class EmpiriaString {

    #region Public methods

    static public string SpeechDate(DateTime date) {
      string temp = String.Empty;

      if (date.Day == 1) {
        temp = "Primero";
      } else {
        temp = EmpiriaString.SpeechInteger(date.Day);
      }
      temp += " de " + date.ToString("MMMM") + " del año ";
      temp += EmpiriaString.SpeechInteger(date.Year);

      return temp;
    }

    static public string SpeechInteger(int value) {
      string result = String.Empty;

      if (value == 0) {
        return "Cero";
      } else if (value == 1) {
        return "Uno";
      }
      if ((value >= 1000000m) && ((int) (value / 1000000)) == 1) {
        result = SpeechHundreds((int) (value / 1000000)) + " Millón ";
      } else if (value > 1000000m) {
        result = SpeechHundreds((int) (value / 1000000)) + " Millones ";
      }
      value = value % 1000000;
      if (value >= 1000m) {
        result += (value / 1000) == 1 ? " Mil " : SpeechHundreds(value / 1000) + " Mil ";
      }
      if (value > 0) {
        result += SpeechHundreds((int) (value % 1000)) + " ";
      }      
      return EmpiriaString.TrimAll(result);
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

    #endregion Public methods

    #region Private methods

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

  }  // class EmpiriaString (Speech)

} // namespace Empiria
