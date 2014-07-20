/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : AuthorizationType                                Pattern  : Storage Item Class                *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents an autorization type.                                                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Security {

  /// <summary>Represents an autorization type.</summary>
  public class AuthorizationType : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.AuthorizationType";

    #endregion Fields

    #region Constructors and parsers

    public AuthorizationType() : base(thisTypeName) {

    }

    protected AuthorizationType(string typeName) : base(typeName) {
      // Empiria Object Type pattern classes always has this constructor. Don't delete.
    }

    static public AuthorizationType Parse(int id) {
      return BaseObject.Parse<AuthorizationType>(thisTypeName, id);
    }

    static public AuthorizationType Empty {
      get {
        return BaseObject.ParseEmpty<AuthorizationType>(thisTypeName);
      }
    }

    #endregion Constructors and parsers

  } // class AuthorizationType

} // namespace Empiria.Security
