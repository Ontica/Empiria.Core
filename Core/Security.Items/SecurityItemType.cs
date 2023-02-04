/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security Items                               Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : PowerType                             *
*  Type     : SecurityItemType                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Powertype used to describe security items.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Security.Items {

  /// <summary>Powertype used to describe security items.</summary>
  [Powertype(typeof(SecurityItem))]
  internal class SecurityItemType : Powertype {

    #region Constructors and parsers

    private SecurityItemType() {
      // Empiria power types always have this constructor.
    }


    static public new SecurityItemType Parse(int typeId) {
      return ObjectTypeInfo.Parse<SecurityItemType>(typeId);
    }


    static internal new SecurityItemType Parse(string typeName) {
      return ObjectTypeInfo.Parse<SecurityItemType>(typeName);
    }


    static public SecurityItemType ClientAppPermission
                => Parse("ObjectType.SecurityItem.Permission.ClientAppPermission");


    static public SecurityItemType ClientAppRole
                => Parse("ObjectType.SecurityItem.Role.ClientAppRole");


    static public SecurityItemType IdentityPermission
                => Parse("ObjectType.SecurityItem.Permission.IdentityPermission");


    static public SecurityItemType IdentityRole
                => Parse("ObjectType.SecurityItem.Role.IdentityRole");


    #endregion Constructors and parsers

  } // class SecurityItemType

} // namespace Empiria.Security.Items
