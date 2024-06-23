/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Storage                            Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Information Holder                      *
*  Types    : ExporTo                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Information holder with file exportation rules.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Storage {

  /// <summary>Information holder with file exportation rules.</summary>
  public class ExportTo {

    static public readonly DateTime DEFAULT_START_DATE = new DateTime(2000, 01, 01);
    static public readonly DateTime DEFAULT_END_DATE = new DateTime(2049, 12, 31);


    private ExportTo() {
      // no-op
    }

    static internal ExportTo Parse(JsonObject json) {
      return new ExportTo {
        Name = json.Get<string>("name"),
        FileType = json.Get<string>("fileType"),
        FileName = json.Get<string>("fileName", string.Empty),
        CsvBuilder = json.Get<string>("csvBuilder", string.Empty),
        TemplateId = json.Get<int>("templateId", -1),
        Dataset = json.Get<string>("dataset", "Default"),
        StartDate = json.Get<DateTime>("startDate", DEFAULT_START_DATE),
        EndDate = json.Get<DateTime>("endDate", DEFAULT_END_DATE),
      };
    }

    public string UID {
      get {
        return $"{Name}.{Dataset}";
      }
    }

    public string CalculatedUID {
      get {
        if (TemplateId == -1) {
          return this.UID;
        }

        var template = FileTemplateConfig.Parse(TemplateId);

        return template.UID;
      }
    }


    public string Name {
      get; private set;
    }


    public string FileType {
      get; private set;
    }


    public string FileName {
      get; private set;
    }


    public string CsvBuilder {
      get; private set;
    }


    public int TemplateId {
      get; private set;
    }


    public string Dataset {
      get; private set;
    }


    public DateTime StartDate {
      get; private set;
    } = DEFAULT_START_DATE;


    public DateTime EndDate {
      get; private set;
    } = DEFAULT_END_DATE;

  }  // class ExportTo

}  // namespace Empiria.Storage
