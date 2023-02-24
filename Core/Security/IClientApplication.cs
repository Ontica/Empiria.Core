/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Security Providers                    *
*  Assembly : Empiria.Core.dll                             Pattern   : Dependency inversion interface        *
*  Type     : IClientApplication                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Interface that represents a client application.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.DataTypes;

using Empiria.Security.Claims;

namespace Empiria.Security {

  public interface IClientApplication: IClaimsSubject, IIdentifiable {

    string Key {
      get;
    }

    FixedList<NameValuePair> WebApiAddresses {
      get;
    }

  }  // interface IClientApplication

}  // namespace Empiria.Security
