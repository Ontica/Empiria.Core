/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Fields DTO                        *
*  Type     : PersonFields                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields used to create or update Person instances.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Fields used to create or update Person instances.</summary>
  public class PersonFields : PartyFields {

    public string FirstName {
      get; set;
    } = string.Empty;


    public string LastName {
      get; set;
    } = string.Empty;


    public string LastName2 {
      get; set;
    } = string.Empty;


    public bool IsFemale {
      get; set;
    }

  }  // class PersonFields

}  // namespace Empiria.Parties
