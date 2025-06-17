/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Common Storage Item                     *
*  Type     : PartyRelationCategory                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Categorizes a party relation.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Categorizes a party relation.</summary>
  public class PartyRelationCategory : CommonStorage {

    #region Constructors and parsers

    static public PartyRelationCategory Parse(int id) => ParseId<PartyRelationCategory>(id);

    static public PartyRelationCategory Parse(string uid) => ParseKey<PartyRelationCategory>(uid);

    static public PartyRelationCategory Empty => ParseEmpty<PartyRelationCategory>();

    static public FixedList<PartyRelationCategory> GetList() {
      return GetStorageObjects<PartyRelationCategory>();
    }

    #endregion Constructors and parsers

    #region Properties

    public new string NamedKey {
      get {
        return base.NamedKey;
      }
    }


    public FixedList<PartyRole> Roles {
      get {
        return PartyRole.GetList()
                        .FindAll(x => x.Category.Equals(this));
      }
    }

    #endregion Properties

  } // class PartyRelationCategory

} // namespace Empiria.Parties
