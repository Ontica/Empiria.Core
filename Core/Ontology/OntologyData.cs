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

      Assertion.Require(id != 0,
        $"Generated Id value can't be zero. Please review DbRules table for type {objectTypeInfo.Name}.");

      return id;
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


    #endregion Internal methods

  } // class OntologyData

} // namespace Empiria.Ontology
