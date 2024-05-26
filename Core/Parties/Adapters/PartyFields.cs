/* Empiria  Core *********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                         Component : Integration Layer                       *
*  Assembly : Empiria.Core.dll                           Pattern   : Fields DTO                              *
*  Type     : PartyFields                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Fields used to create or update general Party instances.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Parties {

  /// <summary>Fields used to create or update general Party instances.</summary>
  public abstract class PartyFields {

    public string Name {
      get; set;
    } = string.Empty;


    public DateTime StartDate {
      get; set;
    } = ExecutionServer.DateMaxValue;

  }

}  // namespace Empiria.Parties
