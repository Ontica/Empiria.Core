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

    private Authorization(AuthorizationRequest request) {
      Assertion.Require(request, "request");

      this.Request = request;
    }


    static internal Authorization Parse(string uid) {
      return BaseObject.ParseKey<Authorization>(uid);
    }


    public static FixedList<Authorization> Authorized() {
      return AuthorizationServiceData.GetReadyToApplyAuthorizations();
    }


    /// <summary>Return all unauthorized authorizations where the current user is involved
    /// as a requester or who authorizes or approves the authorization request.</summary>
    /// <returns>A read only list of authorizations.</returns>
    static public FixedList<Authorization> Pending() {
      return AuthorizationServiceData.GetPendingAuthorizations();
    }


    /// <summary>Return unauthorized authorizations that match the given predicate and
    /// where the current user is involved as a requester or who authorizes or
    /// approves the authorization request.</summary>
    /// <returns>A read only list of filtered authorizations.</returns>
    static public FixedList<Authorization> Pending(Predicate<Authorization> match) {
      return AuthorizationServiceData.GetPendingAuthorizations(match);
    }


    /// <summary>Creates an authorization object from a given authorization request.</summary>
    static public Authorization Create(AuthorizationRequest request) {
      Assertion.Require(request, "request");

      var authorization = new Authorization(request);

      authorization.Save();

      return authorization;
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


    public bool IsReadyToBeAuthorized {
      get {
        return (this.Status == AuthorizationStatus.Pending ||
                this.Status == AuthorizationStatus.Approved ||
                this.Status == AuthorizationStatus.Expired);
      }
    }


    #endregion Properties

    #region Methods

    public void Apply() {
      Assertion.Require(this.Status == AuthorizationStatus.Authorized,
                      $"Authorization request '{this.UID}' is in status {this.Status.ToString()}, " +
                      "so it can not be applied.");

      this.AssertNotExpired();

      this.Status = AuthorizationStatus.Closed;

      this.Save();
    }

    private void AssertNotExpired() {
      if (DateTime.Now < this.ExpirationTime) {
        return;
      }

      this.Status = AuthorizationStatus.Expired;
      this.Save();

      throw new AuthorizationException(AuthorizationException.Msg.AuthorizationWasExpired, this.UID);
    }


    public void Authorize(int expirationMinutes = 60, string notes = "") {
      Assertion.Require(0 <= expirationMinutes,
                       "Expiration minutes must be a positive number.");
      Assertion.Require(this.IsReadyToBeAuthorized,
                       $"Authorization request '{this.UID}' is in status {this.Status.ToString()}, " +
                       "so it can not be authorized.");

      this.AuthorizedBy = EmpiriaUser.Current.AsContact();
      this.AuthorizationTime = DateTime.Now;
      this.ExpirationTime = DateTime.Now.AddMinutes(expirationMinutes);
      this.AuthorizationNotes = notes ?? String.Empty;
      this.Status = AuthorizationStatus.Authorized;

      this.Save();
    }


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
