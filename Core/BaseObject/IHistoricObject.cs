/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Core Types                                 Component : Base Object Management                  *
*  Assembly : Empiria.Core.dll                           Pattern   : Data interface                          *
*  Type     : IHistoricObject                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface for historical objects.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Interface for historical objects.</summary>
  public interface IHistoricObject : IIdentifiable {

    int HistoricId {
      get;
    }

  }  // interface IHistoricObject

} // namespace Empiria
