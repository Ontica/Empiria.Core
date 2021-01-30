/* Empiria Extensions ****************************************************************************************
*                                                                                                            *
*  Module   : Empiria User Management                    Component : Integration Layer                       *
*  Assembly : Empiria.UserManagement.dll                 Pattern   : String constants enumeration            *
*  Type     : MessagingEvents                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : List of messaging events dispatched by the services offered by this component.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.UserManagement.Integration {

  /// <summary>List of messaging events dispatched by the services offered by this component.</summary>
  internal enum MessagingEvents {

    UserPasswordCreated,

    UserPasswordChanged

  } // enum MessagingEvents

}  // namespace Empiria.UserManagement.Integration
