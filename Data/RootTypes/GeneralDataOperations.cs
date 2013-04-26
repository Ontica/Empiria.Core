/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data                                     Assembly : Empiria.Data.dll                  *
*  Type      : GeneralDataOperations                            Pattern  : Data Services Static Class        *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Static library with general purpose data read operations.                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System.Data;

namespace Empiria.Data {

  /// <summary>Static library with general purpose data read operations.</summary>
  static public class GeneralDataOperations {

    #region Public methods

    static public DataOperation CountEntities(string sourceName) {
      return DataOperation.Parse("@getCountEntities", sourceName);
    }

    static public DataOperation CountEntitiesFiltered(string sourceName, string filterExpression) {
      return DataOperation.Parse("@getCountEntitiesFiltered", sourceName, filterExpression);
    }

    static public DataTable GetEntities(string sourceName) {
      return DataReader.GetDataTable(DataOperation.Parse("@qryEntities", sourceName));
    }

    static public DataTable GetEntities(string sourceName, string filterExpression) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesFiltered", sourceName, filterExpression);

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetEntities(string sourceName, string filterExpression,
                                        string sortExpression) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesFilteredAndSorted", sourceName,
                                                    filterExpression, sortExpression);
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
                                              string sourceJoinField, string joinedTargetField) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesJoined", sourceName, joinedSourceName,
                                                    sourceJoinField, joinedTargetField);

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetEntitiesJoined(string sourceName, string joinedSourceName,
                                              string sourceJoinField, string joinedTargetField,
                                              string filterExpression) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesJoinedFiltered", sourceName, joinedSourceName,
                                                    sourceJoinField, joinedTargetField, filterExpression);

      //Empiria.Messaging.Publisher.Publish(operation.GetSourceFull());

      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetEntitiesJoined(string sourceName, string joinedSourceName,
                                              string sourceJoinField, string joinedTargetField,
                                              string filterExpression, string sortExpression) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesJoinedFilteredAndSorted", sourceName, joinedSourceName,
                                                    sourceJoinField, joinedTargetField, filterExpression, sortExpression);
      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetEntitiesJoinedSorted(string sourceName, string joinedSourceName,
                                                    string sourceJoinField, string joinedTargetField,
                                                    string sortExpression) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesJoinedSorted", sourceName, joinedSourceName,
                                                    sourceJoinField, joinedTargetField, sortExpression);
      return DataReader.GetDataTable(operation);
    }

    static public DataTable GetEntitiesSorted(string sourceName, string sortExpression) {
      DataOperation operation = DataOperation.Parse("@qryEntitiesSorted", sourceName, sortExpression);

      return DataReader.GetDataTable(operation);
    }

    static public DataRow GetEntityById(string sourceName, string idFieldName, int entityId) {
      DataOperation operation = DataOperation.Parse("@getEntityByField", sourceName, idFieldName, entityId);

      return DataReader.GetDataRow(operation);
    }

    static public DataRow GetEntityById(string sourceName, string idFieldName, string entityId) {
      DataOperation operation = DataOperation.Parse("@getEntityByField", sourceName,
                                                    idFieldName, "'" + entityId + "'");

      return DataReader.GetDataRow(operation);
    }

    static public DataRow GetEntityByIdFiltered(string sourceName, string idFieldName,
                                                int entityId, string filterExpression) {
      DataOperation operation = DataOperation.Parse("@getEntityByFieldFiltered", sourceName, idFieldName,
                                                    entityId, filterExpression);
      return DataReader.GetDataRow(operation);
    }

    static public DataRow GetEntityByIdFiltered(string sourceName, string idFieldName,
                                                string entityId, string filterExpression) {
      DataOperation operation = DataOperation.Parse("@getEntityByFieldFiltered", sourceName, idFieldName,
                                                    "'" + entityId + "'", filterExpression);

      return DataReader.GetDataRow(operation);
    }

    static public int GetEntityId(string sourceName, string entityIdFieldName,
                                  string searchFieldName, string searchValue) {
      DataOperation operation = DataOperation.Parse("@getEntityField", sourceName, entityIdFieldName,
                                                    searchFieldName + " = '" + searchValue + "'");
      return DataReader.GetObjectId(operation);
    }

    static public T GetEntityField<T>(string sourceName, string selectedFieldName,
                                         string entityIdFieldName, int entityId) {
      DataOperation dataOperation = DataOperation.Parse("@getEntityField", sourceName, selectedFieldName,
                                                         entityIdFieldName + " = " + entityId.ToString());
      return (T) DataReader.GetScalar(dataOperation);
    }

    static public DataOperation SelectEntityUniqueFieldValues(string sourceName, string returnFieldName) {
      return DataOperation.Parse("@qryEntityUniqueFieldValues", sourceName, returnFieldName);
    }

    #endregion Public methods

  } // class GeneralDataOperations

} // namespace Empiria.Data
