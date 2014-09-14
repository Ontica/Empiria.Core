﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : ObjectTypeInfo                                   Pattern  : Type metadata class               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents an object type definition.                                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using Empiria.Collections;
using Empiria.Ontology.Modeler;
using Empiria.Reflection;

namespace Empiria.Ontology {

  public class ObjectTypeInfo : MetaModelType {

    #region Fields

    private DataMappingRules dataMappingRules = null;
    private object lockThreadObject = new object();

    internal const int EmptyInstanceId = -1;
    internal const int UnknownInstanceId = -2;

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

    public new DoubleKeyList<TypeAssociationInfo> Associations {
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

    private ConstructorInfo _typeInstancesConstructor = null;
    /// <summary>Creates a new instance of type T invoking its default constructor.</summary>
    internal T CreateObject<T>() where T : BaseObject  {
      if (_typeInstancesConstructor == null) {
        _typeInstancesConstructor = this.GetTypeInstancesConstructor();
      }
      if (this.IsPowertype) {
        //Partitioned type instances are created using 'constructor(ObjectTypeInfo t)'
        // return (T) _typeInstancesConstructor.Invoke(new object[] { this });
        return (T) powerTypeConstructorDelegate(this);
        //return (T) quickConstructor.DynamicInvoke(this);
      } else {
        //No partitioned type instances are created using parameterless default 'constructor()'

        return (T) defaultConstructorDelegate();
        //return (T) _typeInstancesConstructor.Invoke(null);
      }
    }

    internal void DataBind(BaseObject instance, DataRow row) {
      if (this.IsDataBound) {
        this.AssertMappingRulesAreLoaded();
        dataMappingRules.DataBind(instance, row);
      }
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

    internal Tuple<ObjectTypeInfo, DataRow> GetBaseObjectData(int objectId) {
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
      } else if (derivedTypeId != this.Id) {  // If types are distinct change basetype to derived
        return new Tuple<ObjectTypeInfo, DataRow>(ObjectTypeInfo.Parse(derivedTypeId), dataRow);
      }
      return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
    }

    internal Tuple<ObjectTypeInfo, DataRow> GetBaseObjectData(string objectNamedKey) {
      DataRow dataRow = OntologyData.GetBaseObjectDataRow(this, objectNamedKey);
      if (dataRow == null) {
        throw new OntologyException(OntologyException.Msg.ObjectNamedKeyNotFound,
                                    this.Name, objectNamedKey);
      }
      if (this.TypeIdFieldName.Length == 0) {
        return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
      }

      int derivedTypeId = (int) dataRow[this.TypeIdFieldName];
      if (derivedTypeId != this.Id) {   // If types are distinct change basetype to derived
        new Tuple<ObjectTypeInfo, DataRow>(ObjectTypeInfo.Parse(derivedTypeId), dataRow);
      }
      return new Tuple<ObjectTypeInfo, DataRow>(this, dataRow);
    }

    private BaseObject _emptyInstance = null;
    /// <summary>Return the empty instance for this type.</summary>
    internal T GetEmptyInstance<T>() where T : BaseObject {
      if (_emptyInstance == null) {
        _emptyInstance = BaseObject.ParseIdNoCache<T>(EmptyInstanceId);
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
        _unknownInstance = BaseObject.ParseIdNoCache<T>(UnknownInstanceId);
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
    private ConstructorInfo GetTypeInstancesConstructor() {
      if (this.IsPowertype) {
        //quickConstructor = GetConst();
        //Partitioned type instances are created using 'constructor(ObjectTypeInfo t)'
        powerTypeConstructorDelegate = GetPowerTypeConstructorDelegate();

        return this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.NonPublic,
                                                null, CallingConventions.HasThis,
                                                new Type[] { this.GetType() }, null);
      } else {
        defaultConstructorDelegate = GetDefaultDelegate();
        //No partitioned type instances are created using parameterless default 'constructor()'
        //quickConstructor = GetConst();

        return this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.NonPublic,
                                                null, CallingConventions.HasThis,
                                                new Type[0], null);
      }
    }

    private Delegate GetConst() {
      if (this.IsPowertype) {
        var constr = this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                        BindingFlags.NonPublic,
                                        null, CallingConventions.HasThis,
                                        new Type[] { this.GetType() }, null);
        ParameterExpression param =
            Expression.Parameter(this.GetType(), "powertype");

        NewExpression body = Expression.New(constr, param);
        return Expression.Lambda(body, param).Compile();

      } else {
        NewExpression body = Expression.New(this.UnderlyingSystemType);
        return Expression.Lambda(body).Compile();
      }
    }

    private delegate object PowerTypeConstructorDelegate(ObjectTypeInfo empiriaType);
    private PowerTypeConstructorDelegate powerTypeConstructorDelegate;


    private PowerTypeConstructorDelegate GetPowerTypeConstructorDelegate() {
      ConstructorInfo constructor = this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic,
                                    null, CallingConventions.HasThis,
                                    new Type[] { this.GetType() }, null);

      // Create a new method.
      var dynMethod = new DynamicMethod(this.UnderlyingSystemType.Name + "Ctor",
                                        this.UnderlyingSystemType, new Type[] { this.GetType() },
                                        constructor.Module, true);

      // Generate the intermediate language.
      ILGenerator lgen = dynMethod.GetILGenerator();
      lgen.Emit(OpCodes.Ldarg_1);
      lgen.Emit(OpCodes.Newobj, constructor);
      lgen.Emit(OpCodes.Ret);

      // Finish the method and create new delegate
      // pointing at it.
      return (PowerTypeConstructorDelegate) dynMethod.CreateDelegate(
                                            typeof(PowerTypeConstructorDelegate));
    }


    private delegate object DefaultConstructorDelegate();
    private DefaultConstructorDelegate defaultConstructorDelegate;

    private DefaultConstructorDelegate GetDefaultDelegate() {
      ConstructorInfo constructor = this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic,
                                    null, CallingConventions.HasThis,
                                    new Type[0], null);

      // Create a new method.
      var dynMethod = new DynamicMethod(this.UnderlyingSystemType.Name + "Ctor",
                                        this.UnderlyingSystemType, null,
                                        constructor.Module, true);

      // Generate the intermediate language.
      ILGenerator lgen = dynMethod.GetILGenerator();
      lgen.Emit(OpCodes.Newobj, constructor);
      lgen.Emit(OpCodes.Ret);

      // Finish the method and create new delegate
      // pointing at it.
      return (DefaultConstructorDelegate) dynMethod.CreateDelegate(
                                          typeof(DefaultConstructorDelegate));
    
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
