/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Fields DTO                        *
*  Type     : TaxDataFields                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update tax data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties.Adapters {

  /// <summary>Input fields DTO used to update tax data.</summary>
  public class TaxDataFields {

    #region Properties

    public string TaxCode {
      get; set;
    } = string.Empty;


    public string TaxEntityName {
      get; set;
    } = string.Empty;


    public string TaxRegimeCode {
      get; set;
    } = string.Empty;


    public string TaxZipCode {
      get; set;
    } = string.Empty;

    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      Assertion.Require(TaxCode, nameof(TaxCode));
      Assertion.Require(TaxEntityName, nameof(TaxEntityName));
      Assertion.Require(TaxRegimeCode, nameof(TaxRegimeCode));
      Assertion.Require(TaxZipCode, nameof(TaxZipCode));
    }

    #endregion Methods

  }  // class TaxDataFields

}  // namespace Empiria.Parties.Adapters
