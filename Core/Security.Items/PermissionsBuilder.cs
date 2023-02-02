/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Service provider                      *
*  Type     : PermissionsBuilder                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Builds a string permissions array for a given identity.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Empiria.Security.Items {

  /// <summary>Builds a string permissions array for a given identity.</summary>
  internal class PermissionsBuilder {

    private readonly ClientApplication _clientApp;
    private readonly EmpiriaIdentity _identity;

    internal PermissionsBuilder(ClientApplication clientApp, EmpiriaIdentity identity) {
      Assertion.Require(clientApp, nameof(clientApp));
      Assertion.Require(identity, nameof(identity));

      _clientApp = clientApp;
      _identity = identity;
    }


    internal string[] Build() {
      return GetPermissions().ToArray();
    }


    private FixedList<string> GetPermissions() {
      FixedList<Role> roles = GetRoles();

      var permissions = new List<Permission>(64);

      foreach (var role in roles) {
        permissions.AddRange(role.Grants);
        foreach (var grant in role.Grants) {
          permissions.AddRange(grant.Requires);
        }
      }

      foreach (var role in roles) {
        foreach (var revoke in role.Revokes) {
          permissions.Remove(revoke);
        }
      }

      return permissions.Select(x => x.Key)
                        .Distinct()
                        .ToFixedList();
    }


    private FixedList<Role> GetRoles() {

      FixedList<Role> roles = Role.GetList(_clientApp);

      return roles;
    }

  }  // class PermissionsBuilder

}  // namespace Empiria.Security.Items
