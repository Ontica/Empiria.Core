/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : DurationType                                     Pattern  : Enumeration Type                  *
*                                                                                                            *
*  Summary   : Describes a duration type.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

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

}  // namespace Empiria.DataTypes
