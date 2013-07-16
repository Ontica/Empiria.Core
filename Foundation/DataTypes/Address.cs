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
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;

namespace Empiria.Contacts {

  public class Address {

    #region Fields

    private string street = String.Empty;
    private string extNumber = String.Empty;
    private string intNumber = String.Empty;
    private string borough = String.Empty;
    private string zipCode = String.Empty;
    private string municipality = String.Empty;
    private string state = String.Empty;
    private string country = String.Empty;

    private string address1 = String.Empty;
    private string address2 = String.Empty;
    private string address3 = String.Empty;

    #endregion Fields

    #region Constructors and parsers

    internal Address()
      : base() {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public Address Empty {
      get { return new Address(); }
    }

    static public Address Parse(DataRow row) {
      if (row == null) {
        return Address.Empty;
      }
      Address o = new Address();

      o.street = (string) row["AddressStreet"];
      o.extNumber = (string) row["AddressExtNumber"];
      o.intNumber = (string) row["AddressIntNumber"];
      o.borough = (string) row["AddressBorough"];
      o.zipCode = (string) row["AddressZipCode"];
      o.municipality = (string) row["AddressMunicipality"];
      o.state = (string) row["AddressState"];
      o.country = (string) row["AddressCountry"];
      o.address1 = (string) row["Address1"];
      o.address2 = (string) row["Address2"];
      o.address3 = (string) row["Address3"];

      return o;
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Street {
      get { return street; }
      set { street = value; }
    }

    public string ExtNumber {
      get { return extNumber; }
      set { extNumber = value; }
    }

    public string IntNumber {
      get { return intNumber; }
      set { intNumber = value; }
    }

    public string Borough {
      get { return borough; }
      set { borough = value; }
    }

    public string ZipCode {
      get { return zipCode; }
      set { zipCode = value; }
    }

    public string Municipality {
      get { return municipality; }
      set { municipality = value; }
    }

    public string State {
      get { return state; }
      set { state = value; }
    }

    public string Country {
      get { return country; }
      set { country = value; }
    }

    public string Address1 {
      get { return address1; }
      set { address1 = value; }
    }

    public string Address2 {
      get { return address2; }
      set { address2 = value; }
    }

    public string Address3 {
      get { return address3; }
      set { address3 = value; }
    }

    #endregion Public properties


    #region Public methods


    #endregion Public methods

  } // class Address

} // namespace Empiria.Contacts