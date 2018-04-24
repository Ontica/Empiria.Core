/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : TypeRelationInfo                                 Pattern  : Standard class                    *
*                                                                                                            *
*  Summary   : Sealed class that represents an ontology type relation definition.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Security;
using Empiria.StateEnums;

namespace Empiria.Ontology {

  public enum RelationTypeFamily {
    Attribute = 1,
    Association = 2,
    Aggregation = 3,
    Composition = 4,
    Classification = 5,
  }

  /// <summary>Sealed class that represents an ontology type relation definition.</summary>
  public abstract class TypeRelationInfo : IIdentifiable {

    #region Abstract members

    protected abstract object ImplementsConvert(object value);

    #endregion Abstract members

    #region Fields

    private int id = 0;
    private MetaModelType sourceType = null;
    private MetaModelType targetType = null;
    private RelationTypeFamily relationTypeFamily = RelationTypeFamily.Attribute;
    private string name = String.Empty;
    private string displayName = String.Empty;
    private string documentation = String.Empty;
    private string keywords = String.Empty;
    private object defaultValue = null;
    private bool isSealed = false;
    private bool isRuleBased = false;
    private bool isReadOnly = false;
    private bool isHistorizable = false;
    private bool isKeyword = false;
    private EncryptionMode protectionMode = EncryptionMode.Unprotected;
    private string dataSource = String.Empty;
    private string sourceIdFieldName = String.Empty;
    private string targetIdFieldName = String.Empty;
    private string typeRelationIdFieldName = String.Empty;
    private bool isInherited = false;
    private EntityStatus status = EntityStatus.Active;

    #endregion Fields

    #region Constructors and parsers

    protected TypeRelationInfo(MetaModelType sourceType) {
      this.sourceType = sourceType;
    }

    static public T Parse<T>(int typeRelationId) where T : TypeRelationInfo {
      DataRow dataRow = OntologyData.GetTypeRelation(typeRelationId);
      MetaModelType sourceType = MetaModelType.Parse((int) dataRow["SourceTypeId"]);
      var relationTypeFamily =
                        TypeRelationInfo.ParseRelationTypeFamily((string) dataRow["RelationTypeFamily"]);
      if (relationTypeFamily == RelationTypeFamily.Attribute) {
        return TypeAttributeInfo.Parse(sourceType, dataRow) as T;
      } else {
        return TypeAssociationInfo.Parse(sourceType, dataRow) as T;
      }
    }

    static public T Parse<T>(string typeRelationName) where T : TypeRelationInfo {
      DataRow dataRow = OntologyData.GetTypeRelation(typeRelationName);
      MetaModelType sourceType = MetaModelType.Parse((int) dataRow["SourceTypeId"]);
      var relationTypeFamily =
                        TypeRelationInfo.ParseRelationTypeFamily((string) dataRow["RelationTypeFamily"]);
      if (relationTypeFamily == RelationTypeFamily.Attribute) {
        return TypeAttributeInfo.Parse(sourceType, dataRow) as T;
      } else {
        return TypeAssociationInfo.Parse(sourceType, dataRow) as T;
      }
    }

    static internal RelationTypeFamily ParseRelationTypeFamily(string familyName) {
      try {
        return (RelationTypeFamily) Enum.Parse(typeof(RelationTypeFamily), familyName);
      } catch {
        throw new OntologyException(OntologyException.Msg.UndefinedTypeInfoFamily, familyName);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string DataSource {
      get { return dataSource; }
    }

    public string DisplayName {
      get { return displayName; }
    }

    public string Documentation {
      get { return documentation; }
    }

    internal object DefaultValue {
      get { return defaultValue; }
    }

    public int Id {
      get { return id; }
    }

    public bool IsHistorizable {
      get { return isHistorizable; }
    }

    public bool IsInherited {
      get { return isInherited; }
    }

    public bool IsKeyword {
      get { return isKeyword; }
    }

    public bool IsReadOnly {
      get { return isReadOnly; }
    }

    public bool IsRuleBased {
      get { return isRuleBased; }
    }

    public bool IsSealed {
      get { return isSealed; }
    }

    internal string Keywords {
      get { return keywords; }
    }

    public string Name {
      get { return name; }
    }

    public EncryptionMode ProtectionMode {
      get { return protectionMode; }
    }

    public RelationTypeFamily RelationTypeFamily {
      get { return relationTypeFamily; }
    }

    public EntityStatus Status {
      get { return status; }
      set { status = value; }
    }

    internal string SourceIdFieldName {
      get { return sourceIdFieldName; }
    }

    public MetaModelType SourceType {
      get { return sourceType; }
    }

    public MetaModelType TargetType {
      get { return targetType; }
    }

    internal string TargetIdFieldName {
      get { return targetIdFieldName; }
    }

    internal string TypeRelationIdFieldName {
      get { return typeRelationIdFieldName; }
    }

    #endregion Public properties

    #region Public methods

    internal object Convert(object value) {
      try {
        return ImplementsConvert(value);
      } catch (Exception exception) {
        throw new OntologyException(OntologyException.Msg.ConvertionToTargetTypeFails,
                                    exception, value, this.TargetType.Name);
      }
    }

    internal object GetDefaultValue() {
      return this.DefaultValue;
    }

    #endregion Public methods

    #region Private methods

    protected virtual void LoadDataRow(DataRow row) {
      this.id = (int) row["TypeRelationId"];
      this.targetType = MetaModelType.Parse((int) row["TargetTypeId"]);
      this.name = (string) row["RelationName"];
      this.displayName = (string) row["DisplayName"];
      this.documentation = (string) row["Documentation"];
      this.keywords = (string) row["TypeRelationKeywords"];
      this.dataSource = (string) row["TypeRelationDataSource"];
      this.sourceIdFieldName = (string) row["SourceIdFieldName"];
      this.targetIdFieldName = (string) row["TargetIdFieldName"];
      this.typeRelationIdFieldName = (string) row["TypeRelationIdFieldName"];
      this.status = (EntityStatus) char.Parse((string) row["TypeRelationStatus"]);
      isInherited = (sourceType.Id != (int) row["SourceTypeId"]);
    }

    #endregion Private methods

  } // class TypeRelationInfo

} // namespace Empiria.Ontology
