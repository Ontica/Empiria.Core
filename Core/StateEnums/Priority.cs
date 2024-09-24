/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base types                                   Component : Entity control enumerations           *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : Priority                                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an action or activity priority.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.StateEnums {

  /// <summary>Represents an action or activity priority.</summary>
  public enum Priority {

    Low = 'L',

    Normal = 'N',

    High = 'H',

    Urgent = 'U',

    All = '*'

  }  // enum Priority



  /// <summary>Extension methods for Priority enum.</summary>
  static public class PriorityEnumExtensions {

    static public string GetName(this Priority priority) {
      switch (priority) {

        case Priority.Low:
          return "Baja";

        case Priority.Normal:
          return "Normal";

        case Priority.High:
          return "Alta";

        case Priority.Urgent:
          return "Urgente";

        case Priority.All:
          return "Todas";

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized priority value {priority}.");
      }
    }


    static public NamedEntityDto MapToDto(this Priority priority) {
      return new NamedEntityDto(priority.ToString(), priority.GetName());
    }

  }  // class PriorityEnumExtensions

}  // namespace Empiria.StateEnums
