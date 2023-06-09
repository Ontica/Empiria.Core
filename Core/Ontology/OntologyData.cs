/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Ontology                           Component : Data Layer                              *
*  Assembly : Empiria.Core.dll                           Pattern   : Data services                           *
*  Type     : OntologyData                               License   : Please read LICENSE.txt file            *
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
      return OntologyDataHelpers.GetEntityById(objectTypeInfo.DataSource,
                                                 objectTypeInfo.IdFieldName, objectId);
    }


    static internal DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, string objectKey) {
      if (objectTypeInfo.DataSource.StartsWith("qry") || objectTypeInfo.DataSource.StartsWith("get")) {
        return DataReader.GetDataRow(DataOperation.Parse(objectTypeInfo.DataSource, objectKey));
      }

      if (objectTypeInfo.TypeIdFieldName.Length != 0) {
        var filter = $"({objectTypeInfo.DataSource}.{objectTypeInfo.NamedIdFieldName} = '{objectKey}') AND " +
                     $"(Types.TypeName = '{objectTypeInfo.Name}' OR Types.TypeName LIKE '{objectTypeInfo.Name}.%')";

        DataTable table = OntologyDataHelpers.GetEntitiesJoined(objectTypeInfo.DataSource, "Types",
                                                                  objectTypeInfo.TypeIdFieldName, "TypeId",
                                                                  filter);
        return (table.Rows.Count == 1) ? table.Rows[0] : null;
      } else {
        return OntologyDataHelpers.GetEntityByKey(objectTypeInfo.DataSource,
                                                    objectTypeInfo.NamedIdFieldName, objectKey);
      }
    }


    static internal DataRow GetBaseObjectDataRow(ObjectTypeInfo objectTypeInfo, IFilter condition) {
      return OntologyDataHelpers.GetEntity(objectTypeInfo.DataSource, condition);
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
      return OntologyDataHelpers.GetEntitiesByField("Types", "BaseTypeId", baseTypeId);
    }


    static internal FixedList<T> GetSimpleObjects<T>() where T: BaseObject {
      ObjectTypeInfo objectTypeInfo = ObjectTypeInfo.Parse<T>();

      var operation = DataOperation.Parse("qrySimpleObjects", objectTypeInfo.Id);

      return DataReader.GetFixedList<T>(operation);
    }


    static internal int GetNextObjectId(ObjectTypeInfo objectTypeInfo) {
      int id = DataWriter.CreateId(objectTypeInfo.DataSource);

      Assertion.Require(id != 0,
        $"Generated Id value can't be zero. Please review DbRules table for type {objectTypeInfo.Name}.");

      return id;
    }


    static internal List<T> GetBaseObjectList<T>(string filter = "", string sort = "") where T: BaseObject {
      var typeInfo = ObjectTypeInfo.Parse<T>();

      string fullFilter = String.Empty;

      if (typeInfo.TypeIdFieldName.Length != 0) {
        fullFilter = $"{typeInfo.TypeIdFieldName} IN ({typeInfo.GetSubclassesFilter()})";
      }

      if (fullFilter.Length != 0 && filter.Length != 0) {
        fullFilter = $"{fullFilter} AND {filter}";

      } else if (fullFilter.Length != 0 && filter.Length == 0) {

        // no-op

      } else if (fullFilter.Length == 0 && filter.Length != 0) {
        fullFilter = filter;

      } else {

        // no-op

      }

      return OntologyDataHelpers.GetList<T>(typeInfo.DataSource, fullFilter, sort);
    }


    static internal DataRow GetTypeDataRow(int typeId) {
      DataRow row = OntologyDataHelpers.GetEntityById("Types", "TypeId", typeId);
      if (row != null) {
        return row;
      } else {
        throw new OntologyException(OntologyException.Msg.TypeInfoNotFound, typeId.ToString());
      }
    }


    static internal DataRow GetTypeDataRow(string typeName) {
      DataRow row = OntologyDataHelpers.GetEntityByKey("Types", "TypeName", typeName);
      if (row != null) {
        return row;
      } else {
        throw new OntologyException(OntologyException.Msg.TypeInfoNotFound, typeName);
      }
    }


    static internal DataRow TryGetSystemTypeDataRow(string systemTypeName) {
      var sql = "SELECT * FROM Types " +
                $"WHERE (TypeName = '{systemTypeName}' OR ClassName LIKE '%{systemTypeName}')";

      DataTable table = DataReader.GetDataTable(DataOperation.Parse(sql));

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
