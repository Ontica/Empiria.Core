/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : SecurityClaimType                                Pattern  : Standard Class                    *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a security claim type.                                                             *
*                                                                                                            *
********************************* Copyright (c) 2009-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Security {

  public class SecurityClaimType : BaseObject {

    #region Constructors and parsers

    private SecurityClaimType() {
      // Required by Empiria Framework.
    }

    static public SecurityClaimType Parse(int id) {
      return BaseObjectFactory.Parse<SecurityClaimType>(id);
    }

    static public SecurityClaimType Parse(string securityClaimTypeName) {
      Assertion.AssertObject(securityClaimTypeName, "securityClaimTypeName");

      return BaseObjectFactory.Parse<SecurityClaimType>(securityClaimTypeName);
    }

    internal protected override void OnLoadObjectData(DataRow row) {
      this.Type = (string) row["ObjectType"];
      this.Key = (string) row["ObjectKey"];
      this.Description = (string) row["ObjectName"];
      this.Status = (ObjectStatus) Convert.ToChar((string) row["ObjectStatus"]);
    }

    static private SecurityClaimType _electronicSign = null;
    static public SecurityClaimType ElectronicSign {
      get {
        if (_electronicSign == null) {
          _electronicSign = SecurityClaimType.Parse("ElectronicSign");
        }
        return _electronicSign;
      }
    }

    static private SecurityClaimType _userName = null;
    static public SecurityClaimType UserName {
      get {
        if (_userName == null) {
          _userName = SecurityClaimType.Parse("UserName");
        }
        return _userName;
      }
    }


    static private SecurityClaimType _userPassword = null;
    static public SecurityClaimType UserPassword {
      get {
        if (_userPassword == null) {
          _userPassword = SecurityClaimType.Parse("UserPassword");
        }
        return _userPassword;
      }
    }

    static private SecurityClaimType _userRole = null;
    static public SecurityClaimType UserRole {
      get {
        if (_userRole == null) {
          _userRole = SecurityClaimType.Parse("UserRole");
        }
        return _userRole;
      }
    }

    static private SecurityClaimType _webApiController = null;
    static public SecurityClaimType WebApiController {
      get {
        if (_webApiController == null) {
          _webApiController = SecurityClaimType.Parse("WebApiController");
        }
        return _webApiController;
      }
    }

    static private SecurityClaimType _webApiMethod = null;
    static public SecurityClaimType WebApiMethod {
      get {
        if (_webApiMethod == null) {
          _webApiMethod = SecurityClaimType.Parse("WebApiMethod");
        }
        return _webApiMethod;
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
