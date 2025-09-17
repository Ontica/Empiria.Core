/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaMath                                      Pattern  : Static Data Type                  *
*                                                                                                            *
*  Summary   : Mathematical methods library class.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using System.Security.Cryptography;

namespace Empiria {

  /// <summary>Mathematical methods library class.</summary>
  static public class EmpiriaMath {

    #region Methods

    static public decimal Add(decimal a, params decimal[] list) {
      decimal result = 0m;

      result = a;
      for (int i = 0; i < list.Length; i++) {
        result += list[i];
      }
      return result;
    }


    static public decimal ApplyPercentage(decimal quantity, decimal percentage) {
      return (quantity * percentage) / 100m;
    }


    static public char GetFullRandomDigitOrCharacter(string current = "") {
      const string digitsAndCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

      return GetRandomCharacterHelper(digitsAndCharacters, current);
    }


    static public int GetRandom(int minValue, int maxValue) {
      Assertion.Require(minValue <= maxValue,
                        "minValue parameter must be greater than maxValue parameter.");

      using (var generator = RandomNumberGenerator.Create()) {

        uint range = (uint) (maxValue - minValue + 1);

        // Array to store 4 bytes for the random number
        byte[] bytes = new byte[4];

        uint result;

        while (true) {
          // Fill the array with random bytes
          generator.GetBytes(bytes);

          // Convert the bytes to an unsigned integer
          result = BitConverter.ToUInt32(bytes, 0);

          // Repeat if the result could cause bias
          if (result < uint.MaxValue - (uint.MaxValue % range)) {
            break;
          }
        }

        // Apply the modulus and adjust to the desired range
        return (int) (result % range) + minValue;
      }
    }


    static public bool GetRandomBoolean() {
      int i = GetRandom(1, 100);

      return ((i % 2) == 0);
    }


    static public char GetRandomCharacter(string current = "") {
      const string characters = "ABCDEFHJKLMNPQRSTUVWXYZ";

      return GetRandomCharacterHelper(characters, current);
    }

    static public char GetRandomDigit(string current = "") {
      const string digits = "2345789";

      return GetRandomCharacterHelper(digits, current);
    }

    static public char GetRandomDigitOrCharacter(string current = "") {
      const string digitsAndCharacters = "ABC2D3E45FH7J8K9MLNPQRSTUVWXYZ";

      return GetRandomCharacterHelper(digitsAndCharacters, current);
    }


    static public int[] GetRange(int start, int end) {
      Assertion.Require(start <= end, "start is greater than end parameter.");

      var list = new List<int>(end - start + 1);

      for (int i = start; i <= end; i++) {
        list.Add(i);
      }

      return list.ToArray();
    }


    static public bool IsMemberOf(int value, int[] array) {
      foreach (int arrayItem in array) {
        if (arrayItem == value) {
          return true;
        }
      }
      return false;
    }

    static public int Max(int a, int b, int c) {
      return Math.Max(Math.Max(a, b), c);
    }

    static public int Min(int a, int b, int c) {
      return Math.Min(Math.Min(a, b), c);
    }

    static public decimal ScalarProduct(decimal[] a, decimal[] b) {
      decimal result = 0;

      for (int i = 0; i < a.Length; i++) {
        result += a[i] * b[i];
      }
      return result;
    }

    #endregion Methods

    #region Helpers

    static private char GetRandomCharacterHelper(string characters, string current) {
      string attempts = string.Empty;

      while (true) {

        char character = characters[GetRandom(0, characters.Length - 1)];

        if (!current.Contains(character.ToString())) {
          return character;

        } else if (attempts.Length >= characters.Length) {
          return character;

        } else {
          attempts += character;

        }
      }
    }

    #endregion Helpers

  }  // class EmpiriaMath

} // namespace Empiria
