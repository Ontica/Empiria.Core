/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Transfer Object                    *
*  Types    : ExporToDto                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for file exportation rules.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Storage {

  /// <summary>Output DTO for file exportation rules.</summary>
  public class ExportToDto {

    internal ExportToDto() {
      // no-op
    }

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string FileType {
      get; internal set;
    }

    public string Dataset {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    } = ExportTo.DEFAULT_START_DATE;


    public DateTime EndDate {
      get; internal set;
    } = ExportTo.DEFAULT_END_DATE;

  }  // class ExportToDto

}  // namespace Empiria.Storage
