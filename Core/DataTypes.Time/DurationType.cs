/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                            Component : Time-Related Data Types                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Enumeration Type                        *
*  Type     : DurationType                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a duration type.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.DataTypes.Time {

  /// <summary>Describes a duration type.</summary>
  public enum DurationType {

    Unknown = -1,

    Hours = 1,

    CalendarDays = 2,

    BusinessDays = 3,

    Months = 4,

    Years = 5,

    NA = 6,

  }  // enum DurationType

}  // namespace Empiria.DataTypes.Time
