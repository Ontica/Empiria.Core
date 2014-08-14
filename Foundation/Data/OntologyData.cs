/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Ontology               *
*  Namespace : Empiria.Ontology                                 Assembly : Empiria.dll                       *
*  Type      : OntologyData                                     Pattern  : Data Services Static Class        *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Provides data read methods for Empiria Foundation Ontology objects.                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Ontology {

  static internal class OntologyData {

    #region Internal methods

    static internal DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, int objectId) {
      if (objectTypeInfo.DataSource.StartsWith("qry") || objectTypeInfo.DataSource.StartsWith("get")) {
        return DataReader.GetDataRow(DataOperation.Parse(objectTypeInfo.DataSource, objectId));
      }
      return GeneralDataOperations.GetEntityById(objectTypeInfo.DataSource, objectTypeInfo.IdFieldName, objectId);
    }

    static internal DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, string objectId) {
      if (objectTypeInfo.DataSource.StartsWith("qry") || objectTypeInfo.DataSource.StartsWith("get")) {
        return DataReader.GetDataRow(DataOperation.Parse(objectTypeInfo.DataSource, objectId));
      }
      return GeneralDataOperations.GetEntityByKey(objectTypeInfo.DataSource, objectTypeInfo.NamedIdFieldName, objectId);
    }

    static internal DataTable GetDerivedTypes(int baseTypeId) {
      return GeneralDataOperations.GetEntitiesByField("EOSTypes", "BaseTypeId", baseTypeId);
    }

    static internal DataTable GetGeneralObjectsDataTable(ObjectTypeInfo objectTypeInfo) {
      DataOperation dataOp = DataOperation.Parse("qryEOSGeneralObjects", objectTypeInfo.Id);

      return DataReader.GetDataTable(dataOp);
    }

    static internal FixedList<KeyValuePair> GetKeyValueListItems(GeneralList list) {
      DataOperation dataOp = DataOperation.Parse("qryEOSKeyValueList", list.NamedKey);
      DataTable table = DataReader.GetDataTable(dataOp);

      return new FixedList<KeyValuePair>((x) => KeyValuePair.Parse(x), table);
    }

    static internal int GetNextObjectId(ObjectTypeInfo objectTypeInfo) {
      int id = DataWriter.CreateId(objectTypeInfo.DataSource);
      Assertion.Assert(id != 0, String.Format("Generated Id value can't be zero. Please review DbRules" +
                                              "table for type {0}.", objectTypeInfo.Name));
      return id;
    }

    static internal int GetNextRelationId(TypeRelationInfo typeRelationInfo) {
      return DataWriter.CreateId(typeRelationInfo.DataSource);
    }

    static internal int GetNextTypeId() {
      return DataWriter.CreateId("EOSTypes");
    }

    static internal int GetNextTypeRelationId() {
      return DataWriter.CreateId("EOSTypeRelations");
    }

    static internal DataTable GetObjectLinksTable(TypeRelationInfo typeRelation, IStorable source) {
      string filter = GetTableIdFieldEqualsTo(typeRelation.DataSource, typeRelation.TypeRelationIdFieldName,
                                              typeRelation.Id);
      filter += " AND ";
      filter += GetTableIdFieldEqualsTo(typeRelation.DataSource, typeRelation.SourceIdFieldName, source.Id);
      //string sortExpression = GetFieldName(typeRelation.DataSource, "LinkIndex");

      return GeneralDataOperations.GetEntitiesJoined(typeRelation.TargetType.DataSource, typeRelation.DataSource,
                                                     typeRelation.TargetType.IdFieldName, typeRelation.TargetIdFieldName,
                                                     filter);
    }

    static internal DataRow GetObjectLinkDataRow(TypeRelationInfo typeRelation, IStorable source) {
      DataTable table = GetObjectLinksTable(typeRelation, source);

      if (table.Rows.Count != 0) {
        return table.Rows[0];
      }
      return null;
    }

    static internal DataTable GetObjectLinksTable(TypeRelationInfo typeRelation, IStorable source,
                                                  TimePeriod period) {
      string filter = GetTableIdFieldEqualsTo(typeRelation.DataSource, typeRelation.TypeRelationIdFieldName,
                                              typeRelation.Id);
      filter += " AND ";
      filter += GetTableIdFieldEqualsTo(typeRelation.DataSource, typeRelation.SourceIdFieldName, source.Id);

      return GeneralDataOperations.GetEntitiesJoined(typeRelation.TargetType.DataSource, typeRelation.DataSource,
                                                     typeRelation.TargetType.IdFieldName, typeRelation.TargetIdFieldName,
                                                     filter);
    }

    static internal DataRow GetRule(int ruleId) {
      string sql = "SELECT * FROM vwEOSRules WHERE (RuleId = " + ruleId.ToString() + ")";

      return DataReader.GetDataRow(DataOperation.Parse(sql));
    }

    static internal DataTable GetRulesLibrary(int rulesLibraryId) {
      string sql = "SELECT * FROM vwEOSRules WHERE (RulesLibraryId = " + rulesLibraryId.ToString() + ")";

      return DataReader.GetDataTable(DataOperation.Parse(sql));
    }

    static internal DataTable GetRuleItems(int ruleId, string ruleItemType) {
      string sql = "SELECT * FROM EOSRuleStructure " +
                   "WHERE (RuleId = " + ruleId.ToString() + " AND RuleItemType = '" + ruleItemType + "') " +
                   "ORDER BY RuleItemIndex";

      return DataReader.GetDataTable(DataOperation.Parse(sql));
    }

    static internal DataRow GetTypeDataRow(int typeId) {
      DataRow row = GeneralDataOperations.GetEntityById("EOSTypes", "TypeId", typeId);
      if (row != null) {
        return row;
      } else {
        throw new OntologyException(OntologyException.Msg.TypeInfoNotFound, typeId.ToString());
      }
    }

    static internal DataRow GetTypeDataRow(string typeName) {
      DataRow row = GeneralDataOperations.GetEntityByKey("EOSTypes", "TypeName", typeName);
      if (row != null) {
        return row;
      } else {
        throw new OntologyException(OntologyException.Msg.TypeInfoNotFound, typeName);
      }
    }

    static internal DataRow GetTypeMethodDataRow(int typeMethodId) {
      return GeneralDataOperations.GetEntityById("EOSTypeMethods", "TypeMethodId", typeMethodId);
    }

    static internal DataTable GetTypeMethods(int typeId) {
      return GeneralDataOperations.GetEntitiesByField("EOSTypeMethods", "SourceTypeId", typeId);
    }

    static internal DataTable GetTypeMethodParameters(int typeMethodId) {
      return GeneralDataOperations.GetEntitiesByField("EOSTypeMethodsParameters", "TypeMethodId", typeMethodId);
    }

    static internal DataRow GetTypeRelation(int typeRelationId) {
      return GeneralDataOperations.GetEntityById("EOSTypeRelations", "TypeRelationId", typeRelationId);
    }

    static internal DataTable GetTypeRelations(string typeName) {
      return DataReader.GetDataTable(DataOperation.Parse("qryEOSTypeRelations", typeName));
    }

    static internal DataRow TryGetSystemType(string systemTypeName) {
      string filter = String.Format("([TypeName] = '{0}' OR ClassName LIKE '%{0}')", systemTypeName);
      
      DataTable table = GeneralDataOperations.GetEntities("EOSTypes", filter);

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
      DataOperation operation = DataOperation.Parse("writeEOSObjectLink", GetNextRelationId(assocationInfo),
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
