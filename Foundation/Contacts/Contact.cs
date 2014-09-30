/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 Assembly : Empiria.dll                       *
*  Type      : Contact                                          Pattern  : Empiria Semiabstract Object Type  *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents either a person, an organization or a group that has a meaningful name and can be  *
*              contacted in some way and can play one or more roles.                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Contacts {

  public class Contact : BaseObject {

    #region Constructors and parsers

    protected Contact() {
      // Required by Empiria Framework.
    }

    static public Contact Parse(int id) {
      return BaseObject.ParseId<Contact>(id);
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

    public TempAddress Address {
      get;
      set;
    }

    [DataField("ShortName")]
    public string Alias {
      get;
      protected set;
    }

    [DataField("EMail")]
    public string EMail {
      get;
      set;
    }

    [DataField("TaxIDNumber")]
    public string TaxTag {
      get;
      protected set;
    }

    public string FormattedTaxTag {
      get {
        return EmpiriaString.FormatTaxTag(this.TaxTag);
      }
    }

    [DataField("ImageFilename")]
    public string Image {
      get;
      protected set;
    }

    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(FullName, Alias, Nickname, EMail, TaxTag);
      }
    }

    [DataField("FingerprintTemplate")]
    protected string ExtendedData {
      get;
      set;
    }

    public T GetExtensionData<T>() {
      return Empiria.Data.JsonConverter.ToObject<T>(this.ExtendedData);
    }

    [DataField("ContactFullName")]
    public string FullName {
      get;
      protected set;
    }

    [DataField("Nickname")]
    public string Nickname {
      get;
      set;
    }

    [DataField("ContactStatus", Default = GeneralObjectStatus.Active)]
    public GeneralObjectStatus Status {
      get;
      protected set;
    }

    /// <summary>OOJJOO</summary>
    [DataField("EMail2TypeId")]
    internal int organizationId {
      get;
      set;
    }

    /// <summary>OOJJOO</summary>
    [DataField("Phone2TypeId")]
    internal int externalObjectId {
      get;
      set;
    }

    #endregion Public properties

    #region Public methods

    public FixedList<T> GetContactsInRole<T>(string roleName) where T : Contact {
      return base.GetLinks<T>(roleName);
    }

    public FixedList<T> GetContactsInRole<T>(string roleName, TimePeriod period) where T : Contact {
      return base.GetLinks<T>(roleName, period);
    }

    protected override void OnLoadObjectData(DataRow row) {
      this.Address = TempAddress.Parse(row);
    }

    protected override void OnSave() {
      ContactsData.WriteContact(this);
    }

    internal void SaveTempUserSettings() {
      ContactsData.WriteTempUserSettings(this.Id, organizationId, externalObjectId);
    }

    #endregion Public methods

  } // class Contact

} // namespace Empiria.Contacts
