/* Empiria® Foundation Framework 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Foundation Framework                    System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 Assembly : Empiria.dll                       *
*  Type      : Contact                                          Pattern  : Empiria Semiabstract Object Type  *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents either a person, an organization or a group that has a meaningful name and can be  *
*              contacted in some way and can play one or more roles.                                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;

namespace Empiria.Contacts {

  public class Contact : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.Contact";

    private string fullName = String.Empty;
    private string alias = String.Empty;
    private string nickname = String.Empty;
    private string taxTag = String.Empty;
    private string image = String.Empty;
    private string keywords = String.Empty;
    private string eMail = String.Empty;
    private Address address = Address.Empty;

    private GeneralObjectStatus status = GeneralObjectStatus.Active;

    #endregion Fields

    #region Constructors and parsers

    protected Contact(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public Contact Parse(int id) {
      return BaseObject.Parse<Contact>(thisTypeName, id);
    }

    static public Contact Parse(DataRow dataRow) {
      return BaseObject.Parse<Contact>(thisTypeName, dataRow);
    }

    static public Contact ParseFromBelow(int id) {
      return BaseObject.ParseFromBelow<Contact>(thisTypeName, id);
    }

    #endregion Constructors and parsers

    #region Public properties

    public Address Address {
      get { return address; }
      set { address = value; }
    }

    public string Alias {
      get { return alias; }
      protected set { alias = value; }
    }

    public string EMail {
      get { return eMail; }
      set { eMail = value; }
    }

    public string TaxTag {
      get { return taxTag; }
      protected set { taxTag = value; }
    }

    public string FormattedTaxTag {
      get {
        return EmpiriaString.FormatTaxTag(this.TaxTag);
      }
    }

    public string Image {
      get { return image; }
      protected set { image = value; }
    }

    public string Keywords {
      get { return keywords; }
      protected set { keywords = value; }
    }

    public string FullName {
      get { return fullName; }
      protected set { fullName = value; }
    }

    public string Nickname {
      get { return nickname; }
      set { nickname = value; }
    }

    public GeneralObjectStatus Status {
      get { return status; }
      protected set { status = value; }
    }

    #endregion Public properties

    #region Public methods

    public ObjectList<T> GetContactsInRole<T>(string roleName) where T : Contact {
      return base.GetLinks<T>(roleName);
    }

    public ObjectList<T> GetContactsInRole<T>(string roleName, TimePeriod period) where T : Contact {
      return base.GetLinks<T>(roleName, period);
    }

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.fullName = (string) row["ContactFullName"];
      this.alias = (string) row["ShortName"];
      this.nickname = (string) row["Nickname"];
      this.taxTag = (string) row["TaxIDNumber"];
      this.image = (string) row["ImageFilename"];
      this.eMail = (string) row["EMail"];
      this.address = Address.Parse(row);
      this.keywords = (string) row["ContactKeywords"];
      this.status = (GeneralObjectStatus) Convert.ToChar(row["ContactStatus"]);
    }

    protected override void ImplementsSave() {
      ContactsData.WriteContact(this);
    }

    #endregion Public methods

  } // class Contact

} // namespace Empiria.Contacts