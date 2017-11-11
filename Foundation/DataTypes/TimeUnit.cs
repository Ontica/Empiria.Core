/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : TimeUnit                                         Pattern  : Enumeration Type                  *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a time unit.                                                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
