/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Types Library                *
*  Namespace : Empiria.DataTypes                                Assembly : Empiria.Foundation.dll            *
*  Type      : DurationType                                     Pattern  : Enumeration Type                  *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Describes a duration type.                                                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Describes a duration type.</summary>
  public enum DurationType {

    Unknown = -1,

    Hours = 1,

    Days = 2,

    WorkingDays = 3,

    Months = 4,

    Years = 5,

  }  // enum DurationType

}  // namespace Empiria.DataTypes
