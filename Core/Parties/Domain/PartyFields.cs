/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Fields DTO                        *
*  Type     : PartyFields                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create or update Party instances.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Parties {

  /// <summary>Input fields used to create or update Party instances.</summary>
  abstract public class PartyFields {

    public string Name {
      get; set;
    } = string.Empty;


    public string Code {
      get; set;
    } = string.Empty;


    public string[] Identificators {
      get; set;
    } = new string[0];


    public string[] Tags {
      get; set;
    } = new string[0];


    public string[] Roles {
      get; set;
    } = new string[0];


    public DateTime StartDate {
      get; set;
    } = DateTime.Today;


    public DateTime EndDate {
      get; set;
    } = ExecutionServer.DateMaxValue;

  }  // class PartyFields

}  // namespace Empiria.Parties
