/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security.Data                            Assembly : Empiria.dll                       *
*  Type      : AttributeReader                                  Pattern  : Data Services Static Class        *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Provides data read methods for security information.                                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Security {

  static internal class SecurityData {

    static internal void ChangePassword(string apiKey, string username, string password) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", username));
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.UserIDPasswordNotFound);
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

    static internal DataRow GetUserWithCredentials(string username, string password, string entropy) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", username));

      //No user found
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.UserIDPasswordNotFound);
      }

      string p = Cryptographer.Decrypt((string) dataRow["UserPassword"], username);
      if (!String.IsNullOrWhiteSpace(entropy)) {
        //Password rule w/entropy = MD5(MD5(secret) + entropy, else password rule = MD5(secret)
        p = Cryptographer.GetMD5HashCode(p + entropy);
      }

      //Invalid password
      if (p != password) {
        throw new SecurityException(SecurityException.Msg.UserIDPasswordNotFound);
      }
      dataRow.Table.Columns.Remove("UserPassword");

      return dataRow;
    }

    static internal int WriteAuditTrail(AuditTrail o) {
      var operation = DataOperation.Parse("apdAuditTrail", o.Id, (char) o.AuditTrailType,
                                          o.SessionId, o.Event, o.Operation, (char) o.Result,
                                          o.InstanceId, o.Data.ToString(), o.Timestamp);
      return DataWriter.Execute(operation);
    }

    static internal int WriteAuthorization(Authorization o) {
      var operation = DataOperation.Parse("writeAuthorization", o.Guid, o.TypeId, o.ReasonId,
                                          o.ObjectId, o.AuthorizedById, o.SessionToken, o.Code,
                                          o.Observations, o.Date);
      return DataWriter.Execute(operation);
    }

    static internal int WriteSession(EmpiriaSession o) {
      Assertion.Assert(o.Id != 0, "Session.Id was not assigned.");

      var operation = DataOperation.Parse("writeUserSession", o.Id, o.Token, o.ServerId,
                                          o.ClientAppId, o.UserId, o.ExpiresIn,
                                          o.RefreshToken, o.ExtendedData.ToString(),
                                          o.StartTime, o.EndTime);
      return DataWriter.Execute(operation);
    }

  } // class SecurityData

} // namespace Empiria.Security.Data
