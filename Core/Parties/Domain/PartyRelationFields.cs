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

    public string UID {
      get; set;
    } = string.Empty;


    [Newtonsoft.Json.JsonProperty(PropertyName = "PartyRelationTypeUID")]
    public string PartyRelationCategoryUID {
      get; set;
    } = string.Empty;


    public string CommissionerUID {
      get; set;
    } = string.Empty;


    public string ResponsibleUID {
      get; set;
    } = string.Empty;


    public string RoleUID {
      get; set;
    } = string.Empty;


    public string Code {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


    public DateTime StartDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public Party GetCommissioner() {
      return Party.Parse(CommissionerUID);
    }

    public Party GetResponsible() {
      return Party.Parse(ResponsibleUID);
    }

    public PartyRole GetRole() {
      return PartyRole.Parse(RoleUID);
    }

  }  // class PartyRelationFields

}  // namespace Empiria.Parties
