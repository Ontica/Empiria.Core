/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : SecurityClaimType                                Pattern  : Standard Class                    *
*                                                                                                            *
*  Summary   : Represents a security claim type.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

namespace Empiria.Security {

  public class SecurityClaimType : BaseObject {

    #region Constructors and parsers

    private SecurityClaimType() {
      // Required by Empiria Framework
    }

    static public SecurityClaimType Parse(int id) {
      return BaseObject.ParseId<SecurityClaimType>(id);
    }

    static public SecurityClaimType Parse(string securityClaimTypeName) {
      Assertion.AssertObject(securityClaimTypeName, "securityClaimTypeName");

      return BaseObject.ParseKey<SecurityClaimType>(securityClaimTypeName);
    }

    internal protected override void OnLoadObjectData(DataRow row) {
      this.Type = (string) row["ObjectType"];
      this.Key = (string) row["ObjectKey"];
      this.Description = (string) row["ObjectName"];
      this.Status = (ObjectStatus) Convert.ToChar((string) row["ObjectStatus"]);
    }

    static public SecurityClaimType ActivationToken {
      get {
        return SecurityClaimType.Parse("ActivationToken");
      }
    }

    static public SecurityClaimType ElectronicSign {
      get {
        return SecurityClaimType.Parse("ElectronicSign");
      }
    }

    static public SecurityClaimType ResetPasswordToken {
      get {
        return SecurityClaimType.Parse("ResetPasswordToken");
      }
    }

    static public SecurityClaimType UserPassword {
      get {
        return SecurityClaimType.Parse("UserPassword");
      }
    }

    static public SecurityClaimType UserRole {
      get {
        return SecurityClaimType.Parse("UserRole");
      }
    }

    static public SecurityClaimType WebApiController {
      get {
        return SecurityClaimType.Parse("WebApiController");
      }
    }

    static public SecurityClaimType WebApiMethod {
      get {
        return SecurityClaimType.Parse("WebApiMethod");
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

  } // class SecurityClaimType

} // namespace Empiria.Security
