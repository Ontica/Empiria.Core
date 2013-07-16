/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EMail                                            Pattern  : Static Class                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Operations for send e-mails.                                                                  *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System.IO;

using System.Net;
using System.Net.Mail;

namespace Empiria.Messaging {

  /// <summary>
  /// This static class allows assertion checking and automatic publishing of assertions fails.
  /// </summary>
  static public class EMail {

    #region Public methods

    static public void Send(string eMail, string subject, string body, FileInfo[] attachments) {
      SmtpClient smtp = new SmtpClient("mail.masautopartes.com.mx");

      NetworkCredential credential = new NetworkCredential("pineda@masautopartes.com.mx", "Hercules0201");
      smtp.Credentials = credential;
      smtp.Port = 50;

      MailMessage message = new MailMessage();
      message.From = new MailAddress("pineda@masautopartes.com.mx", "Auto Refacciones Pineda, S.A. de C.V.");
      message.To.Add(new MailAddress(eMail));
      message.Bcc.Add(new MailAddress("facturas.pineda@masautopartes.com.mx"));
      message.Subject = subject;
      message.Body = body;
      if (attachments != null) {
        for (int i = 0; i < attachments.Length; i++) {
          message.Attachments.Add(new Attachment(attachments[i].FullName));
        }
      }
      smtp.Send(message);
      message.Attachments.Dispose();
      message.Dispose();
    }

    #endregion Public methods

    #region Private methods

    #endregion Private methods

  } //class EMail

} //namespace Empiria.Messaging