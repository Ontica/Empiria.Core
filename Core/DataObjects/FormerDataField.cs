/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Data Objects                                 Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : DataField                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about a data element to be used as a field in forms or user interfaces.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria.DataObjects {

  /// <summary>Holds information about a data element to be used as a field in forms or user interfaces.</summary>
  public class FormerDataField {

    static public FormerDataField Parse(JsonObject json) {
      var dataField = new FormerDataField {
        Label = json.Get<string>("label"),
        Field = json.Get<string>("field"),
        DataType = json.Get<string>("dataType"),
        IsRequired = json.Get<bool>("isRequired", true),
        SourceTypeId = json.Get("sourceTypeId", -1),
      };

      dataField.Values = dataField.GetValues(json);

      return dataField;
    }

    public string Label {
      get; private set;
    } = string.Empty;


    public string Field {
      get; private set;
    } = string.Empty;


    public string DataType {
      get; private set;
    } = string.Empty;


    public bool IsRequired {
      get; private set;
    } = true;


    public FixedList<NamedEntity> Values {
      get; private set;
    } = new FixedList<NamedEntity>();


    private int SourceTypeId {
      get; set;
    } = -1;


    public DataFieldDto MapToDto() {
      return new DataFieldDto {
        Label = this.Label,
        Field = this.Field,
        DataType = this.DataType,
        IsRequired = this.IsRequired,
        Values = this.Values.MapToNamedEntityList(),
      };
    }

    #region Helpers

    private FixedList<NamedEntity> GetValues(JsonObject json) {
      if (!json.HasValue("values")) {
        return new FixedList<NamedEntity>();
      }

      if (SourceTypeId == -1) {
        return json.GetFixedList<NamedEntity>("values", false);
      }

      return GetSourceTypeValues(json.GetFixedList<int>("values", false));
    }


    private FixedList<NamedEntity> GetSourceTypeValues(FixedList<int> objectsIds) {
      var oti = ObjectTypeInfo.Parse(SourceTypeId);

      var list = new List<NamedEntity>(objectsIds.Count);

      foreach (var objectId in objectsIds) {
        var instance = (INamedEntity) ObjectFactory.InvokeParseMethod(oti.UnderlyingSystemType, objectId);

        list.Add(new NamedEntity(instance.UID, instance.Name));
      }

      return list.ToFixedList();
    }

    #endregion Helpers

  }  // public class DataField

}  // namespace Empiria.DataObjects
