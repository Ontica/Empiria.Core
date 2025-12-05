/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Contacts Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : Organization                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a government entity or agency, an enterprise or a non-profit organization.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.Contacts {

  /// <summary>Represents a government entity or agency, an enterprise or a non-profit organization.</summary>
  public class Organization : Contact, INamedEntity {

    #region Constructors and parsers

    protected Organization() {
      // Required by Empiria Framework.
    }


    static public new Organization Parse(int id) {
      return BaseObject.ParseId<Organization>(id);
    }

    static public new Organization Parse(string uid) {
      return BaseObject.ParseKey<Organization>(uid);
    }

    static public new Organization Empty => BaseObject.ParseEmpty<Organization>();


    static public Organization Unknown => BaseObject.ParseUnknown<Organization>();


    #endregion Constructors and parsers

    string INamedEntity.Name {
      get {
        if (base.Initials.Length != 0) {
          return $"({base.Initials}) {base.FullName}";
        }
        return base.FullName;
      }
    }

  } // class Organization

} // namespace Empiria.Contacts
