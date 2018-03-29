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
using System.Data;

namespace Empiria.Security.Claims {

  /// <summary>Powertype used to describe a security claim.</summary>
  public class ClaimType : BaseObject {

    #region Constructors and parsers

    private ClaimType() {
      // Required by Empiria Framework
    }

    static public ClaimType Parse(int id) {
      return BaseObject.ParseId<ClaimType>(id);
    }

    static public ClaimType Parse(string securityClaimTypeName) {
      Assertion.AssertObject(securityClaimTypeName, "securityClaimTypeName");

      return BaseObject.ParseKey<ClaimType>(securityClaimTypeName);
    }

    internal protected override void OnLoadObjectData(DataRow row) {
      this.Type = (string) row["ObjectType"];
      this.Key = (string) row["ObjectKey"];
      this.Description = (string) row["ObjectName"];
      this.Status = (ObjectStatus) Convert.ToChar((string) row["ObjectStatus"]);
    }

    static public ClaimType ActivationToken {
      get {
        return ClaimType.Parse("ActivationToken");
      }
    }

    static public ClaimType ElectronicSign {
      get {
        return ClaimType.Parse("ElectronicSign");
      }
    }

    static public ClaimType ElectronicSignPrivateKeyFilePath {
      get {
        return ClaimType.Parse("ElectronicSignPrivateKeyFilePath");
      }
    }

    static public ClaimType ResetPasswordToken {
      get {
        return ClaimType.Parse("ResetPasswordToken");
      }
    }

    static public ClaimType UserPassword {
      get {
        return ClaimType.Parse("UserPassword");
      }
    }

    static public ClaimType UserRole {
      get {
        return ClaimType.Parse("UserRole");
      }
    }

    static public ClaimType WebApiController {
      get {
        return ClaimType.Parse("WebApiController");
      }
    }

    static public ClaimType WebApiMethod {
      get {
        return ClaimType.Parse("WebApiMethod");
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("ObjectType")]
    public string Type {
      get;
      private set;
    }

    [DataField("ObjectKey")]
    public string Key {
      get;
      private set;
    }

    [DataField("ObjectName")]
    public string Description {
      get;
      private set;
    }

    [DataField("ObjectStatus", Default = ObjectStatus.Active)]
    protected ObjectStatus Status {
      get;
      private set;
    }

    #endregion Properties

  } // class ClaimType

} // namespace Empiria.Security.Claims
