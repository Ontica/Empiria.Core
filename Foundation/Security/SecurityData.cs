/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security.Data                            Assembly : Empiria.dll                       *
*  Type      : AttributeReader                                  Pattern  : Data Services Static Class        *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Provides data read methods for security information.                                          *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

using Empiria.Data;

namespace Empiria.Security {

  internal sealed class SecurityData {

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