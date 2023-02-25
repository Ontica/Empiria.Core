/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Security                                     Component : Authentication services               *
*  Assembly : Empiria.Core.dll                             Pattern   : Data transfer object                  *
*  Type     : UserCredentialsDto                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds user credentials data for authentication.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Json;

namespace Empiria.Security {


  public interface IUserCredentials {

    string ClientAppKey {
      get;
    }

    string Username {
      get;
    }

    string Password {
      get;
    }

    string Entropy {
      get;
    }

    JsonObject ContextData {
      get;
    }

  }  // interface IUserCredentials



  /// <summary>Holds user credentials data for authentication.</summary>
  public class UserCredentialsDto : IUserCredentials {

    public string ClientAppKey {
      get; set;
    } = string.Empty;


    public string Username {
      get; set;
    } = string.Empty;


    public string Password {
      get; set;
    } = string.Empty;


    public string Entropy {
      get; set;
    } = string.Empty;


    public JsonObject ContextData {
      get; set;
    }

  }  // class UserCredentialsDto

}  // namespace Empiria.Security
