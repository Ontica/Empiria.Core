/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Enumerated type                         *
*  Type     : FileType                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates a file technology.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Storage {

  /// <summary>Enumerates a file technology.</summary>
  public enum FileType {

    Csv,

    Excel,

    Pdf,

    Text,

    Word,

    Xml,

    Unknown,

  }  // enum FileType

} // namespace Empiria.Storage
