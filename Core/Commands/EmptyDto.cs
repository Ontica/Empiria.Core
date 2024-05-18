/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Commands                           Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Transfer Object                    *
*  Type     : EmptyDto                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an empty Data Transfer Object.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria {

  /// <summary>Base interface for DTOs instances.</summary>
  public interface IDto {

  }

  /// <summary>Represents an empty Data Transfer Object.</summary>
  public class EmptyDto : IDto {

    private static readonly EmptyDto _instance = new EmptyDto();

    private EmptyDto() {
      // no-op
    }

    public static IDto Instance => _instance;

  }  // class EmptyDto

}  // namespace Empiria
