/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Fields DTO                        *
*  Type     : PartyRelationFields                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create or update party relation instances.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Parties {

  /// <summary>Fields used to create or update party relation instances.</summary>
  public class PartyRelationFields {

    public string RoleUID {
      get; internal set;
    } = string.Empty;


    public string CommissionerUID {
      get; internal set;
    } = string.Empty;


    public string ResponsibleUID {
      get; internal set;
    } = string.Empty;


    public DateTime StartDate {
      get; set;
    } = ExecutionServer.DateMaxValue;

  }  // class PartyRelationFields

}  // namespace Empiria.Parties
