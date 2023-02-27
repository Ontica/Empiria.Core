/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Contacts Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : Person                                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a human person.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Contacts {

  /// <summary>Represents a human person.</summary>
  public class Person : Contact {

    #region Constructors and parsers

    protected Person() {
      // Required by Empiria Framework.
    }


    static public new Person Parse(int id) {
      return BaseObject.ParseId<Person>(id);
    }


    static public new Person Empty => BaseObject.ParseEmpty<Person>();


    static public Person Unknown => BaseObject.ParseUnknown<Person>();


    #endregion Constructors and parsers

    #region Properties

    public string FirstName {
      get {
        return base.ExtendedData.Get("person/firstName", string.Empty);
      }
      private set {
        base.ExtendedData.Set("person/firstName", EmpiriaString.TrimAll(value));
      }
    }


    public string LastName {
      get {
        return base.ExtendedData.Get("person/lastName", string.Empty);
      }
      private set {
        base.ExtendedData.Set("person/lastName", EmpiriaString.TrimAll(value));
      }
    }


    public string LastName2 {
      get {
        return base.ExtendedData.Get("person/lastName2", string.Empty);
      }
      private set {
        base.ExtendedData.Set("person/lastName2", EmpiriaString.TrimAll(value));
      }
    }


    public string JobTitle {
      get {
        return ExtendedData.Get("employee/jobTitle", string.Empty);
      }
      private set {
        ExtendedData.Set("employee/jobTitle", EmpiriaString.TrimAll(value));
      }
    }


    public string JobPosition {
      get {
        return ExtendedData.Get("employee/position", string.Empty);
      }
      private set {
        ExtendedData.Set("employee/position", EmpiriaString.TrimAll(value));
      }
    }


    public string EmployeeNo {
      get {
        return ExtendedData.Get("employee/employeeNo", string.Empty);
      }
      private set {
        ExtendedData.Set("employee/employeeNo", EmpiriaString.TrimAll(value));
      }
    }


    public bool IsFemale {
      get {
        return ExtendedData.Get("person/isFemale", false);
      }
      private set {
        ExtendedData.Set("person/isFemale", value);
      }
    }


    #endregion Properties

  } // class Person

} // namespace Empiria.Contacts
