/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : User                                             Pattern  : Ontology Relation Type            *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a human user.                                                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;
using System.Web.Security;

using Empiria.Contacts;

namespace Empiria.Security {

  public sealed class User : BaseObject, IEmpiriaUser {

    #region Fields

    private const string thisTypeName = "ObjectType.User";

    private string userName = String.Empty;
    private bool isActive = false;
    private string namedKey = String.Empty;
    private string uiTheme = String.Empty;

    private bool isAuthenticated = false;

    #endregion Fields

    #region Constructors and parsers

    public User()
      : base(thisTypeName) {

    }

    private User(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public User Parse(int id) {
      return BaseObject.Parse<User>(thisTypeName, id);
    }

    #endregion Constructors and parsers

    #region Public propertiese

    public Contact Contact {
      get { return Contact.Parse(base.Id); }
    }

    public bool IsActive {
      get { return isActive; }
      set { isActive = value; }
    }

    public bool IsAdministrator {
      get {
        return namedKey == "ADMINISTRATOR";
      }
    }

    public bool IsAuthenticated {
      get { return isAuthenticated; }
    }

    public bool IsGuest {
      get {
        return namedKey == "GUEST";
      }
    }

    public bool IsSystemUser {
      get { return (base.Id <= 0); }
    }

    public string UITheme {
      get { return uiTheme; }
      set { uiTheme = value; }
    }

    public string UserName {
      get { return userName; }
      set { userName = value; }
    }

    #endregion Public properties

    #region Public methods

    static internal User Authenticate(string userName, string password, string entropy) {
      Assertion.RequireObject(userName, "userName");
      Assertion.RequireObject(password, "password");
      Assertion.RequireObject(entropy, "entropy");

      //Remove this two lines
      userName = Cryptographer.Decrypt(userName, entropy);
      password = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, Cryptographer.Decrypt(password, entropy), userName);

      int userId = ContactsData.GetContactIdWithUserName(userName);
      if (userId == 0) {
        return null;
      }
      User user = User.Parse(userId);

      if (user.IsSystemUser && !user.IsAdministrator) {
        return null;
      }
      if (!user.IsActive) {
        return null;
      }
      if (Cryptographer.Decrypt(ContactsData.GetContactAttribute<string>(user.Contact, "UserPassword"), userName).Equals(Cryptographer.Decrypt(password, userName))) {
        user.isAuthenticated = true;
        return user;
      } else {
        user.isAuthenticated = false;
        return null;
      }
    }

    static internal User Authenticate(FormsAuthenticationTicket remoteTicket) {
      Assertion.RequireObject(remoteTicket, "remoteTicket");

      int userId = ContactsData.GetContactIdWithUserName(remoteTicket.Name);
      if (userId == 0) {
        return null;
      }
      return User.Parse(userId);
    }

    static internal User AuthenticateGuest() {
      int userId = ContactsData.GetContactIdWithUserName("GUEST");
      if (userId == 0) {
        return null;
      }
      User user = User.Parse(userId);

      if (!user.IsActive) {
        return null;
      }
      user.isAuthenticated = true;

      return user;
    }

    static private string[] GetUsersInRole(string operationTag) {
      return ConfigurationData.GetString("User.Operation.Tag." + operationTag).Split('|');
    }

    public bool CanExecute(string operationTag) {
      string[] users = GetUsersInRole(operationTag);

      for (int i = 0; i < users.Length; i++) {
        if (users[i].Trim() == this.Id.ToString()) {
          return true;
        }
      }
      return false;
    }

