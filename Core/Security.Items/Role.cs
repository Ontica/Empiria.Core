/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : Role                                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an identity role that holds feature access permissions.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Items {

  /// <summary>Represents an identity role that holds feature access permissions.</summary>
  internal class Role : SecurityItem {

    #region Constructors and parsers

    internal Role(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new Role Parse(int id) {
      return BaseObject.ParseId<Role>(id);
    }


    static internal FixedList<Role> GetList(ClientApplication app) {
      return SecurityItemsDataReader.GetContextItems<Role>(app,
                                                           SecurityItemType.ClientAppRole);
    }


    static internal FixedList<Role> GetList(ClientApplication app, EmpiriaIdentity identity) {
      return SecurityItemsDataReader.GetIdentityTargetItems<Role>(app, identity,
                                                                  SecurityItemType.IdentityRole);
    }

    #endregion Constructors and parsers

    public string Name {
      get {
        return ExtensionData.Get("roleName", base.BaseKey);
      }
    }

    internal Feature[] Grants {
      get {
        return ExtensionData.GetList<Feature>("grants", false)
                            .ToArray();
      }
    }

    internal Feature[] Revokes {
      get {
        return ExtensionData.GetList<Feature>("revokes", false)
                            .ToArray();
      }
    }

  }  // class Role

}  // namespace Empiria.Security.Items
