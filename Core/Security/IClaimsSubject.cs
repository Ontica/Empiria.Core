/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Core                                 System  : Security Services                       *
*  Assembly : Empiria.Security.dll                         Pattern : Interface                               *
*  Type     : IClaimsSubject                               License : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a set of attributes that describe a user or some other securable entity.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Represents a set of attributes that describe a user or some other securable entity.</summary>
  public interface IClaimsSubject {

    string ClaimsToken {
      get;
    }

    void OnClaimsSubjectRegistered(string claimsToken);

  }  // interface IClaimsSubject

}  // namespace Empiria.Security
