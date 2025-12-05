/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Interface                               *
*  Type     : ITaxableParty                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a taxable party.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Represents a taxable party.</summary>
  public interface ITaxableParty {

    TaxData TaxData {
      get;
    }

  }  // interface ITaxableParty

}  // namespace Empiria.Parties
