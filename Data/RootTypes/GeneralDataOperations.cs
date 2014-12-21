/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : GeneralDataOperations                            Pattern  : Data Services Static Class        *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static library with general purpose data read operations.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Data {

  /// <summary>Static library with general purpose data read operations.</summary>
  static public class GeneralDataOperations {

    #region Public properties

    static public string NoRecordsFilter {
      get {
        return "(1 = 0)";
      }
    }

    #endregion Public properties

    #region Public methods

    static public string BuildSqlAndFilter(string firstFilter, string secondFilter, params string[] otherFilters) {
      string filter = String.Empty;

      firstFilter = FormatFilterString(firstFilter);
      secondFilter = FormatFilterString(secondFilter);

      if (firstFilter.Length != 0 && secondFilter.Length != 0) {
        filter = firstFilter + " AND " + secondFilter;
      } else if (firstFilter.Length != 0 && secondFilter.Length == 0) {
        filter = firstFilter;
      } else if (firstFilter.Length == 0 && secondFilter.Length != 0) {
        filter = secondFilter;
      } else if (firstFilter.Length == 0 && secondFilter.Length == 0) {
        filter = string.Empty;
      } else {
        Assertion.AssertNoReachThisCode();
      }

      for (int i = 0; i < otherFilters.Length; i++) {
        otherFilters[i] = FormatFilterString(otherFilters[i]);
        if (otherFilters[i].Length == 0) {
          continue;
        }
        if (filter.Length != 0) {
          filter += " AND ";
        }
        filter += otherFilters[i];
      }
      return filter;
    }

    static public string BuildSqlInSetClause(string fieldName, int[] values) {
      if (values == null || values.Length == 0) {
        return String.Empty;
      }
      if (values.Length == 1) {
        return fieldName + " = " + values[0];
      }
      string sql = String.Empty;
      foreach (int value in values) {
        if (sql.Length != 0) {
          sql += ",";
        }
        sql += value;
      }
      return fieldName + " IN (" + sql + ")";
    }

    static public string BuildSqlInSetClause(string fieldName, string[] values) {
      if (values == null || values.Length == 0) {
        return String.Empty;
      }
      if (values.Length == 1) {
        return fieldName + " = '" + values[0] + "'";
      }
      string sql = String.Empty;
      foreach (string value in values) {
        if (sql.Length != 0) {
          sql += ",";
        }
        sql += "'" + value + "'";
      }
      return fieldName + " IN (" + sql + ")";
    }

    static public string BuildSqlOrFilter(string firstFilter, string secondFilter, params string[] otherFilters) {
      string filter = String.Empty;

      firstFilter = FormatFilterString(firstFilter);
      secondFilter = FormatFilterString(secondFilter);

      if (firstFilter.Length != 0 && secondFilter.Length != 0) {
        filter = firstFilter + " OR " + secondFilter;
      } else if (firstFilter.Length != 0 && secondFilter.Length == 0) {
        filter = firstFilter;
      } else if (firstFilter.Length == 0 && secondFilter.Length != 0) {
        filter = secondFilter;
      } else if (firstFilter.Length == 0 && secondFilter.Length == 0) {
        filter = string.Empty;
      } else {
        Assertion.AssertNoReachThisCode();
      }

      for (int i = 0; i < otherFilters.Length; i++) {
        otherFilters[i] = FormatFilterString(otherFilters[i]);
        if (otherFilters[i].Length == 0) {
          continue;
        }
        if (filter.Length != 0) {
          filter += " OR ";
        }
        filter += otherFilters[i];
      }
      return filter;
    }

    static public DataOperation CountEntities(string sourceName, string filterExpression = "") {
      if (!String.IsNullOrWhiteSpace(filterExpression)) {
        return DataOperation.Parse("@getCountEntities", sourceName);
      } else {
        return DataOperation.Parse("@getCountEntitiesFiltered", sourceName, filterExpression);
      }
    }

    static public string FormatFilterString(string filter) {
      string temp = String.IsNullOrWhiteSpace(filter) ? String.Empty : filter;

      temp = temp.Trim();
      if (temp.Length != 0 && !temp.StartsWith("(")) {
        temp = "(" + temp + ")";
      }
      return temp;
    }

    static public string FormatSortString(string sort) {
      return String.IsNullOrWhiteSpace(sort) ? String.Empty : sort;
    }

    static public DataTable GetEntities(string sourceName, string filterExpression = "",
                                        string sortExpression = "") {
      DataOperation operation = null;

      if (String.IsNullOrWhiteSpace(filterExpression) &&
          String.IsNullOrWhiteSpace(sortExpression)) {
        operation = DataOperation.Parse("@qryEntities", sourceName);
      } else if (!String.IsNullOrWhiteSpace(filterExpression) &&
                 String.IsNullOrWhiteSpace(sortExpression)) {
        operation = DataOperation.Parse("@qryEntitiesFiltered", sourceName, filterExpression);
      } else if (String.IsNullOrWhiteSpace(filterExpression) &&
                 !String.IsNullOrWhiteSpace(sortExpression)) {
        operation = DataOperation.Parse("@qryEntitiesSorted", sourceName, sortExpression);
      } else if (!String.IsNullOrWhiteSpace(filterExpression) &&
                 !String.IsNullOrWhiteSpace(sortExpression)) {
        operation = DataOperation.Parse("@qryEntitiesFilteredAndSorted", sourceName,
                                        filterExpression, sortExpression);
      } else {
        Assertion.AssertNoReachThisCode();
      }
      return DataReader.GetDataTable(operation);
    }

 
    static public DataTable GetEntitiesByField(string sourceName, string searchFieldName, int fieldValue) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesFiltered", sourceName,
                                                    searchFieldName + " = " + fieldValue.ToString());
      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetEntitiesByField(string sourceName, string searchFieldName, string fieldValue) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesFiltered", sourceName,
                                                    searchFieldName + " = '" + fieldValue + "'");
      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetEntitiesJoined(string sourceName, string joinedSourceName,
                                              string sourceJoinField, string joinedTargetField,
                                              string filterExpression = "", string sortExpression = "") {
      DataOperation operation = null;

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
        Assertion.AssertNoReachThisCode();
      }      
      return DataReader.GetDataTable(operation);
    }

    static public DataRow GetEntity(string sourceName, IFilter condition) {
      var operation = DataOperation.Parse("@qryEntitiesFiltered", sourceName, condition.Value);

      return DataReader.GetDataRow(operation);
    }

    static public DataRow GetEntityById(string sourceName, string idFieldName, int entityId) {
      DataOperation operation = DataOperation.Parse("@getEntityByField", sourceName, idFieldName, entityId);

      return DataReader.GetDataRow(operation);
    }

    static public DataRow GetEntityByIdFiltered(string sourceName, string idFieldName,
                                                int entityId, string filterExpression) {
      DataOperation operation = DataOperation.Parse("@getEntityByFieldFiltered", sourceName, idFieldName,
                                                    entityId, filterExpression);
      return DataReader.GetDataRow(operation);
    }

    static public DataRow GetEntityByKey(string sourceName, string idFieldName, string entityUniqueKey) {
      DataOperation operation = DataOperation.Parse("@getEntityByField", sourceName,
                                                    idFieldName, "'" + entityUniqueKey + "'");

      return DataReader.GetDataRow(operation);
    }

    static public DataRow GetEntityByKeyFiltered(string sourceName, string idFieldName,
                                                 string entityUniqueKey, string filterExpression) {
      DataOperation operation = DataOperation.Parse("@getEntityByFieldFiltered", sourceName, idFieldName,
                                                    "'" + entityUniqueKey + "'", filterExpression);

      return DataReader.GetDataRow(operation);
    }

    static public int GetEntityId(string sourceName, string entityIdFieldName,
                                  string searchFieldName, string searchValue) {
      DataOperation operation = DataOperation.Parse("@getEntityField", sourceName, entityIdFieldName,
                                                    searchFieldName + " = '" + searchValue + "'");
      return DataReader.GetScalar<int>(operation);
    }

    static public T GetEntityField<T>(string sourceName, string selectedFieldName,
                                      string entityIdFieldName, int entityId) {
      DataOperation dataOperation = DataOperation.Parse("@getEntityField", sourceName, selectedFieldName,
                                                         entityIdFieldName + " = " + entityId.ToString());
      return DataReader.GetScalar<T>(dataOperation);
    }

    static public string GetFilterSortSqlString(string filter, string sort) {
      if (String.IsNullOrWhiteSpace(filter) && String.IsNullOrWhiteSpace(sort)) {
        return String.Empty;
      } else if (String.IsNullOrWhiteSpace(filter) && !String.IsNullOrWhiteSpace(sort)) {
        return " ORDER BY " + sort;
      } else if (!String.IsNullOrWhiteSpace(filter) && String.IsNullOrWhiteSpace(sort)) {
        return " WHERE " + filter;
      } else if (!String.IsNullOrWhiteSpace(filter) && !String.IsNullOrWhiteSpace(sort)) {
        return " WHERE " + filter + " ORDER BY " + sort;
      } else {
        throw Assertion.AssertNoReachThisCode();
      }
    }

    static public DataOperation SelectEntityUniqueFieldValues(string sourceName, 
                                                              string returnFieldName) {
      return DataOperation.Parse("@qryEntityUniqueFieldValues", sourceName, returnFieldName);
    }

    #endregion Public methods

  } // class GeneralDataOperations

} // namespace Empiria.Data
