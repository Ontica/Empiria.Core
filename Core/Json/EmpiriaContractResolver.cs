/* Empiria Core  *********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Json Services                      Component : Json Converters                         *
*  Assembly : Empiria.Core.dll                           Pattern   : Contract resolver                       *
*  Type     : EmpiriaContractResolver                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Applies camelCase naming only to .NET object properties (DTO contracts), preserving literal    *
*             dictionary keys (e.g., JsonObject-backed data) as originally written.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Newtonsoft.Json.Serialization;

namespace Empiria.Json {

  /// <summary>Applies camelCase naming only to .NET object properties (DTO contracts),
  /// preserving literal dictionary keys (e.g., JsonObject-backed data) as originally written.</summary>
  public class EmpiriaContractResolver : CamelCasePropertyNamesContractResolver {

    protected override JsonDictionaryContract CreateDictionaryContract(System.Type objectType) {
      JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

      // Preserve literal dictionary keys; camelCase must apply only to typed object properties.
      contract.DictionaryKeyResolver = key => key;

      return contract;
    }

  }  // class EmpiriaContractResolver

}  // namespace Empiria.Json
