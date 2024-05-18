/* Empiria Core **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Commands                           Component : Domain Layer                            *
*  Assembly : Empiria.Core.dll                           Pattern   : Interface                               *
*  Type     : IPositionable                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface that represents a positionable object in an ordered list of objects.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Commands {

  /// <summary>Interface that represents a positionable object in an ordered list of objects.</summary>
  public interface IPositionable : IIdentifiable {

    int Position { get; }

  }  // interface IPositionable

}  // namespace Empiria.Commands
