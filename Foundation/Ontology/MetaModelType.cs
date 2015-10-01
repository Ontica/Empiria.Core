/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : MetaModelType                                    Pattern  : Abstract Class With items Cache   *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : This type is the root of the item type metadata information hierarchy. All types information  *
*              classes must be descendants of this type.                                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Empiria.Collections;
using Empiria.Data;
using Empiria.Reflection;

namespace Empiria.Ontology {

  public enum MetaModelTypeFamily {
    FundamentalType,
    InterfaceType,
    MetaModelType,
    MethodType,
    ObjectType,
    PartitionedType,
    PowerType,
    RelationType,
    RuleType,
    StaticType,
    ValueType,
  }

  public abstract class MetaModelType : IStorable {

    #region Fields

    static private DoubleKeyList<MetaModelType> cache = new DoubleKeyList<MetaModelType>();

    private int id = 0;
    private string name = String.Empty;
    private int baseTypeId = 0;
    private MetaModelType baseType = null;
    private MetaModelTypeFamily typeFamily = MetaModelTypeFamily.ObjectType;
    private string assemblyName = String.Empty;
    private string className = String.Empty;
    private string solutionName = String.Empty;
    private string systemName = String.Empty;
    private string version = String.Empty;
    private DateTime lastUpdate = DateTime.Now;
    private string displayName = String.Empty;
    private string displayPluralName = String.Empty;
    private bool femaleGenre = false;
    private string documentation = String.Empty;
    private string extensionData = String.Empty;
    private string keywords = String.Empty;
    private string dataSource = String.Empty;
    private string idFieldName = String.Empty;
    private string typeIdFieldName = String.Empty;
    private string namedIdFieldName = String.Empty;
    private bool isAbstract = false;
    private bool isSealed = false;
    private bool isHistorizable = false;
    private GeneralObjectStatus status = GeneralObjectStatus.Active;

    private Type underlyingSystemType = null;
    private DoubleKeyList<TypeAssociationInfo> associationInfoList = null;
    private TypeMethodInfo[] methodsArray = null;

    #endregion Fields

    #region Constructors and parsers

    protected internal MetaModelType(MetaModelTypeFamily typeFamily) {
      this.typeFamily = typeFamily;
    }

    static internal MetaModelType Parse(int typeId) {
      var value = cache.TryGetValue(typeId);
      if (value != null) {
        return value;
      } else {
        return MetaModelType.Parse(OntologyData.GetTypeDataRow(typeId));
      }
    }

    static internal MetaModelType Parse(string empiriaTypeName) {
      var value = cache.TryGetValue(empiriaTypeName);
      if (value != null) {
        return value;
      } else {
        return MetaModelType.Parse(OntologyData.GetTypeDataRow(empiriaTypeName));
      }
    }

    static protected MetaModelType Parse(DataRow row) {
      MetaModelTypeFamily typeFamily = MetaModelType.ParseMetaModelTypeFamily((string) row["TypeFamily"]);

      MetaModelType instance = MetaModelType.CreateInstance(typeFamily, row);
      instance.LoadDataRow(row);

      //Load instance into cache
      if (!cache.ContainsId(instance.Id)) {
        cache.Add(instance.Name, instance);
      } else {
        cache[instance.Name] = instance;
      }
      instance.SetBaseType();

      return instance;
    }

    protected static T Parse<T>(Type type) where T : MetaModelType {
      DataRow dataRow = OntologyData.GetBaseObjectTypeInfoDataRowWithType(type);

      return (T) MetaModelType.Parse(dataRow);
    }

    static internal T Parse<T>(int typeId) where T : MetaModelType {
      return (T) MetaModelType.Parse(typeId);
    }

    static internal T Parse<T>(string empiriaTypeName) where T : MetaModelType {
      if (cache.ContainsKey(empiriaTypeName)) {
        return (T) cache[empiriaTypeName];
      } else {
        return (T) MetaModelType.Parse(OntologyData.GetTypeDataRow(empiriaTypeName));
      }
    }

