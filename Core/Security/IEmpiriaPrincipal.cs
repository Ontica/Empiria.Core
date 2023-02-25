/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Dependency inversion interface        *
*  Type     : IEmpiriaPrincipal                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Defines an Empiria Framework security principal.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using System.Security.Principal;

using Empiria.Collections;
using Empiria.Contacts;

using Empiria.Security.Claims;

namespace Empiria.Security {

  /// <summary>Defines an Empiria Framework security principal.</summary>
  public interface IEmpiriaPrincipal : IPrincipal {

    IEmpiriaSession Session {
      get;
    }


    AssortedDictionary ContextItems {
      get;
    }


    new IEmpiriaIdentity Identity {
      get;
    }


    FixedList<string> Permissions {
      get;
    }


    IClientApplication ClientApp {
      get;
    }


    bool HasDataAccessTo<T>(T entity) where T : IIdentifiable;


    void Logout();


  }  // interface IEmpiriaPrincipal



  /// <summary>Defines an Empiria Framework authenticated identity.</summary>
  public interface IEmpiriaIdentity : IIdentity {

    IEmpiriaUser User {
      get;
    }

  }  // interface IEmpiriaIdentity



  /// <summary>Defines an Empiria Framework user.</summary>
  public interface IEmpiriaUser : IClaimsSubject {

    Contact Contact {
      get;
    }


    string UserName {
      get;
    }

  }  // interface IEmpiriaUser

}  // namespace Empiria.Security
