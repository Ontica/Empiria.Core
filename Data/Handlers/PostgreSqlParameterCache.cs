﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            Assembly : Empiria.Data.dll                  *
*  Type      : PostgreSqlParameterCache                         Pattern  : Static Class With Objects Cache   *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : This type is a wrapper of a static hash table that contains the loaded SqlParameters.         *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;

namespace Empiria.Data.Handlers {

  [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "8b7fe9c60c0f43bd")]
  static internal class PostgreSqlParameterCache {

    #region Fields

    static private Dictionary<string, Npgsql.NpgsqlParameter[]> parametersCache = new Dictionary<string, Npgsql.NpgsqlParameter[]>();

    #endregion Fields

    #region Internal methods

    static internal Npgsql.NpgsqlParameter[] GetParameters(string connectionString, string sourceName) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      Npgsql.NpgsqlParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        Npgsql.NpgsqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }
      if ((cachedParameters != null) && (cachedParameters.Length != 0)) {
        throw new EmpiriaDataException(EmpiriaDataException.Msg.WrongQueryParametersNumber, sourceName);
      }
      return CloneParameters(cachedParameters);
    }

    static internal Npgsql.NpgsqlParameter[] GetParameters(string connectionString, string sourceName,
                                                 object[] parameterValues) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      Npgsql.NpgsqlParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        Npgsql.NpgsqlParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
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

    static private Npgsql.NpgsqlParameter[] CloneParameters(Npgsql.NpgsqlParameter[] sourceParameters) {
      Npgsql.NpgsqlParameter[] clonedParameters = new Npgsql.NpgsqlParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (Npgsql.NpgsqlParameter) ((ICloneable) sourceParameters[i]).Clone();
      }
      return clonedParameters;
    }

    static private Npgsql.NpgsqlParameter[] CloneParameters(Npgsql.NpgsqlParameter[] sourceParameters,
                                                            object[] parameterValues) {
      Npgsql.NpgsqlParameter[] clonedParameters = new Npgsql.NpgsqlParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (Npgsql.NpgsqlParameter) ((ICloneable) sourceParameters[i]).Clone();
        clonedParameters[i].Value = parameterValues[i];
      }
      return clonedParameters;
    }

    static private Npgsql.NpgsqlParameter[] DiscoverParameters(string connectionString, string sourceName) {
      Npgsql.NpgsqlParameter[] discoveredParameters = null;

      using (Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(connectionString)) {
        Npgsql.NpgsqlCommand command = new Npgsql.NpgsqlCommand("qryDBPostgreSqlQueryParameters", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@QueryName", NpgsqlTypes.NpgsqlDbType.Varchar, 64);
        command.Parameters["@QueryName"].Value = sourceName;
        command.CommandType = CommandType.StoredProcedure;
        connection.Open();

        Npgsql.NpgsqlDataReader reader = command.ExecuteReader();
        Npgsql.NpgsqlParameter parameter;
        int i = 0;
        while (reader.Read()) {
          if (discoveredParameters == null) {
            discoveredParameters = new Npgsql.NpgsqlParameter[(int) reader["ParameterCount"]];
          }
          parameter = new Npgsql.NpgsqlParameter((string) reader["Name"], (SqlDbType) reader["ParameterSqlType"]);
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

  } // class PostgreSqlParameterCache

} // namespace Empiria.Data.Handlers