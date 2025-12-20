/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : Person                                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a human person.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Represents a human person.</summary>
  public class Person : Party, INamedEntity {

    #region Constructors and parsers

    protected Person() {
      // Required by Empiria Framework.
    }

    protected Person(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Person Parse(int id) => ParseId<Person>(id);

    static public new Person Parse(string uid) => ParseKey<Person>(uid);

    static public new Person Empty => ParseEmpty<Person>();

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


    public bool IsFemale {
      get {
        return ExtendedData.Get("person/isFemale", false);
      }
      private set {
        ExtendedData.Set("person/isFemale", value);
      }
    }


    string INamedEntity.Name {
      get {
        return FullName;
      }
    }


    public string FullName {
      get {
        if (Code.Length > 0) {
          return $"{Name} ({Code})";
        } else {
          return Name;
        }
      }
    }

    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(base.Keywords, LastName, LastName2, FirstName);
      }
    }

    #endregion Properties

    #region Methods

    protected void Update(PersonFields fields) {
      base.Update(fields);

      FirstName = Patcher.PatchClean(fields.FirstName, FirstName);
      LastName = Patcher.PatchClean(fields.LastName, LastName);
      LastName2 = fields.LastName2;
      IsFemale = fields.IsFemale;
    }

    #endregion Methods

  } // class Person

} // namespace Empiria.Parties
