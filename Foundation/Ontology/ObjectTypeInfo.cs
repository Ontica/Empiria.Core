/* Empiria Foundation Framework 2014 *************************************************************************
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
using System.Reflection;

using Empiria.Collections;
using Empiria.Ontology.Modeler;
using Empiria.Reflection;

namespace Empiria.Ontology {

  public class ObjectTypeInfo : MetaModelType {

    #region Fields

    static private DoubleKeyList<ObjectTypeInfo> cacheByUnderlyingType = new DoubleKeyList<ObjectTypeInfo>();

    private DataMappingRules dataMappingRules = null;
    private object lockThreadObject = new object();

    #endregion Fields

    #region Constructors and parsers

    protected internal ObjectTypeInfo(int id) : base(MetaModelTypeFamily.ObjectType, id) {

    }

    protected internal ObjectTypeInfo(string name) : base(MetaModelTypeFamily.ObjectType, name) {

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

    static internal ObjectTypeInfo Parse(Type type) {
      string underlyingTypeFullName = type.FullName;

      if (!cacheByUnderlyingType.ContainsKey(underlyingTypeFullName)) {
        lock (cacheByUnderlyingType) {
          if (!cacheByUnderlyingType.ContainsKey(underlyingTypeFullName)) {
            Type powertypeType = ObjectTypeInfo.TryGetPowertypeType(type);
            ObjectTypeInfo objectTypeInfo = null;
            if (powertypeType != null) {
              int typeId = (int) OntologyData.GetBaseObjectTypeInfoDataRowWithType(type)["TypeId"];
              objectTypeInfo = (ObjectTypeInfo) ObjectFactory.ParseObject(powertypeType, typeId);
            } else {
              objectTypeInfo = (ObjectTypeInfo)
                ObjectTypeInfo.Parse(OntologyData.GetBaseObjectTypeInfoDataRowWithType(type));
            }
            cacheByUnderlyingType.Add(underlyingTypeFullName, objectTypeInfo);
          }
        }  // lock
      }
      return cacheByUnderlyingType[underlyingTypeFullName];
    }

    #endregion Constructors and parsers

    #region Public properties

    public new DoubleKeyList<TypeAssociationInfo> Associations {
      get { return base.Associations; }
    }

    public new DoubleKeyList<TypeAttributeInfo> Attributes {
      get { return base.Attributes; }
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
    /// Powertype types should override this property and return true. Furthermore they should
    /// been decorated with the PowerType attribute.</summary>
    public virtual bool IsPowertype {
      get {
        return false;
      }
    }

    public new DoubleKeyList<TypeMethodInfo> Methods {
      get { return base.Methods; }
    }

    #endregion Public properties

    #region Public methods

    internal T CreateObject<T>() where T : BaseObject  {
      return this.InvokeBaseObjectConstructor<T>();
    }

    internal void DataBind(BaseObject instance, DataRow row) {
      if (this.IsDataBound) {
        this.AssertMappingRulesAreLoaded();
        dataMappingRules.DataBind(instance, row);
      }
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

    /// <summary>Returns a comma separated string with this ObjectType Id and all 
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
    private ConstructorInfo GetBaseObjectConstructor() {
      if (this.IsPowertype) {
        //Partitioned type instances are created using 'constructor(ObjectTypeInfo t)'
        return this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.NonPublic,
                                                null, CallingConventions.HasThis,
                                                new Type[] { this.GetType() }, null);
      } else {
        //No partitioned type instances are created using parameterless default 'constructor()'
        return this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.NonPublic,
                                                null, CallingConventions.HasThis,
                                                new Type[0], null);
      }
    }

    private ConstructorInfo _baseObjectConstructor = null;
    /// <summary>Creates a new instance of type T invoking its default constructor.</summary>
    private T InvokeBaseObjectConstructor<T>() where T : BaseObject {
      if (_baseObjectConstructor == null) {
        _baseObjectConstructor = this.GetBaseObjectConstructor();
      }
      if (this.IsPowertype) {
        //Partitioned type instances are created using 'constructor(ObjectTypeInfo t)'
        return (T) _baseObjectConstructor.Invoke(new object[] { this });     
      } else {
        //No partitioned type instances are created using parameterless default 'constructor()'
        return (T) _baseObjectConstructor.Invoke(null);
      }
    }

    static private Type TryGetPowertypeType(Type type) {
      var attribute = (PartitionedTypeAttribute) 
                            Attribute.GetCustomAttribute(type, typeof(PartitionedTypeAttribute));
      if (attribute != null) {
        return attribute.Powertype;
      } else {
        return null;
      }
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
