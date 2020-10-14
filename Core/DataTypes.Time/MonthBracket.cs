/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Structure                               *
*  Type     : MonthBracket                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : A month bracket is a month period (e.g., April-June) and a related due month (e.g. July).      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes.Time {

  /// <summary>A month bracket is a month period (e.g., April-June) and a related
  /// due month (e.g. July).</summary>
  public struct MonthBracket : IEquatable<MonthBracket> {

    #region Constructors and parsers

    internal MonthBracket(int startMonth, int endMonth) {
      this.StartMonth = startMonth;
      this.EndMonth = endMonth;
    }


    #endregion Constructors and parsers

    #region Properties

    public int StartMonth {
      get;
    }

    public int EndMonth {
      get;
    }

    public int DueMonth {
      get {
        int dueMonth = this.EndMonth + 1;

        if (dueMonth > 12) {
          return dueMonth - 12;
        } else {
          return dueMonth;
        }
      }
    }

    #endregion Properties

    #region IEquatable interface

    public bool Equals(MonthBracket other) {
      return (this.StartMonth == other.StartMonth &&
              this.EndMonth == other.EndMonth);
    }

    #endregion IEquatable interface

  }  // class MonthBracket

}  // namespace Empiria.DataTypes.Time
