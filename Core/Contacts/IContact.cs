/* Empiria  Core *********************************************************************************************
*                                                                                                            *
*  Module   : Contacts Management                        Component : Integration Layer                       *
*  Assembly : Empiria.Core.dll                           Pattern   : Data interface                          *
*  Type     : IContact                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a contact.                                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Contacts {

  public interface IContact: IIdentifiable {

    string FullName {
      get;
    }

  }  // interface IContact


  /// <summary>Fields used to create or update Person instances.</summary>
  public class PersonFields {


    public int FormerId {
      get; set;
    }


    public string FullName {
      get; set;
    } = string.Empty;


    public string ShortName {
      get; set;
    } = string.Empty;


    public string FirstName {
      get; set;
    } = string.Empty;


    public string LastName {
      get; set;
    } = string.Empty;


    public string LastName2 {
      get; set;
    } = string.Empty;


    public bool IsFemale {
      get; set;
    }


    public string Initials {
      get; set;
    } = string.Empty;


    public string EMail {
      get; set;
    } = string.Empty;


    public string Tags {
      get; set;
    } = string.Empty;


    public Organization Organization {
      get; set;
    } = Organization.Empty;


    public string JobTitle {
      get; set;
    } = string.Empty;


    public string JobPosition {
      get; set;
    } = string.Empty;


    public string EmployeeNo {
      get; set;
    } = string.Empty;

  }  // class PersonFields

}  // namespace Empiria.Contacts