    static internal Type TryGetSystemType(string systemTypeName) {
      MetaModelType empiriaType = cache.FirstOrDefault((x) => x.className == systemTypeName);
      if (empiriaType != null) {
        return empiriaType.UnderlyingSystemType;
      }
      DataRow dataRow = OntologyData.TryGetSystemTypeDataRow(systemTypeName);
      if (dataRow != null) {
        empiriaType = MetaModelType.Parse(dataRow);
      }
      if (empiriaType != null) {
        return empiriaType.UnderlyingSystemType;
      } else {
        return null;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    protected internal DoubleKeyList<TypeAssociationInfo> Associations {
      get {
        if (associationInfoList == null) {
          lock (this) {
            LoadAssociations();
          }
        }
        return associationInfoList;
      }
    }

    protected MetaModelType BaseType {
      get { return baseType; }
    }

    internal string DataSource {
      get { return dataSource; }
      set { dataSource = EmpiriaString.TrimAll(value); }
    }

    public string DisplayName {
      get { return displayName; }
      protected set { displayName = EmpiriaString.TrimAll(value); }
    }

    public string DisplayPluralName {
      get { return displayPluralName; }
      protected set { displayPluralName = EmpiriaString.TrimAll(value); }
    }

    public string Documentation {
      get { return documentation; }
      protected set { documentation = EmpiriaString.TrimAll(value); }
    }

    public string ExtensionData {
      get { return extensionData; }
      protected set { extensionData = EmpiriaString.TrimAll(value); }
    }

    public bool FemaleGenre {
      get { return femaleGenre; }
      protected set { femaleGenre = value; }
    }

    public int Id {
      get { return id; }
    }

    internal string IdFieldName {
      get { return idFieldName; }
      set { idFieldName = EmpiriaString.TrimAll(value); }
    }

    public bool IsAbstract {
      get { return isAbstract; }
      protected set { isAbstract = value; }
    }

    public bool IsHistorizable {
      get { return isHistorizable; }
      protected set { isHistorizable = value; }
    }

    public bool IsPrimitive {
      get { return (this.id == this.baseTypeId); }
    }

    public bool IsSealed {
      get { return isSealed; }
      protected set { isSealed = value; }
    }

    protected TypeMethodInfo[] Methods {
      get {
        if (methodsArray == null) {
          lock (this) {
            LoadMethods();
          }
        }
        return methodsArray;
      }
    }

    public string Name {
      get { return name; }
      protected set { name = EmpiriaString.TrimAll(value); }
    }

    internal string NamedIdFieldName {
      get { return namedIdFieldName; }
      set { namedIdFieldName = EmpiriaString.TrimAll(value); }
    }

    protected GeneralObjectStatus Status {
      get { return status; }
      set { status = value; }
    }

    public string SolutionName {
      get { return solutionName; }
      protected set { solutionName = EmpiriaString.TrimAll(value); }
    }

    public string SystemName {
      get { return systemName; }
      protected set { systemName = EmpiriaString.TrimAll(value); }
    }

    public Type UnderlyingSystemType {
      get {
        if (underlyingSystemType == null) {
          try {
            underlyingSystemType = ObjectFactory.GetType(this.assemblyName, this.className);
          } catch (Exception e) {
            throw new OntologyException(OntologyException.Msg.CannotGetUnderlyingSystemType, e,
                                        this.Id, this.Name, this.assemblyName, this.className);
          }
        }
        return underlyingSystemType;
      }
    }

    public bool UsesNamedKey {
      get { return (this.namedIdFieldName.Length != 0); }
    }

    public string Version {
      get { return version; }
      protected set { version = EmpiriaString.TrimAll(value); }
    }

    protected DateTime LastUpdate {
      get { return lastUpdate; }
      set { lastUpdate = value; }
    }

    public MetaModelTypeFamily TypeFamily {
      get { return typeFamily; }
    }

    internal string TypeIdFieldName {
      get { return typeIdFieldName; }
      set { typeIdFieldName = EmpiriaString.TrimAll(value); }
    }

    #endregion Public properties

    #region Public methods

    public override bool Equals(object obj) {
      if (obj == null || GetType() != obj.GetType()) {
        return false;
      }
      return base.Equals(obj) && (this.Id == ((MetaModelType) obj).Id);
    }

    public bool Equals(MetaModelType obj) {
      if (obj == null) {
        return false;
      }
      return (this.Id == obj.Id);
    }

    public override int GetHashCode() {
      return this.Id;
    }

    protected internal void Reload() {
      cache.Remove(this.Name);
      cache.Add(this.Name, MetaModelType.Parse(this.Id));
    }

    public override string ToString() {
      return this.name;
    }

    #endregion Public methods

    #region Private methods

    private void AssertValidTypeFamily(MetaModelTypeFamily parametrizedTypeFamily) {
      if (this.typeFamily == MetaModelTypeFamily.ObjectType &&
          (parametrizedTypeFamily == MetaModelTypeFamily.PartitionedType ||
           parametrizedTypeFamily == MetaModelTypeFamily.PowerType)) {
        return;
      }
      if (this.typeFamily != parametrizedTypeFamily) {
        throw new OntologyException(OntologyException.Msg.TypeInfoFamilyNotMatch,
                                    this.Id, this.typeFamily.ToString());
      }
    }

    /// <summary> Factory method to create MetaModelType type instances.</summary>
    static private MetaModelType CreateInstance(MetaModelTypeFamily typeFamily,
                                                DataRow dataRow) {
      switch (typeFamily) {
        case MetaModelTypeFamily.MetaModelType:
          return ObjectFactory.CreateObject<MetaModelTypeInfo>();

        case MetaModelTypeFamily.ObjectType:
          return ObjectFactory.CreateObject<ObjectTypeInfo>();

        case MetaModelTypeFamily.FundamentalType:
          return ObjectFactory.CreateObject<FundamentalTypeInfo>();

        case MetaModelTypeFamily.PowerType:
          Type type = MetaModelType.GetType(dataRow);

          var powerType = (Powertype) ObjectFactory.CreateObject(type);
          powerType.typeFamily = MetaModelTypeFamily.PowerType;
          return powerType;

        case MetaModelTypeFamily.PartitionedType:
          type = MetaModelType.GetType(dataRow);
          /// Partitioned types return the powertype instance defined with their PartitionedTypeAttribute.
          var attribute = (PartitionedTypeAttribute)
                           Attribute.GetCustomAttribute(type, typeof(PartitionedTypeAttribute));

          powerType = (Powertype) ObjectFactory.CreateObject(attribute.Powertype);
          powerType.typeFamily = MetaModelTypeFamily.PartitionedType;
          return powerType;

        case MetaModelTypeFamily.ValueType:
          return ObjectFactory.CreateObject<ValueTypeInfo>();

        case MetaModelTypeFamily.RuleType:
          return ObjectFactory.CreateObject<RuleTypeInfo>();

        case MetaModelTypeFamily.StaticType:
          return ObjectFactory.CreateObject<StaticTypeInfo>();

        default:
          throw Assertion.AssertNoReachThisCode();
      }
    }

    static private Type GetType(DataRow dataRow) {
      string assembly = (string) dataRow["AssemblyName"];
      string className = (string) dataRow["ClassName"];

      return ObjectFactory.GetType(assembly, className);
    }

    private void LoadAssociations() {
      if (associationInfoList != null) {
        return;
      }

      DataTable dataTable = OntologyData.GetTypeRelations(this.Name);
      this.associationInfoList = new DoubleKeyList<TypeAssociationInfo>(0);
      foreach (DataRow dataRow in dataTable.Rows) {
        RelationTypeFamily family = TypeRelationInfo.ParseRelationTypeFamily((string) dataRow["RelationTypeFamily"]);
        TypeAssociationInfo associationInfo = TypeAssociationInfo.Parse(this, dataRow);
        this.associationInfoList.Add(associationInfo.Name, associationInfo);
      }
    }

    private void LoadDataRow(DataRow dataRow) {
      this.id = (int) dataRow["TypeId"];

      AssertValidTypeFamily(ParseMetaModelTypeFamily((string) dataRow["TypeFamily"]));

      this.name = (string) dataRow["TypeName"];
      this.baseTypeId = (int) dataRow["BaseTypeId"];
      this.assemblyName = (string) dataRow["AssemblyName"];
      this.className = (string) dataRow["ClassName"];
      this.displayName = (string) dataRow["DisplayName"];
      this.displayPluralName = (string) dataRow["DisplayPluralName"];
      this.femaleGenre = (bool) dataRow["FemaleGenre"];
      this.documentation = (string) dataRow["Documentation"];
      this.extensionData = (string) dataRow["TypeExtData"];
      this.keywords = (string) dataRow["TypeKeywords"];
      this.solutionName = (string) dataRow["SolutionName"];
      this.systemName = (string) dataRow["SystemName"];
      this.version = (string) dataRow["Version"];
      this.lastUpdate = (DateTime) dataRow["LastUpdate"];
      this.dataSource = (string) dataRow["TypeDataSource"];
      this.idFieldName = (string) dataRow["IdFieldName"];
      this.namedIdFieldName = (string) dataRow["NamedIdFieldName"];
      this.typeIdFieldName = (string) dataRow["TypeIdFieldName"];
      this.isAbstract = (bool) dataRow["IsAbstract"];
      this.isSealed = (bool) dataRow["IsSealed"];
      this.isHistorizable = (bool) dataRow["IsHistorizable"];
      this.status = (GeneralObjectStatus) char.Parse((string) dataRow["TypeStatus"]);
    }

    private void LoadMethods() {
      if (methodsArray != null) {
        return;
      }

      // There is no Methods table defined in this Empiria version 6.0,
      // so just return an empty array.Future versions should remove this line
      // and uncomment the code below.
      this.methodsArray = new TypeMethodInfo[0];

      //DataTable dataTable = OntologyData.GetTypeMethods(this.Id);

      //this.methodsArray = new TypeMethodInfo[dataTable.Rows.Count];
      //for (int i = 0, j = dataTable.Rows.Count; i < j; i++) {
      //  this.methodsArray[i] = TypeMethodInfo.Parse(this, dataTable.Rows[i]);
      //}
    }

    static private MetaModelTypeFamily ParseMetaModelTypeFamily(string typeFamilyName) {
      try {
        return (MetaModelTypeFamily) Enum.Parse(typeof(MetaModelTypeFamily), typeFamilyName);
      } catch {
        throw new OntologyException(OntologyException.Msg.UndefinedTypeInfoFamily, typeFamilyName);
      }
    }

    private void SetBaseType() {
      if (this.IsPrimitive) {
        baseType = this;
      } else if (cache.ContainsId(this.baseTypeId)) {
        baseType = cache[this.baseTypeId];
      } else {
        baseType = MetaModelType.Parse(this.baseTypeId);
      }
    }

    #endregion Private methods

  } // class MetaModelType

} // namespace Empiria.Ontology
