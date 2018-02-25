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

namespace Empiria.Contacts {

  public class TempAddress {

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

    private TempAddress() {

    }

    static public TempAddress Empty {
      get {
        return new TempAddress();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Description {
      get {
        return "Principal";
      }
    }

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

  } // class TempAddress

} // namespace Empiria.Contacts
