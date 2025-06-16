/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Partitioned Type                        *
*  Type     : PartyRelation                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Partitioned type that represents a relation between parties.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

using Empiria.Parties.Data;

namespace Empiria.Parties {

  /// <summary>Partitioned type that represents a relation between parties.</summary>
  [PartitionedType(typeof(PartyRelationType))]
  abstract public class PartyRelation : BaseObject {

    #region Constructors and parsers

    protected PartyRelation() {
      // Required by Empiria Framework.
    }

    protected PartyRelation(PartyRelationType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected PartyRelation(PartyRole role, Party commissioner, Party responsible) {
      Assertion.Require(role, nameof(role));
      Assertion.Require(!role.IsEmptyInstance, nameof(role));
      Assertion.Require(commissioner, nameof(commissioner));
      Assertion.Require(!commissioner.IsEmptyInstance, nameof(commissioner));
      Assertion.Require(responsible, nameof(responsible));
      Assertion.Require(!responsible.IsEmptyInstance, nameof(responsible));

      Role = role;
      Commissioner = commissioner;
      Responsible = responsible;
      StartDate = DateTime.Today;
      EndDate = ExecutionServer.DateMaxValue;
    }

    static public PartyRelation Parse(int id) => ParseId<PartyRelation>(id);

    static public PartyRelation Parse(string uid) => ParseKey<PartyRelation>(uid);

    static public PartyRelation Empty => ParseEmpty<PartyRelation>();

    #endregion Constructors and parsers

    #region Properties

    public PartyRelationType PartyRelationType {
      get {
        return (PartyRelationType) base.GetEmpiriaType();
      }
    }


    [DataField("PTY_REL_CATEGORY_ID")]
    protected internal PartyRelationCategory Category {
      get; private set;
    }


    [DataField("PTY_REL_ROLE_ID")]
    protected internal PartyRole Role {
      get; private set;
    }


    [DataField("PTY_REL_COMMISSIONER_ID")]
    protected internal Party Commissioner {
      get; private set;
    }


    [DataField("PTY_REL_RESPONSIBLE_ID")]
    protected internal Party Responsible {
      get; private set;
    }


    [DataField("PTY_REL_CODE")]
    protected internal string Code {
      get; private set;
    }


    [DataField("PTY_REL_DESCRIPTION")]
    protected internal string Description {
      get; private set;
    }


    [DataField("PTY_REL_IDENTIFICATORS")]
    private string _identificators = string.Empty;

    protected internal FixedList<string> Identificators {
      get {
        return _identificators.Split(' ')
                              .ToFixedList();
      }
    }

    [DataField("PTY_REL_TAGS")]
    private string _tags = string.Empty;

    protected internal FixedList<string> Tags {
      get {
        return _tags.Split(' ')
                    .ToFixedList();
      }
    }

    [DataField("PTY_REL_EXT_DATA")]
    protected internal JsonObject ExtData {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Code, _identificators, _tags,
                                           Commissioner.Keywords, Responsible.Keywords);
      }
    }


    [DataField("PTY_REL_START_DATE")]
    public DateTime StartDate {
      get; private set;
    }


    [DataField("PTY_REL_END_DATE")]
    public DateTime EndDate {
      get; private set;
    }


    [DataField("PTY_REL_POSTED_BY_ID")]
    internal Party PostedBy {
      get; private set;
    }


    [DataField("PTY_REL_POSTING_TIME")]
    internal DateTime PostingTime {
      get; private set;
    }


    [DataField("PTY_REL_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

    #region Methods

    protected void ChangeStatus(EntityStatus status) {
      this.Status = status;
    }


    protected void Delete() {
      this.Status = EntityStatus.Deleted;
      this.EndDate = DateTime.Today;
    }

    protected override void OnSave() {
      if (base.IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      PartyDataService.WritePartyRelation(this);
    }


    protected void Update(PartyRelationFields fields) {
      Category = PatchField(fields.PartyRelationCategoryUID, Category);

      Code = PatchCleanField(fields.Code, Code);
      Description = PatchCleanField(fields.Description, Description);

      _tags = string.Join(" ", fields.Tags);
    }

    #endregion Methods

  } // class PartyRelation

} // namespace Empiria.Parties