    //public bool CanExecute(Ontology.TypeMethodInfo typeMethodInfo, Contacts.Organization organization) {
    //  if (ExecutionServer.LicenseName == "Zacatecas") {
    //    if (this.Id == -3 || this.Id == 11 || this.Id == 12 || this.Id == 212 || this.Id == 217 ||
    //        this.Id == 416 || this.Id == 441 || this.Id == 242 || this.Id == 243 ||
    //        this.Id == 421 || this.Id == 38 || this.Id == 413 || this.Id == 417 || this.Id == 429) {
    //      return true;
    //    }
    //  } else if (ExecutionServer.LicenseName == "Tlaxcala") {
    //    if (this.Id == -3 || this.Id == 11 || this.Id == 12 || this.Id == 38 ||
    //        this.Id == 220 || this.Id == 221 || this.Id == 266 || this.Id == 228 || this.Id == 349 ||
    //        this.Id == 240 || this.Id == 205 || this.Id == 235 || this.Id == 280 || this.Id == 276 ||
    //        this.Id == 300 || this.Id == 36 || this.Id == 37 || this.Id == 301) {
    //      return true;
    //    }
    //  }
    //  return false;
    //}

    public void ChangePassword(string currentPassword, string newPassword, string userID) {
      Assertion.RequireObject(currentPassword, "currentPassword");
      Assertion.RequireObject(newPassword, "newPassword");
      Assertion.RequireObject(userID, "userName");

      if (!isAuthenticated) {
        SecurityException exception =
            new SecurityException(SecurityException.Msg.CantChangePasswordOnUnauthenticatedUser, userID);
        exception.Publish();
        throw exception;
      }
      if (IsSystemUser && !IsAdministrator) {
        return;
      }
      if (!IsActive) {
        return;
      }
      string password = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, newPassword, userID);

      ContactsData.WriteContactAttribute(this.Contact, "UserPassword", password);

      //Security.Data.SecurityData.AppendAuditTrail(ExecutionServer.CurrentIdentity.Session.Guid,
      //                                            this.MetaModelType.Id, this.Id, 'U', DateTime.Now, "ChangePassword / Success");
    }

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.userName = (string) row["UserName"];
      this.isActive = (bool) row["IsActiveUser"];
      this.namedKey = (string) row["Nickname"];
      this.uiTheme = "default";
    }

    protected override void ImplementsSave() {
      ContactsData.WriteUser(this);
    }

    public void SetPassword(string newPassword, string userID) {
      Assertion.RequireObject(newPassword, "newPassword");
      Assertion.RequireObject(userID, "userName");

      if (IsSystemUser) {
        //Security.Data.SecurityData.AppendAuditTrail(ExecutionServer.CurrentIdentity.Session.Guid,
        //                                      this.MetaModelType.Id, this.Id, 'U', DateTime.Now, "Attempt to reset manager password");
      }
      string password = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, newPassword, userID);

      ContactsData.WriteContactAttribute(this.Contact, "UserPassword", password);

      //Security.Data.SecurityData.AppendAuditTrail(ExecutionServer.CurrentIdentity.Session.Guid,
      //                                      this.MetaModelType.Id, this.Id, 'U', DateTime.Now, "SetPassword / Success");
    }

    public bool VerifyElectronicSign(string eSign) {
      string userName = Cryptographer.Encrypt(EncryptionMode.EntropyKey, ExecutionServer.CurrentUser.UserName, ExecutionServer.CurrentSessionToken);
      string password = Cryptographer.Encrypt(EncryptionMode.EntropyKey, eSign, ExecutionServer.CurrentSessionToken);

      User user = Authenticate(userName, password, ExecutionServer.CurrentSessionToken);

      if (user == null) {
        return false;
      } else if (user.Id != ExecutionServer.CurrentUserId) {
        return false;
      } else {
        return true;
      }
    }

    //public bool VerifyPassword(string password, string userID) {
    //  Assertion.RequireObject(password, "password");
    //  Assertion.RequireObject(userID, "userName");

    //  if (IsSystemUser && !IsAdministrator) {
    //    return false;
    //  }
    //  if (!isAuthenticated) {
    //    SecurityException exception = 
    //        new SecurityException(SecurityException.Msg.CantVerifyPasswordOnUnauthenticatedUser, userID);
    //    exception.Publish();
    //    throw exception;
    //  }
    //  if (Cryptographer.Decrypt(ContactsData.GetContactAttribute<string>(this.Contact, "UserPassword"), userID) == password) {
    //    return true;
    //  }
    //  return false;
    //}

    #endregion Public methods

  } // class User

} // namespace Empiria.Security