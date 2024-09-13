/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Data Objects                                 Component : Interface Adapter                     *
*  Assembly : Empiria.Core.dll                             Pattern   : Output DTO                            *
*  Type     : DataFieldDto                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO for a data field.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.DataObjects {

  /// <summary>Output DTO for a data field.</summary>
  public class DataFieldDto {

    public string Label {
      get; internal set;
    }

    public string Field {
      get; internal set;
    }

    public string DataType {
      get; internal set;
    }

    public bool IsRequired {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Values {
      get; internal set;
    }

  }  // public class DataFieldDto

}  // namespace Empiria.DataObjects
