/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Common Storage Item                     *
*  Type     : PartyRole                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a played role in a relation between parties.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Represents a played role in a relation between parties.</summary>
  public class PartyRole : CommonStorage {

    #region Constructors and parsers

    static public PartyRole Parse(int id) => ParseId<PartyRole>(id);

    static public PartyRole Parse(string uid) => ParseKey<PartyRole>(uid);

    static public PartyRole Empty => ParseEmpty<PartyRole>();

    static public FixedList<PartyRole> GetList() {
      return GetStorageObjects<PartyRole>();
    }

    #endregion Constructors and parsers

    #region Properties

    public new string NamedKey {
      get {
        return base.NamedKey;
      }
    }

    #endregion Properties

  } // class PartyRole

} // namespace Empiria.Parties
