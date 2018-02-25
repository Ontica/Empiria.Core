/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                License  : Please read LICENSE.txt file      *
*  Type      : TimeUnit                                         Pattern  : Enumeration Type                  *
*                                                                                                            *
*  Summary   : Represents a time unit.                                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Represents a time unit.</summary>
  public enum TimeUnit {

    Undefined = -1,

    NA = -2,

    CalendarDays = 1,

    BusinessDays = 2,

    Hours = 10,

  }  // enum TimeUnit

}  // namespace Empiria.DataTypes
