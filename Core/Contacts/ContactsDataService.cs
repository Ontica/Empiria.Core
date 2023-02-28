/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Contacts Management                          Component : Data Access Layer                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Data service                          *
*  Type     : ContactsDataService                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides contacts persistence methods.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Contacts {

  /// <summary>Provides contacts persistence methods.</summary>
  static internal class ContactsDataService {

    static internal void WriteContact(Contact o) {
      var op = DataOperation.Parse("writeContact",
                 o.Id, o.UID, o.GetEmpiriaType().Id,
                 o.FullName, o.ShortName, o.Initials,
                 o.Organization.Id, o.EMail, o.Tags,
                 o.ExtendedData.ToString(), o.Keywords,
                 (char) o.Status);

      DataWriter.Execute(op);
    }

  } // class ContactsDataService

} // namespace Empiria.Contacts
