/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information holder                    *
*  Type     : Permission                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about an authorization permission.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Items {

  /// <summary>Holds information about an authorization permission.</summary>
  internal class Permission : SecurityItem {

    #region Constructors and parsers

    private Permission(SecurityItemType powerType) : base(powerType) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal new Permission Parse(int id) {
      return BaseObject.ParseId<Permission>(id);
    }


    static internal Permission Parse(string permissionKey) {
      var permission = BaseObject.TryParse<Permission>($"SecurityItemKey = '{permissionKey}'");

      if (permission != null) {
        return permission;
      }

      return Permission.Empty;
    }


    static internal FixedList<Permission> GetList(ClientApplication app) {
      return SecurityItemsDataReader.GetSubjectSecurityItems<Permission>(app, SecurityItemType.ClientAppPermission);
    }


    public static Permission Empty => ParseEmpty<Permission>();


    #endregion Constructors and parsers

    #region Properties


    public string Key {
      get {
        return base.BaseKey;
      }
    }


    public string Name {
      get {
        return ExtensionData.Get("permissionName", this.Key);
      }
    }


    public bool IsAssignable {
      get {
        return ExtensionData.Get("assignable", true);
      }
    }


    public string[] Requires {
      get {
        return ExtensionData.GetList<string>("requires", false)
                            .ToArray();
      }
    }

    #endregion Properties

  }  // class Permission

}  // namespace Empiria.Security.Items
