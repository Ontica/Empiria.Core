﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            License  : Please read LICENSE.txt file      *
*  Type      : SqlParameterCache                                Pattern  : Static Class With Objects Cache   *
*                                                                                                            *
*  Summary   : This type is a wrapper of a static hash table that contains the loaded SqlParameters.         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Empiria.Data.Handlers {

  static internal class SqlParameterCache {

    #region Fields

    static private Dictionary<string, SqlParameter[]> parametersCache = new Dictionary<string, SqlParameter[]>();

    #endregion Fields

    #region Internal methods

    static internal SqlParameter[] GetParameters(string connectionString, string sourceName) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      SqlParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        SqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }
      if ((cachedParameters != null) && (cachedParameters.Length != 0)) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.WrongQueryParametersNumber, sourceName);
      }
      return CloneParameters(cachedParameters);
    }

    static internal SqlParameter[] GetParameters(string connectionString, string sourceName,
                                                 object[] parameterValues) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      SqlParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        SqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }
      if ((cachedParameters == null) || (cachedParameters.Length != parameterValues.Length)) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.WrongQueryParametersNumber, sourceName);
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
        if (parameterValues[i] is IValueObject) {
          clonedParameters[i].Value = parameterValues[i].ToString();
        } else if (parameterValues[i] is IIdentifiable) {
          clonedParameters[i].Value = ((IIdentifiable) parameterValues[i]).Id;
        } else {
          clonedParameters[i].Value = parameterValues[i];
        }
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
        connection.Open();

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
