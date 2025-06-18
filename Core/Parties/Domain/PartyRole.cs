/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Common Storage Item                     *
*  Type     : PartyRole                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a played role in a relation between parties.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties.Data;

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

    public FixedList<string> AppliesTo {
      get {
        return base.Roles;
      }
    }


    public PartyRelationCategory Category {
      get {
        return base.GetCategory<PartyRelationCategory>();
      }
    }


    public new string NamedKey {
      get {
        return base.NamedKey;
      }
    }


    public bool RequiresCode {
      get {
        return base.ExtData.Get("requiresCode", false);
      }
    }

    #endregion Properties

    #region Methods

    public FixedList<Party> SearchSecurityPlayers(string keywords) {
      keywords = keywords ?? string.Empty;

      return PartyDataService.SearchPartyRoleSecurityPlayers(this, keywords);
    }

    #endregion Methods

  } // class PartyRole

} // namespace Empiria.Parties
