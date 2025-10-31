/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                               Component : Interface adapters                      *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Interface                          *
*  Types    : NamedEntity, NamedEntityDto                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an entity with a unique ID and a name.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.Json;

namespace Empiria {

  /// <summary>Represents an entity with a unique ID and a name.</summary>
  public interface INamedEntity : IUniqueID {

    string Name {
      get;
    }

  }  // interface INamedEntity


  /// <summary>Data transfer object for INamedEntity instances.</summary>
  public class NamedEntity : INamedEntity {

    #region Constructors and parsers

    public NamedEntity(INamedEntity entity) {
      Assertion.Require(entity, nameof(entity));

      this.UID = entity.UID;
      this.Name = entity.Name;
    }

    public NamedEntity(string uid, string name) {
      Assertion.Require(uid, nameof(uid));
      Assertion.Require(name, nameof(name));

      this.UID = uid;
      this.Name = name;
    }

    static public NamedEntity Parse(JsonObject json) {
      Assertion.Require(json, nameof(json));

      return new NamedEntity(json.Get<string>("uid"), json.Get<string>("name"));
    }

    #endregion Constructors and parsers

    public string UID {
      get; private set;
    }

    public string Name {
      get; private set;
    }

  }  // class NamedEntity


  /// <summary>Data transfer object for INamedEntity instances.</summary>
  public class NamedEntityDto : INamedEntity {

    #region Constructors and parsers

    private NamedEntityDto() {
      this.UID = string.Empty;
      this.Name = string.Empty;
    }


    public NamedEntityDto(INamedEntity entity) {
      Assertion.Require(entity, nameof(entity));

      this.UID = entity.UID;
      this.Name = entity.Name;
    }


    public NamedEntityDto(string uid, string name) {
      Assertion.Require(uid, nameof(uid));
      Assertion.Require(name, nameof(name));

      this.UID = uid;
      this.Name = name;
    }


    static public NamedEntityDto Parse(JsonObject json) {
      Assertion.Require(json, nameof(json));

      return new NamedEntityDto(json.Get<string>("uid"),
                                json.Get<string>("name"));
    }

    static public NamedEntityDto Empty {
      get {
        return new NamedEntityDto();
      }
    }

    #endregion Constructors and parsers

    public string UID {
      get;
    }

    public string Name {
      get;
    }

  }  // class NamedEntityDto



  /// <summary>Extension methods used for map INamedEntity instances to their
  /// NamedEntityDto objects.</summary>
  static public class NamedEntityMappingExtensions {

    static public NamedEntityDto MapToNamedEntity(this INamedEntity instance) {
      return new NamedEntityDto(instance);
    }


    static public NamedEntityDto MapToNamedEntity(this INamedEntity instance, string whenIsEmptyName) {
      Assertion.Require(whenIsEmptyName, nameof(whenIsEmptyName));

      if (instance.UID.Length == 0 || instance.UID.ToLower() == "empty") {
        return new NamedEntityDto("Empty", whenIsEmptyName);
      }
      return new NamedEntityDto(instance);
    }


    static public NamedEntityDto[] MapToNamedEntityArray(this IEnumerable<INamedEntity> list) {
      return list.Select((x) => MapToNamedEntity(x)).ToArray();
    }


    static public FixedList<NamedEntityDto> MapToNamedEntityList(this IEnumerable<INamedEntity> list) {
      return list.Select((x) => MapToNamedEntity(x))
                 .ToFixedList()
                 .Sort((x, y) => x.Name.CompareTo(y.Name));
    }


    static public FixedList<NamedEntityDto> MapToNamedEntityList(this IEnumerable<INamedEntity> list, bool sortByName) {
      var result = list.Select((x) => MapToNamedEntity(x))
                       .ToFixedList();

      if (!sortByName) {
        return result;
      }

      return result.Sort((x, y) => x.Name.CompareTo(y.Name));
    }

  } // class NamedEntityMappingExtensions

}  // namespace Empiria
