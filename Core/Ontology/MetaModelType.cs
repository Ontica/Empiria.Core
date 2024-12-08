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


  public abstract class MetaModelType : IIdentifiable, INamedEntity {

    #region Fields

    static readonly private IdentifiablesCache<MetaModelType> _cache = new IdentifiablesCache<MetaModelType>(256);

    private Type _underlyingSystemType = null;

    private static readonly object _static_locker = new object();

    #endregion Fields

    #region Constructors and parsers

    protected internal MetaModelType(MetaModelTypeFamily typeFamily) {
      this.TypeFamily = typeFamily;
    }

    static internal MetaModelType Parse(int typeId) {
      var value = _cache.TryGetValue(typeId);
      if (value != null) {
        return value;
      } else {
        return MetaModelType.Parse(OntologyData.GetTypeDataRow(typeId));
      }
    }

    static internal MetaModelType Parse(string empiriaTypeName) {
      var value = _cache.TryGetValue(empiriaTypeName);
      if (value != null) {
        return value;
      } else {
        return MetaModelType.Parse(OntologyData.GetTypeDataRow(empiriaTypeName));
      }
    }

    static protected MetaModelType Parse(DataRow row) {
      Assertion.Require(row, nameof(row));

      MetaModelTypeFamily typeFamily = MetaModelType.ParseMetaModelTypeFamily((string) row["TypeFamily"]);

      MetaModelType instance = MetaModelType.CreateInstance(typeFamily, row);

      instance.LoadDataRow(row);

      _cache.Insert(instance.Name, instance);

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
      Assertion.Require(empiriaTypeName, nameof(empiriaTypeName));

      if (_cache.ContainsKey(empiriaTypeName)) {
        return (T) _cache[empiriaTypeName];
      } else {
        return (T) MetaModelType.Parse(OntologyData.GetTypeDataRow(empiriaTypeName));
      }
    }

    static internal Type TryGetSystemType(string systemTypeName) {
      lock (_static_locker) {
        MetaModelType empiriaType = _cache.FirstOrDefault((x) => x.ClassName == systemTypeName);
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
      get;
      private set;
    }

    public string DataSource {
      get;
      private set;
    }

    string INamedEntity.Name {
      get {
        return DisplayName;
      }
    }

    public string DisplayName {
      get;
      protected set;
    }

    public string DisplayPluralName {
      get;
      protected set;
    }

    public string Documentation {
      get;
      protected set;
    }

    public JsonObject ExtensionData {
      get;
      protected set;
    }

    public bool FemaleGenre {
      get;
      protected set;
    }

    public int Id {
      get; private set;
    }

    public string UID {
      get {
        return this.Name;
      }
    }

    internal string IdFieldName {
      get;
      set;
    }

    public bool IsAbstract {
      get;
      protected set;
    }

    public bool IsHistorizable {
      get;
      protected set;
    }

    public bool IsPrimitive {
      get { return (this.Id == this.BaseTypeId); }
    }

    public bool IsSealed {
      get;
      protected set;
    }


    public string Name {
      get;
      protected set;
    }

    public int BaseTypeId {
      get;
      private set;
    }

    public string AssemblyName {
      get;
      private set;
    }

    public string ClassName {
      get;
      private set;
    }

    public string NamedKey {
      get {
        return this.ExtensionData.Get("NamedKey", String.Empty);
      }
    }


    public string NamedIdFieldName {
      get;
      internal set;
    }

    protected EntityStatus Status {
      get;
      set;
    }

    public bool StoreInstancesInCache {
      get {
        return this.ExtensionData.Get<bool>("storeInstancesInCache", true);
      }
    }

    public string SolutionName {
      get;
      protected set;
    }

    public string SystemName {
      get;
      protected set;
    }

    public Type UnderlyingSystemType {
      get {
        if (_underlyingSystemType == null) {
          try {
            _underlyingSystemType = ObjectFactory.GetType(this.AssemblyName, this.ClassName);
          } catch (Exception e) {
            throw new OntologyException(OntologyException.Msg.CannotGetUnderlyingSystemType, e,
                                        this.Id, this.Name, this.AssemblyName, this.ClassName);
          }
        }
        return _underlyingSystemType;
      }
    }

    public bool UsesNamedKey {
      get { return (this.NamedIdFieldName.Length != 0); }
    }

    public string Version {
      get;
      protected set;
    }

    protected DateTime LastUpdate {
      get;
      set;
    }

    public MetaModelTypeFamily TypeFamily {
      get; private set;
    }

    public string TypeIdFieldName {
      get;
      internal set;
    }

    #endregion Public properties

    #region Public methods

    public override bool Equals(object obj) => this.Equals(obj as MetaModelType);

    public bool Equals(MetaModelType obj) {
      if (obj == null) {
        return false;
      }
      if (Object.ReferenceEquals(this, obj)) {
        return true;
      }
      if (this.GetType() != obj.GetType()) {
        return false;
      }
      return (this.Id == obj.Id);
    }

    public override int GetHashCode() {
      return (this.Id).GetHashCode();
    }

    protected internal void Reload() {
      _cache.Remove(this.Name);
      _cache.Insert(this.Name, MetaModelType.Parse(this.Id));
    }

    public override string ToString() {
      return this.Name;
    }

    #endregion Public methods

    #region Private methods

    private void AssertValidTypeFamily(MetaModelTypeFamily parametrizedTypeFamily) {
      if (this.TypeFamily == MetaModelTypeFamily.ObjectType &&
          (parametrizedTypeFamily == MetaModelTypeFamily.PartitionedType ||
           parametrizedTypeFamily == MetaModelTypeFamily.PowerType)) {
        return;
      }
      if (this.TypeFamily != parametrizedTypeFamily) {
        throw new OntologyException(OntologyException.Msg.TypeInfoFamilyNotMatch,
                                    this.Id, this.TypeFamily.ToString());
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
          powerType.TypeFamily = MetaModelTypeFamily.PowerType;
          return powerType;

        case MetaModelTypeFamily.PartitionedType:
          type = MetaModelType.GetType(dataRow);

          /// Partitioned types return the powertype instance defined with their PartitionedTypeAttribute.
          var attribute = Attribute.GetCustomAttribute(type, typeof(PartitionedTypeAttribute)) as PartitionedTypeAttribute;

          if (attribute == null) {
            throw new OntologyException(OntologyException.Msg.PartitionedTypeAttributeMissed, type.FullName);
          }

          powerType = (Powertype) ObjectFactory.CreateObject(attribute.Powertype);
          powerType.TypeFamily = MetaModelTypeFamily.PartitionedType;
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
      this.Id = (int) dataRow["TypeId"];

      AssertValidTypeFamily(ParseMetaModelTypeFamily((string) dataRow["TypeFamily"]));

      this.Name               = (string) dataRow["TypeName"];
      this.BaseTypeId         = (int) dataRow["BaseTypeId"];
      this.AssemblyName       = (string) dataRow["AssemblyName"];
      this.ClassName          = (string) dataRow["ClassName"];
      this.DisplayName        = (string) dataRow["DisplayName"];
      this.DisplayPluralName  = EmpiriaString.ToString(dataRow["DisplayPluralName"]);
      this.FemaleGenre        = Convert.ToBoolean(dataRow["FemaleGenre"]);
      this.Documentation      = EmpiriaString.ToString(dataRow["Documentation"]);
      this.ExtensionData      = JsonObject.Parse(EmpiriaString.ToString(dataRow["TypeExtData"]));
      this.SolutionName       = EmpiriaString.ToString(dataRow["SolutionName"]);
      this.SystemName         = EmpiriaString.ToString(dataRow["SystemName"]);
      this.Version            = EmpiriaString.ToString(dataRow["Version"]);
      this.LastUpdate         = (DateTime) dataRow["LastUpdate"];
      this.DataSource         = EmpiriaString.ToString(dataRow["TypeDataSource"]);
      this.IdFieldName        = EmpiriaString.ToString(dataRow["IdFieldName"]);
      this.NamedIdFieldName   = EmpiriaString.ToString(dataRow["NamedIdFieldName"]);
      this.TypeIdFieldName    = EmpiriaString.ToString(dataRow["TypeIdFieldName"]);
      this.IsAbstract         = Convert.ToBoolean(dataRow["IsAbstract"]);
      this.IsSealed           = Convert.ToBoolean(dataRow["IsSealed"]);
      this.IsHistorizable     = Convert.ToBoolean(dataRow["IsHistorizable"]);
      this.Status             = (EntityStatus) char.Parse((string) dataRow["TypeStatus"]);
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
        BaseType = this;
      } else if (_cache.ContainsId(this.BaseTypeId)) {
        BaseType = _cache[this.BaseTypeId];
      } else {
        BaseType = MetaModelType.Parse(this.BaseTypeId);
      }
    }

    #endregion Private methods

  } // class MetaModelType

} // namespace Empiria.Ontology
