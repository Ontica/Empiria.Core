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

    string AppKey {
      get;
    }

    string UserID {
      get;
    }

    string Password {
      get;
    }

    string UserHostAddress {
      get;
    }

    JsonObject ContextData {
      get;
    }

    string Entropy {
      get;
    }

  }  // interface IUserCredentials



  /// <summary>Holds user credentials data for authentication.</summary>
  public class UserCredentialsDto : IUserCredentials {

    public string AppKey {
      get; set;
    } = string.Empty;


    public string UserID {
      get; set;
    } = string.Empty;


    public string Password {
      get; set;
    } = string.Empty;


    public string UserHostAddress {
      get; set;
    } = string.Empty;


    public JsonObject ContextData {
      get; set;
    } = new JsonObject();


    public string Entropy {
      get; set;
    } = string.Empty;

  }  // class UserCredentialsDto

}  // namespace Empiria.Security
