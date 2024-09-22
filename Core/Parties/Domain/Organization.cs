/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : Organization                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a government entity or agency, an enterprise or a non-profit organization.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Parties.Adapters;

namespace Empiria.Parties {

  /// <summary>Represents a government entity or agency, an organizational unit an enterprise or
  /// a non-profit organization.</summary>
  public class Organization : Party {

    #region Constructors and parsers

    protected Organization() {
      // Required by Empiria Framework.
    }


    protected Organization(PartyType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Organization Parse(int id) {
      return BaseObject.ParseId<Organization>(id);
    }

    static public new Organization Parse(string uid) {
      return BaseObject.ParseKey<Organization>(uid);
    }

    static public new Organization Empty => BaseObject.ParseEmpty<Organization>();

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


    public TaxData TaxData {
      get {
        return base.ExtendedData.Get("taxData", new TaxData());
      }
      private set {
        base.ExtendedData.SetIfValue("taxData", value.ToJson());
      }
    }


    #endregion Properties

    #region Methods

    public void SetTaxData(TaxDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      this.TaxData = new TaxData(fields);
    }

    #endregion Methods

  } // class Organization

} // namespace Empiria.Parties
