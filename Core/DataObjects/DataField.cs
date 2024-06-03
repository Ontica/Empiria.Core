/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Data Objects                                 Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : DataField                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes a data field.                                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Json;

namespace Empiria.DataObjects {

  /// <summary>Describes a data field.</summary>
  public class DataField {

    static public DataField Parse(JsonObject json) {
      return new DataField {
        Label = json.Get<string>("label"),
        Field = json.Get<string>("field"),
        DataType = json.Get<string>("dataType"),
        Values = json.GetFixedList<NamedEntityDto>("values", false)
      };
    }

    public string Label {
      get; private set;
    }

    public string Field {
      get; private set;
    }

    public string DataType {
      get; private set;
    }

    public FixedList<NamedEntityDto> Values {
      get; private set;
    }

  }  // public class DataField

}  // namespace Empiria.DataObjects
