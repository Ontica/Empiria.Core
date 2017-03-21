/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : ObjectTypeInfo                                   Pattern  : Type metadata class               *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an object type definition.                                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

using Empiria.Collections;
using Empiria.ORM;

namespace Empiria.Ontology {

  public class ObjectTypeInfo : MetaModelType {

    #region Fields

    private DataMappingRules dataMappingRules = null;
    private object lockThreadObject = new object();

    internal const int EmptyInstanceId = -1;
    internal const int UnknownInstanceId = -2;

    // Delegates definition
    private delegate object DefaultConstructorDelegate();
    private delegate object PowertypeConstructorDelegate(ObjectTypeInfo empiriaType);

    // Delegate instances for either default or powertype instances
    private DefaultConstructorDelegate defaultTypeConstructorDelegate;
    private PowertypeConstructorDelegate powertypeConstructorDelegate;


    #endregion Fields

    #region Constructors and parsers

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
      return ObjectTypeInfo.Parse(typeof(T));
    }

    static private Dictionary<int, ObjectTypeInfo> _cacheByUnderlyingType =
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

    public new EmpiriaIdAndKeyDictionary<TypeAssociationInfo> Associations {
      get { return base.Associations; }
    }

    public new ObjectTypeInfo BaseType {
      get { return (ObjectTypeInfo) base.BaseType; }
    }

    private bool? _isDataBoundFlag = null;
    public bool IsDataBound {
      get {
        if (!_isDataBoundFlag.HasValue) {
          _isDataBoundFlag = DataMappingRules.IsDataBound(base.UnderlyingSystemType);
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

    public new TypeMethodInfo[] Methods {
      get { return base.Methods; }
    }

    #endregion Public properties

    #region Public methods

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
        return ObjectTypeInfo.Parse(dataRowTypeIdValue);
      }
    }

    private BaseObject _emptyInstance = null;
    /// <summary>Return the empty instance for this type.</summary>
    internal T GetEmptyInstance<T>() where T : BaseObject {
      if (_emptyInstance == null) {
        _emptyInstance = BaseObject.ParseIdInternal<T>(this, EmptyInstanceId, false);
      }
      return (T) _emptyInstance;
    }

    private ObjectTypeInfo[] _subclassesArray = null;
    public ObjectTypeInfo[] GetSubclasses() {
      if (_subclassesArray == null) {
        lock (lockThreadObject) {
          if (_subclassesArray == null) {
            DataTable dataTable = OntologyData.GetDerivedTypes(this.Id);

            ObjectTypeInfo[] array = new ObjectTypeInfo[dataTable.Rows.Count];
            for (int i = 0; i < dataTable.Rows.Count; i++) {
              array[i] = ObjectTypeInfo.Parse((int) dataTable.Rows[i]["TypeId"]);
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
      ObjectTypeInfo[] subClasses = this.GetSubclasses();
      string subclassesFilter = this.Id.ToString();
      foreach (var subclassType in subClasses) {
        subclassesFilter += "," + subclassType.Id.ToString();
      }
      return subclassesFilter;
    }

    private BaseObject _unknownInstance = null;
    internal T GetUnknownInstance<T>() where T : BaseObject {
      if (_unknownInstance == null) {
        _unknownInstance = BaseObject.ParseIdInternal<T>(this, UnknownInstanceId, false);
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
      var typeIterator = (ObjectTypeInfo) this.BaseType;
      while (true) {
        if (typeIterator.Id == typeInfo.Id) {
          return true;
        } else if (typeIterator.IsPrimitive) {
          return false;
        } else {
          typeIterator = (ObjectTypeInfo) typeIterator.BaseType;
        }
      }
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
        return new Tuple<ObjectTypeInfo, DataRow>(ObjectTypeInfo.Parse(derivedTypeId), dataRow);
      }
      return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
    }

    #endregion Public methods

    #region Private properties and methods

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

    #endregion Private properties and methods

    //TODO: Review this and maybe move it to other type
    public Data.DataOperation GetListDataOperation(string filter, string sort) {
      string typeFilter = this.TypeIdFieldName + " = " + this.Id;

      string sql = "SELECT * FROM " + this.DataSource + " WHERE " + filter + typeFilter;

      return Data.DataOperation.Parse(sql);
    }

  } // class ObjectTypeInfo

} // namespace Empiria.Ontology
