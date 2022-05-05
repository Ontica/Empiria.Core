/* Empiria Extensions ****************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                               Component : Data Access Library                     *
*  Assembly : Empiria.Core.dll                           Pattern   : Stored procedures parameters cache      *
*  Type     : SqlParameterCache                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Wrapper of a static hash table that contains loaded MS Sql Server stored procedure parameters. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;

namespace Empiria.Data.Handlers {

  /// <summary>Wrapper of a static hash table that contains loaded
  /// MS Sql Server stored procedure parameters./// </summary>
  static internal class SqlParameterCache {

    #region Fields

    static private Dictionary<string, SqlParameter[]> parametersCache = new Dictionary<string, SqlParameter[]>();

    #endregion Fields

    #region Internal methods

    static internal SqlParameter[] GetParameters(string connectionString, string sourceName) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      SqlParameter[] cachedParameters;

      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        SqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }

      return CloneParameters(cachedParameters);
    }


    static internal SqlParameter[] GetParameters(string connectionString, string sourceName,
                                                 object[] parameterValues) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      SqlParameter[] cachedParameters;

      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        SqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }

      return CloneParameters(cachedParameters, parameterValues);
    }

    #endregion Internal methods

    #region Private methods

    static private string BuildHashKey(string connectionString, string sourceName) {
      return connectionString + ":" + sourceName;
    }


    static private SqlParameter[] CloneParameters(SqlParameter[] sourceParameters) {
      SqlParameter[] clonedParameters = new SqlParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (SqlParameter) ((ICloneable) sourceParameters[i]).Clone();
      }

      return clonedParameters;
    }


    static private SqlParameter[] CloneParameters(SqlParameter[] sourceParameters,
                                                  object[] parameterValues) {
      SqlParameter[] clonedParameters = new SqlParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (SqlParameter) ((ICloneable) sourceParameters[i]).Clone();

        clonedParameters[i].Value = parameterValues[i];
      }

      return clonedParameters;
    }


    static private SqlParameter[] DiscoverParameters(string connectionString, string sourceName) {
      SqlParameter[] discoveredParameters = null;

      using (SqlConnection connection = new SqlConnection(connectionString)) {
        SqlCommand command = new SqlCommand("qryDbQueryParameters", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@QueryName", SqlDbType.VarChar, 64);
        command.Parameters["@QueryName"].Value = sourceName;
        command.CommandType = CommandType.StoredProcedure;

        SqlMethods.TryOpenConnection(connection);

        SqlDataReader reader = command.ExecuteReader();
        SqlParameter parameter;

        int i = 0;

        while (reader.Read()) {

          if (discoveredParameters == null) {
            discoveredParameters = new SqlParameter[(int) reader["ParameterCount"]];
          }

          parameter = new SqlParameter((string) reader["Name"], (SqlDbType) reader["ParameterDbType"]);

          parameter.Direction = (ParameterDirection) reader["ParameterDirection"];

          if (reader["ParameterDefaultValue"] == DBNull.Value) {
            parameter.Value = reader["ParameterDefaultValue"];
          }

          discoveredParameters[i] = parameter;

          i++;
        }
        reader.Close();
      }

      return discoveredParameters;
    }

    #endregion Private methods

  } // class SqlParameterCache

} // namespace Empiria.Data.Handlers
