/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : TypeAssociationInfo                              Pattern  : Standard class                    *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Class that represents an ontology type association definition.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
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

    protected TypeAssociationInfo(MetaModelType sourceType) : base(sourceType) {

    }

    static public TypeAssociationInfo Parse(int typeRelationId) {
      return TypeRelationInfo.Parse<TypeAssociationInfo>(typeRelationId);
    }

    static public TypeAssociationInfo Parse(string typeRelationName) {
      return TypeRelationInfo.Parse<TypeAssociationInfo>(typeRelationName);
    }

    static internal TypeAssociationInfo Parse(MetaModelType sourceType, DataRow dataRow) {
      TypeAssociationInfo associationInfo = new TypeAssociationInfo(sourceType);

      associationInfo.LoadDataRow(dataRow);

      return associationInfo;
    }

    static private readonly TypeAssociationInfo _empty = 
                            TypeRelationInfo.Parse<TypeAssociationInfo>(ObjectTypeInfo.EmptyInstanceId);
    static public TypeAssociationInfo Empty {
      get {
        return _empty;
      }
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

    public FixedList<T> GetInverseLinks<T>(BaseObject target) where T : BaseObject {
      throw new NotImplementedException();
    }

    public FixedList<T> GetInverseLinks<T>(BaseObject target, Predicate<T> predicate) where T : BaseObject {
      throw new NotImplementedException();
    }

    public FixedList<T> GetInverseLinks<T>(BaseObject target, TimePeriod period) where T : BaseObject {
      throw new NotImplementedException();
    }

    public FixedList<T> GetInverseLinks<T>(BaseObject target, Comparison<T> sort) where T : BaseObject {
      DataTable table = OntologyData.GetInverseObjectLinksTable(this, target);

      List<T> list = BaseObject.ParseList<T>(table);

      return list.ToFixedList();
    }

    public FixedList<T> GetInverseLinks<T>(BaseObject target, 
                                           Predicate<T> predicate, Comparison<T> sort) where T : BaseObject {
      throw new NotImplementedException();
    }

    public FixedList<T> GetInverseLinks<T>(BaseObject target, 
                                           TimePeriod period, Comparison<T> sort) where T : BaseObject {
      throw new NotImplementedException();
    }

    internal T GetInverseLink<T>(BaseObject target) where T : BaseObject {
      DataRow row = OntologyData.GetObjectLinkDataRow(this, target);

      if (row != null) {
        return BaseObject.ParseDataRow<T>(row);
      } else {
        throw new OntologyException(OntologyException.Msg.LinkNotFoundForTarget, target.Id, 
                                    target.GetEmpiriaType().Name, this.Id, this.Name);
      }
    }

    internal T GetInverseLink<T>(BaseObject source, T defaultValue) where T : BaseObject {
      DataRow row = OntologyData.GetObjectLinkDataRow(this, source);

      if (row != null) {
        return BaseObject.ParseDataRow<T>(row);
      } else {
        return defaultValue;
      }
    }

    internal T GetLink<T>(BaseObject source) where T : BaseObject {
      DataRow row = OntologyData.GetObjectLinkDataRow(this, source);

      if (row != null) {
        return BaseObject.ParseDataRow<T>(row);
      } else {
        throw new OntologyException(OntologyException.Msg.LinkNotFoundForSource, source.Id,
                                    source.GetEmpiriaType().Name, this.Id, this.Name);
      }
    }

    internal T GetLink<T>(BaseObject source, T defaultValue) where T : BaseObject {
      DataRow row = OntologyData.GetObjectLinkDataRow(this, source);

      if (row != null) {
        return BaseObject.ParseDataRow<T>(row);
      } else {
        return defaultValue;
      }
    }

    // Object 1..* Object relation
    internal FixedList<T> GetLinks<T>(BaseObject source) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);
      
      List<T> list = BaseObject.ParseList<T>(table);

      return list.ToFixedList();
    }

    // Object 1..* Object relation (in time period)
    internal FixedList<T> GetLinks<T>(BaseObject source, TimePeriod period) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source, period);

      List<T> list = BaseObject.ParseList<T>(table);

      return list.ToFixedList();
    }

    // Object 1..* Object relation (filtered by predicate)
    internal FixedList<T> GetLinks<T>(BaseObject source, Predicate<T> predicate) where T : BaseObject {
      DataTable table = OntologyData.GetObjectLinksTable(this, source);

      List<T> list = new List<T>(table.Rows.Count);

      for (int i = 0; i < table.Rows.Count; i++) {
        T item = BaseObject.ParseDataRow<T>(table.Rows[i]);
        if (predicate.Invoke(item)) {
          list.Add(item);
        }
      }
      return list.ToFixedList();
    }

    protected override object ImplementsConvert(object value) {
      if (value == null) {
        return null;
      }
      if (value.GetType().Equals(typeof(string)) && String.IsNullOrWhiteSpace((string) value)) {
        return null;
      }
      return ObjectFactory.InvokeParseMethod(this.TargetType.UnderlyingSystemType,
                                             System.Convert.ToInt32(value));
    }

    protected override void LoadDataRow(DataRow row) {
      base.LoadDataRow(row);
      this.associationTypeId = (int) row["AssociationTypeId"];
      this.cardinality = (string) row["Cardinality"];
    }

    #endregion Public methods

  } // class TypeAssociationInfo

} // namespace Empiria.Ontology


