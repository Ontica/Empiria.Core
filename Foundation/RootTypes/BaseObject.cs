/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria                                          Assembly : Empiria.dll                       *
*  Type      : BaseObject                                       Pattern  : Abstract Class                    *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This type is the root of the object type hierarchy in Empiria® Framework. All object types    *
*              that uses the framework must be descendants of this abstract type.                            *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria {

  /// <summary>
  /// This type is the root of the object type hierarchy in Empiria® Framework. 
  /// All object types that uses the framework must be descendants of this abstract type.
  /// </summary>
  public abstract class BaseObject : IStorable {

    #region Abstract members

    protected abstract void ImplementsLoadObjectData(DataRow row);
    protected abstract void ImplementsSave();

    #endregion Abstract members

    #region Fields

    static private BaseObjectCache cache = new BaseObjectCache();

    private ObjectTypeInfo objectTypeInfo = null;
    private int objectId = 0;
    private bool isDirty = true;

    private const int emptyInstanceId = -1;
    private const int unknownInstanceId = -2;

    private DynamicState dynamicState = null;

    #endregion Fields

    #region Constructors and parsers

    protected BaseObject(string typeName) {
      if (typeName.Length != 0) {   // If typeName.Length == 0, is invoked with Parsing using reflection
        this.objectTypeInfo = ObjectTypeInfo.Parse(typeName);
        dynamicState = new DynamicState(this);
      }
    }

    static public T Create<T>(ObjectTypeInfo typeInfo) where T : BaseObject {
      T item = (T) BaseObject.InvokeBaseObjectConstructor(typeInfo);
      item.objectTypeInfo = typeInfo;
      item.dynamicState = new DynamicState(item);

      return item;
    }

    static protected T Parse<T>(string typeName, int id) where T : BaseObject {
      T item = cache.GetItem(typeName, id) as T;
      if (item != null) {
        return item;
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = GetBaseObjectData(typeName, id);

      return BaseObject.CreateBaseObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected T Parse<T>(string typeName, string namedKey) where T : BaseObject {
      T item = cache.GetItem(typeName, namedKey) as T;
      if (item != null) {
        return item;
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = GetBaseObjectData(typeName, namedKey);

      return BaseObject.CreateBaseObject<T>(objectData.Item1, objectData.Item2);
    }

    static internal T Parse<T>(ObjectTypeInfo typeInfo, DataRow dataRow) where T : BaseObject {
      T item = cache.GetItem(typeInfo.Name, (int) dataRow[typeInfo.IdFieldName]) as T;

      if (item != null) {
        return item;  // Only use dataRow when item is not in cache
      }
      return BaseObject.Parse<T>(typeInfo.Name, dataRow);
    }

    static protected T Parse<T>(string typeName, DataRow dataRow) where T : BaseObject {
      ObjectTypeInfo derivedTypeInfo = BaseObject.GetDerivedTypeInfo(typeName, dataRow);
      int objectIdFieldValue = (int) dataRow[derivedTypeInfo.IdFieldName];

      T item = cache.GetItem(derivedTypeInfo.Name, objectIdFieldValue) as T;
      if (item != null) {
        return item;  // Only use dataRow when item is not in cache
      }
      return BaseObject.CreateBaseObject<T>(derivedTypeInfo, dataRow);
    }

    static protected T ParseEmpty<T>(string typeName) where T : BaseObject {
      return BaseObject.Parse<T>(typeName, emptyInstanceId);
    }

    static protected T ParseFromBelow<T>(string typeName, int id) where T : BaseObject {
      Tuple<ObjectTypeInfo, DataRow> objectData = GetBaseObjectData(typeName, id);

      return BaseObject.CreateBaseObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected T ParseFromBelow<T>(string typeName, DataRow dataRow) where T : BaseObject {
      ObjectTypeInfo derivedTypeInfo = BaseObject.GetDerivedTypeInfo(typeName, dataRow);

      return BaseObject.CreateBaseObject<T>(derivedTypeInfo, dataRow);
    }

    static protected ObjectList<T> ParseList<T>(string typeName) where T : BaseObject {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse(typeName);

      DataTable table = OntologyData.GetGeneralObjectsDataTable(objectTypeInfo);
      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);
      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.Parse<T>(objectTypeInfo, table.Rows[i]));
      }
      return list;
    }

    static protected T ParseUnknown<T>(string typeName) where T : BaseObject {
      return BaseObject.Parse<T>(typeName, unknownInstanceId);
    }

    #endregion Constructors and parsers

    #region Public properties

    public ObjectTypeInfo ObjectTypeInfo {
      get { return this.objectTypeInfo; }
    }

    public int Id {
      get { return objectId; }
    }

    protected bool IsDirty {
      get { return this.isDirty; }
    }

    public bool IsEmptyInstance {
      get { return (this.objectId == -1); }
    }

    public bool IsNew {
      get { return (this.objectId == 0); }
    }

    #endregion Public properties

    #region Public methods

    public override bool Equals(object obj) {
      if (obj == null || GetType() != obj.GetType()) {
        return false;
      }
      return base.Equals(obj) && (this.Id == ((BaseObject) obj).Id);
    }

    public bool Equals(BaseObject obj) {
      if (obj == null) {
        return false;
      }
      return objectTypeInfo.Equals(obj.objectTypeInfo) && (this.Id == obj.Id);  // base.Equals(obj)
    }

    protected T GetAttribute<T>(string attributeName) {
      return dynamicState.GetMember<T>(attributeName);
    }

    protected T GetLink<T>(string linkName) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);

      return association.GetLink<T>(this);
    }

    protected ObjectList<T> GetLinks<T>(string linkName) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);

      return association.GetLinks<T>(this);
    }

    protected ObjectList<T> GetLinks<T>(string linkName, Comparison<T> sort) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);
      ObjectList<T> list = association.GetLinks<T>(this);

      list.Sort(sort);

      return list;
    }

    protected ObjectList<T> GetLinks<T>(string linkName, TimePeriod period) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);

      return association.GetLinks<T>(this, period);
    }

    protected ObjectList<T> GetLinks<T>(string linkName, TimePeriod period, Comparison<T> sort) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);
      ObjectList<T> list = association.GetLinks<T>(this, period);

      list.Sort(sort);

      return list;
    }

    protected ObjectList<T> GetLinks<T>(string linkName, Predicate<T> predicate) where T : BaseObject {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);

      return association.GetLinks<T>(this, predicate);
    }

    public override int GetHashCode() {
      return (this.objectTypeInfo.GetHashCode() ^ this.Id);
    }

    protected ObjectList<T> GetTypeLinks<T>(string linkName) where T : MetaModelType {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);

      return association.GetTypeLinks<T>(this);
    }

    protected ObjectList<T> GetTypeRelationLinks<T>(string linkName) where T : TypeRelationInfo {
      TypeAssociationInfo association = objectTypeInfo.GetAssociation(linkName);

      return association.GetTypeRelationLinks<T>(this);
    }

    void IStorable.ImplementsOnStorageUpdateEnds() {
      throw new NotImplementedException("BaseObject.ImplementsOnStorageUpdateEnds");
    }

    DataOperationList IStorable.ImplementsStorageUpdate(StorageContextOperation operation, DateTime timestamp) {
      throw new NotImplementedException("BaseObject.ImplementsStorageUpdate");
    }

    protected void Link(TypeAssociationInfo assocationInfo, IStorable value) {
      OntologyData.WriteLink(assocationInfo, this, value);
    }

    public void Save() {
      if (this.IsNew) {
        this.objectId = OntologyData.GetNextObjectId(this.ObjectTypeInfo);
      }
      Assertion.Assert(this.objectId != 0,
      "Object Id can't be zero. There is an error in GetNextObjectId or in DbRules table.");
      ImplementsSave();
      lock (cache) {
        cache.Insert(this);
      }
    }

    protected void SetAttribute<T>(string name, T value) {
      throw new NotImplementedException("BaseObject.SetAttribute<T>");
    }

    #endregion Public methods

    #region Private methods

    static private T CreateBaseObject<T>(ObjectTypeInfo typeInfo, DataRow dataRow) where T : BaseObject {
      T item = (T) BaseObject.InvokeBaseObjectConstructor(typeInfo);

      item.objectTypeInfo = typeInfo;
      item.objectId = (int) dataRow[typeInfo.IdFieldName];
      item.dynamicState = new DynamicState(item);
      item.ImplementsLoadObjectData(dataRow);
      lock (cache) {
        if (!typeInfo.UsesNamedKey) {
          cache.Insert(item);
        } else {
          cache.Insert(item, (string) dataRow[typeInfo.NamedIdFieldName]);
        }
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
          Assertion.EnsureObject(derivedTypeInfo.TypeIdFieldName, "derivedTypeInfo.TypeIdFieldName");
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

    static private ObjectTypeInfo GetDerivedTypeInfo(string baseTypeName, DataRow dataRow) {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse(baseTypeName);
      if (objectTypeInfo.TypeIdFieldName.Length == 0) {
        return objectTypeInfo;
      } else {
        return ObjectTypeInfo.Parse((int) dataRow[objectTypeInfo.TypeIdFieldName]);
      }
    }

    static private BaseObject InvokeBaseObjectConstructor(ObjectTypeInfo typeInfo) {
      return (BaseObject) ObjectFactory.CreateObject(typeInfo.UnderlyingSystemType, new Type[] { typeof(string) },
                                                     new object[] { String.Empty });
    }

    static public int CacheCount {
      get { return cache.Count; }
    }

    #endregion Private methods

  } // class BaseObject

} // namespace Empiria