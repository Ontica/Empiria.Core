/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Logging services                      *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : AuditTrailData                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides data methods for audit trails.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Security {

  static internal class AuditTrailData {

    static internal long WriteAuditTrail(AuditTrail o) {
      var op = DataOperation.Parse("apdAuditTrail",
                      o.Request.Guid, (char) o.AuditTrailType,
                      o.Timestamp, o.SessionId, o.UserHostAddress,
                      o.Event, o.Operation, o.OperationData.ToString(),
                      o.Request.AppliedToId, o.ResponseCode, o.ResponseItems,
                      o.ResponseTime, o.ResponseData.ToString(), o.Content);

      return DataWriter.Execute<long>(op);
    }

  } // class AuditTrailData

} // namespace Empiria.Security
