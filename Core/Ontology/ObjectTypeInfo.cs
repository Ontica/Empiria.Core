﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Ontology                           Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Type Metadata Information Holder        *
*  Type     : ObjectTypeInfo                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an object type definition.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using System.Reflection;
using System.Reflection.Emit;

using Empiria.ORM;
using Empiria.Reflection;

namespace Empiria.Ontology {

  /// <summary>Represents an object type definition.</summary>
  public class ObjectTypeInfo : MetaModelType {

    #region Fields

    private DataMappingRules dataMappingRules = null;
    private readonly object lockThreadObject = new object();

    internal const int EmptyInstanceId = -1;
    internal const int UnknownInstanceId = -2;

    // Delegates definition
    private delegate object DefaultConstructorDelegate();
    private delegate object PowertypeConstructorDelegate(ObjectTypeInfo empiriaType);

    // Delegate instances for either default or powertype instances
    private DefaultConstructorDelegate defaultTypeConstructorDelegate;
    private PowertypeConstructorDelegate powertypeConstructorDelegate;

    private readonly object _locker = new object();

    #endregion Fields

    #region Properties

    protected internal ObjectTypeInfo() : base(MetaModelTypeFamily.ObjectType) {

    }

    static public new ObjectTypeInfo Parse(int typeId) {
      return MetaModelType.Parse<ObjectTypeInfo>(typeId);
    }

    static public new T Parse<T>(int typeId) where T : ObjectTypeInfo {
      return MetaModelType.Parse<T>(typeId);
    }

    static public new T Parse<T>(string typeName) where T : ObjectTypeInfo {
      return MetaModelType.Parse<T>(typeName);
    }

    static public new ObjectTypeInfo Parse(string name) {
      return MetaModelType.Parse<ObjectTypeInfo>(name);
    }

    /// <summary>Returns the base ObjectTypeInfo for objects of type T.</summary>
    /// <typeparam name="T">The underlying system type associated to the base ObjectTypeInfo
    /// attempted to parse.</typeparam>
    /// <returns>The base ObjectTypeInfo for objects of type T.</returns>
    static public ObjectTypeInfo Parse<T>() where T : BaseObject {
      return Parse(typeof(T));
    }


    static private readonly Dictionary<int, ObjectTypeInfo> _cacheByUnderlyingType =
                                                                  new Dictionary<int, ObjectTypeInfo>();


    static internal ObjectTypeInfo Parse(Type type) {
     int typeHashCode = type.GetHashCode();
      ObjectTypeInfo value = null;
      if (_cacheByUnderlyingType.TryGetValue(typeHashCode, out value)) {
        return value;
      }
      lock (_cacheByUnderlyingType) {
        if (!_cacheByUnderlyingType.ContainsKey(typeHashCode)) {
          _cacheByUnderlyingType.Add(typeHashCode, MetaModelType.Parse<ObjectTypeInfo>(type));
        }
      }  // lock
      return _cacheByUnderlyingType[typeHashCode];
    }


    #endregion Constructors and parsers

    #region Public properties

    [Newtonsoft.Json.JsonIgnore]
    public new ObjectTypeInfo BaseType {
      get { return (ObjectTypeInfo) base.BaseType; }
    }


    public bool GenerateIdOnCreation {
      get {
        return ExtensionData.Get("generateIdOnCreation", false);
      }
    }


    private bool? _isDataBoundFlag = null;
    public bool IsDataBound {
      get {
        if (_isDataBoundFlag.HasValue) {
          return _isDataBoundFlag.Value;
        }
        lock (_locker) {
          if (!_isDataBoundFlag.HasValue) {
            _isDataBoundFlag = DataMappingRules.IsDataBound(base.UnderlyingSystemType);
          }
        }
        return _isDataBoundFlag.Value;
      }
    }

    /// <summary>Virtual method that indicates if this objecttype corresponds to a powertype.
    /// Powertype types must override this property and return true. Furthermore they should
    /// been decorated with the PowerType attribute.</summary>
    public virtual bool IsPowertype {
      get {
        return false;
      }
    }


    public string ReclassificationTag {
      get {
        return this.ExtensionData.Get("reclassificationTag", String.Empty);
      }
    }

    #endregion Properties

    #region Methods

    /// <summary>Creates a new instance of type T invoking its default or powertype constructor.</summary>
    protected internal T CreateObject<T>() where T : BaseObject {
      this.AssertTypeInstancesConstructorIsAssigned();
      if (this.IsPowertype) {
        //Partitioned type instances are created using a delegate to 'constructor(ObjectTypeInfo t)'
        return (T) powertypeConstructorDelegate(this);
      } else {
        // No partitioned type instances are created using a delegate to
        // the the parameterless default 'constructor()'
        return (T) defaultTypeConstructorDelegate();
      }
    }


    internal void DataBind(BaseObject instance, DataRow row) {
      if (this.IsDataBound) {
        this.AssertMappingRulesAreLoaded();

        dataMappingRules.DataBind(instance, row);
      }
    }


