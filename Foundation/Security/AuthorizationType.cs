/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Services                 *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : AuthorizationType                                Pattern  : Storage Item Class                *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents an autorization type.                                                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
