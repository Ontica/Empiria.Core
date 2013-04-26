/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : MetaModelType                                    Pattern  : Abstract Class With items Cache   *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This type is the root of the item type metadata information hierarchy. All types information  *
*              classes must be descendants of this type.                                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;
using Empiria.Collections;
using Empiria.Data;
using Empiria.Reflection;

namespace Empiria.Ontology {

  public enum MetaModelTypeFamily {
    MetaModelType = 1,
    ObjectType = 2,
    FundamentalType = 3,
    EnumerationType = 4,
    StructureType = 5,
    RelationType = 6,
    MethodType = 7,
    RuleType = 8,
    PowerType = 9,
    InterfaceType = 10,
    StaticType = 11,
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
    private string keywords = String.Empty;
    private string dataSource = String.Empty;
    private string idFieldName = String.Empty;
    private string typeIdFieldName = String.Empty;
    private string namedIdFieldName = String.Empty;
    private bool isAbstract = false;
    private bool isSealed = false;
    private bool isHistorizable = false;
    private int postedById = 0;
    private DateTime postingTime = DateTime.Now;
    private GeneralObjectStatus status = GeneralObjectStatus.Active;

    private Type underlyingSystemType = null;
    private DoubleKeyList<TypeRelationInfo> relationsList = null;
    private DoubleKeyList<TypeMethodInfo> methodsList = null;

    #endregion Fields

    #region Constructors and parsers

    protected internal MetaModelType(MetaModelTypeFamily typeFamily, int id) {
      this.typeFamily = typeFamily;
      if (id != 0) {
        Load(OntologyData.GetTypeDataRow(id), id.ToString());
      }
    }

    protected internal MetaModelType(MetaModelTypeFamily typeFamily, string typeName) {
      this.typeFamily = typeFamily;
      Load(OntologyData.GetTypeDataRow(typeName), typeName);
    }

    static internal MetaModelType Parse(int typeId) {
      if (cache.ContainsId(typeId)) {
        return cache[typeId];
      }
      return MetaModelType.Parse(OntologyData.GetTypeDataRow(typeId));
    }

    static internal MetaModelType Parse(string typeName) {
      if (cache.ContainsKey(typeName)) {
        return cache[typeName];
      }
      return MetaModelType.Parse(OntologyData.GetTypeDataRow(typeName));
    }

    static protected MetaModelType Parse(DataRow row) {
      MetaModelTypeFamily typeFamily = MetaModelType.ParseMetaModelTypeFamily((string) row["TypeFamily"]);

      MetaModelType instance = CreateInstance(typeFamily);
      instance.Load(row, ((int) row["TypeId"]).ToString());

      return instance;
    }

    static internal T Parse<T>(int typeId) where T : MetaModelType {
      return (T) MetaModelType.Parse(typeId);
    }

    static internal T Parse<T>(string typeName) where T : MetaModelType {
      if (cache.ContainsKey(typeName)) {
        return (T) cache[typeName];
      } else {
        return (T) MetaModelType.Parse(OntologyData.GetTypeDataRow(typeName));
      }
    }

    static private MetaModelType CreateInstance(MetaModelTypeFamily typeFamily) {
      //string typeInfoName = Enum.GetName(typeof(MetaModelTypeFamily), typeFamily) + "Info";

      //MetaModelTypeInfo metaModelTypeInfo = MetaModelTypeInfo.Parse(typeInfoName);

      //return metaModelTypeInfo.ParseInstance(id);



      Type[] parTypes = new Type[] { typeof(int) };
      object[] pars = new object[] { 0 };

      switch (typeFamily) {
        case MetaModelTypeFamily.MetaModelType:
          return ObjectFactory.CreateObject<MetaModelTypeInfo>(parTypes, pars);
        case MetaModelTypeFamily.ObjectType:
          return ObjectFactory.CreateObject<ObjectTypeInfo>(parTypes, pars);
        case MetaModelTypeFamily.FundamentalType:
          return ObjectFactory.CreateObject<FundamentalTypeInfo>(parTypes, pars);
        case MetaModelTypeFamily.StaticType:
          return ObjectFactory.CreateObject<StaticTypeInfo>(parTypes, pars);
        case MetaModelTypeFamily.PowerType:
          return ObjectFactory.CreateObject<PowerTypeInfo>(parTypes, pars);
        case MetaModelTypeFamily.EnumerationType:
          return ObjectFactory.CreateObject<EnumerationTypeInfo>(parTypes, pars);
        case MetaModelTypeFamily.StructureType:
          return ObjectFactory.CreateObject<StructureTypeInfo>(parTypes, pars);
        case MetaModelTypeFamily.RuleType:
          return ObjectFactory.CreateObject<RuleTypeInfo>(parTypes, pars);
        default:
          throw new OntologyException(OntologyException.Msg.TypeInfoNotFound, typeFamily.ToString());
      }
    }

    static internal MetaModelTypeFamily ParseMetaModelTypeFamily(string typeFamilyName) {
      try {
        return (MetaModelTypeFamily) System.Enum.Parse(typeof(MetaModelTypeFamily), typeFamilyName);
      } catch {
        throw new OntologyException(OntologyException.Msg.UndefinedTypeInfoFamily, typeFamilyName);
      }
    }

