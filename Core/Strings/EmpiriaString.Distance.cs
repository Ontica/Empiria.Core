/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Kernel Types                      *
*  Namespace : Empiria                                          License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaString (Partial)                          Pattern  : Static Data Type                  *
*                                                                                                            *
*  Summary   : Represents a string data type.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Text;

namespace Empiria {

  static public partial class EmpiriaString {

    #region Enumerations

    public enum DistanceAlgorithm {
      Levenshtein = 1,
      DamerauLevenshtein = 2,
      Jaro = 3,
      JaroWinkler = 4
    }

    #endregion Enumerations

    #region Fields

    private const decimal jaroWinklerDefaultPrefixAdjustmentScale = 0.1m;
    private const int jaroWinklerDefaultMaxPrefixDistance = 4;

    #endregion Fields

    #region Public methods

     static public int DamerauLevenshteinDistance(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      int[][] distanceMatrix = new int[stringA.Length + 1][];

      for (int i = 0; i <= stringA.Length; i++) {
        distanceMatrix[i] = new int[stringB.Length + 1];
        distanceMatrix[i][0] = i;
      }
      for (int j = 0; j <= stringB.Length; j++) {
        distanceMatrix[0][j] = j;
      }
      for (int i = 1; i <= stringA.Length; i++) {
        for (int j = 1; j <= stringB.Length; j++) {
          distanceMatrix[i][j] = EmpiriaMath.Min(distanceMatrix[i - 1][j] + 1, distanceMatrix[i][j - 1] + 1,
                                                 distanceMatrix[i - 1][j - 1] + ((stringA[i - 1] == stringB[j - 1]) ? 0 : 1));

          // Transposition
          if ((i > 1) && (j > 1) && (stringA[i - 1] == stringB[j - 2]) && (stringA[i - 2] == stringB[j - 1])) {
            distanceMatrix[i][j] = Math.Min(distanceMatrix[i][j],
                                            distanceMatrix[i - 2][j - 2] + ((stringA[i - 1] == stringB[j - 1]) ? 0 : 1));
          }
        }
      }
      return distanceMatrix[stringA.Length][stringB.Length];
    }

    static public decimal DamerauLevenshteinProximityFactor(string stringA, string stringB) {
      int distance = DamerauLevenshteinDistance(stringA, stringB);

      return decimal.One - ((decimal) distance / Math.Max(stringA.Length, stringB.Length));
    }

    static public decimal JaroProximityFactor(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return decimal.Zero;
      }

      int distanceR = (Math.Max(stringA.Length, stringB.Length) / 2) - 1;
      string stringACommonCharsInB = GetCommonCharacters(stringA, stringB, distanceR);
      if (stringACommonCharsInB.Length == 0) {
        return decimal.Zero;
      }
      string stringBCommonCharsInA = GetCommonCharacters(stringB, stringA, distanceR);
      if (stringBCommonCharsInA.Length == 0) {
        return decimal.Zero;
      }
      int transpositions = 0;
      if (stringACommonCharsInB != stringBCommonCharsInA) {
        for (int i = 0, j = Math.Min(stringACommonCharsInB.Length, stringBCommonCharsInA.Length); i < j; i++) {
          if (stringACommonCharsInB[i] != stringBCommonCharsInA[i]) {
            transpositions++;
          }
        }
      }
      return ((stringACommonCharsInB.Length / (decimal) stringA.Length) + (stringBCommonCharsInA.Length / (decimal) stringB.Length) +
              ((stringACommonCharsInB.Length - (transpositions / 2.0m)) / (decimal) stringACommonCharsInB.Length)) / 3.0m;
    }

    static public decimal JaroWinklerProximityFactor(string stringA, string stringB) {
      return JaroWinklerProximityFactor(stringA, stringB, EmpiriaString.jaroWinklerDefaultPrefixAdjustmentScale,
                                        EmpiriaString.jaroWinklerDefaultMaxPrefixDistance);
    }

