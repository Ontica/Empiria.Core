/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Unit of Work                               Component : Infrastructure provider                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Transfer Object                    *
*  Type     : WorkItemDto                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract DTO used to transfer information associated with a work item.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Abstract DTO used to transfer information associated with a work item.</summary>
  public abstract class WorkItemDto : IWorkItem {

    public string WorkItemUID {
      get; set;
    }

  }  // class WorkItemDto

} //namespace Empiria
