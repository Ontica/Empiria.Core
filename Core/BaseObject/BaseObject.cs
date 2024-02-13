/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Ontology                           Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Layer supertype                         *
*  Type     : BaseObject                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : BaseObject is the root type of the object type hierarchy in Empiria Framework.                 *
*             All object types that uses the framework must be descendants of this abstract type.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;
using Empiria.Collections;

using Empiria.Ontology;

namespace Empiria {

  /// <summary>BaseObject is the root type of the object type hierarchy in Empiria Framework.
  /// All object types that uses the framework must be descendants of this abstract type.</summary>
  public abstract class BaseObject : IIdentifiable, IEquatable<BaseObject> {

    #region Fields

    static private readonly ObjectsCache _cache = new ObjectsCache();

    private ObjectTypeInfo objectTypeInfo = null;
    private int objectId = 0;
    private bool isDirtyFlag = true;
    private bool isNewFlag = true;

    #endregion Fields

    #region Constructors and parsers

    protected BaseObject() {
      objectTypeInfo = ObjectTypeInfo.Parse(this.GetType());

      if (objectTypeInfo.IsDataBound) {
        // Should call InitializeObject only when is not called through BaseObject.ParseEmpiriaObject.
        objectTypeInfo.InitializeObject(this);
      }
    }


    protected BaseObject(ObjectTypeInfo powertype) {
      objectTypeInfo = powertype;

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


    static internal T ParseDataRow<T>(DataRow dataRow) where T : BaseObject {
      ObjectTypeInfo baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      int id = Convert.ToInt32(dataRow[baseTypeInfo.IdFieldName]);
      T item = _cache.TryGetItem<T>(baseTypeInfo.Name, id);
      if (item != null) {
        return item;    // Only use dataRow when item is not in cache
      }

      ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

      return BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow);
    }


    static protected T ParseEmpty<T>() where T : BaseObject {
      var typeInfo = ObjectTypeInfo.Parse(typeof(T));

      return typeInfo.GetEmptyInstance<T>().Clone<T>();
    }


    static protected T ParseEmpty<T>(int emptyId) where T : BaseObject {
      var typeInfo = ObjectTypeInfo.Parse(typeof(T));

      return typeInfo.GetEmptyInstance<T>(emptyId).Clone<T>();
    }


    // TODO: Remove reload flag when it is possible
    static protected internal T ParseId<T>(int id, bool reload = false) where T : BaseObject {
      var typeInfo = ObjectTypeInfo.Parse(typeof(T));

      if (id == ObjectTypeInfo.EmptyInstanceId || id == 0) {    // To Do: Allow zeros using a flag
        return typeInfo.GetEmptyInstance<T>(id).Clone<T>();
      }
      if (id == ObjectTypeInfo.UnknownInstanceId) {
        return typeInfo.GetUnknownInstance<T>().Clone<T>();
      }

      return BaseObject.ParseIdInternal<T>(typeInfo, id, reload);
    }


    // TODO: Remove reload flag when it is possible
    static internal T ParseIdInternal<T>(ObjectTypeInfo typeInfo,
                                         int id, bool reload, bool acceptZeros = false) where T : BaseObject {
      if (!acceptZeros && id == 0) {
        throw new OntologyException(OntologyException.Msg.TryToParseZeroObjectId, typeInfo.Name);
      }
      if (!reload) {
        T item = _cache.TryGetItem<T>(typeInfo.Name, id);
        if (item != null) {
          return item;
        }
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = typeInfo.GetObjectTypeAndDataRow(id);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }


    static protected internal T ParseKey<T>(string namedKey) where T : BaseObject {
      var typeInfo = ObjectTypeInfo.Parse(typeof(T));

      T item = _cache.TryGetItem<T>(typeInfo.Name, namedKey);
      if (item != null) {
        return item;
      }

      Tuple<ObjectTypeInfo, DataRow> objectData = typeInfo.GetObjectTypeAndDataRow(namedKey);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }


    static internal EmpiriaHashTable<T> ParseHashTable<T>(DataTable dataTable,
                                                          Func<T, string> hashFunction) where T : BaseObject {

      if (dataTable == null || dataTable.Rows.Count == 0) {
        return new EmpiriaHashTable<T>();
      }

      var baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      int id = 0;

      try {
        var hashTable = new EmpiriaHashTable<T>(dataTable.Rows.Count);

        foreach (DataRow dataRow in dataTable.Rows) {
          id = Convert.ToInt32(dataRow[baseTypeInfo.IdFieldName]);

          T item = _cache.TryGetItem<T>(baseTypeInfo.Name, id);
          if (item != null) {
            hashTable.Insert(hashFunction.Invoke(item), item);    // Only use dataRow when item is not in cache
          } else {
            ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

            var instance = BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow);

            hashTable.Insert(hashFunction.Invoke(instance), instance);
          }
        }

        return hashTable;

      } catch (Exception e) {
        throw new OntologyException(OntologyException.Msg.CannotParseObjectWithDataRow,
                                    e, baseTypeInfo.Name, id);
      }
    }


