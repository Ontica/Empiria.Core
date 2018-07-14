/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Base Types                                   Component : Ontology                              *
*  Assembly : Empiria.Core.dll                             Pattern   : Layer supertype                       *
*  Type     : BaseObject                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : BaseObject is the root type of the object type hierarchy in Empiria Framework.                 *
*             All object types that uses the framework must be descendants of this abstract type.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.DataTypes;
using Empiria.Ontology;

namespace Empiria {

  /// <summary>BaseObject is the root type of the object type hierarchy in Empiria Framework.
  /// All object types that uses the framework must be descendants of this abstract type.</summary>
  public abstract class BaseObject : IIdentifiable {

    #region Fields

    static private ObjectsCache cache = new ObjectsCache();

    private ObjectTypeInfo objectTypeInfo = null;
    private int objectId = 0;
    private bool isDirtyFlag = true;
    private bool isNewFlag = true;

    #endregion Fields

    #region Constructors and parsers

    protected BaseObject() {
      objectTypeInfo = ObjectTypeInfo.Parse(this.GetType());

      this.OnInitialize();

      if (objectTypeInfo.IsDataBound) {
        // Should call InitializeObject only when is not called through BaseObject.ParseEmpiriaObject.
        objectTypeInfo.InitializeObject(this);
      }
    }


    protected BaseObject(ObjectTypeInfo powertype) {
      objectTypeInfo = powertype;

      this.OnInitialize();

      if (objectTypeInfo.IsDataBound) {
        // Should call InitializeObject only when is not called through BaseObject.ParseEmpiriaObject.
        objectTypeInfo.InitializeObject(this);
      }
    }


    //TODO: Review usage
    static protected T Create<T>(ObjectTypeInfo typeInfo) where T : BaseObject {
      T item = typeInfo.CreateObject<T>();
      item.objectTypeInfo = typeInfo;

      return item;
    }


    static public List<T> GetList<T>(string filter = "", string sort = "") where T : BaseObject {
      return OntologyData.GetBaseObjectList<T>(filter, sort);
    }


    static public T ParseDataRow<T>(DataRow dataRow, bool reload = false) where T : BaseObject {
      ObjectTypeInfo baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      if (!reload) {
        int objectId = (int) dataRow[baseTypeInfo.IdFieldName];
        T item = cache.TryGetItem<T>(baseTypeInfo.Name, objectId);
        if (item != null) {
          return item;    // Only use dataRow when item is not in cache
        }
      }

      ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

      return BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow);
    }


