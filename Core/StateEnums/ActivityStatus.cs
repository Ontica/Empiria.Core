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

    Review = 'R',

    Completed = 'C',

    Canceled = 'L',

    Deleted = 'X',

    All = '*',

  } // enum ActivityStatus



  /// <summary>Extension methods for ActivityStatus.</summary>
  static public class ActivityStatusEnumExtensions {

    static public string GetName(this ActivityStatus status) {
      switch (status) {

        case ActivityStatus.Active:
          return "Activa";

        case ActivityStatus.Pending:
          return "Pendiente";

        case ActivityStatus.Completed:
          return "Terminada";

        case ActivityStatus.Review:
          return "En revisión";

        case ActivityStatus.Suspended:
          return "Suspendida";

        case ActivityStatus.Canceled:
          return "Cancelada";

        case ActivityStatus.Deleted:
          return "Eliminada";

        case ActivityStatus.All:
          return "Todas";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized status {status}");
      }
    }


    static public string GetPluralName(this ActivityStatus status) {
      switch (status) {

        case ActivityStatus.Active:
          return "Activas";

        case ActivityStatus.Pending:
          return "Pendientes";

        case ActivityStatus.Completed:
          return "Terminadas";

        case ActivityStatus.Review:
          return "En revisión";

        case ActivityStatus.Suspended:
          return "Suspendidas";

        case ActivityStatus.Canceled:
          return "Canceladas";

        case ActivityStatus.Deleted:
          return "Eliminadas";

        case ActivityStatus.All:
          return "Todas";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized status {status}");
      }
    }

  }  // class ActivityStatusEnumExtensions

} // namespace Empiria.StateEnums
