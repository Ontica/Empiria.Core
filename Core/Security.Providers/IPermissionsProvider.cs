/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Providers                    *
*  Assembly : Empiria.Core.dll                             Pattern   : Dependency inversion interface        *
*  Type     : IPermissionsProvider                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Interface for subject permissions service providers.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.StateEnums;

namespace Empiria.Security.Providers {

  /// <summary>Interface for subject permissions service providers.</summary>
  public interface IPermissionsProvider {

    FixedList<string> GetFeaturesPermissions(EmpiriaIdentity subject, IClientApplication context);


    FixedList<IObjectAccessRule> GetObjectAccessRules(EmpiriaIdentity subject, IClientApplication context);


    FixedList<string> GetRoles(EmpiriaIdentity subject, IClientApplication context);


    bool IsSubjectInRole(IIdentifiable subject, IClientApplication context, string role);


  }  // interface IPermissionsProvider



  /// <summary>Holds information about a security claim for a subject.</summary>
  public interface ISubjectClaim {

    int SubjectId {
      get;
    }


    string Key {
      get;
    }


    EntityStatus Status {
      get;
    }

  }  // interface ISubjectClaim

}  // namespace Empiria.Security.Providers
