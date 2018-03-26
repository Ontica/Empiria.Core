/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication Services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumerated Type                       *
*  Type     : AnonymousUser                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Constants that describe authentication modes.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Constants that describe authentication modes.</summary>
  public enum AuthenticationMode {

    None = -1,

    Basic = 1,

    Forms = 2,

    Realm = 3,

  }  // enum AuthenticationMode

} // namespace Empiria.Security
