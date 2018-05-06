/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : ParameterInfo                                    Pattern  : Type metadata class               *
*                                                                                                            *
*  Summary   : Represents a method parameter type definition.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.StateEnums;

namespace Empiria.Ontology {

  public sealed class ParameterInfo : IIdentifiable {

    #region Fields

    private int id = 0;
    private int index = 0;
    private TypeMethodInfo typeMethodInfo = null;
    private MetaModelType parameterType = null;
    private string name = String.Empty;
    private string displayName = String.Empty;
    private string description = String.Empty;
    private string documentation = String.Empty;
    private string keywords = String.Empty;
    private bool isArray = false;
    private bool isOptional = false;
    private object defaultValue = null;
    private int postedById = 0;
    private EntityStatus storageStatus = EntityStatus.Active;
    private DateTime startDate = DateTime.Today;
    private DateTime endDate = ExecutionServer.DateMaxValue;

    #endregion Fields

    #region Constructors and parsers

    public ParameterInfo(TypeMethodInfo typeMethodInfo) {
      this.typeMethodInfo = typeMethodInfo;
    }

    private ParameterInfo(TypeMethodInfo typeMethodInfo, DataRow baseDataRow) {
      this.typeMethodInfo = typeMethodInfo;

      LoadFields(baseDataRow);
    }

    static internal ParameterInfo Parse(TypeMethodInfo typeMethodInfo, DataRow baseDataRow) {
      return new ParameterInfo(typeMethodInfo, baseDataRow);
    }

    #endregion Constructors and parsers

    #region Public properties

    public object DefaultValue {
      get { return defaultValue; }
      set { defaultValue = value; }
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

    public int PostedById {
      get { return postedById; }
      set { postedById = value; }
    }

    public int Id {
      get { return id; }
    }

    string IIdentifiable.UID {
      get {
        return this.Name;
      }
    }

    public bool IsArray {
      get { return isArray; }
      set { isArray = value; }
    }

    public bool IsOptional {
      get { return isOptional; }
      set { isOptional = value; }
    }

    public string Keywords {
      get { return keywords; }
      set { keywords = EmpiriaString.TrimAll(value); }
    }

    public string Name {
      get { return name; }
      set { name = EmpiriaString.TrimAll(value); }
    }

    public DateTime StartDate {
      get { return startDate; }
      set { startDate = value; }
    }

    public EntityStatus StorageStatus {
      get { return storageStatus; }
      set { storageStatus = value; }
    }

    public MetaModelType MetaModelType {
      get { return parameterType; }
      set { parameterType = value; }
    }

    public int TypeId {
      get { return parameterType.Id; }
    }

    public TypeMethodInfo TypeMethodInfo {
      get { return typeMethodInfo; }
      set { typeMethodInfo = value; }
    }

    public int TypeMethodInfoId {
      get { return typeMethodInfo.Id; }
    }

    #endregion Public properties

    #region Private methods

    private void LoadFields(DataRow dataRow) {
      if (dataRow == null) {
        throw new OntologyException(OntologyException.Msg.TypeMethodInfoNotFound, this.Id);
      }
      this.id = (int) dataRow["TypeMethodParameterId"];
      index = (int) dataRow["ParameterIndex"];
      parameterType = MetaModelType.Parse((int) dataRow["ParameterTypeId"]);
      name = (string) dataRow["ParameterName"];
      displayName = (string) dataRow["ParameterDisplayName"];
      description = (string) dataRow["ParameterDescription"];
      documentation = (string) dataRow["ParameterDocumentation"];
      keywords = (string) dataRow["ParameterKeywords"];
      isArray = (bool) dataRow["IsArray"];
      isOptional = (bool) dataRow["IsOptional"];
      defaultValue = (string) dataRow["DefaultValue"];
      postedById = (int) dataRow["PostedById"];
      storageStatus = (EntityStatus) Convert.ToChar(dataRow["StorageStatus"]);
      startDate = (DateTime) dataRow["StartDate"];
      endDate = (DateTime) dataRow["EndDate"];
    }

    #endregion Private methods

  } // class ParameterInfo

} // namespace Empiria.Ontology
