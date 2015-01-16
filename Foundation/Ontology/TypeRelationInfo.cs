/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : TypeRelationInfo                                 Pattern  : Standard class                    *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Sealed class that represents an ontology type relation definition.                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Reflection;
using Empiria.Security;

namespace Empiria.Ontology {

  public enum RelationTypeFamily {
    Attribute = 1,
    Association = 2,
    Aggregation = 3,
    Composition = 4,
    Classification = 5,
  }

  /// <summary>Sealed class that represents an ontology type relation definition.</summary>
  public abstract class TypeRelationInfo : IStorable {

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
    private GeneralObjectStatus status = GeneralObjectStatus.Active;

    #endregion Fields

    #region Constructors and parsers

    protected TypeRelationInfo(MetaModelType sourceType) {
      this.sourceType = sourceType;
    }

    static internal T Parse<T>(int typeRelationId) where T : TypeRelationInfo {
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

    static internal T Parse<T>(string typeRelationName) where T : TypeRelationInfo {
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

    internal string DataSource {
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

    public GeneralObjectStatus Status {
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
      this.status = (GeneralObjectStatus) char.Parse((string) row["TypeRelationStatus"]);
      isInherited = (sourceType.Id != (int) row["SourceTypeId"]);

      //defaultValue = this.Convert(row["DefaultValue"]);

      //this.isSealed = (bool) row["IsSealed"];
      //this.isRuleBased = (bool) row["IsRuleBased"];
      //this.isReadOnly = (bool) row["IsReadOnly"];
      //this.isHistorizable = (bool) row["IsHistorizable"];
      //this.isKeyword = (bool) row["IsKeyword"];
      //this.protectionMode = (EncryptionMode) row["ProtectionMode"];
    }

    #endregion Private methods

  } // class TypeRelationInfo

} // namespace Empiria.Ontology
