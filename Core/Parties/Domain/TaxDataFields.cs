/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Parties                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Fields DTO                        *
*  Type     : TaxDataFields                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update tax data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Parties {

  /// <summary>Input fields DTO used to update tax data.</summary>
  public class TaxDataFields {

    #region Properties

    public string TaxCode {
      get; set;
    } = string.Empty;


    public string TaxEntityKind {
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


    public string SubledgerAccount {
      get; set;
    } = string.Empty;


    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      TaxCode = EmpiriaString.Clean(TaxCode);

      Assertion.Require(TaxCode, nameof(TaxCode));
    }

    #endregion Methods

  }  // class TaxDataFields

}  // namespace Empiria.Parties
