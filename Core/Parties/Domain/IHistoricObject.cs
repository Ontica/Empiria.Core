/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Partitioned Type                      *
*  Type     : Party                                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Abstract partitioned type that represents a person, an organization, an organizational unit    *
*             or a team, that has a meaningful name and can play one or more roles in parties relationships. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
namespace Empiria.Parties {
  public interface IHistoricObject : IIdentifiable {

    int HistoricId {
      get;
    }

  }  // interface IHistoricObject

} // namespace Empiria.Parties
