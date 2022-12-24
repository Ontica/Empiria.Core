/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : MetaModelType                                    Pattern  : Abstract Class With items Cache   *
*                                                                                                            *
*  Summary   : This type is the root of the item type metadata information hierarchy. All types information  *
*              classes must be descendants of this type.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;
using System.Linq;

using Empiria.Collections;
using Empiria.Json;
using Empiria.Reflection;
using Empiria.StateEnums;

namespace Empiria.Ontology {

  public enum MetaModelTypeFamily {

    FundamentalType,

    MetaModelType,

    ObjectType,

    PartitionedType,

    PowerType,

    ValueType,
  }


  public abstract class MetaModelType : IIdentifiable {

    #region Fields

    static private IdentifiablesCache<MetaModelType> cache = new IdentifiablesCache<MetaModelType>(256);

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
    private JsonObject extensionData = JsonObject.Empty;
    private string dataSource = String.Empty;
    private string idFieldName = String.Empty;
    private string typeIdFieldName = String.Empty;
    private string namedIdFieldName = String.Empty;
    private bool isAbstract = false;
    private bool isSealed = false;
    private bool isHistorizable = false;
    private EntityStatus status = EntityStatus.Active;

    private Type underlyingSystemType = null;

    private readonly object _locker = new object();

    private static readonly object _static_locker = new object();

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
      Assertion.Require(row, "row");

      MetaModelTypeFamily typeFamily = MetaModelType.ParseMetaModelTypeFamily((string) row["TypeFamily"]);

      MetaModelType instance = MetaModelType.CreateInstance(typeFamily, row);

      instance.LoadDataRow(row);

      cache.Insert(instance.Name, instance);

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
      lock (_static_locker) {
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
    }

    #endregion Constructors and parsers

    #region Public properties


    protected MetaModelType BaseType {
      get { return baseType; }
    }

    public string DataSource {
      get { return dataSource; }
      private set { dataSource = EmpiriaString.TrimAll(value); }
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

    public JsonObject ExtensionData {
      get { return extensionData; }
      protected set { extensionData = value; }
    }

    public bool FemaleGenre {
      get { return femaleGenre; }
      protected set { femaleGenre = value; }
    }

    public int Id {
      get { return id; }
    }

    public string UID {
      get {
        return this.Name;
      }
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


    public string Name {
      get { return name; }
      protected set { name = EmpiriaString.TrimAll(value); }
    }

    public string NamedKey {
      get {
        return this.ExtensionData.Get("NamedKey", String.Empty);
      }
    }

    public string NamedIdFieldName {
      get { return namedIdFieldName; }
      internal set { namedIdFieldName = EmpiriaString.TrimAll(value); }
    }

    protected EntityStatus Status {
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

    public string TypeIdFieldName {
      get { return typeIdFieldName; }
      internal set { typeIdFieldName = EmpiriaString.TrimAll(value); }
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
      cache.Insert(this.Name, MetaModelType.Parse(this.Id));
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
          var attribute = Attribute.GetCustomAttribute(type, typeof(PartitionedTypeAttribute)) as PartitionedTypeAttribute;

          if (attribute == null) {
            throw new OntologyException(OntologyException.Msg.PartitionedTypeAttributeMissed, type.FullName);
          }

          powerType = (Powertype) ObjectFactory.CreateObject(attribute.Powertype);
          powerType.typeFamily = MetaModelTypeFamily.PartitionedType;
          return powerType;

        case MetaModelTypeFamily.ValueType:
          return ObjectFactory.CreateObject<ValueTypeInfo>();

        default:
          throw Assertion.EnsureNoReachThisCode();
      }
    }

    static private Type GetType(DataRow dataRow) {
      string assembly = (string) dataRow["AssemblyName"];
      string className = (string) dataRow["ClassName"];

      return ObjectFactory.GetType(assembly, className);
    }


    private void LoadDataRow(DataRow dataRow) {
      this.id = (int) dataRow["TypeId"];

      AssertValidTypeFamily(ParseMetaModelTypeFamily((string) dataRow["TypeFamily"]));

      this.name = (string) dataRow["TypeName"];
      this.baseTypeId = (int) dataRow["BaseTypeId"];
      this.assemblyName = (string) dataRow["AssemblyName"];
      this.className = (string) dataRow["ClassName"];
      this.displayName = (string) dataRow["DisplayName"];
      this.displayPluralName = EmpiriaString.ToString(dataRow["DisplayPluralName"]);
      this.femaleGenre = Convert.ToBoolean(dataRow["FemaleGenre"]);
      this.documentation = EmpiriaString.ToString(dataRow["Documentation"]);
      this.extensionData = JsonObject.Parse(EmpiriaString.ToString(dataRow["TypeExtData"]));
      this.solutionName = EmpiriaString.ToString(dataRow["SolutionName"]);
      this.systemName = EmpiriaString.ToString(dataRow["SystemName"]);
      this.version = EmpiriaString.ToString(dataRow["Version"]);
      this.lastUpdate = (DateTime) dataRow["LastUpdate"];
      this.dataSource = EmpiriaString.ToString(dataRow["TypeDataSource"]);
      this.idFieldName = EmpiriaString.ToString(dataRow["IdFieldName"]);
      this.namedIdFieldName = EmpiriaString.ToString(dataRow["NamedIdFieldName"]);
      this.typeIdFieldName = EmpiriaString.ToString(dataRow["TypeIdFieldName"]);
      this.isAbstract = Convert.ToBoolean(dataRow["IsAbstract"]);
      this.isSealed = Convert.ToBoolean(dataRow["IsSealed"]);
      this.isHistorizable = Convert.ToBoolean(dataRow["IsHistorizable"]);
      this.status = (EntityStatus) char.Parse((string) dataRow["TypeStatus"]);
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
