/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : EntityStatus                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the status of an entity that can be reviewed, activated or discontinued.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.StateEnums {

  /// <summary>Describes the status of an entity that can be reviewed, activated or discontinued.</summary>
  public enum EntityStatus {

    Pending = 'P',

    Active = 'A',

    OnReview = 'R',

    Suspended = 'S',

    Discontinued = 'D',

    Deleted = 'X',

  } // EntityStatus

} // namespace Empiria.StateEnums
