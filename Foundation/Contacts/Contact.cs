/* Empiria Foundation Framework ******************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 Assembly : Empiria.Foundation.dll            *
*  Type      : Contact                                          Pattern  : Empiria Semiabstract Object Type  *
*  Version   : 6.8                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents either a person, an organization or a group that has a meaningful name and can be  *
*              contacted in some way and can play one or more roles.                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.DataTypes;
using Empiria.Json;

namespace Empiria.Contacts {

  public class Contact : BaseObject {

    #region Constructors and parsers

    protected Contact() {
      // Required by Empiria Framework.
    }

    static public Contact Parse(int id) {
      return BaseObject.ParseId<Contact>(id);
    }

    static public Contact Parse(string uid) {
      return BaseObject.ParseKey<Contact>(uid);
    }

    static private readonly Contact _empty = BaseObject.ParseEmpty<Contact>();
    static public Contact Empty {
      get {
        return _empty.Clone<Contact>();
      }
    }

    static public FixedList<Contact> GetList(string filter) {
      return ContactsData.GetContacts(filter);
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("ContactUID")]
    public string UID {
      get;
      private set;
    }

    [Newtonsoft.Json.JsonIgnore]
    public TempAddress Address {
      get {
        return this.ExtendedData.Get<TempAddress>("Address", TempAddress.Empty);
      }
    }

    [DataField("ShortName")]
    public string Alias {
      get;
      protected set;
    }

    [DataField("ContactEmail")]
    public string EMail {
      get;
      set;
    }

    [Newtonsoft.Json.JsonIgnore]
    public string TaxIDNumber {
      get {
        return this.ExtendedData.Get<string>("TaxIDNumber", String.Empty);
      }
    }

    [Newtonsoft.Json.JsonIgnore]
    public string FormattedTaxIDNumber {
      get {
        return EmpiriaString.FormatTaxTag(this.TaxIDNumber);
      }
    }

    [Newtonsoft.Json.JsonIgnore]
    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(FullName, Alias, Nickname, EMail, TaxIDNumber);
      }
    }

    [Newtonsoft.Json.JsonIgnore]
    [DataField("ContactExtData")]
    public JsonObject ExtendedData {
      get;
      private set;
    }

    [DataField("ContactFullName")]
    public string FullName {
      get;
      protected set;
    }

    [Newtonsoft.Json.JsonIgnore]
    [DataField("Nickname")]
    public string Nickname {
      get;
      set;
    }

    [Newtonsoft.Json.JsonIgnore]
    [DataField("ContactStatus", Default = GeneralObjectStatus.Active)]
    public GeneralObjectStatus Status {
      get;
      protected set;
    }

    #endregion Public properties

    #region Public methods

    public FixedList<T> GetContactsInRole<T>(string roleName) where T : Contact {
      return base.GetLinks<T>(roleName);
    }

    public FixedList<T> GetContactsInRole<T>(string roleName, TimeFrame period) where T : Contact {
      return base.GetLinks<T>(roleName, period);
    }

    protected override void OnSave() {
      ContactsData.WriteContact(this);
    }

    #endregion Public methods

  } // class Contact

} // namespace Empiria.Contacts
