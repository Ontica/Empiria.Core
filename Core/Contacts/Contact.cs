/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 License  : Please read LICENSE.txt file      *
*  Type      : Contact                                          Pattern  : Empiria Semiabstract Object Type  *
*                                                                                                            *
*  Summary   : Represents either a person, an organization or a group that has a meaningful name and can be  *
*              contacted in some way and can play one or more roles.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.DataTypes.Time;
using Empiria.Json;
using Empiria.StateEnums;

namespace Empiria.Contacts {

  public class Contact : BaseObject, IContact, INamedEntity {

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
    [DataField("ContactStatus", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      protected set;
    }

    string INamedEntity.Name {
      get {
        return this.FullName;
      }
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