    static protected internal void ResetCache() {
      foreach (MetaModelType metaModelType in cache.Values) {
        metaModelType.Reload();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

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

    public bool FemaleGenre {
      get { return femaleGenre; }
      protected set { femaleGenre = value; }
    }

    protected DoubleKeyList<TypeMethodInfo> Methods {
      get {
        if (methodsList == null) {
          LoadMethods();
        }
        return methodsList;
      }
    }

    protected DoubleKeyList<TypeRelationInfo> Relations {
      get {
        if (relationsList == null) {
          LoadRelations();
        }
        return relationsList;
      }
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

    public string Name {
      get { return name; }
      protected set { name = EmpiriaString.TrimAll(value); }
    }

    internal string NamedIdFieldName {
      get { return namedIdFieldName; }
      set { namedIdFieldName = EmpiriaString.TrimAll(value); }
    }

    protected int PostedById {
      get { return postedById; }
      set { postedById = value; }
    }

    protected DateTime PostingTime {
      get { return postingTime; }
      set { postingTime = value; }
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
          underlyingSystemType = ObjectFactory.GetType(this.assemblyName, this.className);
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

    internal MetaModelTypeFamily TypeFamily {
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
      return (this.Id == obj.Id); // base.Equals(obj) && 
    }

    public override int GetHashCode() {
      return this.Id;
    }

    protected T GetRelation<T>(int id) where T : TypeRelationInfo {
      if (this.Relations.ContainsId(id)) {
        return (T) this.Relations[id];
      } else {
        throw new OntologyException(OntologyException.Msg.TypeRelationInfoNotFound, id, this.Name);
      }
    }

    protected T GetRelation<T>(string key) where T : TypeRelationInfo {
      if (this.Relations.ContainsKey(key)) {
        return (T) this.Relations[key];
      } else {
        throw new OntologyException(OntologyException.Msg.TypeRelationInfoNotFound, key, this.Name);
      }
    }
    void IStorable.ImplementsOnStorageUpdateEnds() {
      //cache.Insert(this);
      throw new NotImplementedException("BaseObject.ImplementsOnStorageUpdateEnds");
    }

    DataOperationList IStorable.ImplementsStorageUpdate(StorageContextOperation operation, DateTime timestamp) {
      //if (this.IsNew) {
      //  this.objectId = ObjectReader.GetNextObjectId(this.ObjectTypeInfo);
      //}
      throw new NotImplementedException("BaseObject.ImplementsStorageUpdate");
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

    private string BuildKeywords() {
      return EmpiriaString.BuildKeywords(typeFamily.ToString(), displayName, displayPluralName,
                                         assemblyName, className, documentation);
    }

    private void ConstructBaseType() {
      if (!IsPrimitive && !cache.ContainsId(this.baseTypeId)) {
        baseType = MetaModelType.Parse(this.baseTypeId);
      } else if (cache[this.baseTypeId].TypeFamily == this.TypeFamily) {
        baseType = cache[this.baseTypeId];
      } else {
        throw new OntologyException(OntologyException.Msg.TypeInfoFamilyNotMatch,
                                    this.baseTypeId, typeFamily.ToString());
      }
    }

    private void Load(DataRow dataRow, string parsingValue) {
      Validate(dataRow, parsingValue);
      LoadDataRow(dataRow);
      if (!cache.ContainsId(this.Id)) {
        cache.Add(this.Name, this);
      } else {
        cache[this.Name] = this;
      }
      ConstructBaseType();
    }

    private void LoadDataRow(DataRow dataRow) {
      this.id = (int) dataRow["TypeId"];
      this.name = (string) dataRow["TypeName"];
      this.baseTypeId = (int) dataRow["BaseTypeId"];
      this.assemblyName = (string) dataRow["AssemblyName"];
      this.className = (string) dataRow["ClassName"];
      this.displayName = (string) dataRow["DisplayName"];
      this.displayPluralName = (string) dataRow["DisplayPluralName"];
      this.femaleGenre = (bool) dataRow["FemaleGenre"];
      this.documentation = (string) dataRow["Documentation"];
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
      this.postedById = (int) dataRow["PostedById"];
      this.postingTime = (DateTime) dataRow["PostingTime"];
      this.status = (GeneralObjectStatus) char.Parse((string) dataRow["TypeStatus"]);
    }

    private void LoadMethods() {
      DataTable dataTable = OntologyData.GetTypeMethods(this.Id);

      this.methodsList = new DoubleKeyList<TypeMethodInfo>(dataTable.Rows.Count);

      for (int i = 0, j = dataTable.Rows.Count; i < j; i++) {
        TypeMethodInfo item = TypeMethodInfo.Parse(this, dataTable.Rows[i]);
        this.methodsList.Add(item.Name, item);
      }
    }

    private void LoadRelations() {
      DataTable dataTable = OntologyData.GetTypeRelations(this.Name);

      this.relationsList = new DoubleKeyList<TypeRelationInfo>(dataTable.Rows.Count);

      for (int i = 0, j = dataTable.Rows.Count; i < j; i++) {
        TypeRelationInfo item = TypeRelationInfo.Parse(this, dataTable.Rows[i]);
        this.relationsList.Add(item.Name, item);
      }
    }

    private void Validate(DataRow dataRow, string typeIdentifier) {
      if (this.typeFamily != ParseMetaModelTypeFamily((string) dataRow["TypeFamily"])) {
        throw new OntologyException(OntologyException.Msg.TypeInfoFamilyNotMatch,
                                    typeIdentifier, this.typeFamily.ToString());
      }
    }

    static public void DumpCache() {
      string s = String.Empty;
      System.Collections.Generic.IList<int> keys = cache.Ids;

      for (int i = 0; i < keys.Count; i++) {
        MetaModelType item = cache[keys[i]];
        s += item.Id.ToString("0000") + "  " + item.Name + " \t ->" + item.GetType().Name + Environment.NewLine;
      }
      s = "Dump Cache de tipos " + Environment.NewLine + Environment.NewLine + s;
      Empiria.Messaging.Publisher.Publish(s);
    }

    #endregion Private methods

  } // class MetaModelType

} // namespace Empiria.Ontology