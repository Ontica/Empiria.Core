/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : IRequest                                         Pattern  : Loose coupling interface          *
*  Version   : 6.5                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Interface containing data that represents a system request.                                   *
*                                                                                                            *
********************************* Copyright (c) 1999-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Security {

  /// <summary>Interface containing data that represents a system request.</summary>
  public interface IRequest {

    Guid Guid {
      get;
    }

    EmpiriaPrincipal Principal {
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
