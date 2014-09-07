/* Empiria Foundation Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Foundation Framework                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 Assembly : Empiria.dll                       *
*  Type      : Person                                           Pattern  : Ontology Object Type              *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Information specific to a person, where a person references one and only one single world     *
*              person.                                                                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Contacts {

  public enum Gender {
    NotApply = 'N',
    Female = 'F',
    Male = 'M',
    Unknown = 'U',
  }

  public class Person : Contact {

    #region Fields

    private string firstName = String.Empty;
    private string lastName = String.Empty;
    private string lastName2 = String.Empty;
    private DateTime bornDate = ExecutionServer.DateMinValue;
    private Gender gender = Gender.Male;

    #endregion Fields

    #region Constructors and parsers

    private Person() {
      // Required by Empiria Framework.
    }

    static public new Person Parse(int id) {
      return BaseObject.ParseId<Person>(id);
    }

    static private readonly Person _empty = BaseObject.ParseEmpty<Person>();
    static public Person Empty {
      get {
        return _empty.Clone<Person>();
      }
    }

    static private readonly Person _unknown = BaseObject.ParseUnknown<Person>();
    static public Person Unknown {
      get {
        return _unknown.Clone<Person>();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public DateTime BornDate {
      get { return bornDate; }
      set { bornDate = value; }
    }

    public string FamilyFullName {
      get {
        return EmpiriaString.TrimAll(this.LastName + " " + this.LastName2 + ", " + this.FirstName);
      }
    }

    public string FirstName {
      get { return firstName; }
      set { firstName = value; }
    }


    public new string FullName {
      get {
        if (this.FirstName.Length == 0 && this.LastName.Length == 0) {
          return base.FullName;
        }
        return EmpiriaString.TrimAll(this.FirstName + " " + this.LastName + " " + this.LastName2);
      }
    }

    public Gender Gender {
      get { return gender; }
      set { gender = value; }
    }

    public string LastName {
      get { return lastName; }
      set { lastName = value; }
    }

    public string LastName2 {
      get { return lastName2; }
      set { lastName2 = value; }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnLoadObjectData(DataRow row) {
      base.OnLoadObjectData(row);
      this.firstName = (string) row["FirstName"];
      this.lastName = (string) row["LastName"];
      this.lastName2 = (string) row["LastName2"];
      this.bornDate = (DateTime) row["BornDate"];
      this.gender = (Gender) Convert.ToChar(row["Gender"]);
    }

    #endregion Public methods

  } // class Person

} // namespace Empiria.Contacts
