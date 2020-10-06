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

    #region Constructors and parsers

    private TempAddress() {

    }


    static public TempAddress Empty {
      get {
        return new TempAddress();
      }
    }

    #endregion Constructors and parsers


    #region Properties

    public string Description { get; set; } = "Principal";

    public string Street { get; set; } = String.Empty;

    public string ExtNumber { get; set; } = String.Empty;

    public string IntNumber { get; set; } = String.Empty;

    public string Borough { get; set; } = String.Empty;

    public string ZipCode { get; set; } = String.Empty;

    public string Municipality { get; set; } = String.Empty;

    public string State { get; set; } = String.Empty;

    public string Country { get; set; } = String.Empty;

    public string Address1 { get; set; } = String.Empty;

    public string Address2 { get; set; } = String.Empty;

    public string Address3 { get; set; } = String.Empty;

    #endregion Properties

  } // class TempAddress

} // namespace Empiria.Contacts
