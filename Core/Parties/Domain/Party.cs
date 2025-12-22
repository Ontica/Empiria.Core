/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Partitioned Type                        *
*  Type     : Party                                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract partitioned type that represents a person, an organization, an organizational unit,   *
*             a group or a team, that can play one or more roles in parties relationships.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

using Empiria.Parties.Data;

namespace Empiria.Parties {

  /// <summary>Abstract partitioned type that represents a person, an organization, an organizational unit
  ///  a group or a team, that can play one or more roles in parties relationships.</summary>
  [PartitionedType(typeof(PartyType))]
  abstract public class Party : BaseObject, IHistoricObject, INamedEntity {

    #region Constructors and parsers

    protected Party() {
      // Required by Empiria Framework.
    }

    protected Party(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public Party Parse(int id) {
      return BaseObject.ParseId<Party>(id);
    }


    static public Party Parse(string uid) {
      return BaseObject.ParseKey<Party>(uid);
    }


    static public Party ParseWithContact(Contacts.Contact contact) {
      var party = BaseObject.TryParse<Party>($"PARTY_CONTACT_ID = {contact.Id}");

      Assertion.Require(party, $"A party associated with Contact {contact.Id} was not found.");

      return party;
    }


    static public FixedList<T> GetList<T>(DateTime date) where T : Party {
      return PartyDataService.GetPartiesInDate<T>(date);
    }


    static public FixedList<Party> GetPartiesInRole(string roleName, string keywords = "") {
      Assertion.Require(roleName, nameof(roleName));

      return PartyDataService.SearchPartiesInRole(roleName, keywords);
    }


    public static Party TryParseWithID(string partyID) {
      Assertion.Require(partyID, nameof(partyID));

      return TryParse<Party>($"PARTY_CODE = '{partyID}'");
    }

    static public Party Empty => ParseEmpty<Party>();

    #endregion Constructors and parsers

    #region Properties

    public PartyType PartyType {
      get {
        return (PartyType) base.GetEmpiriaType();
      }
    }


    [DataField("PARTY_NAME")]
    public string Name {
      get; private set;
    }


    [DataField("PARTY_CODE")]
    public string Code {
      get; protected set;
    }


    [DataField("PARTY_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    public FixedList<string> Identificators {
      get {
        return EmpiriaString.Tagging(_identificators);
      }
    }


    [DataField("PARTY_ROLES")]
    private string _roles = string.Empty;

    public FixedList<string> Roles {
      get {
        return EmpiriaString.Tagging(_roles);
      }
    }


    [DataField("PARTY_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }

    [DataField("PARTY_EXT_DATA")]
    public JsonObject ExtendedData {
      get; protected set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Name, Code, _identificators, _tags);
      }
    }


    [DataField("PARTY_HISTORIC_ID")]
    public int HistoricId {
      get; private set;
    }


    [DataField("PARTY_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("PARTY_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("PARTY_PARENT_ID")]
    protected internal int ParentId {
      get; private set;
    }


    [DataField("PARTY_POSTED_BY_ID")]
    internal int PostedById {
      get; private set;
    }


    [DataField("PARTY_POSTING_TIME")]
    internal DateTime PostingTime {
      get; private set;
    }


    [DataField("PARTY_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }


    [DataField("PARTY_CONTACT_ID")]
    public Contacts.Contact Contact {
      get; private set;
    }

    #endregion Properties

    #region Methods

    public FixedList<PartyRole> GetSecurityRoles() {
      return PartyDataService.GetPartySecurityRoles(this);
    }


    protected override void OnSave() {
      if (base.IsNew) {
        HistoricId = this.Id;
        PostedById = ExecutionServer.CurrentUserId;
        PostingTime = DateTime.Now;
        EndDate = ExecutionServer.DateMaxValue;
      }
      PartyDataService.WriteParty(this);
    }


    public bool PlaysRole(string role) {
      Assertion.Require(role, nameof(role));

      return this.Roles.Contains(role);
    }


    internal void SaveHistoric(IHistoricObject historyOf) {
      Assertion.Require(base.IsNew, "Can't save already living instances as historic.");

      this.HistoricId = historyOf.HistoricId;
      PostedById = ExecutionServer.CurrentUserId;
      PostingTime = DateTime.Now;

      PartyDataService.CloseHistoricParty(historyOf);
      PartyDataService.WriteParty(this);
    }


    protected void SetStatus(EntityStatus status) {
      Status = status;
    }

    protected void Update(PartyFields fields) {
      Name = Patcher.PatchClean(fields.Name, this.Name);
      Code = Patcher.PatchClean(fields.Code, this.Code);
      _identificators = EmpiriaString.Tagging(fields.Identificators);
      _tags = EmpiriaString.Tagging(fields.Tags);
      _roles = EmpiriaString.Tagging(fields.Roles);
      StartDate = Patcher.Patch(fields.StartDate, DateTime.Today);
      EndDate = Patcher.Patch(fields.EndDate, ExecutionServer.DateMaxValue);
    }

    #endregion Methods

  } // class Party

} // namespace Empiria.Parties
