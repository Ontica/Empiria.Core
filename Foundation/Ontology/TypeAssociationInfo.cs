/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : TypeAssociationInfo                              Pattern  : Standard class                    *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Class that represents an ontology type association definition.                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;

using Empiria.Reflection;

namespace Empiria.Ontology {

  /// <summary>Class that represents an ontology type association definition.</summary>
  public class TypeAssociationInfo : TypeRelationInfo {

    #region Fields

    private int associationTypeId = 0;
    private string cardinality = String.Empty;

    private ObjectTypeInfo associationType = null;

    #endregion Fields

    #region Constructors and parsers

    protected TypeAssociationInfo(MetaModelType sourceType)
      : base(sourceType) {

    }

    static public TypeAssociationInfo Parse(int typeRelationId) {
      return TypeRelationInfo.Parse<TypeAssociationInfo>(typeRelationId);
    }

    static public TypeAssociationInfo Empty {
      get { return TypeRelationInfo.Parse<TypeAssociationInfo>(-1); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public ObjectTypeInfo AssociationType {
      get {
        if (associationType == null) {
          associationType = ObjectTypeInfo.Parse(associationTypeId);
        }
        return associationType;
      }
    }

    public string Cardinality {
      get { return cardinality; }
    }

    #endregion Public properties

    #region Public methods

    internal T GetLink<T>(BaseObject source) where T : BaseObject {
      DataRow dataRow = OntologyData.GetObjectLinkDataRow(this, source);

      return BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, dataRow);
    }

    internal ObjectList<T> GetLinks<T>(BaseObject source) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, table.Rows[i]));
      }
      return list;
    }

    internal ObjectList<T> GetLinks<T>(ObjectTypeInfo source) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, table.Rows[i]));
      }
      return list;
    }

    internal ObjectList<T> GetLinks<T>(BaseObject source, TimePeriod period) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source, period);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, table.Rows[i]));
      }
      return list;
    }

    internal ObjectList<T> GetLinks<T>(BaseObject source, Predicate<T> predicate) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        T item = BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, table.Rows[i]);
        if (predicate.Invoke(item)) {
          list.Add(item);
        }
      }
      return list;
    }

    internal ObjectList<T> GetTypeLinks<T>(BaseObject source) where T : MetaModelType {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      if (this.TargetType.TypeFamily == MetaModelTypeFamily.PowerType) {
        Type powerTypeSystemType = this.TargetType.UnderlyingSystemType;
        for (int i = 0; i < table.Rows.Count; i++) {
          T item = (T) ObjectFactory.ParseObject(powerTypeSystemType, (int) table.Rows[i][this.TargetType.IdFieldName]);
          list.Add(item);
        }
        return list;
      } else {
        for (int i = 0; i < table.Rows.Count; i++) {
          list.Add(MetaModelType.Parse<T>((int) table.Rows[i][this.TargetType.IdFieldName]));
        }
        return list;
      }
    }

    internal ObjectList<T> GetTypeRelationLinks<T>(BaseObject source) where T : TypeRelationInfo {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);


      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add((T) TypeRelationInfo.Parse(this.SourceType, table.Rows[i]));
      }
      return list;
    }

    protected override object ImplementsConvert(object value) {
      if (value == null) {
        return null;
      }
      if (value.GetType().Equals(typeof(string)) && String.IsNullOrWhiteSpace((string) value)) {
        return null;
      }
      return ObjectFactory.ParseObject(this.TargetType.UnderlyingSystemType, System.Convert.ToInt32(value));
    }

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.associationTypeId = (int) row["AssociationTypeId"];
      this.cardinality = (string) row["Cardinality"];
    }

    #endregion Public methods

  } // class TypeAssociationInfo

} // namespace Empiria.Ontology