    static internal List<T> ParseList<T>(DataTable dataTable) where T : BaseObject {
      if (dataTable == null || dataTable.Rows.Count == 0) {
        return new List<T>();
      }

      var baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      int id = 0;

      try {
        List<T> list = new List<T>(dataTable.Rows.Count);

        foreach (DataRow dataRow in dataTable.Rows) {
          id = Convert.ToInt32(dataRow[baseTypeInfo.IdFieldName]);

          T item = _cache.TryGetItem<T>(baseTypeInfo.Name, id);
          if (item != null) {
            list.Add(item);
          } else {
            ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

            list.Add(BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow));
          }
        }


        return list;

      } catch (Exception e) {
        throw new OntologyException(OntologyException.Msg.CannotParseObjectWithDataRow,
                                    e, baseTypeInfo.Name, id);
      }
    }


    static protected T ParseUnknown<T>() where T : BaseObject {
      var typeInfo = ObjectTypeInfo.Parse(typeof(T));

      return typeInfo.GetUnknownInstance<T>().Clone<T>();
    }


    // TODO: Remove reload flag when it is possible
    static protected T TryParse<T>(string condition, bool reload = false) where T : BaseObject {
      IFilter filter = Empiria.Data.SqlFilter.Parse(condition);

      var typeInfo = ObjectTypeInfo.Parse(typeof(T));

      Tuple<ObjectTypeInfo, DataRow> objectData = typeInfo.TryGetObjectTypeAndDataRow(filter);

      if (objectData == null) {
        return null;
      }

      int id = Convert.ToInt32(objectData.Item2[typeInfo.IdFieldName]);

      if (!reload) {
        T item = _cache.TryGetItem<T>(typeInfo.Name, id);
        if (item != null) {
          return item;
        }
      }
      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }


    #endregion Constructors and parsers

    #region Public properties

    public int Id {
      get {
        return this.objectId;
      }
      private set {
        this.objectId = value;
      }
    }

    public virtual string UID {
      get;
      private set;
    } = String.Empty;


    protected bool IsDirty {
      get {
        return this.isDirtyFlag || this.IsNew;
      }
    }


    [Newtonsoft.Json.JsonIgnore]
    public bool IsEmptyInstance {
      get {
        return (this.objectId == ObjectTypeInfo.EmptyInstanceId);
      }
    }


