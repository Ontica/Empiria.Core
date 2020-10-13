/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                             Component : Time-Related Data Types                *
*  Assembly : Empiria.Core.dll                            Pattern   : Information Holder                     *
*  Type     : MonthBracket                                License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : A month bracket is a month period (e.g., April-June) and a related due month (e.g. July).      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  /// <summary>A month bracket is a month period (e.g., April-June) and a related
  /// due month (e.g. July).</summary>
  public struct MonthBracket : IEquatable<MonthBracket> {

    internal MonthBracket(int startMonth, int endMonth) {
      EmpiriaLog.Debug($"Bracket created with {startMonth}, {endMonth}");

      this.StartMonth = startMonth;
      this.EndMonth = endMonth;
    }


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


    public bool Equals(MonthBracket other) {
      return (this.StartMonth == other.StartMonth &&
              this.EndMonth == other.EndMonth);
    }


  }  // class MonthBracket

}  // namespace Empiria.DataTypes
