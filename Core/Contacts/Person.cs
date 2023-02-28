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

    public Person(PersonFields fields) {
      Assertion.Require(fields, nameof(fields));

      Load(fields);
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


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(base.Keywords, this.EmployeeNo,
                                           this.JobTitle, this.JobPosition);
      }
    }

    #endregion Properties

    #region Helpers

    private void Load(PersonFields fields) {
      this.FullName     = EmpiriaString.Clean(fields.FullName);
      this.ShortName    = EmpiriaString.Clean(fields.ShortName);
      this.FirstName    = EmpiriaString.Clean(fields.FirstName);
      this.LastName     = EmpiriaString.Clean(fields.LastName);
      this.LastName2    = EmpiriaString.Clean(fields.LastName2);
      this.Initials     = EmpiriaString.Clean(fields.Initials);

      this.EMail        = EmpiriaString.Clean(fields.EMail);
      this.Tags         = EmpiriaString.Clean(fields.Tags);
      this.JobPosition  = EmpiriaString.Clean(fields.JobPosition);
      this.JobTitle     = EmpiriaString.Clean(fields.JobTitle);
      this.EmployeeNo   = EmpiriaString.Clean(fields.EmployeeNo);

      this.IsFemale = fields.IsFemale;

      this.SetOrganization(fields.Organization);
    }


    protected override void OnSave() {
      ContactsDataService.WriteContact(this);
    }

    #endregion Helpers

  } // class Person

} // namespace Empiria.Contacts
