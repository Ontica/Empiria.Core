/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Value type                            *
*  Type     : EmpiriaIdentity                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an authenticated user in Empiria Framework.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Security.Principal;

namespace Empiria.Security {

  /// <summary>Describes authentication modes.</summary>
  public enum AuthenticationMode {

    None,

    Basic,

    Forms,

    Realm,

  }  // enum AuthenticationMode



  /// <summary>Represents an authenticated user in Empiria Framework.</summary>
  public sealed class EmpiriaIdentity : IIdentity {

    private readonly AuthenticationMode _authenticationMode;

    #region Constructors and parsers

    internal EmpiriaIdentity(EmpiriaUser user, AuthenticationMode mode) {
      Assertion.Require(user, nameof(user));

      this.User = user;
      _authenticationMode = mode;
    }

    #endregion Constructors and parsers

    #region Public properties


    public string AuthenticationType {
      get {
        return _authenticationMode.ToString();
      }
    }


    public bool IsAuthenticated {
      get {
        return _authenticationMode != AuthenticationMode.None;
      }
    }


    public string Name {
      get {
        return this.User.UserName;
      }
    }


    public EmpiriaUser User {
      get;
    }

    #endregion Public properties

  } // class EmpiriaIdentity

} // namespace Empiria.Security
