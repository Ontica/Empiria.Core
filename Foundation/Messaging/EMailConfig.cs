/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Services                *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Foundation.dll            *
*  Type      : EMailConfig                                      Pattern  : Empiria Semiabstract Object Type  *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Configuration data for email SMTP clients.                                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;

using Empiria.Json;

namespace Empiria.Messaging {

  /// <summary>Configuration data for email SMTP clients.</summary>
  internal class EMailConfig {

    #region Constructor and parsers

    private EMailConfig(JsonObject json) {
      this.LoadData(json);
    }

    static public EMailConfig Default {
      get {
        return EMailConfig.Parse("Default");
      }
    }

    static public EMailConfig Parse(string serverName) {
      Assertion.AssertObject(serverName, "serverName");

      string jsonString = ConfigurationData.GetString(serverName + ".Email.ConfigData");
      var jsonObject = JsonObject.Parse(jsonString);

      return new EMailConfig(jsonObject);
    }

    #endregion Constructor and parsers

    #region Private properties

    public string Host {
      get;
      private set;
    }

    public int Port {
      get;
      private set;
    }

    public bool EnableSsl {
      get;
      private set;
    }

    public bool UseDefaultCredentials {
      get;
      private set;
    }

    public string SenderEMailAddress {
      get;
      private set;
    }

    public string SenderName {
      get;
      private set;
    }

    public string SenderEMailPassword {
      get;
      private set;
    }

    public string BccMirrorEMailAddress {
      get;
      private set;
    }

    #endregion Private properties

    #region Private methods

    private void LoadData(JsonObject json) {
      this.Host = json.Get<string>("SmtpClientHost");
      this.Port = json.Get<int>("SmtpClientPort");
      this.EnableSsl = json.Get("EnableSsl", false);
      this.UseDefaultCredentials = json.Get("UseDefaultCredentials", true);
      this.SenderEMailAddress = json.Get<string>("SenderEMailAddress");
      this.SenderName = json.Get<string>("SenderName");
      this.SenderEMailPassword = json.Get<String>("SenderEMailPassword");
      this.BccMirrorEMailAddress = json.Get<string>("BccMirrorEMailAddress", String.Empty);
    }

    #endregion Private methods

  } //class EMail

} //namespace Empiria.Messaging
