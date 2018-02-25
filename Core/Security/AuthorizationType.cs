/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 License  : Please read LICENSE.txt file      *
*  Type      : AuthorizationType                                Pattern  : Storage Item Class                *
*                                                                                                            *
*  Summary   : Represents an autorization type.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Security {

  /// <summary>Represents an autorization type.</summary>
  public class AuthorizationType : GeneralObject {

    #region Constructors and parsers

    private AuthorizationType() {
      // Required by Empiria Framework.
    }

    static public AuthorizationType Parse(int id) {
      return BaseObject.ParseId<AuthorizationType>(id);
    }

    static public AuthorizationType Empty {
      get {
        return BaseObject.ParseEmpty<AuthorizationType>();
      }
    }

    #endregion Constructors and parsers

  } // class AuthorizationType

} // namespace Empiria.Security
