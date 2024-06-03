/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Partitioned Type                      *
*  Type     : PartyRelation                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Partitioned type that represents a relation between parties.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Parties {

  /// <summary>Partitioned type that represents a relation between parties.</summary>
  [PartitionedType(typeof(PartyRelationType))]
  public class PartyRelation : BaseObject, IHistoricObject {

    #region Constructors and parsers

    protected PartyRelation() {
      // Required by Empiria Framework.
    }

    protected PartyRelation(PartyRelationType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public PartyRelation Parse(int id) {
      return BaseObject.ParseId<PartyRelation>(id);
    }


    static public PartyRelation Parse(string uid) {
      return BaseObject.ParseKey<PartyRelation>(uid);
    }

    static public PartyRelation Empty => BaseObject.ParseEmpty<PartyRelation>();

    #endregion Constructors and parsers

    #region Properties

    public PartyRelationType PartyRelationType {
      get {
        return (PartyRelationType) base.GetEmpiriaType();
      }
    }

    [DataField("PARTY_RELATION_ROLE_ID")]
    public PartyRelationRole Role {
      get; protected set;
    }


    [DataField("COMMISSIONER_PARTY_ID")]
    public Party Commissioner {
      get; protected set;
    }

    [DataField("RESPONSIBLE_PARTY_ID")]
    public Party Responsible {
      get; protected set;
    }

    [DataField("PTY_RELATION_EXT_DATA")]
    protected internal JsonObject ExtendedData {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Commissioner.Keywords, Responsible.Keywords);
      }
    }


    [DataField("PTY_RELATION_HISTORIC_ID")]
    public int HistoricId {
      get; private set;
    }


    [DataField("PTY_RELATION_START_DATE")]
    internal DateTime StartDate {
      get; set;
    }

    [DataField("PTY_RELATION_END_DATE")]
    internal DateTime EndDate {
      get; private set;
    }

    [DataField("PTY_RELATION_POSTED_BY_ID")]
    internal int PostedById {
      get; private set;
    }

    [DataField("PTY_RELATION_POSTING_TIME")]
    internal DateTime PostingTime {
      get; private set;
    }

    [DataField("PTY_RELATION_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; protected set;
    }

    #endregion Properties

    #region Methods

    public void ChangeStatus(EntityStatus status) {
      this.Status = status;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        HistoricId = this.Id;
        PostedById = ExecutionServer.CurrentUserId;
        PostingTime = DateTime.Now;
        EndDate = ExecutionServer.DateMaxValue;
      }
      PartyDataService.WritePartyRelation(this);
    }


    internal void SaveHistoric(IHistoricObject historyOf) {
      Assertion.Require(base.IsNew, "Can't save already living instances as historic.");

      this.HistoricId = historyOf.HistoricId;
      PostedById = ExecutionServer.CurrentUserId;
      PostingTime = DateTime.Now;

      PartyDataService.CloseHistoricPartyRelation(historyOf);
      PartyDataService.WritePartyRelation(this);
    }

    protected void Update(PartyRelationFields fields) {
      this.Role = PartyRelationRole.Parse(fields.RoleUID);
      this.Commissioner = Party.Parse(fields.CommissionerUID);
      this.Responsible = Party.Parse(fields.ResponsibleUID);
      this.StartDate = fields.StartDate == ExecutionServer.DateMaxValue ? StartDate : fields.StartDate;
    }

    #endregion Methods

  } // class PartyRelation

} // namespace Empiria.Parties
