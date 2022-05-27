/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Data Service                          *
*  Type     : ClaimsData                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides data read and write methods for security claims.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Security.Claims {

  /// <summary>Provides data read and write methods for security claims.</summary>
  static internal class ClaimsData {

    static internal Claim GetPendingSecurityClaim(ClaimType claimType,
                                                  IClaimsSubject subject) {
      string sql = $"SELECT * FROM SecurityClaims WHERE SecurityClaimTypeId = {claimType.Id} AND " +
                   $"SubjectToken = '{subject.ClaimsToken}' AND ClaimStatus = 'P'";

      DataRow row = DataReader.GetDataRow(DataOperation.Parse(sql));

      if (row == null) {
        throw new SecurityException(SecurityException.Msg.SubjectClaimNotFound,
                                    claimType.DisplayName, subject.ClaimsToken, "?");
      }

      return BaseObject.ParseDataRow<Claim>(row);
    }


    static internal FixedList<Claim> GetSecurityClaims(IClaimsSubject subject) {
      Assertion.Require(subject, "subject");

      var op = DataOperation.Parse("qryResourceSecurityClaims", subject.ClaimsToken);

      return DataReader.GetFixedList<Claim>(op);
    }


    static internal void RemoveClaim(Claim claim) {
      string sql = "UPDATE SecurityClaims SET ClaimStatus = 'X' WHERE SecurityClaimId = " + claim.Id;

      DataWriter.Execute(DataOperation.Parse(sql));
    }


    static internal void WriteSecurityClaim(Claim o) {
      var op = DataOperation.Parse("writeSecurityClaim", o.Id, o.ClaimType.Id, o.UID,
                                   o.Subject.ClaimsToken, o.Value, (char) o.Status);

      DataWriter.Execute(op);
    }

  } // class ClaimsData

} // namespace Empiria.Security.Claims
