/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Messaging Services                *
*  Namespace : Empiria.Messaging                                Assembly : Empiria.Foundation.dll            *
*  Type      : EMail                                            Pattern  : Empiria Semiabstract Object Type  *
*  Version   : 6.7                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Static methods for send e-mails.                                                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Empiria.Messaging {

  /// <summary>Static methods for send e-mails.</summary>
  static public class EMail {

    #region Public methods

    static public void Send(string eMail, string subject, string body,
                            bool isBodyHtml = false, bool highPriority = false,
                            FileInfo[] attachments = null) {
      Assertion.AssertObject(eMail, "eMail");
      Assertion.AssertObject(subject, "subject");
      Assertion.AssertObject(body, "body");

      MailMessage message = EMail.GetMailMessage(eMail, subject, body,
                                                 isBodyHtml, highPriority);
      if (attachments != null) {
        foreach (var file in attachments) {
          message.Attachments.Add(new Attachment(file.FullName));
        }
      }
      SmtpClient smtp = EMail.GetSmptClient();
      smtp.Send(message);
    }

    #endregion Public methods

    #region Private methods

    static private EMailConfig _eMailConfig = null;
    static private EMailConfig GetDefaultEMailConfig() {
      if (_eMailConfig == null) {
        _eMailConfig = EMailConfig.Default;
      }
      return _eMailConfig;
    }

    static private MailMessage GetMailMessage(string eMail, string subject, string body,
                                              bool isBodyHtml, bool highPriority) {
      EMailConfig eMailConfig = EMail.GetDefaultEMailConfig();

      var message = new MailMessage();

      message.From = new MailAddress(eMailConfig.SenderEMailAddress, eMailConfig.SenderName);

      // converts any semicolon to comma and removes spaces
      if (eMail.Contains(";")) {
        eMail = eMail.Replace(";", ",");
      }
      eMail = eMail.Replace(" ", String.Empty);

      if (eMail.Contains(",")) {
        message.To.Add(eMail);  // Multiple addresses separated by commas
      } else {
        message.To.Add(new MailAddress(eMail));
      }

      if (eMailConfig.BccMirrorEMailAddress.Length != 0) {
        message.Bcc.Add(new MailAddress(eMailConfig.BccMirrorEMailAddress));
      }
      message.Subject = subject;
      message.Body = body;
      message.IsBodyHtml = isBodyHtml;
      if (highPriority) {
        message.Priority = MailPriority.High;
      }
      return message;
    }

    static private SmtpClient GetSmptClient() {
      EMailConfig eMailConfig = EMail.GetDefaultEMailConfig();

      var smtp = new SmtpClient(eMailConfig.Host, eMailConfig.Port);

      smtp.EnableSsl = eMailConfig.EnableSsl;
      smtp.UseDefaultCredentials = eMailConfig.UseDefaultCredentials;
      smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
      if (!smtp.UseDefaultCredentials) {
        smtp.Credentials = new NetworkCredential(eMailConfig.SenderEMailAddress,
                                                 eMailConfig.SenderEMailPassword);
      }
      return smtp;
    }

    #endregion Private methods

  } //class EMail

} //namespace Empiria.Messaging
