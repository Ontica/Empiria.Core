﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization Services                *
*  Assembly : Empiria.Core.dll                             Pattern   : Data Services                         *
*  Type     : AuthorizationServiceData                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides database read and write methods for authorization services entities.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Security.Authorization {

  /// <summary>Provides database read and write methods used by authorization services.</summary>
  static internal class AuthorizationServiceData {

    #region Public methods

    static internal FixedList<Authorization> GetActiveAuthorizations() {
      int userId = EmpiriaUser.Current.Id;

      string sql = $"SELECT * FROM Authorizations " +
                   $"WHERE (RequestedById = {userId} OR ApprovedById = {userId} OR AuthorizedById = {userId}) AND " +
                   $"AuthorizationStatus NOT IN ('C', 'L', 'X') " +
                   $"ORDER BY RequestTime";

      return DataReader.GetFixedList<Authorization>(DataOperation.Parse(sql));
    }


    static internal void WriteAuthorization(Authorization o) {

      var op = DataOperation.Parse("writeAuthorization", o.Id, o.UID,
                                    o.Request.Operation, o.Request.ExternalObjectUID,
                                    o.Request.RequestTime, o.Request.Notes, o.Request.RequestedBy.Id,
                                    o.ApprovedBy.Id, o.AuthorizedBy.Id, o.AuthorizationNotes,
                                    o.AuthorizationTime, o.ExpirationTime, o.ExtensionData.ToString(),
                                    o.Status, String.Empty);
      DataWriter.Execute(op);
    }

    #endregion Internal methods

  } // class AuthorizationServiceData

} // namespace Empiria.Security.Authorization
