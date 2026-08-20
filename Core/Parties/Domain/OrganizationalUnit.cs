/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : OrganizationalUnit                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an organizational unit that is a part of an organization.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

namespace Empiria.Parties {

  /// <summary>Represents an organizational unit that is a part of an organization.</summary>
  public class OrganizationalUnit : Party, INamedEntity {

    #region Constructors and parsers

    protected OrganizationalUnit() {
      // Required by Empiria Framework.
    }

    protected OrganizationalUnit(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new OrganizationalUnit Parse(int id) => ParseId<OrganizationalUnit>(id);

    static public new OrganizationalUnit Parse(string uid) => ParseKey<OrganizationalUnit>(uid);

    static public new OrganizationalUnit Empty => ParseEmpty<OrganizationalUnit>();

    static public FixedList<OrganizationalUnit> GetList() {
      return GetFullList<OrganizationalUnit>("PARTY_STATUS <> 'X'", "PARTY_CODE");
    }

    static public new OrganizationalUnit TryParseWithID(string orgUnitID) {
      Assertion.Require(orgUnitID, nameof(orgUnitID));


      var party = TryParse<Party>($"PARTY_TYPE_ID = {Empty.PartyType.Id} AND PARTY_CODE = '{orgUnitID}'");

      if (party == null || !(party is OrganizationalUnit)) {
        return null;
      }

      return (OrganizationalUnit) party;
    }

    #endregion Constructors and parsers

    #region Properties

    public string Acronym {
      get {
        return base.ExtendedData.Get("acronym", string.Empty);
      }
      private set {
        base.ExtendedData.SetIfValue("acronym", value);
      }
    }

    string INamedEntity.Name {
      get {
        return FullName;
      }
    }


    public string FullName {
      get {
        if (Code.Length > 0) {
          return $"{Code} - {Name}";
        } else {
          return Name;
        }
      }
    }

    public OrganizationalUnit Parent {
      get {
        if (base.ParentId == -1) {
          return Empty;
        }
        return Parse(base.ParentId);
      }
    }


    public int Level {
      get {
        if (Parent.IsEmptyInstance) {
          return 1;
        } else {
          return Parent.Level + 1;
        }
      }
    }

    public override string Keywords {
      get {
        return Code + " " + EmpiriaString.BuildKeywords(Acronym, base.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    public FixedList<OrganizationalUnit> GetAllChildren(bool includeRoot = false) {
      if (this.IsEmptyInstance) {
        return new FixedList<OrganizationalUnit>();
      }

      var result = new List<OrganizationalUnit>(1000);

      if (includeRoot) {
        result.Add(this);
      }

      foreach (var child in GetChildren()) {
        result.Add(child);
        result.AddRange(child.GetAllChildren());
      }

      return result.ToFixedList();
    }


    public FixedList<OrganizationalUnit> GetChildren() {
      if (this.IsEmptyInstance) {
        return new FixedList<OrganizationalUnit>();
      }
      return GetFullList<OrganizationalUnit>($"PARTY_PARENT_ID = {this.Id} AND PARTY_STATUS <> 'X'", "PARTY_CODE");
    }

    #endregion Methods

  } // class OrganizationalUnit

} // namespace Empiria.Parties
