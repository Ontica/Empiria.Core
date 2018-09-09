/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authorization Services                *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumeration                           *
*  Type     : AuthorizationStatus                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes the status of an authorization.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Authorization {

  /// <summary>Describes the status of an authorization.</summary>
  public enum AuthorizationStatus {

    Pending = 'P',

    Approved = 'A',

    Authorized = 'T',

    Expired = 'E',

    Closed = 'C',

    Canceled = 'L',

    Deleted = 'X',

  } // enum AuthorizationStatus

} // namespace Empiria.Security.Authorization
