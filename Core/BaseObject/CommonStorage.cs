/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Base Objects                               Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : CommonStorage                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract type that holds object instances which are stored in a general common table.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Ontology;

namespace Empiria {

  /// <summary>Abstract type that holds object instances which are
  /// stored in a general common table.</summary>
  public abstract class CommonStorage : BaseObject, INamedEntity {

    #region Constructors and parsers

    protected CommonStorage() {
      // Required by Empiria Framework.
    }


    protected CommonStorage(Powertype powertype) : base(powertype) {
      // Used by partitioned derived types.
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("OBJECT_NAME")]
    public virtual string Name {
      get; protected set;
    }


    [DataField("OBJECT_DESCRIPTION")]
    public virtual string Description {
      get; protected set;
    }


    [DataField("OBJECT_CATEGORY_ID")]
    protected int ObjectCategoryId {
      get; set;
    }


    [DataField("OBJECT_NAMED_KEY")]
    public string NamedKey {
      get; set;
    }


    [DataField("OBJECT_CODE")]
    public string Code {
      get; set;
    }


    [DataField("OBJECT_IDENTIFICATORS")]
    public string Identificators {
      get; set;
    }


    [DataField("OBJECT_TAGS")]
    public string Tags {
      get; set;
    }


    [DataField("OBJECT_EXT_DATA")]
    protected JsonObject ExtData {
      get; private set;
    }


    [DataField("OBJECT_HISTORIC_ID")]
    protected int HistoricId {
      get; private set;
    }


    [DataField("OBJECT_START_DATE")]
    public DateTime StartDate {
      get; protected set;
    }


    [DataField("OBJECT_END_DATE")]
    public DateTime EndDate {
      get; protected set;
    }


    [DataField("OBJECT_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("OBJECT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("OBJECT_STATUS", Default = 'A')]
    private char Status {
      get; set;
    }


    [DataField("PARENT_OBJECT_ID")]
    private int _parentObjectId = -1;

    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Name, Code, Identificators, Tags, Description);
      }
    }

    #endregion Properties

    #region Methods


    public T GetParent<T>() where T : BaseObject {
      if (_parentObjectId == -1) {
        return BaseObject.ParseEmpty<T>();
      }
      return BaseObject.ParseId<T>(_parentObjectId);
    }


    public T GetStatus<T>() where T : Enum {
      return (T) Enum.Parse(typeof(T), ((int) this.Status).ToString());
    }


    protected void SetParent<T>(T parent) where T : BaseObject {
      Assertion.Require(parent, nameof(parent));

      _parentObjectId = parent.Id;
    }


    protected void SetStatus(Enum status) {
      this.Status = Convert.ToChar(status);
    }

    #endregion Methods

  } // class CommonStorage

} // namespace Empiria
