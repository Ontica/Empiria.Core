/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.dll                       *
*  Type      : UserSettings                                     Pattern  : Data Type                         *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Hash list of user settings.                                                                   *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using Empiria.Collections;

namespace Empiria.Security {

  /// <summary>Defines a list of columns used in a general purpose ValuesTable.</summary>
  public class UserSettings : EmpiriaHashList<object> {

    #region Fields

    private Contacts.Contact contact;

    #endregion Fields

    #region Constructors and parsers

    public UserSettings(Contacts.Contact contact) : base(true) {
      this.contact = contact;
      this.Add("OrganizationId", contact.organizationId);
      this.Add("Trade.MyCurrentOrder", contact.externalObjectId);
    }

    #endregion Constructors and parsers

    #region Public methods

    public bool Contains(string key) {
      return base.ContainsKey(key);
    }

    public T GetValue<T>(string key) {
      return (T) base[key];
    }

    public T GetValue<T>(string key, T defaultValue) {
      if (this.Contains(key)) {
        return (T) base[key];
      } else {
        return defaultValue;
      }
    }

    public void SetValue<T>(string key, int value) {
      base[key] = value;
      if (key == "OrganizationId") {
        contact.organizationId = value;
      } else if (key == "Trade.MyCurrentOrder") {
        contact.externalObjectId = value;
      }
      Empiria.Contacts.ContactsData.WriteTempUserSettings(contact.Id,
                                                          GetValue<int>("OrganizationId"),
                                                          GetValue<int>("Trade.MyCurrentOrder"));
    }

    #endregion Public methods

  }  // class UserSettings

} // namespace Empiria.Security