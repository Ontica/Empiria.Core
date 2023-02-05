﻿/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain entity                         *
*  Type     : EmpiriaUser                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a system's user.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.StateEnums;

using Empiria.Security.Claims;

namespace Empiria.Security {

  /// <summary>Represents a system's user.</summary>
  public sealed class EmpiriaUser : BaseObject, IClaimsSubject {

    #region Constructors and parsers

    private EmpiriaUser() {
      // Required by Empiria Framework
    }

    static public EmpiriaUser Parse(int id) {
      return BaseObject.ParseId<EmpiriaUser>(id);
    }

    static public EmpiriaUser Parse(string username, string email) {
      Assertion.Require(username, "username");
      Assertion.Require(email, "email");

      EmpiriaUser user = SecurityData.TryGetUserWithUserName(username);

      if (user == null) {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, username, email);
      }
      if (user.EMail.Equals(email)) {
        return user;
      } else {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, username, email);
      }
    }


    static public EmpiriaUser Current {
      get {
        if (ExecutionServer.IsAuthenticated) {
          return ExecutionServer.CurrentIdentity.User;
        } else {
          throw new SecurityException(SecurityException.Msg.UnauthenticatedIdentity);
        }
      }
    }

    static public bool Exists(string userName) {
      Assertion.Require(userName, "userName");

      return (SecurityData.TryGetUserWithUserName(userName) != null);
    }


    /// <summary>Determines whether a contact belongs to the specified role.</summary>
    static public bool IsInRole(Contact user, string role) {
      string[] users = SecurityData.GetUsersInRole(role);

      for (int i = 0; i < users.Length; i++) {
        if (users[i].Trim() == user.Id.ToString()) {
          return true;
        }
      }
      return false;
    }


    #endregion Constructors and parsers

    #region Authenticate methods

    static internal EmpiriaUser Authenticate(ClientApplication clientApplication,
                                             string username,
                                             string password,
                                             string entropy) {
      Assertion.Require(clientApplication, "clientApplication");
      Assertion.Require(username, "username");
      Assertion.Require(password, "password");
      Assertion.Require(entropy, "entropy");

      EmpiriaUser user = SecurityData.GetUserWithCredentials(clientApplication,
                                                             username, password,
                                                             entropy);
      user.EnsureCanAuthenticate();
      user.IsAuthenticated = true;

      return user;
    }


    static internal EmpiriaUser Authenticate(EmpiriaSession activeSession) {
      Assertion.Require(activeSession, "activeSession");

      if (!activeSession.IsStillActive) {
        throw new SecurityException(SecurityException.Msg.ExpiredSessionToken, activeSession.Token);
      }

      EmpiriaUser user = EmpiriaUser.Parse(activeSession.UserId);

      user.EnsureCanAuthenticate();
      user.IsAuthenticated = true;

      return user;
    }


    static internal EmpiriaUser AuthenticateAnonymous(AnonymousUser anonymousUser) {
      var anonymous = EmpiriaUser.Parse((int) anonymousUser);

      anonymous.EnsureCanAuthenticate();
      anonymous.IsAuthenticated = true;

      return anonymous;
    }


    #endregion Authenticate methods

    #region Public propertiese

    public bool IsActive {
      get;
      private set;
    }

    public bool IsAuthenticated {
      get;
      private set;
    }

    public bool PasswordExpired {
      get;
      private set;
    }

    public string UserName {
      get;
      private set;
    }

    public string FullName {
      get;
      private set;
    }

    public string EMail {
      get;
      private set;
    }

    string IClaimsSubject.ClaimsToken {
      get {
        return this.AsContact().UID;
      }
    }

    #endregion Public properties

    #region Public methods

    public Contact AsContact() {
      return Contact.Parse(this.Id);
    }

    #endregion Public methods

    #region Private methods

    private void EnsureCanAuthenticate() {
      if (!this.IsActive) {
        throw new SecurityException(SecurityException.Msg.NotActiveUser, this.UserName);
      }

      if (this.PasswordExpired) {
        throw new SecurityException(SecurityException.Msg.UserPasswordExpired, this.UserName);
      }
    }


    internal protected override void OnLoadObjectData(DataRow row) {
      this.UserName = EmpiriaString.ToString(row["UserName"]);
      this.IsAuthenticated = false;
      this.IsActive = ((EntityStatus) Convert.ToChar((string) row["ContactStatus"]) == EntityStatus.Active);
      this.PasswordExpired = false;
      this.EMail = EmpiriaString.ToString(row["ContactEmail"]);
      this.FullName = EmpiriaString.ToString(row["ContactFullName"]);
    }

    #endregion Private methods

  } // class EmpiriaUser

} // namespace Empiria.Security
