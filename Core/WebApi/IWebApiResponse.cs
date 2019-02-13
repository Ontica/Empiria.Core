/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                                 Component : Web Api Services                      *
*  Assembly : Empiria.Core.dll                             Pattern   : Separated interface                   *
*  Type     : IWebApiResponse                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Interface used to describe an object with a HttpResponse message.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Net.Http;

namespace Empiria.WebApi {

  /// <summary>Interface used to call Empiria WebApiClient services
  /// included in a separated component.</summary>
  public interface IWebApiResponse {

    HttpResponseMessage Response {
      get;
    }

  }  // interface IWebApiResponse

}  // namespace Empiria.WebApi
