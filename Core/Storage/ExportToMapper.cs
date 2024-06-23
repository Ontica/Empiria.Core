/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Adapters Layer                          *
*  Assembly : Empiria.Core.dll                           Pattern   : Mapper                                  *
*  Types    : ExportToMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapper class for ExportTo entities.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Storage {

  /// <summary>Mapper class for ExportTo entities.</summary>
  static public class ExportToMapper {

    static public FixedList<ExportToDto> Map(FixedList<ExportTo> exportTo) {
      return exportTo.Select(x => Map(x))
                     .ToFixedList();
    }


    static public ExportToDto Map(ExportTo exportTo) {
      return new ExportToDto {
        UID = exportTo.UID,
        Name = exportTo.Name,
        FileType = exportTo.FileType,
        Dataset = exportTo.Dataset,
        StartDate = exportTo.StartDate,
        EndDate = exportTo.EndDate
      };
    }

    public static FixedList<ExportToDto> Map(FixedList<FileType> exportTo) {
      return exportTo.Select(x => Map(x))
                     .ToFixedList();
    }

    private static ExportToDto Map(FileType fileType) {
      return new ExportToDto {
        UID = fileType.ToString(),
        Name = fileType.ToString(),
        FileType = fileType.ToString(),
      };
    }

    static public FixedList<ExportToDto> MapWithCalculatedUID(FixedList<ExportTo> exportTo) {
      return exportTo.Select(x => MapWithCalculatedUID(x))
                     .ToFixedList();
    }


    static private ExportToDto MapWithCalculatedUID(ExportTo exportTo) {
      return new ExportToDto {
        UID = exportTo.CalculatedUID,
        Name = exportTo.Name,
        FileType = exportTo.FileType,
        Dataset = exportTo.Dataset
      };
    }

  }  // class ExportToMapper

}  // namespace Empiria.Storage
