/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumerated Type                       *
*  Type     : AnonymousUser                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Constants that describe anonymous users.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Constants that describe anonymous users.</summary>
  public enum AnonymousUser {

    Guest = -5,

    Testing = -4

  }  // enum AnonymousUser

} // namespace Empiria.Security
