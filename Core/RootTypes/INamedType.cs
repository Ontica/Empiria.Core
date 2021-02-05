/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Core                               Component : Interface adapters                      *
*  Assembly : Empiria.Core.dll                           Pattern   : Data Interface                          *
*  Types    : INamedType, NamedTypeDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a named type.                                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

namespace Empiria {

  /// <summary>Represents a named type.</summary>
  public interface INamedType {

    string Type {
      get;
    }

    string Name {
      get;
    }

  }  // interface INamedType


  /// <summary>Data transfer object for INamedType instances.</summary>
  public class NamedTypeDto {

    public NamedTypeDto(string type, string name) {
      this.Type = type;
      this.Name = name;
    }

    public string Type {
      get;
    }

    public string Name {
      get;
    }

  }  // class NamedTypeDto


}  // namespace Empiria
