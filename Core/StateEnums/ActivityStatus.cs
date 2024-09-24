/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : ActivityStatus                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the status of an entity that can be tracked in the time, as a task or activity.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.StateEnums {

  /// <summary>Describes the status of an entity that can be tracked
  ///in the time, as a task or activity.</summary>
  public enum ActivityStatus {

    Active = 'A',

    Pending = 'P',

    Waiting = 'W',

    Suspended = 'S',

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
          return "En ejecución";

        case ActivityStatus.Pending:
          return "Pendiente";

        case ActivityStatus.Waiting:
          return "En espera";

        case ActivityStatus.Completed:
          return "Terminada";

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
          return "En ejecución";

        case ActivityStatus.Pending:
          return "Pendientes";

        case ActivityStatus.Waiting:
          return "En espera";

        case ActivityStatus.Completed:
          return "Terminadas";

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


    static public NamedEntityDto MapToDto(this ActivityStatus status) {
      return new NamedEntityDto(status.ToString(), status.GetName());
    }

  }  // class ActivityStatusEnumExtensions

} // namespace Empiria.StateEnums
