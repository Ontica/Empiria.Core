/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : ExecutionStatus                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the status of a process execution.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.StateEnums {

  /// <summary>Describes the status of a process execution.</summary>
  public enum ExecutionStatus {

    Pending = 'P',

    Completed = 'C',

    Failed = 'F',

    Canceled = 'L',

    Deleted = 'X',

  } // enum ExecutionStatus

} // namespace Empiria.StateEnums
