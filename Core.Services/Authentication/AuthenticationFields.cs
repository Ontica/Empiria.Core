/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Authentication Services                    Component : Interface adapters                      *
*  Assembly : Empiria.Core.Services.dll                  Pattern   : Input Data Holder                       *
*  Type     : AuthenticationFields                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input data used to authenticate a user.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Services.Authentication {

  /// <summary>Input data used to authenticate a user.</summary>
  public class AuthenticationFields {

    #region Properties

    public string UserID {
      get; set;
    } = string.Empty;


    public string Password {
      get; set;
    } = string.Empty;


    public string AppKey {
      get; set;
    } = string.Empty;


    public string IpAddress {
      get; set;
    } = string.Empty;

    #endregion Properties

  }  // class AuthenticationFields

} // namespace Empiria.Services.Authentication
