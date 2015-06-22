/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : SecurityData                                     Pattern  : Data Services Static Class        *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides data read and write methods for the security namespace types.                        *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Security {

  static internal class SecurityData {

    static internal void ChangePassword(string username, string password) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", username));
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      // if (Land) {
        password = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, password, username);
        password = Cryptographer.Decrypt(password, username);

        password = Cryptographer.Encrypt(EncryptionMode.EntropyKey, password, username);
      //} else {

         //password"= Cryptographer.Encrypt(EncryptionMode.EntropyKey,
         //                                 Cryptographer.GetMD5HashCode(password), username);


      //}

      string sql = "UPDATE Contacts SET UserPassword = '{0}' WHERE UserName = '{1}'";

      DataWriter.Execute(DataOperation.Parse(String.Format(sql, password, username)));
    }

    static internal void ChangePassword(string apiKey, string username, string password) {
      if (apiKey != ConfigurationData.GetString("ChangePasswordKey")) {
        throw new SecurityException(SecurityException.Msg.InvalidClientAppKey, apiKey);
      }
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", username));
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      string p = Cryptographer.Encrypt(EncryptionMode.EntropyKey,
                                       Cryptographer.GetMD5HashCode(password), username);

      string sql = "UPDATE Contacts SET UserPassword = '{0}' WHERE UserName = '{1}'";

      DataWriter.Execute(DataOperation.Parse(String.Format(sql, p, username)));
    }

    static internal int GetNextSessionId() {
      return DataWriter.CreateId("UserSessions");
    }

    static internal DataRow GetSessionData(string sessionToken) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getUserSession", sessionToken));

      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.SessionTokenNotFound, sessionToken);
      }
      return dataRow;
    }

    internal static string[] GetUsersInRole(string role) {
      return ConfigurationData.GetString("User.Operation.Tag." + role).Split('|');
    }

    static internal DataRow GetUserWithCredentials(string username, string password, string entropy) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", username));

      //No user/password found
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      string p = Cryptographer.Decrypt((string) dataRow["UserPassword"], username);
      if (!String.IsNullOrWhiteSpace(entropy)) {
        //Password rule w/entropy = MD5(MD5(secret) + entropy), else password rule = MD5(secret)
        p = Cryptographer.GetMD5HashCode(p + entropy);
      }

      //Invalid password
      if (p != password) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }
      dataRow.Table.Columns.Remove("UserPassword");

      return dataRow;
    }

    static internal long WriteAuditTrail(AuditTrail o) {
      var op = DataOperation.Parse("apdAuditTrail", o.Request.Guid, (char) o.AuditTrailType,
                                   o.Timestamp, o.SessionId, o.Event, o.Operation,
                                   o.OperationData.ToString(), o.Request.AppliedToId, o.ResponseCode,
                                   o.ResponseItems, o.ResponseTime, o.ResponseData.ToString());
      return DataWriter.Execute(op);
    }


    static internal int WriteAuthorization(Authorization o) {
      var operation = DataOperation.Parse("writeAuthorization", o.Guid, o.TypeId, o.ReasonId,
                                          o.ObjectId, o.AuthorizedById, o.SessionToken, o.Code,
                                          o.Observations, o.Date);
      return DataWriter.Execute(operation);
    }

    static internal int WriteSession(EmpiriaSession o) {
      Assertion.Assert(o.Id != 0, "Session.Id was not assigned.");

      var op = DataOperation.Parse("writeUserSession", o.Id, o.Token, o.ServerId,
                                    o.ClientAppId, o.UserId, o.ExpiresIn,
                                    o.RefreshToken, o.ExtendedData.ToString(),
                                    o.StartTime, o.EndTime);
      return DataWriter.Execute(op);
    }

  } // class SecurityData

} // namespace Empiria.Security.Data
