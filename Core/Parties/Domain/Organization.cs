/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Type     : Organization                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a government entity or agency, an enterprise or a non-profit organization.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Represents a government entity or agency, an enterprise or a non-profit organization.</summary>
  public class Organization : Party {

    static private int PRIMARY_ORGANIZATION_ID = ConfigurationData.Get("PrimaryOrganizationId", 1);

    #region Constructors and parsers

    protected Organization() {
      // Required by Empiria Framework.
    }


    protected Organization(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Organization Parse(int id) => ParseId<Organization>(id);

    static public new Organization Parse(string uid) => ParseKey<Organization>(uid);

    static public new Organization Empty => ParseEmpty<Organization>();

    static public Organization Primary => Parse(PRIMARY_ORGANIZATION_ID);

    #endregion Constructors and parsers

    #region Properties

    public string CommonName {
      get {
        return base.ExtendedData.Get("commonName", string.Empty);
      }
      private set {
        base.ExtendedData.SetIfValue("commonName", value);
      }
    }


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(base.Keywords, CommonName);
      }
    }

    #endregion Properties

  } // class Organization

} // namespace Empiria.Parties
