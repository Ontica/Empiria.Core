/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : TypeMethodInfo                                   Pattern  : Type metadata class               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a method type definition.                                                          *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.Reflection;

namespace Empiria.Ontology {

  public enum AuditTrailMode {
    None = 0,
  }

  public sealed class TypeMethodInfo : IStorable {

    #region Fields

    private int id = 0;
    private MetaModelType sourceType = null;
    private string name = String.Empty;
    private string fullName = String.Empty;
    private string displayName = String.Empty;
    private string description = String.Empty;
    private string documentation = String.Empty;
    private string keywords = String.Empty;
    private MetaModelType returnType = null;
    private bool isProperty = false;
    private bool isSealed = false;
    private bool isStatic = false;
    private bool isInherited = false;
    private bool isHardcoded = true;
    private bool isPrivate = false;
    private AuditTrailMode auditTrailMode = AuditTrailMode.None;
    private string sourceCode = String.Empty;
    private int postedById = 0;
    private GeneralObjectStatus storageStatus = GeneralObjectStatus.Active;
    private DateTime startDate = DateTime.Today;
    private DateTime endDate = ExecutionServer.DateMaxValue;

    private ParameterInfo[] parameters = null;

    #endregion Fields

    #region Constructors and parsers

    public TypeMethodInfo(MetaModelType sourceType) {
      this.sourceType = sourceType;
    }

    private TypeMethodInfo(MetaModelType sourceType, DataRow baseDataRow) {
      this.sourceType = sourceType;

      LoadDataRow(baseDataRow);
      LoadParameterInfo();
    }

    private void LoadParameterInfo() {
      DataTable dataTable = OntologyData.GetTypeMethodParameters(this.Id);

      parameters = new ParameterInfo[dataTable.Rows.Count];
      for (int i = 0, j = dataTable.Rows.Count; i < j; i++) {
        parameters[i] = ParameterInfo.Parse(this, dataTable.Rows[i]);
      }
    }

    static public TypeMethodInfo Parse(int id) {
      DataRow dataRow = OntologyData.GetTypeMethodDataRow(id);

      MetaModelType sourceType = MetaModelType.Parse((int) dataRow["SourceTypeId"]);

      return TypeMethodInfo.Parse(sourceType, dataRow);
    }

    static internal TypeMethodInfo Parse(MetaModelType sourceType, DataRow baseDataRow) {
      return new TypeMethodInfo(sourceType, baseDataRow);
    }

    #endregion Constructors and parsers

    #region Public properties

    public AuditTrailMode AuditTrailMode {
      get { return auditTrailMode; }
      set { auditTrailMode = value; }
    }

    public string Description {
      get { return description; }
      set { description = EmpiriaString.TrimAll(value); }
    }

    public string DisplayName {
      get { return displayName; }
      set { displayName = EmpiriaString.TrimAll(value); }
    }

    public string Documentation {
      get { return documentation; }
      set { documentation = EmpiriaString.TrimAll(value); }
    }

    public DateTime EndDate {
      get { return endDate; }
      set { endDate = value; }
    }

    public string FullName {
      get { return fullName; }
    }

    public int Id {
      get { return id; }
    }

    public bool IsHardcoded {
      get { return isHardcoded; }
      set { isHardcoded = value; }
    }

    public bool IsInherited {
      get { return isInherited; }
    }

    public bool IsPrivate {
      get { return isPrivate; }
      set { isPrivate = value; }
    }

    public bool IsProperty {
      get { return isProperty; }
      set { isProperty = value; }
    }

    public bool IsSealed {
      get { return isSealed; }
      set { isSealed = value; }
    }

    public bool IsStatic {
      get { return isStatic; }
      set { isStatic = value; }
    }

    public string Keywords {
      get { return keywords; }
      set { keywords = EmpiriaString.TrimAll(value); }
    }

    public string Name {
      get { return name; }
      set { name = EmpiriaString.TrimAll(value); }
    }

    public int PostedById {
      get { return postedById; }
      set { postedById = value; }
    }

    public string SourceCode {
      get { return sourceCode; }
      set { sourceCode = value; }
    }

    public MetaModelType SourceType {
      get { return sourceType; }
    }

    public DateTime StartDate {
      get { return startDate; }
      set { startDate = value; }
    }

    public GeneralObjectStatus StorageStatus {
      get { return storageStatus; }
      set { storageStatus = value; }
    }

    public MetaModelType ReturnType {
      get { return returnType; }
      set { returnType = value; }
    }

    public ParameterInfo[] Parameters {
      get { return parameters; }
    }

    #endregion Public properties

    #region Public methods

    public object Invoke(object[] parametersArray) {
      return MethodInvoker.Execute(sourceType.UnderlyingSystemType, this.Name, parametersArray);
    }

    public object Invoke(object instance, object[] parametersArray) {
      return MethodInvoker.Execute(instance, this.Name, parametersArray);
    }

    #endregion Public methods

    #region Private methods

    DataOperationList IStorable.ImplementsStorageUpdate(StorageContextOperation operation, DateTime timestamp) {
      throw new NotImplementedException();
    }

    void IStorable.ImplementsOnStorageUpdateEnds() {
      throw new NotImplementedException();
    }

    private void LoadDataRow(DataRow dataRow) {
      if (dataRow == null) {
        throw new OntologyException(OntologyException.Msg.TypeMethodInfoNotFound, this.Id);
      }
      this.id = (int) dataRow["TypeMethodId"];
      this.name = (string) dataRow["MethodName"];
      this.fullName = this.sourceType.Name + "." + name;
      this.displayName = (string) dataRow["MethodDisplayName"];
      this.description = (string) dataRow["MethodDescription"];
      this.documentation = (string) dataRow["MethodDocumentation"];
      this.keywords = (string) dataRow["MethodKeywords"];
      this.returnType = MetaModelType.Parse<MetaModelType>((int) dataRow["ReturnTypeId"]);
      this.isProperty = (bool) dataRow["IsProperty"];
      this.isSealed = (bool) dataRow["IsSealed"];
      this.isStatic = (bool) dataRow["IsStatic"];
      this.isInherited = (sourceType.Id != (int) dataRow["SourceTypeId"]);
      this.isPrivate = (bool) dataRow["IsPrivate"];
      this.isHardcoded = (bool) dataRow["IsHardcoded"];
      this.sourceCode = (string) dataRow["SourceCode"];
      this.auditTrailMode = (AuditTrailMode) Convert.ToChar(dataRow["AuditTrailMode"]);
      this.postedById = (int) dataRow["PostedById"];
      this.storageStatus = (GeneralObjectStatus) Convert.ToChar(dataRow["StorageStatus"]);
      this.startDate = (DateTime) dataRow["StartDate"];
      this.endDate = (DateTime) dataRow["EndDate"];
    }

    #endregion Private methods

  } // class TypeMethodInfo

} // namespace Empiria.Ontology
