/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : BaseObject                                       Pattern  : Layer Supertype                   *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : BaseObject is the root type of the object type hierarchy in Empiria Framework.                *
*              All object types that uses the framework must be descendants of this abstract type.           *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data;
using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria {

  /// <summary>
  /// BaseObject is the root type of the object type hierarchy in Empiria Framework. 
  /// All object types that uses the framework must be descendants of this abstract type.
  /// </summary>
  public abstract class BaseObject : IStorable {

    #region Fields

    static private BaseObjectCache cache = new BaseObjectCache();

    private ObjectTypeInfo objectTypeInfo = null;
    private int objectId = 0;
    private bool isDirty = true;
    private bool isNewFlag = true;

    private const int emptyInstanceId = -1;
    private const int unknownInstanceId = -2;

    #endregion Fields

    #region Constructors and parsers

    protected BaseObject() {
      objectTypeInfo = ObjectTypeInfo.Parse(this.GetType());
      if (objectTypeInfo.IsDataBound) {
        objectTypeInfo.InitializeObject(this);
      }
      this.OnInitialize();
    }

    protected BaseObject(ObjectTypeInfo powertype) {
      objectTypeInfo = powertype;
      if (objectTypeInfo.IsDataBound) {
        objectTypeInfo.InitializeObject(this);
      }
      this.OnInitialize();
    }

    //protected BaseObject(string typeName) {
    //  if (typeName.Length != 0) {   // If typeName.Length == 0, then is invoked with Parsing using reflection
    //    objectTypeInfo = ObjectTypeInfo.Parse(typeName);
    //    if (objectTypeInfo.IsDataBound) {
    //      objectTypeInfo.InitializeObject(this);
    //    }
    //  }
    //  this.OnInitialize();
    //}

    //TODO: Review usage
    static protected T Create<T>(ObjectTypeInfo typeInfo) where T : BaseObject {
      T item = typeInfo.CreateObject<T>();
      item.objectTypeInfo = typeInfo;

      return item;
    }

    static public T ParseDataRow<T>(DataRow dataRow) where T : BaseObject {
      try {
        ObjectTypeInfo baseObjectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

        ObjectTypeInfo derivedTypeInfo = BaseObject.GetDerivedTypeInfo(baseObjectTypeInfo, dataRow);
        int objectId = (int) dataRow[derivedTypeInfo.IdFieldName];

        T item = cache.TryGetItem<T>(derivedTypeInfo.Name, objectId);
        if (item != null) {
          return item;    // Only use dataRow when item is not in cache
        }
        return BaseObject.CreateBaseObject<T>(derivedTypeInfo, dataRow);
      } catch (Exception e) {
        var exception = new OntologyException(OntologyException.Msg.CannotParseObjectWithDataRow,
                                              e, typeof(T).FullName);
        exception.Publish();
        throw exception;
      }
    }

    static internal T ParseIdNoCache<T>(int id) where T : BaseObject {
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse(typeof(T));
      if (id == 0) {
        throw new OntologyException(OntologyException.Msg.TryToParseZeroObjectId, typeInfo.Name);
      }
      T item = cache.TryGetItem<T>(typeInfo.Name, id);
      if (item != null) {
        return item;
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = BaseObject.GetBaseObjectData(typeInfo.Name, id);

      return BaseObject.CreateBaseObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected T ParseId<T>(int id) where T : BaseObject {
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse(typeof(T));
      if (id == 0) {
        throw new OntologyException(OntologyException.Msg.TryToParseZeroObjectId, typeInfo.Name);
      }
      if (id == emptyInstanceId) {
        return typeInfo.GetEmptyInstance<T>().Clone<T>();
      }
      if (id == unknownInstanceId) {
        return typeInfo.GetUnknownInstance<T>().Clone<T>();
      }
      T item = cache.TryGetItem<T>(typeInfo.Name, id);
      if (item != null) {
        return item;
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = BaseObject.GetBaseObjectData(typeInfo.Name, id);

      return BaseObject.CreateBaseObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected T ParseKey<T>(string namedKey) where T : BaseObject {
      string typeName = ObjectTypeInfo.Parse(typeof(T)).Name;

      T item = cache.TryGetItem<T>(typeName, namedKey);
      if (item != null) {
        return item;
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = GetBaseObjectData(typeName, namedKey);

      return BaseObject.CreateBaseObject<T>(objectData.Item1, objectData.Item2);
    }

    // TODO: review usage /// GeneralObject + TypeAssociationInfo
    static internal T Parse<T>(ObjectTypeInfo typeInfo, DataRow dataRow) where T : BaseObject {
      T item = cache.TryGetItem<T>(typeInfo.Name, (int) dataRow[typeInfo.IdFieldName]);
      if (item != null) {
        return item;      // Only use dataRow when item is not in cache
      }
      return BaseObject.Parse<T>(typeInfo.Name, dataRow);
    }

    //TODO: Remove this method
    static private T Parse<T>(string typeName, DataRow dataRow) where T : BaseObject {
      try {
        ObjectTypeInfo baseObjectTypeInfo = ObjectTypeInfo.Parse(typeName);

        ObjectTypeInfo derivedTypeInfo = BaseObject.GetDerivedTypeInfo(baseObjectTypeInfo, dataRow);
        int objectId = (int) dataRow[derivedTypeInfo.IdFieldName];

        T item = cache.TryGetItem<T>(derivedTypeInfo.Name, objectId);
        if (item != null) {
          return item;    // Only use dataRow when item is not in cache
        }
        return BaseObject.CreateBaseObject<T>(derivedTypeInfo, dataRow);
      } catch (Exception e) {
        var exception = new OntologyException(OntologyException.Msg.CannotParseObjectWithDataRow,
                                              e, typeName);
        exception.Publish();
        throw exception;
      }
    }

    static protected T ParseEmpty<T>() where T : BaseObject {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));
      
      return objectTypeInfo.GetEmptyInstance<T>().Clone<T>();
    }

    static protected T ParseFromBelow<T>(int id) where T : BaseObject {
      string typeName = ObjectTypeInfo.Parse(typeof(T)).Name;
      if (id == 0) {
        Assertion.Assert(id != 0, new OntologyException(OntologyException.Msg.TryToParseZeroObjectId,
                                                        typeName));
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = GetBaseObjectData(typeName, id);

      return BaseObject.CreateBaseObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected T ParseFromBelow<T>(DataRow dataRow) where T : BaseObject {
      ObjectTypeInfo baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      ObjectTypeInfo derivedTypeInfo = BaseObject.GetDerivedTypeInfo(baseTypeInfo, dataRow);

      return BaseObject.CreateBaseObject<T>(derivedTypeInfo, dataRow);
    }

    static public List<T> ParseList<T>(DataTable dataTable) where T : BaseObject {
      if (dataTable == null || dataTable.Rows.Count == 0) {
        return new List<T>();
      }
      ObjectTypeInfo typeInfo = ObjectTypeInfo.Parse(typeof(T));
      try {
        List<T> list = new List<T>(dataTable.Rows.Count);
        foreach (DataRow dataRow in dataTable.Rows) {
          ObjectTypeInfo derivedTypeInfo = BaseObject.GetDerivedTypeInfo(typeInfo, dataRow);
          int objectId = (int) dataRow[derivedTypeInfo.IdFieldName];

          T item = cache.TryGetItem<T>(derivedTypeInfo.Name, objectId);
          if (item != null) {
            list.Add(item);    // Only use dataRow when item is not in cache
          } else {
            list.Add(BaseObject.CreateBaseObject<T>(derivedTypeInfo, dataRow));
          }
        }
        return list;
      } catch (Exception e) {
        var exception = new OntologyException(OntologyException.Msg.CannotParseObjectWithDataRow,
                                              e, typeInfo.Name);
        exception.Publish();
        throw exception;
      }
    }

    static protected T ParseUnknown<T>() where T : BaseObject {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      return objectTypeInfo.GetUnknownInstance<T>().Clone<T>();
    }

    #endregion Constructors and parsers

    #region Public properties

    protected AttributesBag AttributesBag {
      get;
      private set;
    }

    public ObjectTypeInfo ObjectTypeInfo {
      get { return this.objectTypeInfo; }
    }

    public int Id {
      get { return this.objectId; }
    }

    protected bool IsDirty {
      get { return this.isDirty; }
    }

    public bool IsEmptyInstance {
      get { return (this.objectId == emptyInstanceId); }
    }

    public bool IsNew {
      get { return (this.objectId == 0 || isNewFlag == true); }
    }

    protected internal bool IsSpecialCase {
      get {
        return (this.objectId == emptyInstanceId ||
                this.objectId == unknownInstanceId);
      }
    }

    protected bool IsUnknownInstance {
      get {
        return (this.objectId == unknownInstanceId);
      }
    }

    #endregion Public properties

    #region Public methods

    /// <summary>Virtual method that creates a shallow copy of the current instance.</summary>
    protected virtual T Clone<T>() where T : BaseObject {
      return (T) this.MemberwiseClone();
    }

    protected void DataBind(DataRow row) {
      this.ObjectTypeInfo.DataBind(this, row);
    }

    protected void LoadAttributesBagUsingHashTable(DataRow row) {
      this.AttributesBag = new AttributesBag(this, row);
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

    public override int GetHashCode() {
      return (this.objectTypeInfo.GetHashCode() ^ this.Id);
    }

    //protected T GetLink<T>(string linkName) where T : BaseObject {
    //  TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

    //  return association.GetLink<T>(this);
    //}

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

    protected FixedList<T> GetLinks<T>(string linkName, TimePeriod period) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

      return association.GetLinks<T>(this, period);
    }

    protected FixedList<T> GetLinks<T>(string linkName, TimePeriod period,
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

    protected FixedList<TypeAssociationInfo> GetTypeAssociationLinks(string linkName) {
      TypeAssociationInfo association = objectTypeInfo.Associations[linkName];

      return association.GetAssociationLinks(this);
    }

    protected void Link(TypeAssociationInfo assocationInfo, IStorable value) {
      OntologyData.WriteLink(assocationInfo, this, value);
    }

    /// <summary>Raised for new and stored instances, after object creation and before
    /// databinding if their type is marked as IsDatabounded.</summary>
    protected virtual void OnInitialize() {

    }

    /// <summary>Raised after initialization and after databinding if their type is 
    /// marked as IsDatabounded.</summary>
    protected virtual void OnLoadObjectData(DataRow row) {

    }

    /// <summary>Raised when Save() method is called and after objectId was created.</summary>
    protected virtual void OnSave() {
      throw new NotImplementedException();
    }

    public void Save() {
      // Never save special case instances (e.g. Empty or Unknown)
      if (this.IsSpecialCase) {
        return;
      }
      if (this.objectId == 0) {
        this.objectId = OntologyData.GetNextObjectId(this.ObjectTypeInfo);
      }
      this.OnSave();
      this.isNewFlag = false;
      cache.Insert(this);
    }


    // TODO: Review usage only in FilesFolder
    protected DataRow GetDataRow() {
      return OntologyData.GetBaseObjectDataRow(this.ObjectTypeInfo, this.Id);
    }

    #endregion Public methods

    #region Private methods

    static private T CreateBaseObject<T>(ObjectTypeInfo typeInfo, 
                                         DataRow dataRow) where T : BaseObject {
      T item = typeInfo.CreateObject<T>();
      
      item.objectTypeInfo = typeInfo;
      item.objectId = (int) dataRow[typeInfo.IdFieldName];
      if (item.objectTypeInfo.IsDataBound) {
        item.DataBind(dataRow);
      }
      item.OnLoadObjectData(dataRow);
      item.isNewFlag = false;

      if (typeInfo.UsesNamedKey) {
        cache.Insert(item, (string) dataRow[typeInfo.NamedIdFieldName]);
      } else {
        cache.Insert(item);
      }
      return item;
    }

    static private Tuple<ObjectTypeInfo, DataRow> GetBaseObjectData(string baseTypeName, int objectId) {
      ObjectTypeInfo derivedTypeInfo = ObjectTypeInfo.Parse(baseTypeName);
      DataRow dataRow = OntologyData.GetBaseObjectDataRow(derivedTypeInfo, objectId);
      if (dataRow == null) {
        throw new OntologyException(OntologyException.Msg.ObjectIdNotFound,
                                    objectId, baseTypeName);
      }
      if ((objectId == emptyInstanceId || objectId == unknownInstanceId)) {
        if (derivedTypeInfo.IsAbstract) {
          Assertion.AssertObject(derivedTypeInfo.TypeIdFieldName, "derivedTypeInfo.TypeIdFieldName");
          derivedTypeInfo = ObjectTypeInfo.Parse((int) dataRow[derivedTypeInfo.TypeIdFieldName]);
        }
      } else if (derivedTypeInfo.TypeIdFieldName.Length != 0) {      // Is base powertyped
        derivedTypeInfo = ObjectTypeInfo.Parse((int) dataRow[derivedTypeInfo.TypeIdFieldName]);
      }
      return new Tuple<ObjectTypeInfo, DataRow>(derivedTypeInfo, dataRow);
    }

    static private Tuple<ObjectTypeInfo, DataRow> GetBaseObjectData(string baseTypeName, string objectNamedKey) {
      ObjectTypeInfo derivedTypeInfo = ObjectTypeInfo.Parse(baseTypeName);
      DataRow dataRow = OntologyData.GetBaseObjectDataRow(derivedTypeInfo, objectNamedKey);
      if (dataRow == null) {
        throw new OntologyException(OntologyException.Msg.ObjectNamedKeyNotFound,
                                    baseTypeName, objectNamedKey);
      }
      if (derivedTypeInfo.TypeIdFieldName.Length != 0) {      // Is base powertyped
        derivedTypeInfo = ObjectTypeInfo.Parse((int) dataRow[derivedTypeInfo.TypeIdFieldName]);
      }
      return new Tuple<ObjectTypeInfo, DataRow>(derivedTypeInfo, dataRow);
    }

    static private ObjectTypeInfo GetDerivedTypeInfo(ObjectTypeInfo baseTypeInfo, DataRow dataRow) {
      if (baseTypeInfo.TypeIdFieldName.Length == 0) {
        return baseTypeInfo;
      }
      int dataRowTypeIdValue = (int) dataRow[baseTypeInfo.TypeIdFieldName];
      if (dataRowTypeIdValue == baseTypeInfo.Id) {
        return baseTypeInfo;
      } else {
        return ObjectTypeInfo.Parse(dataRowTypeIdValue);
      }
    }

    #endregion Private methods

    protected void AssociateOne(IStorable instance) {
      throw new NotImplementedException();
    }

    protected void AssociateOne(IStorable instance, string associationName) {
      throw new NotImplementedException();
    }

    protected void AssociateWith(IStorable instance) {
      throw new NotImplementedException();
    }

    protected void AssociateWith(IStorable instance, string associationName) {
      throw new NotImplementedException();
    }

    protected FixedList<T> GetAssociations<T>() where T : IStorable {
      throw new NotImplementedException();
    }

    protected FixedList<T> GetAssociations<T>(string associationName) where T : IStorable {
      throw new NotImplementedException();
    }

    protected FixedList<T> GetAssociations<T>(Predicate<T> predicate) where T : IStorable {
      throw new NotImplementedException();
    }

    protected void ReclassifyAs(ObjectTypeInfo newType) {
      Assertion.AssertObject(newType, "newType");
      Assertion.Assert(!this.ObjectTypeInfo.Equals(newType),
                       "newType should be distinct to the current one.");
      Assertion.Assert(!this.ObjectTypeInfo.UnderlyingSystemType.Equals(newType.UnderlyingSystemType),
                       "newType underlying system type should be the same to the current one's.");

      cache.Remove(this);
      this.objectTypeInfo = newType;
      cache.Insert(this);
    }

  } // class BaseObject

} // namespace Empiria
