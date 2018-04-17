/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Core Data Types                              Component : Date Time                             *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : StartsWhen                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes an activity or event start condition.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.DataTypes {

  /// <summary>Describes an activity or event start condition.</summary>
  public enum StartsWhen {

    Undefined = -1,

    BeforeStart = 1,

    AfterStart = 2,

    During = 3,

    BeforeFinish = 4,

    AfterFinish = 5,

    AnyTime = 6

  }  // enum StartsWhen

}  // namespace Empiria.DataTypes
