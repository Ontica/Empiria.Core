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

      if (fields.FormerId != 0) {
        PatchObjectId(fields.FormerId);
      }
    }


    static public new Person Parse(int id) {
      return BaseObject.ParseId<Person>(id);
    }


    static public new Person Parse(string uid) {
      return BaseObject.ParseKey<Person>(uid);
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

    #region Methods


    public void Update(PersonFields fields) {
      Assertion.Require(fields, nameof(fields));

      Load(fields);
    }

    protected override void OnSave() {
      ContactsDataService.WriteContact(this);
    }

    #endregion Methods

    #region Helpers

    private void Load(PersonFields fields) {
      this.FullName     = Patcher.PatchClean(fields.FullName,    FullName);
      this.ShortName    = Patcher.PatchClean(fields.ShortName,   ShortName);
      this.FirstName    = Patcher.PatchClean(fields.FirstName,   FirstName);
      this.LastName     = Patcher.PatchClean(fields.LastName,    LastName);
      this.LastName2    = Patcher.PatchClean(fields.LastName2,   LastName2);
      this.Initials     = Patcher.PatchClean(fields.Initials,    Initials);

      this.EMail        = Patcher.PatchClean(fields.EMail,       EMail);
      this.Tags         = Patcher.PatchClean(fields.Tags,        Tags);
      this.JobPosition  = Patcher.PatchClean(fields.JobPosition, JobPosition);
      this.JobTitle     = Patcher.PatchClean(fields.JobTitle,    JobTitle);
      this.EmployeeNo   = Patcher.PatchClean(fields.EmployeeNo,  EmployeeNo);

      this.IsFemale = fields.IsFemale;

      this.SetOrganization(fields.Organization);
    }

    #endregion Helpers

  } // class Person

} // namespace Empiria.Contacts
