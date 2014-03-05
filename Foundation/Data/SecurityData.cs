/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security.Data                            Assembly : Empiria.dll                       *
*  Type      : AttributeReader                                  Pattern  : Data Services Static Class        *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Provides data read methods for security information.                                          *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Data;

namespace Empiria.Security {

  static internal class SecurityData {

    static internal System.Data.DataRow GetSessionData(string sessionToken) {
      string sql = "SELECT * FROM EMPSessions WHERE SessionToken = '{0}'";
      sql = String.Format(sql, sessionToken);

      return DataReader.GetDataRow(DataOperation.Parse(sql));
    }

    static internal DataOperation GetWriteAuditTrailOperation(int typeId, int objectId,
                                                            char operationChar, string observations) {
      return DataOperation.Parse("writeEMPAuditTrail", ExecutionServer.CurrentIdentity.Session.Id,
                                 typeId, objectId, operationChar, DateTime.Now, observations);
    }

    static internal int WriteAuthorization(Authorization o) {
      DataOperation operation = DataOperation.Parse("writeEMPAuthorization", o.Guid, o.TypeId, o.ReasonId,
                                                     o.ObjectId, o.AuthorizedById, o.SessionToken, o.Code,
                                                     o.Observations, o.Date);
      return DataWriter.Execute(operation);
    }

    static internal int WriteSession(EmpiriaSession o) {
      if (o.Id == 0) {
        o.Id = Empiria.Data.DataWriter.CreateId("EMPSessions");
      }
      return DataWriter.Execute(DataOperation.Parse("writeEMPSession", o.Id, o.Token, o.ServerId, o.UserId, o.SystemSession,
                                                     o.ClientAddress, o.ClientEnvironment, o.StartTime, o.EndTime));
    }

  } // class SecurityData

} // namespace Empiria.Security.Data