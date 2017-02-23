/* Empiria Extensions Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extensions Framework                   System   : Empiria Web API Services            *
*  Namespace : Empiria.WebApi.Client                          Assembly : Empiria.WebApi.Client.dll           *
*  Type      : IWebApiClient                                  Pattern  : Separated interface                 *
*  Version   : 1.1                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Interface used to call Empiria WebApiClient services included in a separated component.       *
*                                                                                                            *
********************************* Copyright (c) 2004-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
