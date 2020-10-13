/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                             Component : Time-Related Data Types                *
*  Assembly : Empiria.Core.dll                            Pattern   : Service provider                       *
*  Type     : MonthBracketsBuilder                        License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Generates month brackets lists.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.DataTypes {

  /// <summary>Generates month brackets lists.</summary>
  public class MonthBracketsBuilder {

    #region Constructors

    private readonly int _bracketSize;
    private readonly int _firstBracketDueMonth;

    public MonthBracketsBuilder(int bracketSize, int firstBracketDueMonth) {
      _bracketSize = bracketSize;
      _firstBracketDueMonth = firstBracketDueMonth;
    }

    #endregion Constructors

    #region Public methods

    public MonthBracket GetBracketFor(DateTime date) {
      var brackets = this.BuildBrackets();

      var bracket = brackets.Find(x => x.StartMonth <= date.Month && date.Month <= x.EndMonth);

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
      int startMonth = _firstBracketDueMonth - _bracketSize;

      if (startMonth < 0) {
        startMonth = 12 - startMonth;
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

}  // namespace Empiria.DataTypes
