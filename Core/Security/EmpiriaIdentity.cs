/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : EmpiriaIdentity                                  Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Sealed class that represents a Empiria System identity.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Security.Principal;

namespace Empiria.Security {

  public sealed class EmpiriaIdentity : IIdentity {

    #region Constructors and parsers

    internal EmpiriaIdentity(EmpiriaUser user, AuthenticationMode mode) {
      this.User = user;
      this.SetAuthenticationType(mode);
    }

    static public EmpiriaIdentity Current {
      get {
        return ExecutionServer.CurrentIdentity as EmpiriaIdentity;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string AuthenticationType {
      get;
      private set;
    }

    public bool IsAuthenticated {
      get;
      private set;
    }

    public string Name {
      get {
        return this.User.UserName;
      }
    }

    public EmpiriaUser User {
      get;
      private set;
    }

    #endregion Public properties

    #region Private methods

    private void EnsureValid() {
      Assertion.Require(this.User != null,
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));

      Assertion.Require(this.IsAuthenticated,
                       SecurityException.GetMessage(SecurityException.Msg.WrongAuthentication));
    }

    private void SetAuthenticationType(AuthenticationMode authenticationMode) {
      switch (authenticationMode) {

        case AuthenticationMode.Basic:
          this.AuthenticationType = "Basic";
          this.IsAuthenticated = true;
          return;

        case AuthenticationMode.Realm:
          this.AuthenticationType = "Realm";
          this.IsAuthenticated = true;
          return;

        case AuthenticationMode.Forms:
          this.AuthenticationType = "Forms";
          this.IsAuthenticated = true;
          return;

        case AuthenticationMode.None:
          this.AuthenticationType = "None";
          this.IsAuthenticated = false;
          return;

        default:
          throw Assertion.EnsureNoReachThisCode();
      }
    }

    #endregion Private methods

  } // class EmpiriaIdentity

} // namespace Empiria.Security
