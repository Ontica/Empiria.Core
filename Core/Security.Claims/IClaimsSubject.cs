/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Claims                       *
*  Assembly : Empiria.Core.dll                             Pattern   : Interface                             *
*  Type     : IClaimsSubject                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a set of attributes that describe a user or some other securable entity.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security.Claims {

  /// <summary>Represents a set of attributes that describe a user or some other securable entity.</summary>
  public interface IClaimsSubject {

    string ClaimsToken {
      get;
    }

  }  // interface IClaimsSubject

}  // namespace Empiria.Security.Claims
