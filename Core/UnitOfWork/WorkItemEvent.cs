/* Empiria Extensions ****************************************************************************************
*                                                                                                            *
*  Module   : Unit of Work                               Component : Infrastructure provider                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Immutable Value Type                    *
*  Type     : WorkItemEvent                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Value type that holds information about a work item event.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Value type that holds information about a work item event.</summary>
  public class WorkItemEvent : IWorkItemEvent {

    #region Constructors and parsers

    public WorkItemEvent(string type, IWorkItem workItem) {
      Assertion.Require(type, nameof(type));
      Assertion.Require(workItem, nameof(workItem));

      this.Type = type;
      this.WorkItem = workItem;
    }

    #endregion Constructors and parsers

    #region Properties

    public string Type {
      get;
    }


    public IWorkItem WorkItem {
      get;
    }

    #endregion Properties

  }  // class WorkItemEvent

}  // namespace Empiria
