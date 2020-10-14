/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Enumeration                             *
*  Type     : TimeType                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a time unit.                                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.DataTypes.Time {

  /// <summary>Represents a time unit.</summary>
  public enum TimeUnit {

    Undefined = -1,

    NA = -2,

    CalendarDays = 1,

    BusinessDays = 2,

    Hours = 10,

  }  // enum TimeUnit

}  // namespace Empiria.DataTypes.Time
