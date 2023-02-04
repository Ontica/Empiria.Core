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

      var permissions = new List<Permission>(64);

      FillIdentityPermissions(permissions);

      FixedList<Role> identityRoles = Role.GetList(_clientApp, _identity);

      FillGrantedPermissions(permissions, identityRoles);

      RemoveRevokedPermissions(permissions, identityRoles);

      return permissions.Select(x => x.Key)
                        .Distinct()
                        .ToArray();
    }


    private void FillIdentityPermissions(List<Permission> list) {
      FixedList<Permission> identityPermissions = Permission.GetList(_clientApp, _identity);

      list.AddRange(identityPermissions);

      foreach (var permission in identityPermissions) {
        list.AddRange(permission.Requires);

        foreach (var require in permission.Requires) {
          list.AddRange(require.Requires);

          foreach (var item in require.Requires) {
            list.AddRange(item.Requires);
          }
        }
      }
    }


    private void FillGrantedPermissions(List<Permission> list,
                                        FixedList<Role> roles) {
      foreach (var role in roles) {
        list.AddRange(role.Grants);

        foreach (var grant in role.Grants) {
          list.AddRange(grant.Requires);

          foreach (var require in grant.Requires) {
            list.AddRange(require.Requires);

            foreach (var item in require.Requires) {
              list.AddRange(item.Requires);
            }
          }
        }
      }
    }


    private void RemoveRevokedPermissions(List<Permission> list,
                                          FixedList<Role> roles) {
      foreach (var role in roles) {
        foreach (var revoke in role.Revokes) {
          list.Remove(revoke);
        }
      }
    }

  }  // class PermissionsBuilder

}  // namespace Empiria.Security.Items
