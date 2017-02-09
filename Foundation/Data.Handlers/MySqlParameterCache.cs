﻿/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            Assembly : Empiria.Data.dll                  *
*  Type      : MySqlParameterCache                              Pattern  : Static Class With Objects Cache   *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : This type is a wrapper of a static hash table that contains loaded MySql query parameters.    *
*                                                                                                            *
********************************* Copyright (c) 2006-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Empiria.Data.Handlers {

  static internal class MySqlParameterCache {

    #region Fields

    static private Dictionary<string, MySqlParameter[]> parametersCache = new Dictionary<string, MySqlParameter[]>();

    #endregion Fields

    #region Internal methods

    static internal MySqlParameter[] GetParameters(string connectionString, string sourceName) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      MySqlParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        MySqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }
      return CloneParameters(cachedParameters);
    }

    static internal MySqlParameter[] GetParameters(string connectionString, string sourceName,
                                                   object[] parameterValues) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      MySqlParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        MySqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
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

    static private MySqlParameter[] CloneParameters(MySqlParameter[] sourceParameters) {
      MySqlParameter[] clonedParameters = new MySqlParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (MySqlParameter) ((ICloneable) sourceParameters[i]).Clone();
      }
      return clonedParameters;
    }

    static private MySqlParameter[] CloneParameters(MySqlParameter[] sourceParameters,
                                                  object[] parameterValues) {
      MySqlParameter[] clonedParameters = new MySqlParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (MySqlParameter) ((ICloneable) sourceParameters[i]).Clone();
        clonedParameters[i].Value = parameterValues[i];
      }
      return clonedParameters;
    }

    static private MySqlParameter[] DiscoverParameters(string connectionString, string sourceName) {
      MySqlParameter[] discoveredParameters = null;

      using (MySqlConnection connection = new MySqlConnection(connectionString)) {
        MySqlCommand command = new MySqlCommand("qryDbQueryParameters", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@QueryName", MySqlDbType.VarChar, 64);
        command.Parameters["@QueryName"].Value = sourceName;
        command.CommandType = CommandType.StoredProcedure;
        connection.Open();

        MySqlDataReader reader = command.ExecuteReader();
        MySqlParameter parameter;
        int i = 0;
        while (reader.Read()) {
          if (discoveredParameters == null) {
            discoveredParameters = new MySqlParameter[(int) reader["ParameterCount"]];
          }
          parameter = new MySqlParameter((string) reader["Name"], (MySqlDbType) reader["ParameterDbType"]);
          parameter.Direction = (ParameterDirection) reader["ParameterDirection"];
          if (!(reader["ParameterDefaultValue"] != System.DBNull.Value)) {
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

  } // class MySqlParameterCache

} // namespace Empiria.Data.Handlers