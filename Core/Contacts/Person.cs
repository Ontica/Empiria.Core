/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Core                                     System   : Contacts Management               *
*  Namespace : Empiria.Contacts                                 License  : Please read LICENSE.txt file      *
*  Type      : Person                                           Pattern  : Ontology Object Type              *
*                                                                                                            *
*  Summary   : Information specific to a person.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Contacts {


  /// <summary>Information specific to a person.</summary>
  public class Person : Contact {

    #region Fields

    private string firstName = String.Empty;
    private string lastName = String.Empty;
    private string lastName2 = String.Empty;
    private DateTime bornDate = ExecutionServer.DateMinValue;

    #endregion Fields

    #region Constructors and parsers

    protected Person() {
      // Required by Empiria Framework.
    }

    static public new Person Parse(int id) {
      return BaseObject.ParseId<Person>(id);
    }

    static private readonly Person _empty = BaseObject.ParseEmpty<Person>();
    static public new Person Empty {
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

    [DataField("FirstName")]
    public string FirstName {
      get {
        return firstName;
      }
      set {
        firstName = value;
      }
    }

    [DataField("LastName")]
    public string LastName {
      get {
        return lastName;
      }
      set {
        lastName = value;
      }
    }


    [DataField("LastName2")]
    public string LastName2 {
      get {
        return lastName2;
      }
      set {
        lastName2 = value;
      }
    }


    public string FamilyFullName {
      get {
        return EmpiriaString.TrimAll(this.LastName + " " + this.LastName2 + ", " + this.FirstName);
      }
    }


    public new string FullName {
      get {
        if (base.FullName != String.Empty) {
          return base.FullName;
        }
        return EmpiriaString.TrimAll(this.FirstName + " " + this.LastName + " " + this.LastName2);
      }
    }


    public string JobTitle {
      get {
        return ExtendedData.Get("JobTitle", "No determinado");
      }
    }


    public bool IsFemale {
      get {
        return ExtendedData.Get("IsFemale", false);
      }
    }

    #endregion Public properties

  } // class Person

} // namespace Empiria.Contacts
