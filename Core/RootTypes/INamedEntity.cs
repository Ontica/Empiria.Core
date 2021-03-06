﻿/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                               Component : Interface adapters                      *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Interface                          *
*  Types    : INamedEntity, NamedEntityDto               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an entity with a unique ID and a name.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

namespace Empiria {

  /// <summary>Represents an entity with a unique ID and a name.</summary>
  public interface INamedEntity: IUniqueID {

    string Name {
      get;
    }

  }  // interface INamedEntity


  /// <summary>Data transfer object for INamedEntity instances.</summary>
  public class NamedEntityDto {

    public NamedEntityDto(INamedEntity entity) {
      this.UID = entity.UID;
      this.Name = entity.Name;
    }

    public NamedEntityDto(string uid, string name) {
      this.UID = uid;
      this.Name = name;
    }

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

    static public NamedEntityDto[] MapToNamedEntityArray(this IEnumerable<INamedEntity> list) {
      return list.Select((x) => MapToNamedEntity(x)).ToArray();
    }

    static public FixedList<NamedEntityDto> MapToNamedEntityList(this IEnumerable<INamedEntity> list) {
      return new FixedList<NamedEntityDto>(list.Select((x) => MapToNamedEntity(x)));
    }

  } // class NamedEntityMappingExtensions

}  // namespace Empiria
