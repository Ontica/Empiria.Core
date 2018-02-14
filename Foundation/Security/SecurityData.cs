/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : SecurityData                                     Pattern  : Data Services Static Class        *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides data read and write methods for the security namespace types.                        *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data;

namespace Empiria.Security {

  static internal class SecurityData {

    static internal void ActivateUser(EmpiriaUser empiriaUser) {
      string sql = "UPDATE Contacts SET ContactStatus = 'A' WHERE ContactId = " + empiriaUser.Id;

      DataWriter.Execute(DataOperation.Parse(sql));
    }

    static internal void ChangePassword(string username, string password) {
      if (ConfigurationData.Get("UseFormerPasswordEncryption", false)) {
        ChangePasswordUsingFormerEncryption(username, password);
        return;
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

    static private void ChangePasswordUsingFormerEncryption(string username, string password) {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", username));
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      password = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, password, username);
      password = Cryptographer.Decrypt(password, username);

      password = Cryptographer.Encrypt(EncryptionMode.EntropyKey, password, username);

      // Warning: This is the former encryption model (before Empiria v6.0)
      // password"= Cryptographer.Encrypt(EncryptionMode.EntropyKey,
      //                                  Cryptographer.GetMD5HashCode(password), username);

      string sql = "UPDATE Contacts SET UserPassword = '{0}' WHERE UserName = '{1}'";

      DataWriter.Execute(DataOperation.Parse(String.Format(sql, password, username)));
    }

    static internal SecurityClaim GetPendingSecurityClaim(SecurityClaimType claimType,
                                                          int resourceTypeId, int resourceId) {
      string sql = "SELECT * FROM SecurityClaims WHERE ClaimTypeId = {0} AND " +
                   "ResourceTypeId = {1} AND ResourceId = {2} AND ClaimStatus = 'P'";

      sql = String.Format(sql, claimType.Id, resourceTypeId, resourceId);

      DataRow row = DataReader.GetDataRow(DataOperation.Parse(sql));

      if (row == null) {
        throw new SecurityException(SecurityException.Msg.ResourceClaimNotFound,
                                    claimType.Type, resourceId, "?");
      }

      return BaseObject.ParseDataRow<SecurityClaim>(row);
    }

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

    static internal void CreateUser(EmpiriaUser o, string password, ObjectStatus status) {
      Assertion.Assert(o.Id != 0, "User.Id was not assigned.");
      Assertion.AssertObject(password, "Password can't be null.");

      string p = Cryptographer.Encrypt(EncryptionMode.EntropyKey,
                                       Cryptographer.GetMD5HashCode(password), o.UserName);

      var op = DataOperation.Parse("writeContact", o.Id, o.FullName, o.UserName,
                                   p, o.EMail, o.GetExtendedData().ToString(), (char) status);

      DataWriter.Execute(op);
    }

    static internal int GetNextContactId() {
      return DataWriter.CreateId("Contacts");
    }

    static internal int GetNextSessionId() {
      return DataWriter.CreateId("UserSessions");
    }

    static internal List<SecurityClaim> GetSecurityClaims(int resourceTypeId, IIdentifiable resource) {
      Assertion.AssertObject(resource, "resource");

      var dataTable = DataReader.GetDataTable(DataOperation.Parse("qryResourceSecurityClaims",
                                                                  resourceTypeId, resource.Id));

      return BaseObject.ParseList<SecurityClaim>(dataTable);
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

    static internal DataRow GetUserWithCredentials(string userName, string password, string entropy = "") {
      var dataRow = DataReader.GetDataRow(DataOperation.Parse("getContactWithUserName", userName));

      //No user/password found
      if (dataRow == null) {
        throw new SecurityException(SecurityException.Msg.InvalidUserCredentials);
      }

      string p = Cryptographer.Decrypt((string) dataRow["UserPassword"], userName);
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

    internal static void RemoveClaim(SecurityClaim claim) {
      string sql = "UPDATE SecurityClaims SET ClaimStatus = 'X' WHERE SecurityClaimId = " + claim.Id;

      DataWriter.Execute(DataOperation.Parse(sql));
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

    static internal void WriteSecurityClaim(SecurityClaim o) {
      var op = DataOperation.Parse("writeSecurityClaim", o.Id, o.ClaimType.Id, o.ResourceTypeId,
                                   o.ResourceId, o.Value, (char) o.Status);

      DataWriter.Execute(op);
    }

  } // class SecurityData

} // namespace Empiria.Security.Data
