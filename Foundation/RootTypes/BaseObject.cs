﻿/* Empiria Foundation Framework 2014 *************************************************************************
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
      //var s = System.Environment.StackTrace;
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
        ObjectTypeInfo baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));
        int objectId = (int) dataRow[baseTypeInfo.IdFieldName];

        T item = cache.TryGetItem<T>(baseTypeInfo.Name, objectId);
        if (item != null) {
          return item;    // Only use dataRow when item is not in cache
        }
        ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);
        
        return BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow);
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
      Tuple<ObjectTypeInfo, DataRow> objectData = typeInfo.GetBaseObjectData(id);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected internal T ParseId<T>(int id) where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));
      if (id == 0) {
        throw new OntologyException(OntologyException.Msg.TryToParseZeroObjectId, objectTypeInfo.Name);
      }
      if (id == ObjectTypeInfo.EmptyInstanceId) {
        return objectTypeInfo.GetEmptyInstance<T>().Clone<T>();
      }
      if (id == ObjectTypeInfo.UnknownInstanceId) {
        return objectTypeInfo.GetUnknownInstance<T>().Clone<T>();
      }
      T item = cache.TryGetItem<T>(objectTypeInfo.Name, id);
      if (item != null) {
        return item;
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = objectTypeInfo.GetBaseObjectData(id);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected T ParseKey<T>(string namedKey) where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      T item = cache.TryGetItem<T>(objectTypeInfo.Name, namedKey);
      if (item != null) {
        return item;
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = objectTypeInfo.GetBaseObjectData(namedKey);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }

    static protected T ParseEmpty<T>() where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));
      
      return objectTypeInfo.GetEmptyInstance<T>().Clone<T>();
    }

    static protected T ParseFromBelow<T>(int id) where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));
      if (id == 0) {
        Assertion.Assert(id != 0, new OntologyException(OntologyException.Msg.TryToParseZeroObjectId,
                                                        objectTypeInfo.Name));
      }
      Tuple<ObjectTypeInfo, DataRow> objectData = objectTypeInfo.GetBaseObjectData(id);

      return BaseObject.ParseEmpiriaObject<T>(objectData.Item1, objectData.Item2);
    }

    static public T ParseFromBelow<T>(DataRow dataRow) where T : BaseObject {
      var baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

      return BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow);
    }

    static public List<T> ParseList<T>(DataTable dataTable) where T : BaseObject {
      if (dataTable == null || dataTable.Rows.Count == 0) {
        return new List<T>();
      }
      var baseTypeInfo = ObjectTypeInfo.Parse(typeof(T));
      try {
        List<T> list = new List<T>(dataTable.Rows.Count);

        foreach (DataRow dataRow in dataTable.Rows) {
          int objectId = (int) dataRow[baseTypeInfo.IdFieldName];

          T item = cache.TryGetItem<T>(baseTypeInfo.Name, objectId);
          if (item != null) {
            list.Add(item);    // Only use dataRow when item is not in cache
          } else {
            ObjectTypeInfo derivedTypeInfo = baseTypeInfo.GetDerivedType(dataRow);

            list.Add(BaseObject.ParseEmpiriaObject<T>(derivedTypeInfo, dataRow));
          }
        }
        return list;
      } catch (Exception e) {
        var exception = new OntologyException(OntologyException.Msg.CannotParseObjectWithDataRow,
                                              e, baseTypeInfo.Name);
        exception.Publish();
        throw exception;
      }
    }

    static protected T ParseUnknown<T>() where T : BaseObject {
      var objectTypeInfo = ObjectTypeInfo.Parse(typeof(T));

      return objectTypeInfo.GetUnknownInstance<T>().Clone<T>();
    }

    #endregion Constructors and parsers

    #region Public properties

    protected AttributesBag AttributesBag {
      get;
      private set;
    }

    public int Id {
      get { return this.objectId; }
    }

    protected bool IsDirty {
      get { return this.isDirty; }
    }

    public bool IsEmptyInstance {
      get {
        return (this.objectId == ObjectTypeInfo.EmptyInstanceId);
      }
    }

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

    protected void LoadAttributesBag(DataRow row) {
      this.AttributesBag = new AttributesBag(this, row);
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
        this.objectId = OntologyData.GetNextObjectId(this.GetEmpiriaType());
      }
      this.OnSave();
      this.isNewFlag = false;
      cache.Insert(this);
    }

    #endregion Public methods

    #region Private methods

    private void DataBind(DataRow row) {
      this.GetEmpiriaType().DataBind(this, row);
    }

    static private T ParseEmpiriaObject<T>(ObjectTypeInfo typeInfo,
                                       DataRow dataRow) where T : BaseObject {
      T item = typeInfo.CreateObject<T>();
      item.objectTypeInfo = typeInfo;
      item.objectId = (int) dataRow[typeInfo.IdFieldName];
      if (typeInfo.IsDataBound) {
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
      Assertion.Assert(!this.GetEmpiriaType().Equals(newType),
                       "newType should be distinct to the current one.");
      Assertion.Assert(!this.GetEmpiriaType().UnderlyingSystemType.Equals(newType.UnderlyingSystemType),
                       "newType underlying system type should be the same to the current one's.");

      cache.Remove(this);
      this.objectTypeInfo = newType;
      cache.Insert(this);
    }

  } // class BaseObject

} // namespace Empiria
