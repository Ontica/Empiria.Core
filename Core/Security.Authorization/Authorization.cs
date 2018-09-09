/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization Services                *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain Object                         *
*  Type     : Authorization                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about an operation execution authorization.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;

namespace Empiria.Security.Authorization {

  /// <summary>Holds information about an operation execution authorization.</summary>
  public class Authorization : BaseObject {

    #region Constructors and parsers

    private Authorization() {
      // Required by Empiria Framework.
    }

    internal Authorization(AuthorizationRequest request) {
      Assertion.AssertObject(request, "request");

      this.Request = request;
    }


    static internal Authorization Parse(string uid) {
      return BaseObject.ParseKey<Authorization>(uid);
    }


    #endregion Constructors and parsers

    #region Properties

    [DataObject]
    public AuthorizationRequest Request {
      get;
      private set;
    }


    [DataField("ApprovedById")]
    public Contact ApprovedBy {
      get;
      private set;
    }


    [DataField("AuthorizedById")]
    public Contact AuthorizedBy {
      get;
      private set;
    }


    [DataField("AuthorizationNotes")]
    public string AuthorizationNotes {
      get;
      private set;
    }


    [DataField("AuthorizationTime")]
    public DateTime AuthorizationTime {
      get;
      private set;
    }


    [DataField("ExpirationTime")]
    public DateTime ExpirationTime {
      get;
      private set;
    }


    [DataField("ExtensionData")]
    internal JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("AuthorizationStatus", Default = AuthorizationStatus.Pending)]
    public AuthorizationStatus Status {
      get;
      private set;
    }

    #endregion Properties

    #region Methods

    protected override void OnSave() {
      if (base.IsNew) {
        this.AuthorizationTime = ExecutionServer.DateMaxValue;
        this.ExpirationTime = ExecutionServer.DateMinValue;
      }

      AuthorizationServiceData.WriteAuthorization(this);
    }

    #endregion Methods

  }  // class Authorization

}  // namespace Empiria.Security.Authorization
