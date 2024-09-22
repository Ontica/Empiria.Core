/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties Management                           Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder / Value Type       *
*  Type     : TaxData                                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds tax data for a person or organization.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

using Empiria.Parties.Adapters;

namespace Empiria.Parties {

  /// <summary>Holds tax data for a person or organization.</summary>
  public class TaxData {

    internal TaxData() {
      // no-op
    }


    internal TaxData(TaxDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      this.TaxCode = fields.TaxCode.ToUpperInvariant();
      this.TaxEntityName = fields.TaxEntityName.ToUpperInvariant();
      this.TaxRegimeCode = fields.TaxRegimeCode.ToUpperInvariant();
      this.TaxZipCode = fields.TaxZipCode.ToUpperInvariant();
    }


    static internal TaxData Parse(JsonObject jsonObject) {
      var fields = new TaxDataFields {
        TaxCode = jsonObject.Get("taxCode", string.Empty),
        TaxEntityName = jsonObject.Get("taxEntityName", string.Empty),
        TaxRegimeCode = jsonObject.Get("taxRegimeCode", string.Empty),
        TaxZipCode = jsonObject.Get("taxZipCode", string.Empty)
      };

      return new TaxData(fields);
    }

    #region Properties

    public string TaxCode {
      get; private set;
    } = string.Empty;


    public string TaxEntityName {
      get; private set;
    } = string.Empty;


    public string TaxRegimeCode {
      get; private set;
    } = string.Empty;


    public string TaxZipCode {
      get; private set;
    } = string.Empty;

    #endregion Properties

    #region Methods

    internal JsonObject ToJson() {
      return new JsonObject {
        { "taxCode", TaxCode },
        { "taxEntityName", TaxEntityName },
        { "taxRegimeCode", TaxRegimeCode },
        { "taxZipCode", TaxZipCode }
      };
    }

    #endregion Methods

  }  // class TaxData

}  // namespace Empiria.Parties
