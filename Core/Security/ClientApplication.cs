/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Domain entity                         *
*  Type     : ClientApplication                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a client application of Empiria Framework.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Json;

namespace Empiria.Security {

  /// <summary>Represents a client application of Empiria Framework.</summary>
  public class ClientApplication : BaseObject, IClaimsSubject {

    #region Constructors and parsers

    private ClientApplication() {
      // Required by Empiria Framework
    }


    static internal ClientApplication Parse(int id) {
      return BaseObject.ParseId<ClientApplication>(id);
    }


    static public ClientApplication ParseActive(string clientAppKey) {
      Assertion.AssertObject(clientAppKey, "clientAppKey");

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


    static ClientApplication _innerApplication = null;
    static public ClientApplication Inner {
      get {
        if (_innerApplication == null) {
          string clientAppKey = ConfigurationData.GetString("ApplicationKey");

          _innerApplication = ParseActive(clientAppKey);
        }
        return _innerApplication;
      }
    }


    internal protected override void OnLoadObjectData(DataRow row) {
      this.Key = (string) row["ObjectKey"];
      this.Description = (string) row["ObjectName"];
      this.Status = (ObjectStatus) Convert.ToChar((string) row["ObjectStatus"]);

      var json = JsonObject.Parse((string) row["ObjectExtData"]);
      this.AssignedTo = Contact.Parse(json.Get<Int32>("AssignedToId", -1));

      this.WebApiAddresses = json.GetList<NameValuePair>("WebApiAddresses").ToFixedList();

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


    public FixedList<NameValuePair> WebApiAddresses {
      get;
      private set;
    }


    string IClaimsSubject.ClaimsToken {
      get {
        return this.Key;
      }
    }

    #endregion Properties

    #region Methods

    void IClaimsSubject.OnClaimsSubjectRegistered(string claimsToken) {
      throw new NotImplementedException();
    }

    #endregion Methods

  }  // class ClientApplication

}  // namespace Empiria.Security
