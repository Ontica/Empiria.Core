﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            License  : Please read LICENSE.txt file      *
*  Type      : OleDbParameterCache                              Pattern  : Static Class With Objects Cache   *
*                                                                                                            *
*  Summary   : This type is a wrapper of a static hash table that contains the loaded OleDbParameters.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Empiria.Data.Handlers {

  static internal class OleDbParameterCache {

    #region Fields

    static private Dictionary<string, OleDbParameter[]> parametersCache = new Dictionary<string, OleDbParameter[]>();

    #endregion Fields

    #region Internal methods

    static internal OleDbParameter[] GetParameters(string connectionString, string sourceName) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      OleDbParameter[] cachedParameters;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        OleDbParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }
      return CloneParameters(cachedParameters);
    }

    static internal OleDbParameter[] GetParameters(string connectionString, string sourceName,
                                                   object[] parameterValues) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      OleDbParameter[] cachedParameters;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        OleDbParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
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

    static private OleDbParameter[] CloneParameters(OleDbParameter[] sourceParameters) {
      OleDbParameter[] clonedParameters = new OleDbParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (OleDbParameter) ((ICloneable) sourceParameters[i]).Clone();
      }
      return clonedParameters;
    }

    static private OleDbParameter[] CloneParameters(OleDbParameter[] sourceParameters,
                                                    object[] parameterValues) {
      OleDbParameter[] clonedParameters = new OleDbParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (OleDbParameter) ((ICloneable) sourceParameters[i]).Clone();
        clonedParameters[i].Value = parameterValues[i];
      }
      return clonedParameters;
    }

    static private OleDbParameter[] DiscoverParameters(string connectionString, string sourceName) {
      OleDbParameter[] discoveredParameters = null;

      using (OleDbConnection connection = new OleDbConnection(connectionString)) {
        OleDbCommand command = new OleDbCommand("qryDbQueryParameters", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("pQueryName", OleDbType.VarChar, 128);
        command.Parameters["pQueryName"].Value = sourceName;
        command.CommandType = CommandType.StoredProcedure;
        connection.Open();

        OleDbDataReader reader = command.ExecuteReader();
        OleDbParameter parameter;
        int i = 0;
        while (reader.Read()) {
          if (discoveredParameters == null) {
            discoveredParameters = new OleDbParameter[(int) reader["ParameterCount"]];
          }
          parameter = new OleDbParameter((string) reader["Name"], (OleDbType) reader["ParameterDbType"]);
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

  } // class OleDbParameterCache

} // namespace Empiria.Data.Handlers
