/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : Authorization                                    Pattern  : Standard Class                    *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Sealed class that represents an authorization.                                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

namespace Empiria.Security {

  public sealed class Authorization {

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
    private bool isDirty = false;

    #endregion Member variable declaration

    #region Constructors and parsers

    private Authorization() {

    }

    private Authorization(int typeId, int authorizedById, int objectId, string code) {
      this.typeId = typeId;
      this.authorizedById = authorizedById;
      this.sessionToken = ExecutionServer.CurrentIdentity.Session.Token;
      this.objectId = objectId;
      this.code = code;
      Append();
    }

    static public Authorization Create(string authorizationNS, int objectId, string authorizationCode) {
      int typeId = GetAuthorizationType(authorizationNS);
      int authorizedById = ExecutionServer.CurrentUserId;

      if (authorizedById != 0) {
        return new Authorization(typeId, authorizedById, objectId, authorizationCode);
      } else {
        return null;
      }
    }

    static public Authorization Create(string userName, string password, string publicKey,
                                       string authorizationNS, int objectId) {
      int typeId = GetAuthorizationType(authorizationNS);
      User user = User.Authenticate(userName, password, publicKey);

      if (typeId == 0) {
        return null;
      }
      if (user != null) {
        return new Authorization(typeId, user.Id, objectId, String.Empty);
      } else {
        return null;
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
      set { reasonId = 0; isDirty = true; }
    }

    public int ObjectId {
      get { return objectId; }
      set { objectId = 0; isDirty = true; }
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
      set { observations = value; isDirty = true; }
    }

    public DateTime Date {
      get { return date; }
    }

    #endregion Public properties

    #region Public methods

    public void Save() {
      if (isDirty) {
        SecurityData.WriteAuthorization(this);
      }
    }

    #endregion Public methods

    #region Private methods

    private void Append() {
      if (code.Length == 0) {
        code = GenerateAuthorizationCode();
      }
      SecurityData.WriteAuthorization(this);
    }

    static private int GetAuthorizationType(string authorizationNS) {
      if (authorizationNS == "SaleWithoutStock") {
        return 1;
      } else if (authorizationNS == "SaleUnderRepositionValue") {
        return 2;
      } else if (authorizationNS == "CreditSaleCode") {
        return 3;
      } else if (authorizationNS == "CreditSaleCodeCOBOL") {
        return 7;
      } else {
        return 0;
      }
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
