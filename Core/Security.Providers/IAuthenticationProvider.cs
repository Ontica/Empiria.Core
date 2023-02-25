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

namespace Empiria.Security.Providers {

  /// <summary>Interface for authenticate and retrive users service providers.</summary>
  public interface IAuthenticationProvider {

    IEmpiriaPrincipal Authenticate(string sessionToken, string userHostAddress);


    IEmpiriaPrincipal Authenticate(IUserCredentials credentials);


    IClientApplication AuthenticateClientApp(string clientAppKey);


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
