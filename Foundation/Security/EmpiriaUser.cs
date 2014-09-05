/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : EmpiriaUser                                      Pattern  : Ontology Relation Type            *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a human user.                                                                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Web.Security;

using Empiria.Contacts;

namespace Empiria.Security {

  public sealed class EmpiriaUser : BaseObject, IEmpiriaUser {

    #region Fields

    private const string thisTypeName = "ObjectType.EmpiriaUser";

    private string userName = String.Empty;
    private bool isActive = false;
    private string namedKey = String.Empty;
    private string uiTheme = String.Empty;

    private bool isAuthenticated = false;

    #endregion Fields

    #region Constructors and parsers

    public EmpiriaUser()
      : base(thisTypeName) {

    }

    private EmpiriaUser(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public EmpiriaUser Parse(int id) {
      return BaseObject.ParseId<EmpiriaUser>(id);
    }

    static public EmpiriaUser Current {
      get {
        return ExecutionServer.CurrentUser as EmpiriaUser;
      }
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

    public Organization Organization {
      get {
        return Organization.Parse(1);
      }
    }

    public UserSettings Settings {
      get {
        return new UserSettings(this.Contact);
      }
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

    static internal EmpiriaUser Authenticate(string username, string password, string entropy) {
      Assertion.AssertObject(username, "username");
      Assertion.AssertObject(password, "password");
      Assertion.AssertObject(entropy, "entropy");

      //Remove this two lines
      username = Cryptographer.Decrypt(username, entropy);
      password = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, 
                                       Cryptographer.Decrypt(password, entropy), username);
      int userId = ContactsData.GetContactIdWithUserName(username);
      if (userId == 0) {
        return null;
      }
      EmpiriaUser user = EmpiriaUser.Parse(userId);

      if (user.IsSystemUser && !user.IsAdministrator) {
        return null;
      }
      if (!user.IsActive) {
        return null;
      }
      if (Cryptographer.Decrypt(ContactsData.GetContactAttribute<string>(user.Contact, "UserPassword"), username).
                                Equals(Cryptographer.Decrypt(password, username))) {
        user.isAuthenticated = true;
        return user;
      } else {
        user.isAuthenticated = false;
        return null;
      }
    }

    static internal EmpiriaUser Authenticate(EmpiriaSession activeSession) {
      Assertion.AssertObject(activeSession, "activeSession");

      if (!activeSession.IsStillActive) {
        return null;
      }
      if (activeSession.UserId == 0) {
        return null;
      }
      EmpiriaUser user = EmpiriaUser.Parse(activeSession.UserId);
      if (user.IsSystemUser && !user.IsAdministrator) {
        return null;
      }
      if (!user.IsActive) {
        return null;
      }
      user.isAuthenticated = true;
      return user;
    }

    static internal EmpiriaUser Authenticate(FormsAuthenticationTicket remoteTicket) {
      Assertion.AssertObject(remoteTicket, "remoteTicket");

      int userId = ContactsData.GetContactIdWithUserName(remoteTicket.Name);
      if (userId == 0) {
        return null;
      }
      return EmpiriaUser.Parse(userId);
    }

    static internal EmpiriaUser AuthenticateGuest() {
      int userId = ContactsData.GetContactIdWithUserName("GUEST");
      if (userId == 0) {
        return null;
      }
      EmpiriaUser user = EmpiriaUser.Parse(userId);

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

    public void ChangePassword(string currentPassword, string newPassword, string userID) {
      Assertion.AssertObject(currentPassword, "currentPassword");
      Assertion.AssertObject(newPassword, "newPassword");
      Assertion.AssertObject(userID, "userName");

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
    }

    protected override void OnLoadObjectData(DataRow row) {
      this.userName = (string) row["UserName"];
      this.isActive = (bool) row["IsActiveUser"];
      this.namedKey = (string) row["Nickname"];
      this.uiTheme = "default";
    }

    protected override void OnSave() {
      ContactsData.WriteUser(this);
    }

    public void SetPassword(string newPassword, string userID) {
      Assertion.AssertObject(newPassword, "newPassword");
      Assertion.AssertObject(userID, "userName");

      if (IsSystemUser) {
        //Security.Data.SecurityData.AppendAuditTrail(ExecutionServer.CurrentIdentity.Session.Guid,
        //                                      this.MetaModelType.Id, this.Id, 'U', DateTime.Now, "Attempt to reset manager password");
      }
      string password = Cryptographer.Encrypt(EncryptionMode.EntropyHashCode, newPassword, userID);

      ContactsData.WriteContactAttribute(this.Contact, "UserPassword", password);
    }

    public bool VerifyElectronicSign(string eSign) {
      string userName = Cryptographer.Encrypt(EncryptionMode.EntropyKey,
                                              ExecutionServer.CurrentUser.UserName,
                                              ExecutionServer.CurrentSessionToken);
      string password = Cryptographer.Encrypt(EncryptionMode.EntropyKey, eSign, ExecutionServer.CurrentSessionToken);

      EmpiriaUser user = Authenticate(userName, password, ExecutionServer.CurrentSessionToken);

      if (user == null) {
        return false;
      } else if (user.Id != ExecutionServer.CurrentUserId) {
        return false;
      } else {
        return true;
      }
    }

    #endregion Public methods

  } // class EmpiriaUser

} // namespace Empiria.Security
