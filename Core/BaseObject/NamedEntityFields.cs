/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                               Component : Interface adapters                      *
*  Assembly : Empiria.Core.dll                           Pattern   : Input Fields DTO                        *
*  Types    : NamedEntityFields                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to update general-purpose named entities.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Input fields DTO used to update general-purpose named entities.</summary>
  public class NamedEntityFields : INamedEntity {

    public string UID {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;

  }  // class NamedEntityFields

}  // namespace Empiria
