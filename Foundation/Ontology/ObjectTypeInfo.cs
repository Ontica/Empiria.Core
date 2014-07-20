/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : ObjectTypeInfo                                   Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents an object type definition.                                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System.Collections.Generic;
using System.Data;

namespace Empiria.Ontology {

  public class ObjectTypeInfo : MetaModelType {

    #region Constructors and parsers

    protected internal ObjectTypeInfo(int id)
      : base(MetaModelTypeFamily.ObjectType, id) {

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

  } // class ObjectTypeInfo

} // namespace Empiria.Ontology