    internal Tuple<ObjectTypeInfo, DataRow> GetObjectTypeAndDataRow(int objectId) {
      DataRow dataRow = OntologyData.GetBaseObjectDataRow(this, objectId);

      if (dataRow == null) {
        throw new OntologyException(OntologyException.Msg.ObjectIdNotFound,
                                    objectId, this.Name);
      }

      if (this.TypeIdFieldName.Length == 0) {
        return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
      }

      int derivedTypeId = (int) dataRow[this.TypeIdFieldName];

      if ((objectId == EmptyInstanceId || objectId == UnknownInstanceId)) {
        if (this.IsAbstract) {
          return new Tuple<ObjectTypeInfo, DataRow>(ObjectTypeInfo.Parse(derivedTypeId), dataRow);
        }
      } else if (derivedTypeId != this.Id) {  // If types are distinct then change basetype to derived
        return new Tuple<ObjectTypeInfo, DataRow>(ObjectTypeInfo.Parse(derivedTypeId), dataRow);
      }

      return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
    }


    internal Tuple<ObjectTypeInfo, DataRow> GetObjectTypeAndDataRow(string objectNamedKey) {
      DataRow dataRow = OntologyData.GetBaseObjectDataRow(this, objectNamedKey);
      if (dataRow == null) {
        throw new OntologyException(OntologyException.Msg.ObjectNamedKeyNotFound,
                                    this.Name, objectNamedKey);
      }
      if (this.TypeIdFieldName.Length == 0) {
        return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
      }

      int derivedTypeId = (int) dataRow[this.TypeIdFieldName];
      if (derivedTypeId != this.Id) {   // If types are distinct then change basetype to derived
        return new Tuple<ObjectTypeInfo, DataRow>(ObjectTypeInfo.Parse(derivedTypeId), dataRow);
      }
      return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
    }


    internal ObjectTypeInfo GetDerivedType(DataRow dataRow) {
      if (this.TypeIdFieldName.Length == 0) {
        return this;
      }
      int dataRowTypeIdValue = (int) dataRow[this.TypeIdFieldName];
      if (dataRowTypeIdValue == this.Id) {
        return this;
      } else {
        return Parse(dataRowTypeIdValue);
      }
    }


    private BaseObject _emptyInstance = null;
    /// <summary>Return the empty instance for this type.</summary>
    internal T GetEmptyInstance<T>(int emptyId = EmptyInstanceId) where T : BaseObject {
      if (_emptyInstance == null) {
        _emptyInstance = BaseObject.ParseIdInternal<T>(this, emptyId, true);
      }
      return (T) _emptyInstance;
    }


    public ObjectTypeInfo[] GetAllSubclasses(bool includeBaseClass = true) {
      var allSubclasses = new List<ObjectTypeInfo>(16);

      if (includeBaseClass) {
        allSubclasses.Add(this);
      }

      var subclasses = this.GetSubclasses();

      foreach (var subclass in subclasses) {
        allSubclasses.Add(subclass);
        allSubclasses.AddRange(subclass.GetSubclasses());
      }

      return allSubclasses.ToFixedList().Distinct().ToArray();
    }


    public string GetAllSubclassesFilter() {
      ObjectTypeInfo[] allSubClasses = this.GetAllSubclasses();

      return string.Join(",", allSubClasses.ToFixedList().Select(x => x.Id));
    }


    private ObjectTypeInfo[] _subclassesArray = null;
    public ObjectTypeInfo[] GetSubclasses() {
      if (_subclassesArray == null) {
        lock (lockThreadObject) {
          if (_subclassesArray == null) {
            DataTable dataTable = OntologyData.GetDerivedTypes(this.Id);

            ObjectTypeInfo[] array = new ObjectTypeInfo[dataTable.Rows.Count];
            for (int i = 0; i < dataTable.Rows.Count; i++) {
              array[i] = Parse((int) dataTable.Rows[i]["TypeId"]);
            }
            _subclassesArray = array;
          }
        }  // lock
      }
      return _subclassesArray;
    }

    /// <summary>Returns a comma separated string with this ObjectType.Id and all
    /// their subclasses Id's (e.g. "93, 192, 677")
    /// </summary>
    public string GetSubclassesFilter() {
      ObjectTypeInfo[] subclasses = this.GetSubclasses();

      string joined = string.Join(",", subclasses.ToFixedList().Select(x => x.Id));

      return joined.Length == 0 ? this.Id.ToString() : $"{this.Id}, {joined}";
    }


    private BaseObject _unknownInstance = null;
    internal T GetUnknownInstance<T>() where T : BaseObject {
      if (_unknownInstance == null) {
        _unknownInstance = BaseObject.ParseIdInternal<T>(this, UnknownInstanceId);
      }
      return (T) _unknownInstance;
    }


