/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization Services                *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : AuthorizationRequest                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an operation authorization request.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

namespace Empiria.Security.Authorization {

  /// <summary>Represents an operation authorization request.</summary>
  public class AuthorizationRequest {


    private AuthorizationRequest() {
      // Required by Empiria Framework.
    }


    public AuthorizationRequest(string operationName, string externalObjectUID, string notes = "") {
      Assertion.AssertObject(operationName, "operationName");
      Assertion.AssertObject(externalObjectUID, "externalObjectUID");

      this.OperationName = operationName;
      this.ExternalObjectUID = externalObjectUID;

      this.RequestTime = DateTime.Now;
      this.RequestedBy = EmpiriaUser.Current.AsContact();
      this.Notes = notes ?? String.Empty;
    }


    [DataField("RequestedOperation")]
    public string OperationName {
      get;
      private set;
    }


    [DataField("ExternalObjectUID")]
    public string ExternalObjectUID {
      get;
      private set;
    }


    [DataField("RequestTime")]
    public DateTime RequestTime {
      get;
      private set;
    }


    [DataField("RequestNotes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("RequestedById")]
    public Contact RequestedBy {
      get;
      private set;
    }

  }  // class AuthorizationRequest

}  // namespace Empiria.Security.Authorization
