/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : Role                                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents an identity role that holds permissions.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Items {

  /// <summary>Represents an identity role that holds permissions.</summary>
  internal class Role : SecurityItem {

    internal Role(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Role Parse(int id) {
      return BaseObject.ParseId<Role>(id);
    }

    internal static FixedList<Role> GetList(ClientApplication app) {
      return SecurityItemsDataReader.GetSubjectSecurityItems<Role>(app, SecurityItemType.ClientAppRole);
    }

    public string Name {
      get {
        return ExtensionData.Get("roleName", base.Key);
      }
    }

    internal string[] Grants {
      get {
        return ExtensionData.GetList<string>("grants", false)
                            .ToArray();
      }
    }


    internal string[] Revokes {
      get {
        return ExtensionData.GetList<string>("revokes", false)
                            .ToArray();
      }
    }

  }  // class Role

}  // namespace Empiria.Security.Items
