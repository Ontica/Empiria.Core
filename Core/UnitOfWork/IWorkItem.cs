/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Unit of Work                               Component : Infrastructure provider                 *
*  Assembly : Empiria.Core.dll                           Pattern   : Interface                               *
*  Type     : IWorkItem                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface that represents a work item.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Interface that represents a work item.</summary>
  public interface IWorkItem {

    string WorkItemUID {
      get;
    }

  } // interface IWorkItem

} //namespace Empiria
