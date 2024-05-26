/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Partitioned Type                      *
*  Type     : Party                                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Abstract partitioned type that represents a person, an organization, an organizational unit    *
*             or a team, that has a meaningful name and can play one or more roles in parties relationships. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Parties {

  /// <summary>Abstract partitioned type that represents a person, an organization, an organizational unit
  /// or a team, that has a meaningful name and can play one or more roles in parties relationships.</summary>
  [PartitionedType(typeof(PartyType))]
  public abstract class Party : BaseObject, INamedEntity {

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

    static public Party Empty => BaseObject.ParseEmpty<Person>();

    #endregion Constructors and parsers

    #region Properties

    public PartyType PartyType {
      get {
        return (PartyType) base.GetEmpiriaType();
      }
    }

    [DataField("PARTY_NAME")]
    public string Name {
      get; protected set;
    }


    [DataField("PARTY_EXT_DATA")]
    protected internal JsonObject ExtendedData {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Name);
      }
    }


    [DataField("PARTY_HISTORIC_ID")]
    public int HistoricId {
      get; private set;
    }


    [DataField("PARTY_START_DATE")]
    internal DateTime StartDate {
      get; set;
    }

    [DataField("PARTY_END_DATE")]
    internal DateTime EndDate {
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
      get; protected set;
    }

    #endregion Properties

    #region Methods

    public void ChangeStatus(EntityStatus status) {
      this.Status = status;
    }


    protected void Update(PartyFields fields) {
      this.Name = PatchCleanField(fields.Name, this.Name);
      this.StartDate = fields.StartDate == ExecutionServer.DateMaxValue ? StartDate : fields.StartDate;
    }

    #endregion Methods

  } // class Party

} // namespace Empiria.Parties
