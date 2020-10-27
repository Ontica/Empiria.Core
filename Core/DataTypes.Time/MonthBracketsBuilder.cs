/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Service provider                        *
*  Type     : MonthBracketsBuilder                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Generates month brackets lists.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.DataTypes.Time {

  /// <summary>Generates month brackets lists.</summary>
  public class MonthBracketsBuilder {

    #region Constructors and fields

    private readonly int _bracketSize;
    private readonly int _firstBracketStart;

    public MonthBracketsBuilder(int bracketSize, int firstBracketStart) {
      Assertion.Assert(bracketSize > 0,
                      $"bracketSize value ({bracketSize}) must be greater than zero.");

      Assertion.Assert(1 <= firstBracketStart && firstBracketStart <= 12,
                       $"firstBracketStart value ({firstBracketStart}) out of bounds.");

      _bracketSize = bracketSize;
      _firstBracketStart = firstBracketStart;
    }

    #endregion Constructors and fields

    #region Public methods

    public MonthBracket GetBracketFor(DateTime date) {
      var brackets = this.BuildBrackets();

      var bracket = brackets.Find(x => x.StartMonth <= date.Month && date.Month <= x.EndMonth);

      if (!bracket.Equals(default(MonthBracket))) {
        return bracket;
      }

      bracket = brackets.Find(x => x.EndMonth < x.StartMonth && x.StartMonth <= date.Month);

      Assertion.Assert(!bracket.Equals(default(MonthBracket)),
                        $"A bracket for date ({date}) was not found.");

      return bracket;
    }


    public FixedList<MonthBracket> GetBrackets() {
      var brackets = this.BuildBrackets();

      return brackets.ToFixedList();
    }

    #endregion Public methods

    #region Private methods

    private List<MonthBracket> BuildBrackets() {
      var brackets = new List<MonthBracket>(4);

      MonthBracket nextBracket = BuildFirstBracket();

      while (true) {
        if (brackets.Exists(x => x.DueMonth == nextBracket.DueMonth)) {
          break;
        }

        brackets.Add(nextBracket);

        nextBracket = this.BuildNextBracket(nextBracket);
      }

      return brackets;
    }

    private MonthBracket BuildFirstBracket() {
      int startMonth = _firstBracketStart - _bracketSize;

      if (startMonth < 0) {
        startMonth = 12 - Math.Abs(startMonth);
      }

      int endMonth = startMonth + _bracketSize - 1;

      if (endMonth > 12) {
        endMonth = endMonth - 12;
      }

      return new MonthBracket(startMonth, endMonth);
    }

    private MonthBracket BuildNextBracket(MonthBracket fromBracket) {
      int startMonth = fromBracket.EndMonth + 1;

      if (startMonth > 12) {
        startMonth = startMonth - 12;
      }

      int endMonth = startMonth + _bracketSize - 1;

      if (endMonth > 12) {
        endMonth = endMonth - 12;
      }

      return new MonthBracket(startMonth, endMonth);
    }

    #endregion Private methods

  }  // class MonthBracketsBuilder

}  // namespace Empiria.DataTypes.Time
