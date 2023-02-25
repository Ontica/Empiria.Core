/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : IRequest                                         Pattern  : Loose coupling interface          *
*                                                                                                            *
*  Summary   : Interface containing data that represents a system request.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Interface containing data that represents a system request.</summary>
  public interface IRequest {

    Guid Guid {
      get;
    }

    IEmpiriaPrincipal Principal {
      get;
    }

    DateTime StartTime {
      get;
    }

    int AppliedToId {
      get;
    }

  }  // interface IRequest

}  // namespace Empiria.Security
