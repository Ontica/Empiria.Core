﻿/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Data Access Library               *
*  Namespace : Empiria.Data.Handlers                            Assembly : Empiria.Data.dll                  *
*  Type      : OracleParameterCache                             Pattern  : Static Class With Objects Cache   *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : This type is a wrapper of a static hash table that contains the loaded OracleParameters.      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Security.Permissions;

namespace Empiria.Data.Handlers {

  [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "8b7fe9c60c0f43bd")]
  static internal class OracleParameterCache {

    #region Fields

    static private Dictionary<string, OracleParameter[]> parametersCache = new Dictionary<string, OracleParameter[]>();

    #endregion Fields

    #region Internal methods

    static internal OracleParameter[] GetParameters(string connectionString, string sourceName) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      OracleParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        OracleParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
        parametersCache[hashKey] = spParameters;
        cachedParameters = spParameters;
      }
      return CloneParameters(cachedParameters);
    }

    static internal OracleParameter[] GetParameters(string connectionString, string sourceName,
                                                    object[] parameterValues) {
      string hashKey = BuildHashKey(connectionString, sourceName);

      OracleParameter[] cachedParameters = null;
      if (!parametersCache.TryGetValue(hashKey, out cachedParameters)) {
        OracleParameter[] spParameters = DiscoverParameters(connectionString, sourceName);
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

    static private OracleParameter[] CloneParameters(OracleParameter[] sourceParameters) {
      OracleParameter[] clonedParameters = new OracleParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (OracleParameter) ((ICloneable) sourceParameters[i]).Clone();
      }
      return clonedParameters;
    }

    static private OracleParameter[] CloneParameters(OracleParameter[] sourceParameters,
      object[] parameterValues) {
      OracleParameter[] clonedParameters = new OracleParameter[sourceParameters.Length];

      for (int i = 0, j = sourceParameters.Length; i < j; i++) {
        clonedParameters[i] = (OracleParameter) ((ICloneable) sourceParameters[i]).Clone();
        clonedParameters[i].Value = parameterValues[i];
      }
      return clonedParameters;
    }

    static private OracleParameter[] DiscoverParameters(string connectionString, string sourceName) {
      OracleCommand command = null;

      using (OracleConnection connection = new OracleConnection(connectionString)) {
        command = new OracleCommand(sourceName, connection);
        command.CommandType = CommandType.StoredProcedure;
        connection.Open();
        OracleCommandBuilder.DeriveParameters(command);
      }

      int discoveredCount = command.Parameters.Count;
      if (discoveredCount != 0) {
        OracleParameter[] discoveredParameters = new OracleParameter[discoveredCount];
        command.Parameters.CopyTo(discoveredParameters, 0);
        command.Parameters.Clear();
        for (int i = 0; i < discoveredCount; i++) {
          discoveredParameters[i].Value = DBNull.Value;
        }
        return discoveredParameters;
      } else {
        return null;
      }
    }

    #endregion Private methods

  } // class OracleParameterCache

} // namespace Empiria.Data.Handlers
