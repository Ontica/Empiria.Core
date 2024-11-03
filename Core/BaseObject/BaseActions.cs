/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core types                                 Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Output DTO                              *
*  Type     : BaseActions                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return actions entities actions.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  // <summary>Output DTO used to return actions entities actions.</summary>
  public class BaseActions {

    public bool CanDelete {
      get; set;
    }

    public bool CanEditDocuments {
      get; set;
    }

    public bool CanUpdate {
      get; set;
    }

  } // class BaseActions

}  // namespace Empiria
