/* Empiria Extensions ****************************************************************************************
*                                                                                                            *
*  Module   : Unit of Work                               Component : Infrastructure provider                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Interface                               *
*  Type     : IWorkItemEvent                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface that describes a work item event.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Interface that describes a work item event.</summary>
  public interface IWorkItemEvent {

    string Type {
      get;
    }


    IWorkItem WorkItem {
      get;
    }

  }  // interface IWorkItemEvent

}  // namespace Empiria
