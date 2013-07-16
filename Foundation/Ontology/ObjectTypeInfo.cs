/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : ObjectTypeInfo                                   Pattern  : Type metadata class               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents an object type definition.                                                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
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

    #endregion Constructors and parsers

    #region Public properties

    public new ObjectTypeInfo BaseType {
      get { return (ObjectTypeInfo) base.BaseType; }
    }

    #endregion Public properties

    #region Public methods

    public TypeAssociationInfo GetAssociationInfo(int id) {
      return base.GetRelationInfo<TypeAssociationInfo>(id);
    }

    public TypeAssociationInfo GetAssociationInfo(string name) {
      return base.GetRelationInfo<TypeAssociationInfo>(name);
    }

    public TypeAttributeInfo GetAttributeInfo(int id) {
      return base.GetRelationInfo<TypeAttributeInfo>(id);
    }

    public TypeAttributeInfo GetAttributeInfo(string name) {
      return base.GetRelationInfo<TypeAttributeInfo>(name);
    }

    public TypeMethodInfo[] GetMethods() {
      TypeMethodInfo[] array = new TypeMethodInfo[base.Methods.Count];

      base.Methods.Values.CopyTo(array, 0);

      return array;
    }

    public TypeRelationInfo[] GetRelations() {
      TypeRelationInfo[] array = new TypeRelationInfo[base.Relations.Count];

      base.Relations.Values.CopyTo(array, 0);

      return array;
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
      ObjectTypeInfo typeIterator = (ObjectTypeInfo) this.BaseType;
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