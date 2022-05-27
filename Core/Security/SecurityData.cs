/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Contacts Management               *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : SecurityData                                     Pattern  : Data Services Static Class        *
*                                                                                                            *
*  Summary   : Provides data read and write methods for the security namespace types.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Security {

  static internal class SecurityData {

     static internal void CloseSession(EmpiriaSession o) {
      var op = DataOperation.Parse("doCloseUserSession", o.Token, o.EndTime);

      DataWriter.Execute(op);
    }

    static internal int CreateSession(EmpiriaSession o) {
      var op = DataOperation.Parse("apdUserSession", o.Token, o.ServerId,
                                    o.ClientAppId, o.UserId, o.ExpiresIn,
                                    o.RefreshToken, o.ExtendedData.ToString(),
                                    o.StartTime, o.EndTime);
      return DataWriter.Execute<int>(op);
    }


    static internal int GetNextContactId() {
      return DataWriter.CreateId("Contacts");
    }


    static internal DataRow GetSessionData(string sessionToken) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getUserSession", sessionToken));

      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.SessionTokenNotFound, sessionToken);
      }
      return dataRow;
    }

    internal static string[] GetUsersInRole(string role) {
      var roles = ConfigurationData.Get($"User.Role.{role}", string.Empty);

      if (roles.Length != 0) {
        return roles.Split('|');
      }
      return new string[0];
    }

    static internal DataRow GetUserWithCredentials(string userName, string password, string entropy) {
      var operation = DataOperation.Parse("getContactWithUserName", userName);

      var dataRow = DataReader.GetDataRow(operation);

      //No user/password found
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      bool useSecurityModelV3 = ConfigurationData.Get("UseSecurityModel.V3", false);

      string p;

      if (useSecurityModelV3) {
        p = Cryptographer.Decrypt((string) dataRow["UserPassword"], userName);
        p = Cryptographer.GetSHA256(p + entropy);

      } else if (!String.IsNullOrWhiteSpace(entropy)) {
        p = FormerCryptographer.Decrypt((string) dataRow["UserPassword"], userName);
        p = FormerCryptographer.GetMD5HashCode(p + entropy);

      } else {
        p = FormerCryptographer.Decrypt((string) dataRow["UserPassword"], userName);
      }

      //Invalid password
      if (p != password) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      dataRow.Table.Columns.Remove("UserPassword");

      return dataRow;
    }


    internal static FixedList<int> GetUsersWithDataAccessTo<T>(Type type, T entity) where T : IIdentifiable {
      return new FixedList<int>();
    }


    static internal EmpiriaUser TryGetUserWithUserName(string userName) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", userName));

      if (dataRow != null) {
        return BaseObject.ParseDataRow<EmpiriaUser>(dataRow);
      } else {
        return null;
      }
    }

    static internal long WriteAuditTrail(AuditTrail o) {
      var op = DataOperation.Parse("apdAuditTrail", o.Request.Guid, (char) o.AuditTrailType,
                                   o.Timestamp, o.SessionId, o.Event, o.Operation,
                                   o.OperationData.ToString(), o.Request.AppliedToId, o.ResponseCode,
                                   o.ResponseItems, o.ResponseTime, o.ResponseData.ToString());

      return DataWriter.Execute<long>(op);
    }

  } // class SecurityData

} // namespace Empiria.Security.Data
