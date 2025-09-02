/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Output DTO                              *
*  Type     : FileResultDto                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO that returns information about a server file with a message.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Storage {

  /// <summary>Output DTO that returns information about a server file with a message.</summary>
  public class FileResultDto {

    public FileResultDto(FileDto file, string message) {
      Assertion.Require(file, nameof(file));
      Assertion.Require(message, nameof(message));

      File = file;
      Message = message;
    }

    public FileDto File {
      get;
    }

    public string Message {
      get;
    }

  }  // FileResultDto

}  // namespace Empiria.Storage
