/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Data Objects                                 Component : Domain Layer                          *
*  Assembly : Empiria.Core.dll                             Pattern   : Information Holder                    *
*  Type     : DataElement                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a basic unit of data with a specific meaning. Can be compound or simple.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Json;
using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria.DataObjects {

  /// <summary>Represents a basic unit of data with a specific meaning. Can be compound or simple.</summary>
  public class DataElement : BaseObject {

    public string Label {
      get; private set;
    } = string.Empty;


    public string Field {
      get; private set;
    } = string.Empty;



    public MetaModelType DataType {
      get; private set;
    } = MetaModelType.Parse(1);


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
        DataType = this.DataType.Name,
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

  }  // public class DataElement

}  // namespace Empiria.DataObjects