    [Newtonsoft.Json.JsonIgnore]
    public bool IsNew {
      get {
        return (this.objectId == 0 || isNewFlag);
      }
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


    public override bool Equals(object obj) => this.Equals(obj as BaseObject);

    public bool Equals(BaseObject obj) {
      if (obj == null) {
        return false;
      }
      if (Object.ReferenceEquals(this, obj)) {
        return true;
      }
      if (this.GetType() != obj.GetType()) {
        return false;
      }

      return this.Id == obj.Id && this.GetEmpiriaType().Equals(obj.GetEmpiriaType());
    }


    public ObjectTypeInfo GetEmpiriaType() {
      return this.objectTypeInfo;
    }


    public override int GetHashCode() {
      return (this.objectTypeInfo.GetHashCode() ^ this.Id);
    }


    protected void MarkAsDirty() {
      this.isDirtyFlag = true;
    }


    /// <summary>Raised after initialization and after databinding if their type is
    /// marked as IsDatabounded.</summary>
    protected virtual void OnLoad() {
      // no-op
    }

    /// <summary>Raised after initialization and after databinding if their type is
    /// marked as IsDatabounded.</summary>
    internal protected virtual void OnLoadObjectData(DataRow row) {
      // no-op
    }


    /// <summary>Raised before Save() method is called and before object.Id and object.uid created.</summary>
    protected virtual void OnBeforeSave() {
      // no-op
    }


    /// <summary>Raised when Save() method is called and after objectId and object.uid generated.</summary>
    protected virtual void OnSave() {
      throw new NotImplementedException($"{this.GetType().Name}.OnSave() method.");
    }


    protected string PatchCleanField(string newValue, string defaultValue) {
      string cleaned = EmpiriaString.Clean(newValue);

      return PatchField(cleaned, defaultValue);
    }


    protected string PatchField(string newValue, string defaultValue) {
      return FieldPatcher.PatchField(newValue, defaultValue);
    }


    protected DateTime PatchField(DateTime newValue, DateTime defaultValue) {
      return FieldPatcher.PatchField(newValue, defaultValue);
    }


    protected U PatchField<U>(int newValue, U defaultValue) where U : BaseObject {
      return FieldPatcher.PatchField(newValue, defaultValue);
    }


    protected U PatchField<U>(string newValue, U defaultValue) where U : BaseObject {
      return FieldPatcher.PatchField(newValue, defaultValue);
    }


    protected void PatchObjectId(int objectId) {
      Assertion.Require(this.objectId == 0, "ObjectId already assigned");
      Assertion.Require(this.IsNew && this.IsDirty, "Object is in an invalid status for objectId assignation.");

      this.objectId = objectId;
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

      if (isNewFlag) {
        InsertIntoCache(this);
      }

      this.isNewFlag = false;
      this.isDirtyFlag = false;
    }

    protected void ReclassifyAs(ObjectTypeInfo newType) {
      Assertion.Require(newType, "newType");
      Assertion.Require(!this.GetEmpiriaType().Equals(newType),
                       "newType should be distinct to the current one.");

      // Assertion.Assert(this.GetEmpiriaType().UnderlyingSystemType.Equals(newType.UnderlyingSystemType),
      //                 "newType underlying system type should be the same to the current one's.");
      // Seek for a common ancestor (distinct than ObjectType) between types:
      // eg: if A is a mammal and B is a bird, should be possible to convert A to B or B to A because both are animals

      _cache.Remove(this);

      this.objectTypeInfo = newType;

      InsertIntoCache(this);
    }

    #endregion Public methods

    #region Helpers

    private void DataBind(DataRow row) {
      this.GetEmpiriaType().DataBind(this, row);
    }


    static private T ParseEmpiriaObject<T>(ObjectTypeInfo typeInfo, DataRow dataRow) where T : BaseObject {
      T item = typeInfo.CreateObject<T>();

      item.objectTypeInfo = typeInfo;

      try {
        item.objectId = Convert.ToInt32(dataRow[typeInfo.IdFieldName]);

        if (typeInfo.UsesNamedKey) {
          item.UID = EmpiriaString.ToString(dataRow[typeInfo.NamedIdFieldName]);
        } else {
          item.UID = item.objectId.ToString();
        }

      } catch (Exception e) {
        throw new NotSupportedException(
              $"No pude hacer el parsing del objeto de tipo {item.objectTypeInfo}", e);
      }
      if (typeInfo.IsDataBound) {
        item.DataBind(dataRow);
      }

      item.isNewFlag = false;
      item.OnLoadObjectData(dataRow);
      item.OnLoad();
      item.isDirtyFlag = false;

      InsertIntoCache(item);

      return item;
    }

    static void InsertIntoCache<T>(T item) where T : BaseObject {
      if (item.objectTypeInfo.UsesNamedKey) {
        _cache.Insert(item, item.UID);
      }
      _cache.Insert(item);
    }

    #endregion Helpers

  } // class BaseObject

} // namespace Empiria
