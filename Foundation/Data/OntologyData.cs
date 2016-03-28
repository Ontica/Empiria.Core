/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.Foundation.dll            *
*  Type      : OntologyData                                     Pattern  : Data Services Static Class        *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides data read methods for Empiria Foundation Ontology objects.                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;
using Empiria.DataTypes;

namespace Empiria.Ontology {

  static internal class OntologyData {

    #region Internal methods

    static internal DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, int objectId) {
      if (objectTypeInfo.DataSource.StartsWith("qry") || objectTypeInfo.DataSource.StartsWith("get")) {
        return DataReader.GetDataRow(DataOperation.Parse(objectTypeInfo.DataSource, objectId));
      }
      return GeneralDataOperations.GetEntityById(objectTypeInfo.DataSource,
                                                 objectTypeInfo.IdFieldName, objectId);
    }

    static internal DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, string objectKey) {
      if (objectTypeInfo.DataSource.StartsWith("qry") || objectTypeInfo.DataSource.StartsWith("get")) {
        return DataReader.GetDataRow(DataOperation.Parse(objectTypeInfo.DataSource, objectKey));
      }
      return GeneralDataOperations.GetEntityByKey(objectTypeInfo.DataSource,
                                                  objectTypeInfo.NamedIdFieldName, objectKey);
    }

    internal static DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, IFilter condition) {
      return GeneralDataOperations.GetEntity(objectTypeInfo.DataSource, condition);
    }

    static internal DataRow GetBaseObjectTypeInfoDataRowWithType(Type type) {
      var operation = DataOperation.Parse("getBaseObjectTypeInfoWithTypeName", type.FullName);

      var row = DataReader.GetDataRow(operation);
      if (row != null) {
        return row;
      } else {
        throw new OntologyException(OntologyException.Msg.UnderlyingTypeNotFound, type.FullName);
      }
    }

    static internal DataTable GetDerivedTypes(int baseTypeId) {
      return GeneralDataOperations.GetEntitiesByField("Types", "BaseTypeId", baseTypeId);
    }

    static internal DataTable GetSimpleObjectsDataTable(ObjectTypeInfo objectTypeInfo) {
      var operation = DataOperation.Parse("qrySimpleObjects", objectTypeInfo.Id);

      return DataReader.GetDataTable(operation);
    }

    static internal int GetNextObjectId(ObjectTypeInfo objectTypeInfo) {
      int id = DataWriter.CreateId(objectTypeInfo.DataSource);
      Assertion.Assert(id != 0,
                       "Generated Id value can't be zero. Please review DbRules table for type {0}.",
                       objectTypeInfo.Name);
      return id;
    }

    static internal int GetNextRelationId(TypeRelationInfo typeRelationInfo) {
      return DataWriter.CreateId(typeRelationInfo.DataSource);
    }

    static internal DataRow GetObjectLinkDataRow(TypeRelationInfo typeRelation, IStorable source) {
      DataTable table = GetObjectLinksTable(typeRelation, source);

      if (table.Rows.Count != 0) {
        return table.Rows[0];
      }
      return null;
    }

    static internal DataTable GetObjectLinksTable(TypeRelationInfo typeRelation, IStorable source) {
      string sql = "SELECT [{TARGET.TYPE.TABLE}].* FROM [{TARGET.TYPE.TABLE}] INNER JOIN [{LINKS.TABLE}] " +
                   "ON [{TARGET.TYPE.TABLE}].[{TargetTableIdField}] = [{LINKS.TABLE}].[{TargetIdField}] " +
                   "WHERE [{LINKS.TABLE}].[{TypeRelationIdField}] = {TypeRelationId} AND " +
                   "[{LINKS.TABLE}].[{SourceIdField}] = {SourceId} AND " +
                   "[{LINKS.TABLE}].LinkStatus = 'A' " +
                   "ORDER BY [{LINKS.TABLE}].LinkIndex";

      sql = sql.Replace("{LINKS.TABLE}", typeRelation.DataSource);
      sql = sql.Replace("{TARGET.TYPE.TABLE}", typeRelation.TargetType.DataSource);
      sql = sql.Replace("{TargetTableIdField}", typeRelation.TargetType.IdFieldName);
      sql = sql.Replace("{TargetIdField}", typeRelation.TargetIdFieldName);
      sql = sql.Replace("{TypeRelationIdField}", typeRelation.TypeRelationIdFieldName);
      sql = sql.Replace("{TypeRelationId}", typeRelation.Id.ToString());
      sql = sql.Replace("{SourceIdField}", typeRelation.SourceIdFieldName);
      sql = sql.Replace("{SourceId}", source.Id.ToString());

      return DataReader.GetDataTable(DataOperation.Parse(sql));
    }

    static internal DataTable GetObjectLinksTable(TypeRelationInfo typeRelation, IStorable source,
                                                  TimeFrame period) {
      string sql = "SELECT [{TARGET.TYPE.TABLE}].* FROM [{TARGET.TYPE.TABLE}] INNER JOIN [{LINKS.TABLE}] " +
                   "ON [{TARGET.TYPE.TABLE}].[{TargetTableIdField}] = [{LINKS.TABLE}].[{TargetIdField}] " +
                   "WHERE [{LINKS.TABLE}].[{TypeRelationIdField}] = {TypeRelationId} AND " +
                   "[{LINKS.TABLE}].[{SourceIdField}] = {SourceId} AND " +
                   "([{LINKS.TABLE}].StartDate <= '{TimePeriodStart}' AND " +
                    "[{LINKS.TABLE}].EndDate >= '{TimePeriodEnd}') AND " +
                   "[{LINKS.TABLE}].LinkStatus = 'A' " +
                   "ORDER BY [{LINKS.TABLE}].LinkIndex";

      sql = sql.Replace("{LINKS.TABLE}", typeRelation.DataSource);
      sql = sql.Replace("{TARGET.TYPE.TABLE}", typeRelation.TargetType.DataSource);
      sql = sql.Replace("{TargetTableIdField}", typeRelation.TargetType.IdFieldName);
      sql = sql.Replace("{TargetIdField}", typeRelation.TargetIdFieldName);
      sql = sql.Replace("{TypeRelationIdField}", typeRelation.TypeRelationIdFieldName);
      sql = sql.Replace("{TypeRelationId}", typeRelation.Id.ToString());
      sql = sql.Replace("{SourceIdField}", typeRelation.SourceIdFieldName);
      sql = sql.Replace("{SourceId}", source.Id.ToString());
      sql = sql.Replace("{TimePeriodStart}", period.StartTime.ToString("yyyy-MM-dd"));
      sql = sql.Replace("{TimePeriodEnd}", period.EndTime.ToString("yyyy-MM-dd"));

      return DataReader.GetDataTable(DataOperation.Parse(sql));
    }

    static internal DataTable GetInverseObjectLinksTable(TypeRelationInfo typeRelation, IStorable target) {
      string sql = "SELECT [{SOURCE.TYPE.TABLE}].* FROM [{SOURCE.TYPE.TABLE}] INNER JOIN [{LINKS.TABLE}] " +
             "ON [{SOURCE.TYPE.TABLE}].[{SourceTableIdField}] = [{LINKS.TABLE}].[{SourceIdField}] " +
             "WHERE [{LINKS.TABLE}].[{TypeRelationIdField}] = {TypeRelationId} AND " +
             "[{LINKS.TABLE}].[{TargetIdField}] = {TargetId} AND " +
             "[{LINKS.TABLE}].LinkStatus = 'A' " +
             "ORDER BY [{LINKS.TABLE}].LinkIndex";

      sql = sql.Replace("{LINKS.TABLE}", typeRelation.DataSource);
      sql = sql.Replace("{SOURCE.TYPE.TABLE}", typeRelation.SourceType.DataSource);
      sql = sql.Replace("{SourceTableIdField}", typeRelation.SourceType.IdFieldName);
      sql = sql.Replace("{SourceIdField}", typeRelation.SourceIdFieldName);
      sql = sql.Replace("{TypeRelationIdField}", typeRelation.TypeRelationIdFieldName);
      sql = sql.Replace("{TypeRelationId}", typeRelation.Id.ToString());
      sql = sql.Replace("{TargetIdField}", typeRelation.TargetIdFieldName);
      sql = sql.Replace("{TargetId}", target.Id.ToString());

      return DataReader.GetDataTable(DataOperation.Parse(sql));
    }

    static internal DataRow GetRule(int ruleId) {
      string sql = "SELECT * FROM vwRules WHERE (RuleId = " + ruleId.ToString() + ")";

      return DataReader.GetDataRow(DataOperation.Parse(sql));
    }

    static internal DataTable GetRulesLibrary(int rulesLibraryId) {
      string sql = "SELECT * FROM vwRules WHERE (RulesLibraryId = " + rulesLibraryId.ToString() + ")";

      return DataReader.GetDataTable(DataOperation.Parse(sql));
    }

    static internal DataTable GetRuleItems(int ruleId, string ruleItemType) {
      string sql = "SELECT * FROM RuleStructure " +
                   "WHERE (RuleId = " + ruleId.ToString() + " AND RuleItemType = '" + ruleItemType + "') " +
                   "ORDER BY RuleItemIndex";

      return DataReader.GetDataTable(DataOperation.Parse(sql));
    }

    static internal DataRow GetTypeDataRow(int typeId) {
      DataRow row = GeneralDataOperations.GetEntityById("Types", "TypeId", typeId);
      if (row != null) {
        return row;
      } else {
        throw new OntologyException(OntologyException.Msg.TypeInfoNotFound, typeId.ToString());
      }
    }

    static internal DataRow GetTypeDataRow(string typeName) {
      DataRow row = GeneralDataOperations.GetEntityByKey("Types", "TypeName", typeName);
      if (row != null) {
        return row;
      } else {
        throw new OntologyException(OntologyException.Msg.TypeInfoNotFound, typeName);
      }
    }

    static internal DataRow GetTypeMethodDataRow(int typeMethodId) {
      return GeneralDataOperations.GetEntityById("TypeMethods", "TypeMethodId", typeMethodId);
    }

    static internal DataTable GetTypeMethods(int typeId) {
      try {
        return GeneralDataOperations.GetEntitiesByField("TypeMethods", "SourceTypeId", typeId);
      } catch (Exception e) {
        throw new OntologyException(OntologyException.Msg.TypeMethodInfoNotFound, typeId, e);
      }
    }

    static internal DataTable GetTypeMethodParameters(int typeMethodId) {
      return GeneralDataOperations.GetEntitiesByField("TypeMethodsParameters", "TypeMethodId", typeMethodId);
    }

    static internal DataRow GetTypeRelation(int typeRelationId) {
      return GeneralDataOperations.GetEntityById("TypeRelations", "TypeRelationId", typeRelationId);
    }

    internal static DataRow GetTypeRelation(string typeRelationName) {
      return GeneralDataOperations.GetEntityByKey("TypeRelations", "RelationName", typeRelationName);
    }

    static internal DataTable GetTypeRelations(string typeName) {
      return DataReader.GetDataTable(DataOperation.Parse("qryTypeRelations", typeName));
    }

    static internal DataRow TryGetSystemTypeDataRow(string systemTypeName) {
      string filter = String.Format("([TypeName] = '{0}' OR ClassName LIKE '%{0}')", systemTypeName);

      DataTable table = GeneralDataOperations.GetEntities("Types", filter);

      if (table.Rows.Count == 0) {
        return null;
      } else if (table.Rows.Count == 1) {
        return table.Rows[0];
      }
      DataRow[] select = table.Select(String.Format("[ClassName] = '{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      select = table.Select(String.Format("[TypeName] = '{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      select = table.Select(String.Format("[ClassName] = 'System.{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      select = table.Select(String.Format("[ClassName] = 'Empiria.{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      return null;
    }

    static internal void WriteLink(TypeAssociationInfo assocationInfo, IStorable source, IStorable target) {
      DataOperation operation = DataOperation.Parse("writeObjectLink", GetNextRelationId(assocationInfo),
                                                    assocationInfo.Id, source.Id, target.Id, 0, String.Empty, String.Empty,
                                                    ExecutionServer.CurrentUserId, "A", DateTime.Today,
                                                    ExecutionServer.DateMaxValue);

      DataWriter.Execute(operation);
    }

    #endregion Internal methods

    #region Private methods

    static private string GetFieldName(string source, string fieldName) {
      return "[" + source + "]." + "[" + fieldName + "]";
    }

    static private string GetTableIdFieldEqualsTo(string source, string fieldName, int idFieldValue) {
      return "([" + source + "]." + "[" + fieldName + "] = " + idFieldValue.ToString() + ")";
    }

    #endregion Private methods

  } // class OntologyData

} // namespace Empiria.Ontology
