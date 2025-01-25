/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Value Type                              *
*  Type     : TaxData                                    License   : Please read LICENSE.txt file            *
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

      TaxCode = fields.TaxCode.ToUpperInvariant();
      TaxEntityName = fields.TaxEntityName.ToUpperInvariant();
      TaxRegimeCode = fields.TaxRegimeCode.ToUpperInvariant();
      TaxZipCode = fields.TaxZipCode.ToUpperInvariant();
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
      get;
    } = string.Empty;


    public string TaxEntityName {
      get;
    } = string.Empty;


    public string TaxRegimeCode {
      get;
    } = string.Empty;


    public string TaxZipCode {
      get;
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


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(TaxCode, TaxEntityName, TaxZipCode);
      }
    }

    #endregion Methods

  }  // class TaxData

}  // namespace Empiria.Parties
