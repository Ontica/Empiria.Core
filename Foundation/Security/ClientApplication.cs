/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : ClientApplication                                Pattern  : Standard Class                    *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a client application that uses the backend framework.                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Json;

namespace Empiria.Security {

  public class ClientApplication : BaseObject {

    #region Constructors and parsers

    private ClientApplication() {
      // Required by Empiria Framework
    }

    static internal ClientApplication Parse(int id) {
      return BaseObject.ParseId<ClientApplication>(id);
    }

    static internal ClientApplication ParseActive(string clientAppKey) {
      ClientApplication application =
                BaseObject.TryParse<ClientApplication>("ObjectKey = '" + clientAppKey + "'");

      if (application == null) {
        throw new SecurityException(SecurityException.Msg.InvalidClientAppKey, clientAppKey);
      }
      if (application.Status != ObjectStatus.Active) {
        throw new SecurityException(SecurityException.Msg.NotActiveClientAppKey, clientAppKey);
      }
      return application;
    }

    static public ClientApplication Current {
      get {
        if (EmpiriaPrincipal.Current != null) {
          return EmpiriaPrincipal.Current.ClientApp;
        } else {
          return null;
        }
      }
    }

    internal protected override void OnLoadObjectData(DataRow row) {
      this.Key = (string) row["ObjectKey"];
      this.Description = (string) row["ObjectName"];
      this.Status = (ObjectStatus) Convert.ToChar((string) row["ObjectStatus"]);

      var json = JsonObject.Parse((string) row["ObjectExtData"]);
      this.AssignedTo = Contact.Parse(json.Get<Int32>("AssignedToId", -1));

      this.Claims = new SecurityClaimList(this);
    }

    #endregion Constructors and parsers

    #region Properties

    public Contact AssignedTo {
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
    public ObjectStatus Status {
      get;
      private set;
    }

    public SecurityClaimList Claims {
      get;
      private set;
    }

    #endregion Properties

    #region Methods

    public void AssertClaim(SecurityClaimType claimType, string claimValue,
                            string assertionFailMsg = null) {
      Assertion.AssertObject(claimValue, "claimValue");

      if (this.Claims.Contains(claimType, claimValue)) {
        return;
      }

      if (String.IsNullOrWhiteSpace(assertionFailMsg)) {
        assertionFailMsg = this.BuildAssertionClaimFailMsg(claimType, claimValue);
      }
      throw new SecurityException(SecurityException.Msg.ClientApplicationClaimNotFound, assertionFailMsg);
    }

    private string BuildAssertionClaimFailMsg(SecurityClaimType claimType, string claimValue) {
      return String.Format("Client application '{0}' doesn't have a security claim " +
                           "with value '{1}' of type '{2}'.",
                           this.Description, claimValue, claimType.Key);
    }

    #endregion Methods

  }  // class ClientApplication

}  // namespace Empiria.Security
