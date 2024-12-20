/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Ontology                        Component : Domain Layer                               *
*  Assembly : Empiria.Core.dll                        Pattern   : Abstract Partitioned Type                  *
*  Type     : BaseObjectLink                          License   : Please read LICENSE.txt file               *
*                                                                                                            *
*  Summary  : Abstract partitioned type that represents a link between two objects,                          *
*             both derived from BaseObject.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria {

  /// <summary>Abstract partitioned type that represents a link between two objects,
  /// both derived from BaseObject.</summary>
  [PartitionedType(typeof(BaseObjectLinkType))]
  abstract public class BaseObjectLink : BaseObject {

    #region Constructors and parsers

    protected BaseObjectLink(BaseObjectLinkType powertype) : base(powertype) {
      // Required by Empiria Framework.
    }


    protected BaseObjectLink(BaseObjectLinkType linkType, BaseObject baseObject, BaseObject linkedObject) {
      Assertion.Require(linkType, nameof(linkType));
      Assertion.Require(baseObject, nameof(baseObject));
      Assertion.Require(linkedObject, nameof(linkedObject));

      Assertion.Require(!baseObject.IsEmptyInstance,
                        "baseObject can not be the empty instance.");
      Assertion.Require(!linkedObject.IsEmptyInstance,
                        "linkedObject can not be the empty instance.");

      this.BaseObjectId = baseObject.Id;
      this.LinkedObjectId = linkedObject.Id;
    }


    static public FixedList<T> GetBaseObjectsFor<T>(BaseObjectLinkType linkType,
                                                    BaseObject linkedObject) where T : BaseObject {
      Assertion.Require(linkType, nameof(linkType));
      Assertion.Require(linkedObject, nameof(linkedObject));

      return BaseObjectDataService.GetBaseObjectsFor<T>(linkType, linkedObject);
    }


    static public FixedList<T> GetListWithBaseObject<T>(BaseObjectLinkType linkType,
                                                        BaseObject baseObject) where T : BaseObjectLink {
      Assertion.Require(linkType, nameof(linkType));
      Assertion.Require(baseObject, nameof(baseObject));

      return BaseObjectDataService.GetBaseObjectLinksWithBaseObject<T>(linkType, baseObject);
    }


    static public FixedList<T> GetListWithLinkedObject<T>(BaseObjectLinkType linkType,
                                                          BaseObject linkedObject) where T : BaseObjectLink {
      Assertion.Require(linkType, nameof(linkType));
      Assertion.Require(linkedObject, nameof(linkedObject));

      return BaseObjectDataService.GetBaseObjectLinksWithLinkedObject<T>(linkType, linkedObject);
    }


    static public FixedList<T> GetLinkedObjectsFor<T>(BaseObjectLinkType linkType,
                                                      BaseObject baseObject) where T : BaseObject {
      Assertion.Require(linkType, nameof(linkType));
      Assertion.Require(baseObject, nameof(baseObject));

      return BaseObjectDataService.GetLinkedObjectsFor<T>(linkType, baseObject);
    }


    static public FixedList<T> GetList<T>(BaseObjectLinkType linkType) where T : BaseObjectLink {
      Assertion.Require(linkType, nameof(linkType));

      return BaseObjectDataService.GetBaseObjectLinks<T>(linkType);
    }

    #endregion Constructors and parsers

    #region Properties

    public BaseObjectLinkType BaseObjectLinkType {
      get {
        return (BaseObjectLinkType) base.GetEmpiriaType();
      }
    }


    [DataField("OBJECT_LINK_BASE_OBJECT_ID")]
    private int BaseObjectId {
      get; set;
    }


    internal protected BaseObject BaseObject {
      get {
        return BaseObjectLinkType.ParseBaseObject(this.BaseObjectId);
      }
    }


    [DataField("OBJECT_LINK_LINKED_OBJECT_ID")]
    private int LinkedObjectId {
      get; set;
    }


    internal protected BaseObject LinkedObject {
      get {
        return BaseObjectLinkType.ParseLinkedObject(this.LinkedObjectId);
      }
    }


    [DataField("OBJECT_LINK_LINKED_OBJECT_ROLE")]
    public string LinkedObjectRole {
      get; protected set;
    }


    [DataField("OBJECT_LINK_CODE")]
    public string Code {
      get; protected set;
    }


    [DataField("OBJECT_LINK_DESCRIPTION")]
    public string Description {
      get; protected set;
    }


    [DataField("OBJECT_LINK_TAGS")]
    public string Tags {
      get; protected set;
    }


    [DataField("OBJECT_LINK_EXT_DATA")]
    protected internal JsonObject ExtensionData {
      get; private set;
    }


    [DataField("OBJECT_LINK_START_DATE")]
    public DateTime StartDate {
      get; protected set;
    }


    [DataField("OBJECT_LINK_END_DATE")]
    public DateTime EndDate {
      get; protected set;
    }


    [DataField("OBJECT_LINK_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("OBJECT_LINK_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("OBJECT_LINK_STATUS", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get; protected set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(BaseObjectLinkType.DisplayName, LinkedObjectRole, Description);
      }
    }

    #endregion Properties

    #region Methods

    public virtual void Delete() {
      this.Status = EntityStatus.Deleted;
    }


    protected T GetBaseObject<T>() where T : BaseObject {
      return (T) BaseObject;
    }


    public T GetLinkedObject<T>() where T : BaseObject {
      return (T) LinkedObject;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }
      BaseObjectDataService.WriteBaseObjectLink(this);
    }

    #endregion Methods

  }  // class BaseObjectLink

}  // namespace Empiria
