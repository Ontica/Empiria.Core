/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Ontology                          *
*  Namespace : Empiria.Ontology                                 License  : Please read LICENSE.txt file      *
*  Type      : OntologyData                                     Pattern  : Data Services Static Class        *
*                                                                                                            *
*  Summary   : Provides data read methods for Empiria Ontology objects.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data;
using Empiria.DataTypes.Time;

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

      if (objectTypeInfo.TypeIdFieldName.Length != 0) {
        var filter = $"({objectTypeInfo.DataSource}.{objectTypeInfo.NamedIdFieldName} = '{objectKey}') AND " +
                     $"(Types.TypeName = '{objectTypeInfo.Name}' OR Types.TypeName LIKE '{objectTypeInfo.Name}.%')";

        DataTable table = GeneralDataOperations.GetEntitiesJoined(objectTypeInfo.DataSource, "Types",
                                                                  objectTypeInfo.TypeIdFieldName, "TypeId",
                                                                  filter);
        return (table.Rows.Count == 1) ? table.Rows[0] : null;
      } else {
        return GeneralDataOperations.GetEntityByKey(objectTypeInfo.DataSource,
                                                    objectTypeInfo.NamedIdFieldName, objectKey);
      }
    }

    internal static DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, IFilter condition) {
      return GeneralDataOperations.GetEntity(objectTypeInfo.DataSource, condition);
    }

    static internal DataRow GetBaseObjectTypeInfoDataRowWithType(Type type) {
      var operation = DataOperation.Parse("getBaseTypeWithTypeName", type.FullName);

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

    static internal DataView GetSimpleObjects(ObjectTypeInfo objectTypeInfo, string filter = "", string sort = "") {
      var operation = DataOperation.Parse("qrySimpleObjects", objectTypeInfo.Id);

      return DataReader.GetDataView(operation, filter, sort);
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

    static internal DataRow GetObjectLinkDataRow(TypeRelationInfo typeRelation, IIdentifiable source) {
      DataTable table = GetObjectLinksTable(typeRelation, source);

      if (table.Rows.Count != 0) {
        return table.Rows[0];
      }
      return null;
    }

    static internal DataTable GetObjectLinksTable(TypeRelationInfo typeRelation, IIdentifiable source) {
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

    static internal DataTable GetObjectLinksTable(TypeRelationInfo typeRelation, IIdentifiable source,
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


    internal static List<T> GetBaseObjectList<T>(string filter = "", string sort = "") where T: BaseObject {
      var typeInfo = ObjectTypeInfo.Parse<T>();

      string fullFilter = String.Empty;

      if (typeInfo.TypeIdFieldName.Length != 0) {
        fullFilter = $"{typeInfo.TypeIdFieldName} IN ({typeInfo.GetSubclassesFilter()})";
      }
      if (filter.Length != 0) {
        fullFilter = GeneralDataOperations.BuildSqlAndFilter(fullFilter, filter);
      }

      var table = GeneralDataOperations.GetEntities(typeInfo.DataSource, fullFilter, sort);

      return BaseObject.ParseList<T>(table);
    }


    static internal DataTable GetInverseObjectLinksTable(TypeRelationInfo typeRelation, IIdentifiable target) {
      string sql = "SELECT {SOURCE.TYPE.TABLE}.* FROM {SOURCE.TYPE.TABLE} INNER JOIN {LINKS.TABLE} " +
             "ON {SOURCE.TYPE.TABLE}.{SourceTableIdField} = {LINKS.TABLE}.{SourceIdField} " +
             "WHERE {LINKS.TABLE}.{TypeRelationIdField} = {TypeRelationId} AND " +
             "{LINKS.TABLE}.{TargetIdField} = {TargetId} AND " +
             "{LINKS.TABLE}.LinkStatus = 'A' " +
             "ORDER BY {LINKS.TABLE}.LinkIndex";

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


    static internal DataRow GetTypeRelation(string typeRelationName) {
      return GeneralDataOperations.GetEntityByKey("TypeRelations", "RelationName", typeRelationName);
    }


    static internal DataTable GetTypeRelations(string typeName) {
      return DataReader.GetDataTable(DataOperation.Parse("qryTypeRelations", typeName));
    }


    static internal DataRow TryGetSystemTypeDataRow(string systemTypeName) {
      string filter = String.Format("(TypeName = '{0}' OR ClassName LIKE '%{0}')", systemTypeName);

      DataTable table = GeneralDataOperations.GetEntities("Types", filter);

      if (table.Rows.Count == 0) {
        return null;
      } else if (table.Rows.Count == 1) {
        return table.Rows[0];
      }
      DataRow[] select = table.Select(String.Format("ClassName = '{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      select = table.Select(String.Format("TypeName = '{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      select = table.Select(String.Format("ClassName = 'System.{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      select = table.Select(String.Format("ClassName = 'Empiria.{0}'", systemTypeName));
      if (select.Length == 1) {
        return select[0];
      }
      return null;
    }


    static internal void WriteLink(TypeAssociationInfo assocationInfo, IIdentifiable source, IIdentifiable target) {
      DataOperation operation = DataOperation.Parse("writeObjectLink", GetNextRelationId(assocationInfo),
                                                    assocationInfo.Id, source.Id, target.Id, 0, String.Empty, String.Empty,
                                                    ExecutionServer.CurrentUserId, "A", DateTime.Today,
                                                    ExecutionServer.DateMaxValue);

      DataWriter.Execute(operation);
    }

    #endregion Internal methods

  } // class OntologyData

} // namespace Empiria.Ontology
