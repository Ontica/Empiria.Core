/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                   System   : Empiria Web API Services            *
*  Namespace : Empiria.WebApi.Client                          Assembly : Empiria.Foundation.dll              *
*  Type      : IWebApiClient                                  Pattern  : Separated interface                 *
*  Version   : 6.8                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Interface used to call Empiria WebApiClient services included in a separated component.       *
*                                                                                                            *
********************************* Copyright (c) 2016-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Threading.Tasks;

namespace Empiria.WebApi.Client {

  public interface IWebApiClient {

    Task DeleteAsync(string path, params object[] pars);


    Task<T> DeleteAsync<T>(string path, params object[] pars);


    Task<T> GetAsync<T>(string path, params object[] pars);


    Task PostAsync<T>(T body, string path, params object[] pars);


    Task<R> PostAsync<T, R>(T body, string path, params object[] pars);


    Task PutAsync<T>(T body, string path, params object[] pars);


    Task<R> PutAsync<T, R>(T body, string path, params object[] pars);

  }  // interface IWebApiClient

}  // namespace Empiria.WebApi.Client
