/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain entity                         *
*  Type     : EmpiriaUser                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a system's user.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.StateEnums;

using Empiria.Security.Claims;

namespace Empiria.Security {

  /// <summary>Represents a system's user.</summary>
  public sealed class EmpiriaUser : IClaimsSubject {

    #region Constructors and parsers

    private EmpiriaUser() {
      // Required by Empiria Framework
    }

    internal static EmpiriaUser Parse(Items.Claim credentials) {
      return new EmpiriaUser {
        Contact = Contact.Parse(credentials.SubjectId),
        UserName = credentials.Key,
        Status = credentials.Status,
      };
    }

    static public EmpiriaUser Parse(string username, string email) {
      Assertion.Require(username, "username");
      Assertion.Require(email, "email");

      EmpiriaUser user = SecurityData.TryGetUserWithUserName(ClientApplication.Current,
                                                             username);

      if (user == null) {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, username, email);
      }
      if (user.EMail.Equals(email)) {
        return user;
      } else {
        throw new SecurityException(SecurityException.Msg.UserWithEMailNotFound, username, email);
      }
    }


    static public bool Exists(string userName) {
      Assertion.Require(userName, "userName");

      EmpiriaUser user = SecurityData.TryGetUserWithUserName(ClientApplication.Current,
                                                             userName);

      return (user != null);
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

      EmpiriaUser user = SecurityData.GetSessionUser(activeSession);

      user.EnsureCanAuthenticate();
      user.IsAuthenticated = true;

      return user;
    }

    #endregion Authenticate methods

    #region Public propertiese

    public bool IsActive {
      get {
        return this.Status == EntityStatus.Active;
      }
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
      get {
        return this.Contact.FullName;
      }
    }

    public string EMail {
      get {
        return this.Contact.EMail;
      }
    }


    string IClaimsSubject.ClaimsToken {
      get {
        return this.Contact.UID;
      }
    }


    internal Contact Contact {
      get; private set;
    }


    private EntityStatus Status {
      get; set;
    }

    #endregion Public properties

    #region Private methods

    private void EnsureCanAuthenticate() {
      if (!this.IsActive) {
        throw new SecurityException(SecurityException.Msg.NotActiveUser, this.UserName);
      }

      if (this.PasswordExpired) {
        throw new SecurityException(SecurityException.Msg.UserPasswordExpired, this.UserName);
      }
    }

    #endregion Private methods

  } // class EmpiriaUser

} // namespace Empiria.Security
