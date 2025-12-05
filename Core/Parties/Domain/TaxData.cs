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

namespace Empiria.Parties {

  /// <summary>Holds tax data for a person or organization.</summary>
  public class TaxData {

    static FixedList<NamedEntity> _entityKinds = null;

    static public FixedList<NamedEntity> GetTaxEntityKinds() {
      if (_entityKinds != null) {
        return _entityKinds;
      }

      var storedJson = StoredJson.Parse("TaxEntityKinds");

      var kinds = storedJson.Value.GetFixedList<string>("kinds");

      _entityKinds = kinds.Select(x => new NamedEntity(x, x))
                          .ToFixedList();

      return _entityKinds;
    }


    internal TaxData() {
      // no-op
    }


    internal TaxData(TaxDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      TaxCode = fields.TaxCode.ToUpperInvariant();
      TaxEntityKind = fields.TaxEntityKind;
      TaxEntityName = fields.TaxEntityName.ToUpperInvariant();
      TaxRegimeCode = fields.TaxRegimeCode.ToUpperInvariant();
      TaxZipCode = fields.TaxZipCode.ToUpperInvariant();
      SubledgerAccount = fields.SubledgerAccount;
    }


    static internal TaxData Parse(JsonObject jsonObject) {
      var fields = new TaxDataFields {
        TaxCode = jsonObject.Get("taxCode", string.Empty),
        TaxEntityKind = jsonObject.Get("taxEntityKind", "Desconocido"),
        TaxEntityName = jsonObject.Get("taxEntityName", string.Empty),
        TaxRegimeCode = jsonObject.Get("taxRegimeCode", string.Empty),
        TaxZipCode = jsonObject.Get("taxZipCode", string.Empty),
        SubledgerAccount = jsonObject.Get("subledgerAccount", string.Empty)
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


    public string TaxEntityKind {
      get;
    } = "Desconocido";


    public string TaxRegimeCode {
      get;
    } = string.Empty;


    public string TaxZipCode {
      get;
    } = string.Empty;


    public string SubledgerAccount {
      get;
    } = string.Empty;


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(TaxCode, TaxEntityKind, TaxEntityName, TaxZipCode,
                                           SubledgerAccount);
      }
    }

    #endregion Properties

    #region Methods

    internal JsonObject ToJson() {
      return new JsonObject {
        { "taxCode", TaxCode },
        { "taxEntityKind", TaxEntityKind },
        { "taxEntityName", TaxEntityName },
        { "taxRegimeCode", TaxRegimeCode },
        { "taxZipCode", TaxZipCode },
        { "subledgerAccount", SubledgerAccount }
      };
    }

    #endregion Methods

  }  // class TaxData

}  // namespace Empiria.Parties
