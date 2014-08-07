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

using Empiria.Ontology.Modeler;
using Empiria.Reflection;

namespace Empiria.Ontology {

  public class ObjectTypeInfo : MetaModelType {

    #region Fields

    private ConstructorInfo _baseObjectConstructor = null;
    private DataMappingRules _dataMappingRules = null;
    private object _lockObject = new object();

    #endregion Fields

    #region Constructors and parsers

    protected internal ObjectTypeInfo(int id) : base(MetaModelTypeFamily.ObjectType, id) {

    }

    protected internal ObjectTypeInfo(string name)
      : base(MetaModelTypeFamily.ObjectType, name) {
    }

    static public new ObjectTypeInfo Parse(int id) {
      return MetaModelType.Parse<ObjectTypeInfo>(id);
    }

    static public new ObjectTypeInfo Parse(string name) {
      return MetaModelType.Parse<ObjectTypeInfo>(name);
    }

    static internal bool TryParse(string typeName, out ObjectTypeInfo objectTypeInfo) {
      try {
        objectTypeInfo = ObjectTypeInfo.Parse(typeName);
        return true;
      } catch {
        objectTypeInfo = null;
        return false;  
      }
    }

    static public ObjectTypeInfo Empty { 
      get { return ObjectTypeInfo.Parse(-1); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public new Empiria.Collections.DoubleKeyList<TypeAssociationInfo> Associations {
      get { return base.Associations; }
    }

    public new Empiria.Collections.DoubleKeyList<TypeAttributeInfo> Attributes {
      get { return base.Attributes; }
    }

    public new ObjectTypeInfo BaseType {
      get { return (ObjectTypeInfo) base.BaseType; }
    }

    public virtual bool IsPowerType {
      get { return false; }
    }

    public new Empiria.Collections.DoubleKeyList<TypeMethodInfo> Methods {
      get { return base.Methods; }
    }

    #endregion Public properties

    #region Public methods

    internal T CreateObject<T>() where T : BaseObject {
      return this.InvokeBaseObjectConstructor<T>();
    }

    internal void LoadObject(BaseObject baseObject, DataRow row) {
      if (_dataMappingRules == null) {
        this.LoadDataRules(row.Table.Columns);
      }
      _dataMappingRules.LoadObject(baseObject, row);
    }

    public ObjectTypeInfo[] GetSubclasses() {
      DataTable dataTable = OntologyData.GetDerivedTypes(this.Id);

      ObjectTypeInfo[] array = new ObjectTypeInfo[dataTable.Rows.Count];
      for (int i = 0; i < dataTable.Rows.Count; i++) {
        array[i] = ObjectTypeInfo.Parse((int) dataTable.Rows[i]["TypeId"]);
      }
      return array;
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

    #region Private methods

    private ConstructorInfo GetBaseObjectConstructor() {
      return this.UnderlyingSystemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public |
                                                      BindingFlags.NonPublic,
                                                      null, CallingConventions.HasThis,
                                                      new Type[] { typeof(string) }, null);
    }

    private T InvokeBaseObjectConstructor<T>() {
      if (_baseObjectConstructor == null) {
        _baseObjectConstructor = GetBaseObjectConstructor();
      }
      return (T) _baseObjectConstructor.Invoke(new object[] { String.Empty });
    }

    private void LoadDataRules(DataColumnCollection columns) {
      if (_dataMappingRules == null) {
        lock (_lockObject) {
          if (_dataMappingRules == null) {
            _dataMappingRules = DataMappingRules.Parse(base.UnderlyingSystemType, columns);
          }
        }
      }
    }

    #endregion Private methods

  } // class ObjectTypeInfo

} // namespace Empiria.Ontology
