/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Services                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Enumerated Type                       *
*  Type     : EncryptionMode                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Constants that describe data encryption modes.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Constants that describe data encryption modes.</summary>
  public enum EncryptionMode {

    Unprotected = 0,

    Standard = 1,

    EntropyKey = 2,

    HashCode = 3,

    EntropyHashCode = 4,

    Pure = 5,

  }

} // namespace Empiria.Security
