﻿/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Strings                            Component : Services Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Static methods library                  *
*  Type     : EmpiriaString (partial)                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static class with string distance methods using different algorithms.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Empiria {

  /// <summary>Static class with string distance methods using different algorithms.</summary>
  static public class EmpiriaStringDistance {

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

    #region Methods

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


    static public decimal JaccardProximityFactor(string stringA, string stringB, int tokenSize = 4) {
      HashSet<string> bigramsA = GetBigrams(stringA, tokenSize);
      HashSet<string> bigramsB = GetBigrams(stringB, tokenSize);

      int intersection = bigramsA.Intersect(bigramsB).Count();
      int union = bigramsA.Union(bigramsB).Count();

      // Handle edge case where both have no bigrams (e.g., empty or single-character strings)
      if (union == 0) {
        return stringA.Equals(stringB) ? 1m : 0m;
      }

      return (decimal) intersection / union;
    }


    static public decimal JaroProximityFactor(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      if (string.IsNullOrEmpty(stringA) || string.IsNullOrEmpty(stringB)) {
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
      return JaroWinklerProximityFactor(stringA, stringB, jaroWinklerDefaultPrefixAdjustmentScale,
                                        jaroWinklerDefaultMaxPrefixDistance);
    }


    static public decimal JaroWinklerProximityFactor(string stringA, string stringB,
                                                     decimal prefixAdjustmentScale, int maxPrefixDistance) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      if (string.IsNullOrEmpty(stringA) || string.IsNullOrEmpty(stringB)) {
        return decimal.Zero;
      }

      decimal jaroRatio = JaroProximityFactor(stringA, stringB);
      int prefixLength = JaroWinklerPrefixLength(stringA, stringB, maxPrefixDistance);

      return jaroRatio + (prefixLength * prefixAdjustmentScale) * (1.0m - jaroRatio);
    }


    static public int LevenshteinDistance(string stringA, string stringB) {
      stringA = PrepareForDistance(stringA);
      stringB = PrepareForDistance(stringB);

      var distanceMatrix = new int[stringA.Length + 1][];

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

      var accumulator = decimal.Zero;

      for (int i = 1; i <= stringArrayA.Length; i++) {

        var maximum = decimal.Zero;

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


    static public string KeywordsForDistance(string source) {
      source = PrepareForDistance(source);

      FixedList<string> list = source.Split(' ')
                                      .ToFixedList()
                                      .FindAll(x => x.Length >= 3 || EmpiriaString.IsInteger(x))
                                      .Sort((x, y) => x.CompareTo(y));

      return EmpiriaString.BuildKeywords(EmpiriaString.TrimAll(string.Join(" ", list)));
    }


    static public string PrepareForDistance(string source) {
      return EmpiriaString.TrimAll(EmpiriaString.RemoveNoiseExtended(source)).ToLowerInvariant();
    }


    static public decimal SorensenDiceProximityFactor(string stringA, string stringB, int tokenSize = 4) {
      HashSet<string> bigramsA = GetBigrams(stringA, tokenSize);
      HashSet<string> bigramsB = GetBigrams(stringB, tokenSize);

      int intersection = bigramsA.Intersect(bigramsB).Count();
      int aCount = bigramsA.Count;
      int bCount = bigramsB.Count;

      if (aCount + bCount == 0) {
        return stringA.Equals(stringB) ? 1m : 0m;
      }

      return (2m * intersection) / (aCount + bCount);
    }

    #endregion Methods

    #region Helpers

    static private HashSet<string> GetBigrams(string input, int tokenSize) {
      HashSet<string> bigrams = new HashSet<string>();

      for (int i = 0; i < input.Length - tokenSize - 1; i++) {
        bigrams.Add(input.Substring(i, tokenSize)
                         .Replace(" ", string.Empty));
      }
      return bigrams;
    }


    static private string GetCommonCharacters(string stringA, string stringB, int distanceR) {
      if (string.IsNullOrEmpty(stringA) || string.IsNullOrEmpty(stringB)) {
        return string.Empty;
      }

      var commonCharacters = new StringBuilder();
      var stringBTemp = new StringBuilder(stringB);

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
      if (string.IsNullOrEmpty(stringA) || string.IsNullOrEmpty(stringB)) {
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

    #endregion Helpers

  }  // class EmpiriaStringDistance

} // namespace Empiria
