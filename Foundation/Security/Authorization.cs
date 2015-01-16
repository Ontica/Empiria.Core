/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : Authorization                                    Pattern  : Storage Item Class                *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Sealed class that represents an authorization.                                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Security {

  public sealed class Authorization: BaseObject {

    #region Member variable declaration

    private Guid guid = Guid.NewGuid();
    private int typeId = 0;
    private int reasonId = 0;
    private int objectId = 0;
    private int authorizedById = 0;
    private string sessionToken = String.Empty;
    private string code = String.Empty;
    private string observations = String.Empty;
    private DateTime date = DateTime.Now;

    #endregion Member variable declaration

    #region Constructors and parsers

    private Authorization() {
      // Required by Empiria Framework.
    }

    private Authorization(int typeId, int authorizedById,
                          int objectId, string code) {
      this.typeId = typeId;
      this.authorizedById = authorizedById;
      this.sessionToken = ExecutionServer.CurrentPrincipal.Session.Token;
      this.objectId = objectId;
      this.code = code;
      Append();
    }

    static public Authorization Create(string userName, string password, string publicKey,
                                       string authorizationNS, int objectId) {
      EmpiriaUser user = EmpiriaUser.Authenticate(userName, password, publicKey);

      throw new NotImplementedException();

      //if (typeId == 0) {
      //  return null;
      //}
      //if (user != null) {
      //  return new Authorization(typeId, user.Id, objectId, String.Empty);
      //} else {
      //  return null;
      //}
    }

    static public Authorization Parse(int authorizationId, object authorizedObjectData) {
      throw new NotImplementedException();
    }

    static public Authorization Empty {
      get {
        return BaseObject.ParseEmpty<Authorization>();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public Guid Guid {
      get { return guid; }
    }

    public int TypeId {
      get { return typeId; }
    }

    public int ReasonId {
      get { return reasonId; }
      set { reasonId = 0; }
    }

    public int ObjectId {
      get { return objectId; }
      set { objectId = 0; }
    }

    public int AuthorizedById {
      get { return authorizedById; }
    }

    public string SessionToken {
      get { return sessionToken; }
    }

    public string Code {
      get { return code; }
    }

    public string Observations {
      get { return observations; }
      set { observations = value; }
    }

    public DateTime Date {
      get { return date; }
    }

    #endregion Public properties

    #region Private methods

    private void Append() {
      if (code.Length == 0) {
        code = GenerateAuthorizationCode();
      }
      SecurityData.WriteAuthorization(this);
    }

    private string GenerateAuthorizationCode() {
      string temp = String.Empty;

      temp = typeId.ToString() + objectId.ToString() + authorizedById.ToString() +
             SessionToken.ToString() + DateTime.Now.Ticks.ToString();
      return Math.Abs(temp.GetHashCode()).ToString("0000000000");
    }

    #endregion Private methods

  } // class Authorization

} // namespace Empiria.Security
