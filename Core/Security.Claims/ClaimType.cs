/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Powertype                             *
*  Type     : ClaimType                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Powertype used to describe a security claim.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Security.Claims {

  /// <summary>Powertype used to describe a security claim.</summary>
  [Powertype(typeof(Claim))]
  public class ClaimType : Powertype {

    #region Constructors and parsers

    private ClaimType() {
      // Empiria power types always have this constructor.
    }

    static public new ClaimType Parse(int typeId) {
      return ObjectTypeInfo.Parse<ClaimType>(typeId);
    }


    static internal new ClaimType Parse(string typeName) {
      return ObjectTypeInfo.Parse<ClaimType>(typeName);
    }


    static public ClaimType ApplicationRoles => ClaimType.Parse("ObjectType.Claim.AppRoles");


    static public ClaimType ApplicationFeatures => ClaimType.Parse("ObjectType.Claim.AppFeatures");


    static public ClaimType RoleFeatures => ClaimType.Parse("ObjectType.Claim.RoleFeatures");


    static public ClaimType RoleUsers => ClaimType.Parse("ObjectType.Claim.RoleUsers");


    static public ClaimType UserFeatures => ClaimType.Parse("ObjectType.Claim.UserFeatures");


    static public ClaimType UserRole => ClaimType.Parse("ObjectType.Claim.UserRole");


    static public ClaimType ElectronicSign => ClaimType.Parse("ObjectType.Claim.ElectronicSign");


    static public ClaimType Token => ClaimType.Parse("ObjectType.Claim.Token");


    static public ClaimType UserID => ClaimType.Parse("ObjectType.Claim.UserID");


    static public ClaimType UserAppAccess => ClaimType.Parse("ObjectType.Claim.UserAppAccess");


    static public ClaimType WebApiController => ClaimType.Parse("ObjectType.Claim.WebApiController");


    static public ClaimType WebApiMethod => ClaimType.Parse("ObjectType.Claim.WebApiMethod");


    // To be deprecated

    static public ClaimType ElectronicSignPrivateKeyFilePath =>
                                    ClaimType.Parse("ObjectType.Claim.ElectronicSignPrivateKeyFilePath");

    static public ClaimType ActivationToken =>
                                    ClaimType.Parse("ObjectType.Claim.ActivationToken");


    static public ClaimType ResetPasswordToken =>
                              ClaimType.Parse("ObjectType.Claim.ResetPasswordToken");

    static public ClaimType UserPassword =>
                                    ClaimType.Parse("ObjectType.Claim.UserPassword");


    #endregion Constructors and parsers

  } // class ClaimType

} // namespace Empiria.Security.Claims
