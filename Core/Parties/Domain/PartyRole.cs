/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : General Object                          *
*  Type     : PartyRole                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a played role in a relation between parties.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Represents a played role in a relation between parties.</summary>
  public class PartyRole : GeneralObject {

    #region Constructors and parsers

    private PartyRole() {
      // Required by Empiria Framework.
    }

    static public PartyRole Parse(int id) {
      return ParseId<PartyRole>(id);
    }

    static public PartyRole Parse(string uid) {
      return ParseKey<PartyRole>(uid);
    }

    static public PartyRole Empty => ParseEmpty<PartyRole>();

    #endregion Constructors and parsers

  } // class PartyRole

} // namespace Empiria.Parties
