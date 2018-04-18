/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : ActivityStatus                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the status of an entity that can be tracked in the time, as a task or activity.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.StateEnums {

  /// <summary>Describes the status of an entity that can be tracked
  ///in the time, as a task or activity.</summary>
  public enum ActivityStatus {

    Pending = 'P',

    Active = 'A',

    Suspended = 'S',

    Completed = 'C',

    Canceled = 'L',

    Deleted = 'X',

  } // enum ActivityStatus

} // namespace Empiria.StateEnums
