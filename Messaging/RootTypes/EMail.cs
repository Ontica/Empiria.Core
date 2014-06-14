﻿/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Foundation Framework Library      *
*  Namespace : Empiria                                          Assembly : Empiria.Kernel.dll                *
*  Type      : EMail                                            Pattern  : Static Class                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Operations for send e-mails.                                                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
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
      SmtpClient smtp = new SmtpClient("smtpout.secureserver.net");

      NetworkCredential credential = new NetworkCredential("facturas.pineda@masautopartes.com.mx", "Hercules0201");
      smtp.Credentials = credential;
      smtp.Port = 80;

      MailMessage message = new MailMessage();
      message.From = new MailAddress("facturas.pineda@masautopartes.com.mx", "Auto Refacciones Pineda, S.A. de C.V.");

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

  } //class EMail

} //namespace Empiria.Messaging
