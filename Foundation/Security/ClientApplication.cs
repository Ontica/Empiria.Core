/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : ClientApplication                                Pattern  : Standard Class                    *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a client application that uses the backend framework.                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. Ontica LLC, La Vía Óntica SC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Security {

  public class ClientApplication : BaseObject {

    #region Constructors and parsers

    private ClientApplication() {

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
      if (application.Status != GeneralObjectStatus.Active) {
        throw new SecurityException(SecurityException.Msg.NotActiveClientAppKey, clientAppKey);
      }
      return application;
    }

    protected override void OnLoadObjectData(DataRow row) {
      this.Key = (string) row["ObjectKey"];
      this.Description = (string) row["ObjectName"];
      this.Status = (GeneralObjectStatus) Convert.ToChar((string) row["ObjectStatus"]);
    }

    #endregion Constructors and parsers

    #region Properties

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

    [DataField("ObjectStatus", Default = GeneralObjectStatus.Active)]
    public GeneralObjectStatus Status {
      get;
      private set;
    }

    #endregion Properties

  }  // class ClientApplication

}  // namespace Empiria.Security