    static public decimal JaroWinklerProximityFactor(string stringA, string stringB,
                                                     decimal prefixAdjustmentScale, int maxPrefixDistance) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return decimal.Zero;
      }

      decimal jaroRatio = JaroProximityFactor(stringA, stringB);
      int prefixLength = JaroWinklerPrefixLength(stringA, stringB, maxPrefixDistance);

      return jaroRatio + (prefixLength * prefixAdjustmentScale) * (1.0m - jaroRatio);
    }

    static public int LevenshteinDistance(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      int[][] distanceMatrix = new int[stringA.Length + 1][];

      for (int i = 0; i <= stringA.Length; i++) {
        distanceMatrix[i] = new int[stringB.Length + 1];
        distanceMatrix[i][0] = i;
      }
      for (int j = 0; j <= stringB.Length; j++) {
        distanceMatrix[0][j] = j;
      }
      for (int i = 1; i <= stringA.Length; i++) {
        for (int j = 1; j <= stringB.Length; j++) {
          distanceMatrix[i][j] = EmpiriaMath.Min(distanceMatrix[i - 1][j] + 1, distanceMatrix[i][j - 1] + 1,
                                                 distanceMatrix[i - 1][j - 1] + ((stringA[i - 1] == stringB[j - 1]) ? 0 : 1));
        }
      }
      return distanceMatrix[stringA.Length][stringB.Length];
    }

    static public decimal LevenshteinProximityFactor(string stringA, string stringB) {
      int distance = LevenshteinDistance(stringA, stringB);

      return decimal.One - ((decimal) distance / Math.Max(stringA.Length, stringB.Length));
    }

    static public decimal MongeElkanProximityFactor(DistanceAlgorithm algorithm, string stringA, string stringB) {
      string[] stringArrayA = stringA.Split(' ');
      string[] stringArrayB = stringB.Split(' ');

      decimal accumulator = 0m;
      for (int i = 1; i <= stringArrayA.Length; i++) {
        decimal maximum = decimal.Zero;
        for (int j = 1; j <= stringArrayB.Length; j++) {
          switch (algorithm) {
            case DistanceAlgorithm.Levenshtein:
              maximum = Math.Max(maximum, LevenshteinProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1]));
              break;
            case DistanceAlgorithm.DamerauLevenshtein:
              maximum = Math.Max(maximum, DamerauLevenshteinProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1]));
              break;
            case DistanceAlgorithm.Jaro:
              maximum = Math.Max(maximum, JaroProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1]));
              break;
            case DistanceAlgorithm.JaroWinkler:
              maximum = Math.Max(maximum, JaroWinklerProximityFactor(stringArrayA[i - 1], stringArrayB[j - 1],
                                                                     jaroWinklerDefaultPrefixAdjustmentScale,
                                                                     jaroWinklerDefaultMaxPrefixDistance));
              break;
          }
        }
        accumulator += maximum;
      }
      return accumulator / stringArrayA.Length;
    }

    #endregion Public methods

    #region Private methods

    static private string GetCommonCharacters(string stringA, string stringB, int distanceR) {
      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return String.Empty;
      }
      StringBuilder commonCharacters = new StringBuilder();
      StringBuilder stringBTemp = new StringBuilder(stringB);
      for (int i = 0; i < stringA.Length; i++) {
        for (int j = Math.Max(0, i - distanceR); j < Math.Min(i + distanceR, stringB.Length); j++) {
          if (stringBTemp[j] == stringA[i]) {
            commonCharacters.Append(stringA[i]);
            stringBTemp[j] = (char) 0;
            break;
          }
        }
      }
      return commonCharacters.ToString();
    }

    static private int JaroWinklerPrefixLength(string stringA, string stringB, int minPrefixTestLength) {
      if (String.IsNullOrEmpty(stringA) || String.IsNullOrEmpty(stringB)) {
        return minPrefixTestLength;
      }

      int min = EmpiriaMath.Min(minPrefixTestLength, stringA.Length, stringB.Length);
      for (int i = 0; i < min; i++) {
        if (stringA[i] != stringB[i]) {
          return i;
        }
      }
      return min;
    }

    static private string PrepareForDistance(string source) {
      return TrimAll(RemoveNoiseExtended(source)).ToLowerInvariant();
    }

    #endregion Private methods

  }  // class EmpiriaString

} // namespace Empiria
