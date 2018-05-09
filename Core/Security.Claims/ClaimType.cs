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


    //static public ClaimType Empty {
    //  get {
    //    return ClaimType.Parse("ObjectType.SecurityClaim");
    //  }
    //}


    static public ClaimType ActivationToken {
      get {
        return ClaimType.Parse("ObjectType.Claim.ActivationToken");
      }
    }

    static public ClaimType ElectronicSign {
      get {
        return ClaimType.Parse("ObjectType.Claim.ElectronicSign");
      }
    }

    static public ClaimType ElectronicSignPrivateKeyFilePath {
      get {
        return ClaimType.Parse("ObjectType.Claim.ElectronicSignPrivateKeyFilePath");
      }
    }

    static public ClaimType ResetPasswordToken {
      get {
        return ClaimType.Parse("ObjectType.Claim.ResetPasswordToken");
      }
    }


    static public ClaimType UserID {
      get {
        return ClaimType.Parse("ObjectType.Claim.UserID");
      }
    }


    static public ClaimType UserPassword {
      get {
        return ClaimType.Parse("ObjectType.Claim.UserPassword");
      }
    }

    static public ClaimType UserRole {
      get {
        return ClaimType.Parse("ObjectType.Claim.UserRole");
      }
    }

    static public ClaimType WebApiController {
      get {
        return ClaimType.Parse("ObjectType.Claim.WebApiController");
      }
    }

    static public ClaimType WebApiMethod {
      get {
        return ClaimType.Parse("ObjectType.Claim.WebApiMethod");
      }
    }

    #endregion Constructors and parsers

  } // class ClaimType

} // namespace Empiria.Security.Claims
