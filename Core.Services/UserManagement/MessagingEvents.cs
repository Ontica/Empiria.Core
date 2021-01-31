/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : User Management                            Component : Integration Layer                       *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : String constants enumeration            *
*  Type     : MessagingEvents                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : List of messaging events dispatched by the services offered by this component.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Services.UserManagement.Providers {

  /// <summary>List of messaging events dispatched by the services offered by this component.</summary>
  internal enum MessagingEvents {

    UserPasswordCreated,

    UserPasswordChanged

  } // enum MessagingEvents

}  // namespace Empiria.Services.UserManagement.Providers
