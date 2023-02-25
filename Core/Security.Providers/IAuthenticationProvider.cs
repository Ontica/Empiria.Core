/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Providers                    *
*  Assembly : Empiria.Core.dll                             Pattern   : Dependency inversion interface        *
*  Type     : IAuthenticationProvider                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Interface for authenticate and retrive users service providers.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

namespace Empiria.Security.Providers {

  /// <summary>Interface for authenticate and retrive users service providers.</summary>
  public interface IAuthenticationProvider {

    ISubjectClaim Authenticate(IClientApplication app, string username,
                               string password, string entropy);


    IClientApplication AuthenticateClientApp(string clientAppKey);


    IEmpiriaSession CreateSession(EmpiriaPrincipal principal, JsonObject contextData);


    IEmpiriaSession RetrieveActiveSession(string sessionToken);


    IClientApplication TEMP_AuthenticateClientApp(int clientAppId);


    ISubjectClaim TryGetUser(IEmpiriaSession activeSession);


    ISubjectClaim TryGetUserWithUserName(IClientApplication app, string username);

  }  // interface IAuthenticationProvider



  /// <summary>Holds information about an object access rule.</summary>
  public interface IObjectAccessRule {

    string TypeName {
      get;
    }


    string[] ObjectsUIDs {
      get;
    }

  }  // interface IObjectAccessRule

}  // namespace Empiria.Security.Providers
