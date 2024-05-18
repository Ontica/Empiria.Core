/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Data                               Component : Data Accesss Layer                      *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : DataCommonMethods                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static common methods used to get or format data for databases.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Data {

  /// <summary>Static common methods used to get or format data for databases.</summary>
  static public class DataCommonMethods {

    #region Public methods

    static public string FormatSqlDbDate(DateTime date) {
      string dateAsString = date.Date.ToString("yyyy-MM-dd");

      return $"TO_DATE('{dateAsString}', 'yyyy-MM-dd')";
    }


    static public string FormatSqlDbDateTime(DateTime date) {
      string dateTimeAsString = date.ToString("yyyy-MM-dd HH:mm:ss");

      return $"TO_DATE('{dateTimeAsString}', 'yyyy-MM-dd hh24:mi:ss')";
    }


    static public long GetNextObjectId(string sequenceName) {
      var sql = $"SELECT {sequenceName}.NEXTVAL FROM DUAL";

      var operation = DataOperation.Parse(sql);

      return Convert.ToInt64(DataReader.GetScalar<decimal>(operation));
    }

    #endregion Public methods

  }  // class DataCommonMethods

}  // namespace Empiria.Data