    static protected T ParseEmpty<T>() where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      return objectTypeInfo.GetEmptyInstance<T>().Clone<T>();
    }


    static protected internal T ParseId<T>(int id, bool reload = false) where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      if (id == ObjectTypeInfo.EmptyInstanceId) {
        return objectTypeInfo.GetEmptyInstance<T>().Clone<T>();
      }
      if (id == ObjectTypeInfo.UnknownInstanceId) {
        return objectTypeInfo.GetUnknownInstance<T>().Clone<T>();
      }

      return BaseObject.ParseIdInternal<T>(objectTypeInfo, id, reload);
    }


    static internal T ParseIdInternal<T>(ObjectTypeInfo typeInfo,
                                         int id, bool reload) where T : BaseObject {
      if (id == 0) {
        throw new OntologyException(OntologyException.Msg.TryToParseZeroObjectId, typeInfo.Name);
      }
      if (!reload) {
        T item = cache.TryGetItem<T>(typeInfo.Name, id);
        if (item != null) {
          return item;
        }
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = typeInfo.GetObjectTypeAndDataRow(id);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }


    static protected T ParseKey<T>(string namedKey, bool reload = false) where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      if (!reload) {
        T item = cache.TryGetItem<T>(objectTypeInfo.Name, namedKey);
        if (item != null) {
          return item;
        }
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = objectTypeInfo.GetObjectTypeAndDataRow(namedKey);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }


    static public List<T> ParseList<T>(DataTable dataTable, bool reload = false) where T : BaseObject {
      if (dataTable == null || dataTable.Rows.Count == 0) {
        return new List<T>();
      }
      var baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));
      int objectId = 0;
      try {
        List<T> list = new List<T>(dataTable.Rows.Count);

        foreach (DataRow dataRow in dataTable.Rows) {
          objectId = (int) dataRow[baseTypeInfo.IdFieldName];

          if (!reload) {
            T item = cache.TryGetItem<T>(baseTypeInfo.Name, objectId);
            if (item != null) {
              list.Add(item);    // Only use dataRow when item is not in cache
            } else {
              ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

              list.Add(BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow));
            }
          } else {
            ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

            list.Add(BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow));
          }
        }
        return list;

      } catch (Exception e) {
        var exception = new OntologyException(OntologyException.Msg.CannotParseObjectWithDataRow,
                                              e, baseTypeInfo.Name, objectId);
        exception.Publish();
        throw exception;
      }
    }


    static protected T ParseUnknown<T>() where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      return objectTypeInfo.GetUnknownInstance<T>().Clone<T>();
    }


    static protected T TryParse<T>(string condition, bool reload = false) where T : BaseObject {
      var sqlFilter = Empiria.Data.SqlFilter.Parse(condition);

      return TryParse<T>(sqlFilter, reload);
    }


    static protected T TryParse<T>(IFilter condition,
                                   bool reload = false) where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      Tuple<ObjectTypeInfo, DataRow> objectData = objectTypeInfo.TryGetObjectTypeAndDataRow(condition);

      if (objectData == null) {
        return null;
      }

      int objectId = (int) objectData.Item2[objectTypeInfo.IdFieldName];

      if (!reload) {
        T item = cache.TryGetItem<T>(objectTypeInfo.Name, objectId);
        if (item != null) {
          return item;    // Only use dataRow when item is not in cache
        }
      }
      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }

    #endregion Constructors and parsers

    #region Public properties

    protected AttributesBag AttributesBag {
      get;
      private set;
    }


    public int Id {
      get { return this.objectId; }
      private set {
        this.objectId = value;
      }
    }

    public virtual string UID {
      get;
      private set;
    } = String.Empty;


    protected bool IsDirty {
      get { return this.isDirtyFlag || this.IsNew; }
    }


    [Newtonsoft.Json.JsonIgnore]
    public bool IsEmptyInstance {
      get {
        return (this.objectId == ObjectTypeInfo.EmptyInstanceId);
      }
    }


    [Newtonsoft.Json.JsonIgnore]
    public bool IsNew {
      get { return (this.objectId == 0 || isNewFlag == true); }
    }


    protected internal bool IsSpecialCase {
      get {
        return (this.objectId == ObjectTypeInfo.EmptyInstanceId ||
                this.objectId == ObjectTypeInfo.UnknownInstanceId);
      }
    }


    protected bool IsUnknownInstance {
      get {
        return (this.objectId == ObjectTypeInfo.UnknownInstanceId);
      }
    }

    #endregion Public properties

    #region Public methods

    /// <summary>Virtual method that creates a shallow copy of the current instance.</summary>
    protected virtual T Clone<T>() where T : BaseObject {
      return (T) this.MemberwiseClone();
    }


    public override bool Equals(object obj) {
      if (obj == null || this.GetType() != obj.GetType()) {
        return false;
      }
      return base.Equals(obj) && (this.Id == ((BaseObject) obj).Id);
    }


    public bool Equals(BaseObject obj) {
      if (obj == null) {
        return false;
      }
      return objectTypeInfo.Equals(obj.objectTypeInfo) && (this.Id == obj.Id);
    }


    public ObjectTypeInfo GetEmpiriaType() {
      return this.objectTypeInfo;
    }


    public override int GetHashCode() {
      return (this.objectTypeInfo.GetHashCode() ^ this.Id);
    }


    protected T GetLink<T>(string linkName) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

      return association.GetLink<T>(this);
    }


    protected T GetLink<T>(string linkName, T defaultValue) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

      return association.GetLink<T>(this, defaultValue);
    }


    protected T GetInverseLink<T>(string linkName) where T : BaseObject {
      var association = TypeAssociationInfo.Parse(linkName);

      return association.GetInverseLink<T>(this);
    }


    protected T GetInverseLink<T>(string linkName, T defaultValue) where T : BaseObject {
      var association = TypeAssociationInfo.Parse(linkName);

      return association.GetInverseLink<T>(this, defaultValue);
    }


    protected FixedList<T> GetLinks<T>(string linkName) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

      return association.GetLinks<T>(this);
    }


    protected FixedList<T> GetLinks<T>(string linkName, Comparison<T> sort) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];
      FixedList<T> list = association.GetLinks<T>(this);

      list.Sort(sort);

      return list;
    }


    protected FixedList<T> GetLinks<T>(string linkName, TimeFrame period) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

      return association.GetLinks<T>(this, period);
    }


    protected FixedList<T> GetLinks<T>(string linkName, TimeFrame period,
                                       Comparison<T> sort) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];
      FixedList<T> list = association.GetLinks<T>(this, period);

      list.Sort(sort);

      return list;
    }


    protected FixedList<T> GetLinks<T>(string linkName, Predicate<T> predicate) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

      return association.GetLinks<T>(this, predicate);
    }


    protected void LoadAttributesBag(DataRow row) {
      this.AttributesBag = new AttributesBag(this, row);
    }


    protected void MarkAsDirty() {
      this.isDirtyFlag = true;
    }


    /// <summary>Raised for new and stored instances, after object creation and before
    /// databinding if their type is marked as IsDatabounded.</summary>
    protected virtual void OnInitialize() {

    }


    /// <summary>Raised after initialization and after databinding if their type is
    /// marked as IsDatabounded.</summary>
    internal protected virtual void OnLoadObjectData(DataRow row) {

    }


    /// <summary>Raised before Save() method is called and before object.Id and object.uid created.</summary>
    protected virtual void OnBeforeSave() {

    }


    /// <summary>Raised when Save() method is called and after objectId and object.uid generated.</summary>
    protected virtual void OnSave() {
      throw new NotImplementedException();
    }


    public void Save() {
      // Never save special case instances (e.g. Empty or Unknown)
      if (this.IsSpecialCase) {
        return;
      }

      this.OnBeforeSave();

      if (this.objectId == 0) {
        this.objectId = OntologyData.GetNextObjectId(this.objectTypeInfo);
      }

      if (String.IsNullOrWhiteSpace(this.UID)) {
        if (this.objectTypeInfo.UsesNamedKey) {
          this.UID = Guid.NewGuid().ToString();
        } else {
          this.UID = this.objectId.ToString();
        }
      }

      this.OnSave();

      this.isNewFlag = false;
      this.isDirtyFlag = false;


      if (this.objectTypeInfo.UsesNamedKey) {
        cache.Insert(this, this.UID);
      } else {
        cache.Insert(this);
      }
    }

    #endregion Public methods

    #region Private methods

    private void DataBind(DataRow row) {
      this.GetEmpiriaType().DataBind(this, row);
    }


    static private T ParseEmpiriaObject<T>(ObjectTypeInfo typeInfo, DataRow dataRow) where T : BaseObject {
      T item = typeInfo.CreateObject<T>();

      item.objectTypeInfo = typeInfo;

      item.objectId = (int) dataRow[typeInfo.IdFieldName];

      if (typeInfo.UsesNamedKey) {
        item.UID = (string) dataRow[typeInfo.NamedIdFieldName];
      }

      if (typeInfo.IsDataBound) {
        item.DataBind(dataRow);
      }

      item.isNewFlag = false;
      item.OnLoadObjectData(dataRow);
      item.isDirtyFlag = false;

      if (typeInfo.UsesNamedKey) {
        cache.Insert(item, item.UID);
      } else {
        cache.Insert(item);
      }

      return item;
    }

    #endregion Private methods

    protected void ReclassifyAs(ObjectTypeInfo newType) {
      Assertion.AssertObject(newType, "newType");
      Assertion.Assert(!this.GetEmpiriaType().Equals(newType),
                       "newType should be distinct to the current one.");
      //Assertion.Assert(this.GetEmpiriaType().UnderlyingSystemType.Equals(newType.UnderlyingSystemType),
      //                 "newType underlying system type should be the same to the current one's.");
      // Seek for a common ancestor (distinct than ObjectType) between types:
      // eg: if A is a mammal and B is a bird, should be possible to convert A to B or B to A because both are animals

      cache.Remove(this);

      this.objectTypeInfo = newType;

      if (newType.UsesNamedKey) {
        cache.Insert(this, this.UID);
      } else {
        cache.Insert(this);
      }
    }

    //protected void Link(TypeAssociationInfo assocationInfo, IStorable value) {
    //  OntologyData.WriteLink(assocationInfo, this, value);
    //}

  } // class BaseObject

} // namespace Empiria
