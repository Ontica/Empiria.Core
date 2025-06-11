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


    static public T ParseNamedKey<T>(string namedKey) where T: CommonStorage {
      Assertion.Require(namedKey, nameof(namedKey));

      CommonStorage item = TryParse<T>($"Object_Named_Key = '{namedKey}'");

      Assertion.Require(item, $"An object with named key '{namedKey}' was not found in common storage.");

      return (T) item;
    }


    static public FixedList<T> GetStorageObjects<T>() where T : CommonStorage {
      return BaseObject.GetList<T>("Object_Status <> 'X'", "Object_Name")
                       .ToFixedList();
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
    private int _categoryId = -1;


    [DataField("OBJECT_NAMED_KEY")]
    protected string NamedKey {
      get; set;
    }


    [DataField("OBJECT_CODE")]
    protected string Code {
      get; set;
    }


    [DataField("OBJECT_IDENTIFICATORS")]
    protected string Identificators {
      get; set;
    }


    [DataField("OBJECT_TAGS")]
    protected string Tags {
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
    protected DateTime StartDate {
      get; set;
    }


    [DataField("OBJECT_END_DATE")]
    protected DateTime EndDate {
      get; set;
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

    public T GetCategory<T>() where T : BaseObject {
      if (_categoryId == -1) {
        return BaseObject.ParseEmpty<T>();
      }
      return BaseObject.ParseId<T>(_categoryId);
    }


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


    protected void SetCategory<T>(T category) where T : BaseObject {
      Assertion.Require(category, nameof(category));

      _categoryId = category.Id;
    }


    protected void SetStatus(Enum status) {
      this.Status = Convert.ToChar(status);
    }

    #endregion Methods

  } // class CommonStorage

} // namespace Empiria
