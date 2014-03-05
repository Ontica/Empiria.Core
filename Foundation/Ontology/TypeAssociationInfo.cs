/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : TypeAssociationInfo                              Pattern  : Standard class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Class that represents an ontology type association definition.                                *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    static internal TypeAssociationInfo Parse(MetaModelType sourceType, DataRow dataRow) {
      TypeAssociationInfo associationInfo = new TypeAssociationInfo(sourceType);

      associationInfo.LoadDataRow(dataRow);

      return associationInfo;
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

    internal T GetLink<T>(BaseObject source, T defaultValue = null) where T : BaseObject {
      DataRow row = OntologyData.GetObjectLinkDataRow(this, source);

      if (row != null) {
        return BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, row);
      } else {
        return defaultValue;
      }
    }

    internal T GetLink<T>(ObjectTypeInfo source, T defaultValue = null) where T : BaseObject {
      DataRow row = OntologyData.GetObjectLinkDataRow(this, source);

      if (row != null) {
        return BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, row);
      } else {
        return defaultValue;
      }
    }

    // Object 1..* Object relation
    internal ObjectList<T> GetLinks<T>(BaseObject source) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);
      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      try {
        for (int i = 0; i < table.Rows.Count; i++) {
          list.Add(BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, table.Rows[i]));
        }
      } catch (Exception e) {
        OntologyException exception = new OntologyException(OntologyException.Msg.WrongAssociatedObjectFound, e, source.Id, 
                                                            source.ObjectTypeInfo.Name, this.Name);

        throw exception;
      }
      return list;
    }

    // ObjectType 1..* Object relation
    internal ObjectList<T> GetLinks<T>(ObjectTypeInfo source) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, table.Rows[i]));
      }
      return list;
    }

    // Object 1..* Object relation (in time period)
    internal ObjectList<T> GetLinks<T>(BaseObject source, TimePeriod period) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source, period);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        list.Add(BaseObject.Parse<T>((ObjectTypeInfo) this.TargetType, table.Rows[i]));
      }
      return list;
    }

    // Object 1..* Object relation (filtered by predicate)
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

    // Object 1..* MetaModelType relation
    internal ObjectList<T> GetTypeLinks<T>(BaseObject source) where T : MetaModelType {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      ObjectList<T> list = new ObjectList<T>(table.Rows.Count);

      if (this.TargetType.TypeFamily == MetaModelTypeFamily.PowerType) {
        Type powerTypeSystemType = this.TargetType.UnderlyingSystemType;
        foreach (DataRow row in table.Rows) {
          T item = (T) ObjectFactory.ParseObject(powerTypeSystemType, (int) row[this.TargetType.IdFieldName]);
          list.Add(item);
        }
        return list;
      } else {
        foreach (DataRow row in table.Rows) {
          list.Add(MetaModelType.Parse<T>((int) row[this.TargetType.IdFieldName]));
        }
        return list;
      }
    }

    // MetaModelType -> MetaModelType relation
    internal ObjectList<T> GetTypeLinks<T>(MetaModelType source) where T : MetaModelType {
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

    internal ObjectList<TypeAssociationInfo> GetAssociationLinks(BaseObject source) {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      var list = new ObjectList<TypeAssociationInfo>(table.Rows.Count);
      foreach (DataRow dataRow in table.Rows) {
        list.Add(TypeAssociationInfo.Parse(this.SourceType, dataRow));
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

    protected override void LoadDataRow(DataRow row) {
      base.LoadDataRow(row);
      this.associationTypeId = (int) row["AssociationTypeId"];
      this.cardinality = (string) row["Cardinality"];
    }

    #endregion Public methods

  } // class TypeAssociationInfo

} // namespace Empiria.Ontology