    internal void InitializeObject(BaseObject baseObject) {
      if (this.IsDataBound) {
        this.AssertMappingRulesAreLoaded();
        dataMappingRules.InitializeObject(baseObject);
      }
    }


    public bool IsBaseClassOf(ObjectTypeInfo typeInfo) {
     return typeInfo.IsSubclassOf(this);
    }


    public bool IsSubclassOf(ObjectTypeInfo typeInfo) {
      var typeIterator = this.BaseType;
      while (true) {
        if (typeIterator.Id == typeInfo.Id) {
          return true;
        } else if (typeIterator.IsPrimitive) {
          return false;
        } else {
          typeIterator = typeIterator.BaseType;
        }
      }
    }


    public BaseObject ParseObject(int objectId) {
      return (BaseObject) ObjectFactory.InvokeParseMethod(UnderlyingSystemType, objectId);
    }


    public BaseObject ParseObject(string objectUID) {
      Assertion.Require(objectUID, nameof(objectUID));

      return (BaseObject) ObjectFactory.InvokeParseMethod(UnderlyingSystemType, objectUID);
    }


    internal Tuple<ObjectTypeInfo, DataRow> TryGetObjectTypeAndDataRow(IFilter condition) {
      DataRow dataRow = OntologyData.GetBaseObjectDataRow(this, condition);
      if (dataRow == null) {
        return null;
      }
      if (this.TypeIdFieldName.Length == 0) {
        return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
      }

      int derivedTypeId = (int) dataRow[this.TypeIdFieldName];
      if (derivedTypeId != this.Id) {   // If types are distinct then change basetype to derived
        return new Tuple<ObjectTypeInfo, DataRow>(Parse(derivedTypeId), dataRow);
      }
      return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
    }

    #endregion Methods

    #region Helpers

    private void AssertMappingRulesAreLoaded() {
      if (dataMappingRules == null && this.IsDataBound) {
        lock (lockThreadObject) {
          if (dataMappingRules == null) {
            dataMappingRules = DataMappingRules.Parse(base.UnderlyingSystemType);
          }
        }
      }
    }

    /// <summary>Gets the type default constructor, public or private, that takes no parameters for
    /// standard classes, or that take a powertype constructor for partitioned types.</summary>
    private void AssertTypeInstancesConstructorIsAssigned() {
      if (defaultTypeConstructorDelegate != null || powertypeConstructorDelegate != null) {
        return;
      }
      if (this.IsPowertype) {
        // Partitioned type instances are created using 'constructor(ObjectTypeInfo t)'
        // through a PowertypeConstructorDelegate instance
        powertypeConstructorDelegate = GetPowertypeConstructorDelegate();
      } else {
        // No partitioned type instances are created using parameterless default 'constructor()'
        // through a DefaultConstructorDelegate instance
        defaultTypeConstructorDelegate = GetDefaultConstructorDelegate();
      }
    }


    private DefaultConstructorDelegate GetDefaultConstructorDelegate() {
      ConstructorInfo constructor = this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance |
                                                                             BindingFlags.Public |
                                                                             BindingFlags.NonPublic,
                                                                             null, CallingConventions.HasThis,
                                                                             new Type[0], null);

      if (constructor == null) {
        throw new OntologyException(OntologyException.Msg.DefaultConstructorNotFound,
                                    this.UnderlyingSystemType.FullName);
      }

      var dynMethod = new DynamicMethod(this.UnderlyingSystemType.Name + "Ctor",
                                        this.UnderlyingSystemType, null,
                                        constructor.Module, true);

      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Newobj, constructor);
      codeGenerator.Emit(OpCodes.Ret);

      return (DefaultConstructorDelegate) dynMethod.CreateDelegate(typeof(DefaultConstructorDelegate));
    }


    private PowertypeConstructorDelegate GetPowertypeConstructorDelegate() {
      var constructor = this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance |
                                                                 BindingFlags.Public |
                                                                 BindingFlags.NonPublic,
                                                                 null, CallingConventions.HasThis,
                                                                 new Type[] { this.GetType() }, null);

      if (constructor == null) {
        throw new OntologyException(OntologyException.Msg.PartitionedTypeConstructorNotFound,
                                    this.UnderlyingSystemType.FullName);
      }
      var dynMethod = new DynamicMethod(this.UnderlyingSystemType.Name + "Ctor",
                                        this.UnderlyingSystemType, new Type[] { typeof(ObjectTypeInfo) },
                                        constructor.Module, true);

      // Generate the intermediate language.
      ILGenerator codeGenerator = dynMethod.GetILGenerator();
      codeGenerator.Emit(OpCodes.Ldarg_0);
      codeGenerator.Emit(OpCodes.Newobj, constructor);
      codeGenerator.Emit(OpCodes.Ret);

      return (PowertypeConstructorDelegate) dynMethod.CreateDelegate(typeof(PowertypeConstructorDelegate));
    }

    #endregion Helpers

  } // class ObjectTypeInfo

} // namespace Empiria.Ontology
