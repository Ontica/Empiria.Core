/* Empiria  Core *********************************************************************************************
*                                                                                                            *
*  Module   : Contacts Management                        Component : Integration Layer                       *
*  Assembly : Empiria.Core.dll                           Pattern   : Data interface                          *
*  Type     : IContact                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a contact.                                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Contacts {

  public interface IContact: IIdentifiable {

    string FullName {
      get;
    }


  }  // interface IContact

}  // namespace Empiria.Contacts
