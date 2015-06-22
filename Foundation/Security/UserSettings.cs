/* Empiria Foundation Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Security Framework                *
*  Namespace : Empiria.Security                                 Assembly : Empiria.Foundation.dll            *
*  Type      : UserSettings                                     Pattern  : Data Type                         *
*  Version   : 6.5        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Hash list of user settings.                                                                   *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      throw new NotImplementedException();

      //this.Add("OrganizationId", -1);
      //this.Add("Trade.MyCurrentOrder", -1);
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
      throw new NotImplementedException();

      //base[key] = value;
      //if (key == "OrganizationId") {
      //  contact.organizationId = value;
      //} else if (key == "Trade.MyCurrentOrder") {
      //  contact.externalObjectId = value;
      //}

    }

    #endregion Public methods

  }  // class UserSettings

} // namespace Empiria.Security
