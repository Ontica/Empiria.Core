/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Transfer Object                    *
*  Type     : FileDto                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO that returns information about a server file.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Storage {

  /// <summary>DTO that returns information about a server file.</summary>
  public class FileDto {

    public FileDto(FileType fileType, string url) {
      Assertion.Require(url, "url");

      this.Type = fileType;
      this.Url = url;
    }


    public FileType Type {
      get;
    }


    public string Url {
      get;
    }

  }  // class FileDto

}  // namespace Empiria.Storage
