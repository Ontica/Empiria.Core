/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Ontology                           Component : Data Layer                              *
*  Assembly : Empiria.Core.dll                           Pattern   : Data services                           *
*  Type     : OntologyDataHelpers                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data helpers for class OntologyData.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data;

namespace Empiria.Ontology {

  /// <summary>Static library with general purpose data read operations.</summary>
  static internal class OntologyDataHelpers {

    #region Methods

    static internal List<T> GetList<T>(string sourceName,
                                       string filterExpression = "",
                                       string sortExpression = "") where T : BaseObject {

      DataOperation operation = GetEntitiesDataOperation(sourceName, filterExpression, sortExpression);

      return DataReader.GetList<T>(operation);
    }


    static internal DataTable GetEntitiesByField(string sourceName, string searchFieldName, int fieldValue) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesFiltered", sourceName,
                                                    searchFieldName + " = " + fieldValue.ToString());
      return DataReader.GetDataTable(operation);
    }


    static internal DataTable GetEntitiesJoined(string sourceName, string joinedSourceName,
                                              string sourceJoinField, string joinedTargetField,
                                              string filterExpression = "", string sortExpression = "") {
      DataOperation operation;

      if (String.IsNullOrWhiteSpace(filterExpression) &&
          String.IsNullOrWhiteSpace(sortExpression)) {

        operation = DataOperation.Parse("@qryEntitiesJoined", sourceName, joinedSourceName,
                                        sourceJoinField, joinedTargetField);

      } else if (!String.IsNullOrWhiteSpace(filterExpression) &&
                 String.IsNullOrWhiteSpace(sortExpression)) {

        operation = DataOperation.Parse("@qryEntitiesJoinedFiltered", sourceName, joinedSourceName,
                                        sourceJoinField, joinedTargetField, filterExpression);

      } else if (String.IsNullOrWhiteSpace(filterExpression) &&
                 !String.IsNullOrWhiteSpace(sortExpression)) {

        operation = DataOperation.Parse("@qryEntitiesJoinedSorted", sourceName, joinedSourceName,
                                        sourceJoinField, joinedTargetField, sortExpression);

      } else if (!String.IsNullOrWhiteSpace(filterExpression) &&
                 !String.IsNullOrWhiteSpace(sortExpression)) {

        operation = DataOperation.Parse("@qryEntitiesJoinedFilteredAndSorted", sourceName, joinedSourceName,
                                        sourceJoinField, joinedTargetField, filterExpression, sortExpression);
      } else {
        throw Assertion.EnsureNoReachThisCode();
      }

      return DataReader.GetDataTable(operation);
    }

    static internal DataRow GetEntity(string sourceName, IFilter condition) {
      var operation = DataOperation.Parse("@qryEntitiesFiltered", sourceName, condition.Value);

      return DataReader.GetDataRow(operation);
    }


    static internal DataRow GetEntityById(string sourceName, string idFieldName, int entityId) {
      DataOperation operation = DataOperation.Parse("@getEntityByField", sourceName, idFieldName, entityId);

      return DataReader.GetDataRow(operation);
    }


    static internal DataRow GetEntityByKey(string sourceName, string idFieldName, string entityUniqueKey) {
      DataOperation operation = DataOperation.Parse("@getEntityByField", sourceName,
                                                    idFieldName, "'" + entityUniqueKey + "'");

      return DataReader.GetDataRow(operation);
    }

    #endregion Methods


    #region Helpers


    static private DataOperation GetEntitiesDataOperation(string sourceName,
                                                          string filterExpression = "",
                                                          string sortExpression = "") {

      if (String.IsNullOrWhiteSpace(filterExpression) &&
          String.IsNullOrWhiteSpace(sortExpression)) {

        return DataOperation.Parse("@qryEntities", sourceName);

      } else if (!String.IsNullOrWhiteSpace(filterExpression) &&
                 String.IsNullOrWhiteSpace(sortExpression)) {

        return DataOperation.Parse("@qryEntitiesFiltered", sourceName, filterExpression);

      } else if (String.IsNullOrWhiteSpace(filterExpression) &&
                 !String.IsNullOrWhiteSpace(sortExpression)) {

        return DataOperation.Parse("@qryEntitiesSorted", sourceName, sortExpression);

      } else if (!String.IsNullOrWhiteSpace(filterExpression) &&
                 !String.IsNullOrWhiteSpace(sortExpression)) {

        return DataOperation.Parse("@qryEntitiesFilteredAndSorted", sourceName,
                                        filterExpression, sortExpression);

      } else {
        throw Assertion.EnsureNoReachThisCode();
      }
    }

    #endregion Internal methods

  } // class OntologyDataHelpers

} // namespace Empiria.Data
