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

    All = '*',

  } // EntityStatus



  /// <summary>Extension methods for EntityStatus.</summary>
  static public class EntityStatusEnumExtensions {

    static public string GetName(this EntityStatus status) {
      switch (status) {

        case EntityStatus.Pending:
          return "Pendiente";

        case EntityStatus.Active:
          return "Activo";

        case EntityStatus.OnReview:
          return "En revisión";

        case EntityStatus.Suspended:
          return "Suspendido";

        case EntityStatus.Discontinued:
          return "Descontinuado";

        case EntityStatus.Deleted:
          return "Eliminado";

        case EntityStatus.All:
          return "Todos";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized status {status}");
      }
    }

  }  // class EntityStatusEnumExtensions

} // namespace Empiria.StateEnums
